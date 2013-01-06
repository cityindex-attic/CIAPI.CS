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
import java.util.ArrayList;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarQueryString
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarQueryString
{
  private List<HarQueryParam> queryParams;

  /**
   * Creates a new <code>HarQueryString</code> object
   */
  public HarQueryString()
  {
    queryParams = new ArrayList<HarQueryParam>();
  }

  /**
   * Creates a new <code>HarQueryString</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarQueryString(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    queryParams = new ArrayList<HarQueryParam>();

    // Read the content of the pages element
    if (jp.nextToken() != JsonToken.START_ARRAY)
    {
      throw new JsonParseException("[ missing after \"queryString\" element "
          + jp.getCurrentName(), jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_ARRAY)
    {
      addQueryParam(new HarQueryParam(jp, warnings));
    }
  }

  /**
   * Creates a new <code>HarQueryString</code> object from a database. Retrieves
   * the HarQueryParam objects that corresponds to the HarLog object with the
   * specified id.
   * 
   * @param config the database configuration
   * @param requestId the requestId this object refers to
   * @throws SQLException if a database error occurs
   */
  public HarQueryString(HarDatabaseConfig config, long requestId)
      throws SQLException
  {
    queryParams = new ArrayList<HarQueryParam>();
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + HarQueryParam.TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c.prepareStatement("SELECT id,name,value,comment FROM " + tableName
          + " WHERE request_id=?");
      ps.setLong(1, requestId);
      rs = ps.executeQuery();
      while (rs.next())
        addQueryParam(new HarQueryParam(config, rs.getLong(1), rs.getString(2),
            rs.getString(3), rs.getString(4)));
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
    g.writeArrayFieldStart("queryString");
    for (HarQueryParam queryParam : queryParams)
      queryParam.writeHar(g);
    g.writeEndArray();
  }

  /**
   * Write this object in the given database referencing the specified
   * requestId.
   * 
   * @param requestId the requestId this object refers to
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long requestId, HarDatabaseConfig config, long logId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + HarQueryParam.TABLE_NAME;
    if (!config.isCreatedTable(tableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + tableName + " (id "
            + config.getDbAutoGeneratedId() + ",name "
            + config.getStringDbType() + ",value " + config.getStringDbType()
            + ",comment " + config.getStringDbType() + ",request_id "
            + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(tableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement queryPs = c.prepareStatement("INSERT INTO " + tableName
        + " (name,value,comment,request_id) VALUES (?,?,?,?)",
        Statement.RETURN_GENERATED_KEYS);
    try
    {
      for (HarQueryParam queryParam : queryParams)
        queryParam.writeJDBC(config, requestId, queryPs, logId);
    }
    finally
    {
      try
      {
        if (queryPs != null)
          queryPs.close();
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
    String queryTableName = config.getTablePrefix() + HarQueryParam.TABLE_NAME;
    if (!config.isCreatedTable(queryTableName))
      return;

    Connection c = config.getConnection();
    PreparedStatement ps = null;
    try
    {
      // Delete timings first
      ps = c.prepareStatement("DELETE FROM " + queryTableName
          + " WHERE request_id IN (SELECT id FROM " + config.getTablePrefix()
          + "request WHERE entry_id IN (SELECT entry_id FROM "
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
   * Add a new query parameter to the list
   * 
   * @param queryParam the header to add
   */
  public void addQueryParam(HarQueryParam queryParam)
  {
    queryParams.add(queryParam);
  }

  /**
   * Returns the query parameters value.
   * 
   * @return Returns the query parameters.
   */
  public List<HarQueryParam> getQueryParams()
  {
    return queryParams;
  }

  /**
   * Sets the query parameters value.
   * 
   * @param queryParams The queryParams to set.
   */
  public void setQueryParams(List<HarQueryParam> queryParams)
  {
    this.queryParams = queryParams;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    StringBuffer sb = new StringBuffer("  \"queryString\": [");
    if (queryParams != null)
    {
      boolean first = true;
      for (HarQueryParam queryParam : queryParams)
      {
        if (first)
          first = false;
        else
          sb.append(", ");
        sb.append(queryParam);
      }
    }
    sb.append("]\n");
    return sb.toString();
  }

}
