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
 * This class defines a HarContent
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarContent
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "content";

  private Long            size;
  private Long            compression;
  private String          mimeType;
  private String          text;
  private String          encoding;
  private String          comment;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarContent</code> object
   * 
   * @param size size [number] - Length of the returned content in bytes. Should
   *          be equal to response.bodySize if there is no compression and
   *          bigger when the content has been compressed.
   * @param compression compression [number, optional] - Number of bytes saved.
   *          Leave out this field if the information is not available.
   * @param mimeType mimeType [string] - MIME type of the response text (value
   *          of the Content-Type response header). The charset attribute of the
   *          MIME type is included (if available).
   * @param text text [string, optional] - Response body sent from the server or
   *          loaded from the browser cache. This field is populated with
   *          textual content only. The text field is either HTTP decoded text
   *          or a encoded (e.g. "base64") representation of the response body.
   *          Leave out this field if the information is not available.
   * @param encoding encoding [string, optional] (new in 1.2) - Encoding used
   *          for response text field e.g "base64". Leave out this field if the
   *          text field is HTTP decoded (decompressed & unchunked), than
   *          trans-coded from its original character set into UTF-8.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarContent(long size, long compression, String mimeType, String text,
      String encoding, String comment)
  {
    this.size = size;
    this.compression = compression;
    this.mimeType = mimeType;
    this.text = text;
    this.encoding = encoding;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarContent</code> object using mandatory fields
   * 
   * @param size size [number] - Length of the returned content in bytes. Should
   *          be equal to response.bodySize if there is no compression and
   *          bigger when the content has been compressed.
   * @param mimeType mimeType [string] - MIME type of the response text (value
   *          of the Content-Type response header). The charset attribute of the
   *          MIME type is included (if available).
   */
  public HarContent(long size, String mimeType)
  {
    this.size = size;
    this.mimeType = mimeType;
  }

  /**
   * Creates a new <code>HarContent</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarContent(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"content\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("size".equals(name))
        setSize(jp.getValueAsLong());
      else if ("compression".equals(name))
        setCompression(jp.getValueAsLong());
      else if ("mimeType".equals(name))
        setMimeType(jp.getText());
      else if ("text".equals(name))
        setText(jp.getText());
      else if ("encoding".equals(name))
        setEncoding(jp.getText());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in content element", jp.getCurrentLocation());
      }
    }
    if (size == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing size field in content element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing size field in content element",
            jp.getCurrentLocation());
    }
    if (mimeType == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing mimeType field in content element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing mimeType field in content element",
            jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarContent</code> object from a database. Retrieves the
   * HarContent objects that corresponds to the specified response id.
   * 
   * @param config the database configuration to use
   * @param responseId the response id we refer to
   * @throws SQLException if a database error occurs
   */
  public HarContent(HarDatabaseConfig config, long responseId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT id,size,compression,mime_type,text,encoding,comment FROM "
              + tableName + " WHERE response_id=?");
      ps.setLong(1, responseId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarContent for response id " + responseId
            + " found in database");
      long contentId = rs.getLong(1);
      setSize(rs.getLong(2));
      setCompression(rs.getLong(3));
      if (rs.wasNull())
        compression = null;
      setMimeType(rs.getString(4));
      setText(rs.getString(5));
      setEncoding(rs.getString(6));
      setComment(rs.getString(7));
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARCONTENT, contentId);
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
    g.writeObjectFieldStart("content");
    g.writeNumberField("size", size);
    if (compression != null)
      g.writeNumberField("compression", compression);
    g.writeStringField("mimeType", mimeType);
    if (text != null)
      g.writeStringField("text", text);
    if (encoding != null)
      g.writeStringField("encoding", encoding);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object to a database according to the given configuration
   * 
   * @param responseId the response id this object refers to
   * @param config the database configuration to use
   * @throws SQLException if a database error occurs
   */
  public void writeJDBC(long responseId, HarDatabaseConfig config, long logId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(tableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + tableName + " (id "
            + config.getDbAutoGeneratedId() + ",size " + config.getLongDbType()
            + ",compression " + config.getLongDbType() + ",mime_type "
            + config.getStringDbType() + ",text " + config.getStringDbType()
            + ",encoding " + config.getStringDbType() + ",comment "
            + config.getStringDbType() + ",response_id "
            + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(tableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement(
              "INSERT INTO "
                  + tableName
                  + " (size,compression,mime_type,text,encoding,comment,response_id) VALUES (?,?,?,?,?,?,?)",
              Statement.RETURN_GENERATED_KEYS);
      ps.setLong(1, size);
      if (compression == null)
        ps.setNull(2, Types.BIGINT);
      else
        ps.setLong(2, compression);
      ps.setString(3, mimeType);
      if (text == null)
        ps.setNull(4, Types.LONGVARCHAR);
      else
        ps.setString(4, text);
      if (encoding == null)
        ps.setNull(5, Types.LONGVARCHAR);
      else
        ps.setString(5, encoding);
      if (comment == null)
        ps.setNull(6, Types.LONGVARCHAR);
      else
        ps.setString(6, comment);
      ps.setLong(7, responseId);
      ps.executeUpdate();
      rs = ps.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarContent entry");
      long contentId = rs.getLong(1);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARCONTENT, contentId, logId);
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
   * Delete objects in the database referencing the specified logId.
   * 
   * @param config the database configuration
   * @param logId the logId this object refers to
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId)
      throws SQLException
  {
    String tableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(tableName))
      return;

    Connection c = config.getConnection();
    PreparedStatement ps = null;
    try
    {
      ps = c.prepareStatement("DELETE FROM " + tableName
          + " WHERE response_id IN (SELECT id FROM " + config.getTablePrefix()
          + "response" + " WHERE entry_id IN (SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?))");
      ps.setLong(1, logId);
      ps.executeUpdate();
      config.dropTableIfEmpty(c, tableName, config);
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
   * Returns the size value.
   * 
   * @return Returns the size.
   */
  public long getSize()
  {
    return size;
  }

  /**
   * Sets the size value.
   * 
   * @param size The size to set.
   */
  public void setSize(long size)
  {
    this.size = size;
  }

  /**
   * Returns the compression value.
   * 
   * @return Returns the compression.
   */
  public long getCompression()
  {
    return compression;
  }

  /**
   * Sets the compression value.
   * 
   * @param compression The compression to set.
   */
  public void setCompression(long compression)
  {
    this.compression = compression;
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
   * Returns the encoding value.
   * 
   * @return Returns the encoding.
   */
  public String getEncoding()
  {
    return encoding;
  }

  /**
   * Sets the encoding value.
   * 
   * @param encoding The encoding to set.
   */
  public void setEncoding(String encoding)
  {
    this.encoding = encoding;
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
    return "\"content\": { \"size\": " + size + ", \"compression\": "
        + compression + ", \"mimeType\": \"" + mimeType + "\", \"text\": "
        + "\"" + text + "\", \"encoding\": " + "\"" + encoding
        + "\", \"comment\": " + "\"" + comment + "\", " + customFields + "}\n";
  }

}
