/**
 * BenchLab: Internet Scale Benchmarking.
 * Copyright (C) 2010-2011 Emmanuel Cecchet.
 * Contact: cecchet@cs.umass.edu
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. 
 *
 * Initial developer(s): Emmanuel Cecchet.
 * Contributor(s): Fabien Mottet.
 */

package edu.umass.cs.benchlab.har;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarPageTimings
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarPageTimings
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "page_timings";

  private Long            onContentLoad;
  private Long            onLoad;
  private String          comment;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarPageTimings</code> object with default values.
   */
  public HarPageTimings()
  {
  }

  /**
   * Creates a new <code>HarPageTimings</code> object
   * 
   * @param onContentLoad Content of the page loaded. Number of milliseconds
   *          since page load started (page.startedDateTime). Use -1 if the
   *          timing does not apply to the current request.
   * @param onLoad Page is loaded (onLoad event fired). Number of milliseconds
   *          since page load started (page.startedDateTime). Use -1 if the
   *          timing does not apply to the current request.
   * @param comment A comment provided by the user or the application.
   */
  public HarPageTimings(long onContentLoad, long onLoad, String comment)
  {
    this.onContentLoad = onContentLoad;
    this.onLoad = onLoad;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarPageTimings</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarPageTimings(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"pageTimings\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("onContentLoad".equals(name))
        setOnContentLoad(jp.getValueAsLong());
      else if ("onLoad".equals(name))
        setOnLoad(jp.getValueAsLong());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in page element", jp.getCurrentLocation());
      }
    }
  }

  /**
   * Creates a new <code>HarPageTimings</code> object from a database. Retrieves
   * the HarPageTimings objects that corresponds to the specified page id.
   * 
   * @param config the database configuration to use
   * @param pageId the page id referenced
   * @throws SQLException if a database error occurs
   */
  public HarPageTimings(HarDatabaseConfig config, long pageId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT page_timings_id,on_content_load,on_load,comment FROM "
              + tableName + " WHERE page_id=?");
      ps.setLong(1, pageId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarPage for page id " + pageId
            + " found in database");
      long pageTimingsId = rs.getLong(1);
      setOnContentLoad(rs.getLong(2));
      setOnLoad(rs.getLong(3));
      setComment(rs.getString(4));
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARPAGETIMING, pageTimingsId);
    }
    finally
    {
      try
      {
        if (rs != null)
          rs.close();
      }
      catch (Exception ignore)
      {
      }
      try
      {
        if (ps != null)
          ps.close();
      }
      catch (Exception ignore)
      {
      }
      config.closeConnection(c);
    }
  }

  /**
   * Write this object on a JsonGenerator stream
   * 
   * @param g a JsonGenerator
   * @throws IOException if an IO error occurs
   * @throws JsonGenerationException if the generator fails
   * @see HarFileWriter#writeHarFile(HarLog, java.io.File)
   */
  public void writeHar(JsonGenerator g) throws JsonGenerationException,
      IOException
  {
    g.writeObjectFieldStart("pageTimings");
    if (onContentLoad != null)
      g.writeNumberField("onContentLoad", onContentLoad);
    if (onLoad != null)
      g.writeNumberField("onLoad", onLoad);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified logId.
   * 
   * @param pageId the database page id this object refers to
   * @param timingPs PreparedStatement to write page timings data
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long pageId, HarDatabaseConfig config,
      PreparedStatement timingPs, long logId) throws SQLException
  {
    if (onContentLoad == null)
      timingPs.setNull(1, Types.BIGINT);
    else
      timingPs.setLong(1, onContentLoad);
    if (onLoad == null)
      timingPs.setNull(2, Types.BIGINT);
    else
      timingPs.setLong(2, onLoad);
    if (comment == null)
      timingPs.setNull(3, Types.LONGVARCHAR);
    else
      timingPs.setString(3, comment);
    timingPs.setLong(4, pageId);
    timingPs.executeUpdate();
    ResultSet rs = timingPs.getGeneratedKeys();
    if (!rs.next())
      throw new SQLException(
          "The database did not generate a key for an HarEntry row");
    long pageTimingId = rs.getLong(1);
    this.customFields.writeCustomFieldsJDBC(config,
        HarCustomFields.Type.HARPAGETIMING, pageTimingId, logId);

  }

  /**
   * Returns the onContentLoad value.
   * 
   * @return Returns the onContentLoad.
   */
  public long getOnContentLoad()
  {
    return onContentLoad;
  }

  /**
   * Sets the onContentLoad value.
   * 
   * @param onContentLoad The onContentLoad to set.
   */
  public void setOnContentLoad(long onContentLoad)
  {
    this.onContentLoad = onContentLoad;
  }

  /**
   * Returns the onLoad value.
   * 
   * @return Returns the onLoad.
   */
  public long getOnLoad()
  {
    return onLoad;
  }

  /**
   * Sets the onLoad value.
   * 
   * @param onLoad The onLoad to set.
   */
  public void setOnLoad(long onLoad)
  {
    this.onLoad = onLoad;
  }

  /**
   * Returns the comment value.
   * 
   * @return Returns the comment.
   */
  public String getComment()
  {
    return comment;
  }

  /**
   * Sets the comment value.
   * 
   * @param comment The comment to set.
   */
  public void setComment(String comment)
  {
    this.comment = comment;
  }

  /**
   * Returns the customFields value.
   * 
   * @return Returns the customFields.
   */
  public HarCustomFields getCustomFields()
  {
    return customFields;
  }

  /**
   * Sets the customFields value.
   * 
   * @param customFields The customFields to set.
   */
  public void setCustomFields(HarCustomFields customFields)
  {
    this.customFields = customFields;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    return "\"pageTimings\": { \"onContentLoad\": " + onContentLoad
        + ",\"onLoad\":" + onLoad + ", \"comment\": \"" + comment + "\", "
        + customFields + " }";
  }

}
