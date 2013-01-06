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
 * This class defines a HarCache
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarCache
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "maincache";

  private HarCacheRequest beforeRequest;
  private HarCacheRequest afterRequest;
  private String          comment;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarCache</code> object - no mandatory field
   */
  public HarCache()
  {
  }

  /**
   * Creates a new <code>HarCache</code> object
   * 
   * @param beforeRequest beforeRequest [object, optional] - State of a cache
   *          entry before the request. Leave out this field if the information
   *          is not available.
   * @param afterRequest afterRequest [object, optional] - State of a cache
   *          entry after the request. Leave out this field if the information
   *          is not available.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarCache(HarCacheRequest beforeRequest, HarCacheRequest afterRequest,
      String comment)
  {
    this.beforeRequest = beforeRequest;
    this.afterRequest = afterRequest;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarCache</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarCache(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"cache\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("beforeRequest".equals(name))
        setBeforeRequest(new HarCacheRequest(jp, true, warnings));
      else if ("afterRequest".equals(name))
        setAfterRequest(new HarCacheRequest(jp, false, warnings));
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in cache element", jp.getCurrentLocation());
      }
    }
  }

  /**
   * Creates a new <code>HarCache</code> object from a database. Retrieves the
   * HarCache objects that corresponds to the specified entry id.
   * 
   * @param config the database configuration to use
   * @param entryId the entry id to read
   * @throws SQLException if a database error occurs
   */
  public HarCache(HarDatabaseConfig config, long entryId) throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + "maincache";
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c.prepareStatement("SELECT id,comment FROM " + tableName
          + " WHERE entry_id=?");
      ps.setLong(1, entryId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarCache for entry id " + entryId
            + " found in database");
      long cacheId = rs.getLong(1);
      setComment(rs.getString(2));
      try
      {
        beforeRequest = new HarCacheRequest(config, cacheId, true);
      }
      catch (Exception ignore)
      { // No entry found in database
        beforeRequest = null;
      }
      try
      {
        afterRequest = new HarCacheRequest(config, cacheId, false);
      }
      catch (Exception ignore)
      { // No entry found in database
        afterRequest = null;
      }
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARCACHE, cacheId);
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
    g.writeObjectFieldStart("cache");
    if (beforeRequest != null)
      beforeRequest.writeHar(g);
    if (afterRequest != null)
      afterRequest.writeHar(g);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object to a database according to the given configuration
   * 
   * @param entryId the entry id this object refers to
   * @param config the database configuration to use
   * @throws SQLException if a database error occurs
   */
  public void writeJDBC(long entryId, HarDatabaseConfig config, long logId)
      throws SQLException
  {
    if (comment == null && afterRequest == null && beforeRequest == null)
      return;

    Connection c = config.getConnection();
    String mainCacheTableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(mainCacheTableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + mainCacheTableName + " (id "
            + config.getDbAutoGeneratedId() + ",comment "
            + config.getStringDbType() + ",entry_id " + config.getLongDbType()
            + ")");
        s.close();
        config.addCreatedTable(mainCacheTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    String cacheTableName = config.getTablePrefix()
        + HarCacheRequest.TABLE_NAME;
    if (!config.isCreatedTable(cacheTableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + cacheTableName + " (id "
            + config.getDbAutoGeneratedId() + ",expires "
            + config.getTimestampDbType() + ",last_access "
            + config.getTimestampDbType() + ",etag " + config.getStringDbType()
            + ",hit_count " + config.getLongDbType() + ",comment "
            + config.getStringDbType() + ",is_before "
            + config.getSmallIntDbType() + ",cache_id "
            + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(cacheTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement ps = null;
    ResultSet rs = null;
    long cacheId;
    PreparedStatement cachePs = null;
    try
    {
      ps = c
          .prepareStatement("INSERT INTO " + mainCacheTableName
              + " (comment,entry_id) VALUES (?,?)",
              Statement.RETURN_GENERATED_KEYS);
      if (comment == null)
        ps.setNull(1, Types.LONGVARCHAR);
      else
        ps.setString(1, comment);
      ps.setLong(2, entryId);
      ps.executeUpdate();
      rs = ps.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarCache entry");
      cacheId = rs.getLong(1);
      cachePs = c
          .prepareStatement("INSERT INTO "
              + cacheTableName
              + " (expires,last_access,etag,hit_count,comment,is_before,cache_id) VALUES (?,?,?,?,?,?,?)");
      if (beforeRequest != null)
        beforeRequest.writeJDBC(cacheId, config, cachePs, logId);
      if (afterRequest != null)
        afterRequest.writeJDBC(cacheId, config, cachePs, logId);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARCACHE, cacheId, logId);
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
      try
      {
        if (cachePs != null)
          cachePs.close();
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
    String mainCacheTableName = config.getTablePrefix() + TABLE_NAME;
    String cacheTableName = config.getTablePrefix()
        + HarCacheRequest.TABLE_NAME;
    if (!config.isCreatedTable(mainCacheTableName))
      return;

    Connection c = config.getConnection();
    PreparedStatement ps = null;
    PreparedStatement cachePs = null;
    try
    {
      if (config.isCreatedTable(cacheTableName))
      {
        cachePs = c.prepareStatement("DELETE FROM " + cacheTableName
            + " WHERE cache_id IN (SELECT id FROM " + mainCacheTableName
            + " WHERE entry_id IN (SELECT entry_id FROM "
            + config.getTablePrefix() + "entries WHERE log_id=?))");
        cachePs.setLong(1, logId);
        cachePs.executeUpdate();
        config.dropTableIfEmpty(c, cacheTableName, config);
      }

      ps = c.prepareStatement("DELETE FROM " + mainCacheTableName
          + " WHERE entry_id IN (SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?)");
      ps.setLong(1, logId);
      ps.executeUpdate();
      config.dropTableIfEmpty(c, mainCacheTableName, config);
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
      try
      {
        if (cachePs != null)
          cachePs.close();
      }
      catch (Exception ignore)
      {
      }
      config.closeConnection(c);
    }
  }

  /**
   * Returns the beforeRequest value.
   * 
   * @return Returns the beforeRequest.
   */
  public HarCacheRequest getBeforeRequest()
  {
    return beforeRequest;
  }

  /**
   * Sets the beforeRequest value.
   * 
   * @param beforeRequest The beforeRequest to set.
   */
  public void setBeforeRequest(HarCacheRequest beforeRequest)
  {
    this.beforeRequest = beforeRequest;
  }

  /**
   * Returns the afterRequest value.
   * 
   * @return Returns the afterRequest.
   */
  public HarCacheRequest getAfterRequest()
  {
    return afterRequest;
  }

  /**
   * Sets the afterRequest value.
   * 
   * @param afterRequest The afterRequest to set.
   */
  public void setAfterRequest(HarCacheRequest afterRequest)
  {
    this.afterRequest = afterRequest;
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
    return "\"cache\": {" + beforeRequest + "," + afterRequest
        + ",\"comment\": " + comment + "\", " + customFields + "}\n";
  }

}
