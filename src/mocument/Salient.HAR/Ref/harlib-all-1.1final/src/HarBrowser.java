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
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarBrowser ("browser" element of the HAR specification)
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarBrowser
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "browser";

  private String          name;
  private String          version;
  private String          comment;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarBrowser</code> object
   * 
   * @param name Name of the application/browser used to export the log
   * @param version Version of the application/browser used to export the log
   */
  public HarBrowser(String name, String version)
  {
    this.name = name;
    this.version = version;
  }

  /**
   * Creates a new <code>HarBrowser</code> object
   * 
   * @param name Name of the application/browser used to export the log
   * @param version Version of the application/browser used to export the log
   * @param comment An optional comment provided by the user of the application
   */
  public HarBrowser(String name, String version, String comment)
  {
    this.name = name;
    this.version = version;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarEntries</code> objectfrom a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarBrowser(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"browser\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("version".equals(name))
        setVersion(jp.getText());
      else if ("name".equals(name))
        setName(jp.getText());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in browser element", jp.getCurrentLocation());
      }
    }
    if (name == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing name field in browser element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing name field in browser element",
            jp.getCurrentLocation());
    }
    if (version == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing version field in browser element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing version field in browser element", jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarBrowser</code> object from a database. Retrieves the
   * HarBrowser object that corresponds to the HarLog object with the specified
   * id.
   * 
   * @param config the database configuration to use
   * @param logId the HarLog id to read
   * @throws SQLException if a database error occurs
   */
  public HarBrowser(HarDatabaseConfig config, long logId) throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c.prepareStatement("SELECT id,name,version,comment FROM "
          + tableName + " WHERE log_id=?");
      ps.setLong(1, logId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarBrowser for log id " + logId
            + " found in database");
      long browserId = rs.getLong(1);
      setName(rs.getString(2));
      setVersion(rs.getString(3));
      setComment(rs.getString(4));
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARBROWSER, browserId);
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
    g.writeObjectFieldStart("browser");
    g.writeStringField("name", name);
    g.writeStringField("version", version);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long logId, HarDatabaseConfig config)
      throws SQLException
  {
    long browserId = config.writeNameValueCommentJDBC(logId, config,
        TABLE_NAME, "name", name, "version", version, "comment", comment);
    this.customFields.writeCustomFieldsJDBC(config,
        HarCustomFields.Type.HARBROWSER, browserId, logId);
  }

  /**
   * Delete objects of this kind in the given database referencing the specified
   * logId.
   * 
   * @param logId the logId objects refer to
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId)
      throws SQLException
  {
    config.deleteFromTable(logId, config, "browser");
  }

  /**
   * Returns the name value.
   * 
   * @return Returns the name.
   */
  public String getName()
  {
    return name;
  }

  /**
   * Sets the name value.
   * 
   * @param name The name to set.
   */
  public void setName(String name)
  {
    this.name = name;
  }

  /**
   * Returns the version value.
   * 
   * @return Returns the version.
   */
  public String getVersion()
  {
    return version;
  }

  /**
   * Sets the version value.
   * 
   * @param version The version to set.
   */
  public void setVersion(String version)
  {
    this.version = version;
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
    return "\"browser\": { \"name\": \"" + name + "\", \"version\": \""
        + version + "\", \"comment\": " + "\"" + comment + "\", "
        + customFields + " }";
  }

}
