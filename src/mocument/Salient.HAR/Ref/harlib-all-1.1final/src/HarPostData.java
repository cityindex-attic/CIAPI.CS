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
import java.sql.Statement;
import java.sql.Types;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarPostData
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarPostData
{
  /**
   * Database table name where the data is stored
   */
  public static String      TABLE_NAME   = "post";

  private String            mimeType;
  private HarPostDataParams params;
  private String            text;
  private String            comment;
  private HarCustomFields   customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarPostData</code> object
   * 
   * @param mimeType mimeType [string] - Mime type of posted data.
   * @param params params [array] - List of posted parameters (in case of URL
   *          encoded parameters).
   * @param text text [string] - Plain text posted data
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarPostData(String mimeType, HarPostDataParams params, String text,
      String comment)
  {
    this.mimeType = mimeType;
    this.params = params;
    this.text = text;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarPostData</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarPostData(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"postData\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("mimeType".equals(name))
        setMimeType(jp.getText());
      else if ("params".equals(name))
        setParams(new HarPostDataParams(jp, warnings));
      else if ("text".equals(name))
        setText(jp.getText());
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
    if (mimeType == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing mimeType field in postData element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing mimeType field in postData element",
            jp.getCurrentLocation());
    }

    if (params == null)
    {
      if (warnings != null)
      {
        warnings.add(new HarWarning("Missing params field in postData element",
            jp.getCurrentLocation()));
        setParams(new HarPostDataParams());
      }
      else
        throw new JsonParseException(
            "Missing params field in postData element", jp.getCurrentLocation());
    }
    if (text == null)
    {
      if (warnings != null)
      {
        warnings.add(new HarWarning("Missing text field in postData element",
            jp.getCurrentLocation()));
        setText("");
      }
      else
        throw new JsonParseException("Missing text field in postData element",
            jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarPostData</code> object from a database. Retrieves
   * the HarPostData objects that corresponds to the specified page id.
   * 
   * @param config the database configuration to use
   * @param requestId the request id that is referred to
   * @throws SQLException if a database error occurs
   */
  public HarPostData(HarDatabaseConfig config, long requestId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c.prepareStatement("SELECT id,mime_type,text,comment FROM "
          + tableName + " WHERE request_id=?");
      ps.setLong(1, requestId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarPostData for request id " + requestId
            + " found in database");
      long postDataId = rs.getLong(1);
      setMimeType(rs.getString(2));
      setText(rs.getString(3));
      setComment(rs.getString(4));
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARPOSTDATA, postDataId);
      params = new HarPostDataParams(config, postDataId);
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
    g.writeObjectFieldStart("postData");
    g.writeStringField("mimeType", mimeType);
    params.writeHar(g);
    g.writeStringField("text", text);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified logId.
   * 
   * @param requestId the requestId this object refers to
   * @param config the database configuration to use
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long requestId, HarDatabaseConfig config, long logId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String postTableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(postTableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + postTableName + " (id "
            + config.getDbAutoGeneratedId() + ",mime_type "
            + config.getStringDbType() + ",text " + config.getStringDbType()
            + ",comment " + config.getStringDbType() + ",request_id "
            + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(postTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    String paramsTableName = config.getTablePrefix()
        + HarPostDataParam.TABLE_NAME;
    if (!config.isCreatedTable(paramsTableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + paramsTableName + " (id "
            + config.getDbAutoGeneratedId() + ",name "
            + config.getStringDbType() + ",value " + config.getStringDbType()
            + ",filename " + config.getStringDbType() + ",content_type "
            + config.getStringDbType() + ",comment " + config.getStringDbType()
            + ",post_id " + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(paramsTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement postPs = null;
    ResultSet rs = null;
    PreparedStatement paramsPs = c
        .prepareStatement(
            "INSERT INTO "
                + paramsTableName
                + " (name,value,filename,content_type,comment,post_id) VALUES (?,?,?,?,?,?)",
            PreparedStatement.RETURN_GENERATED_KEYS);
    try
    {
      postPs = c.prepareStatement("INSERT INTO " + postTableName
          + " (mime_type,text,comment,request_id) VALUES (?,?,?,?)",
          Statement.RETURN_GENERATED_KEYS);
      postPs.setString(1, mimeType);
      postPs.setString(2, text);
      if (comment == null)
        postPs.setNull(3, Types.LONGVARCHAR);
      else
        postPs.setString(3, comment);
      postPs.setLong(4, requestId);
      postPs.executeUpdate();
      rs = postPs.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarPostData entry");
      long postId = rs.getLong(1);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARPOSTDATA, postId, logId);
      params.writeJDBC(config, postId, paramsPs, logId);
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
        if (postPs != null)
          postPs.close();
      }
      catch (Exception ignore)
      {
      }
      try
      {
        if (paramsPs != null)
          paramsPs.close();
      }
      catch (Exception ignore)
      {
      }
      config.closeConnection(c);
    }
  }

  /**
   * Delete objects in the database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String queryTableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(queryTableName))
      return;

    params.deleteFromJDBC(config, logId);

    PreparedStatement ps = null;
    try
    { // Delete timings first
      ps = c.prepareStatement("DELETE FROM " + queryTableName
          + " WHERE request_id IN (SELECT id FROM " + config.getTablePrefix()
          + "request" + " WHERE entry_id IN (SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?))");
      ps.setLong(1, logId);
      ps.executeUpdate();
      config.dropTableIfEmpty(c, queryTableName, config);
    }
    finally
    {
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
   * Returns the mimeType value.
   * 
   * @return Returns the mimeType.
   */
  public String getMimeType()
  {
    return mimeType;
  }

  /**
   * Sets the mimeType value.
   * 
   * @param mimeType The mimeType to set.
   */
  public void setMimeType(String mimeType)
  {
    this.mimeType = mimeType;
  }

  /**
   * Returns the params value.
   * 
   * @return Returns the params.
   */
  public HarPostDataParams getParams()
  {
    return params;
  }

  /**
   * Sets the params value.
   * 
   * @param params The params to set.
   */
  public void setParams(HarPostDataParams params)
  {
    this.params = params;
  }

  /**
   * Returns the text value.
   * 
   * @return Returns the text.
   */
  public String getText()
  {
    return text;
  }

  /**
   * Sets the text value.
   * 
   * @param text The text to set.
   */
  public void setText(String text)
  {
    this.text = text;
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
    return "\"postData\": { \"mimeType\": \"" + mimeType + "\", " + params
        + ", " + " \"text\": " + "\"" + text + " \", " + " \"comment\": "
        + "\"" + comment + "\", " + customFields + " }\n";
  }

}
