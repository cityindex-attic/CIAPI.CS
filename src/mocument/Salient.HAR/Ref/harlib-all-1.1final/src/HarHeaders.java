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
 * This class defines a HarHeaders
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarHeaders
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME = "headers";

  private List<HarHeader> headers;

  /**
   * Creates a new <code>HarHeaders</code> object
   */
  public HarHeaders()
  {
    headers = new ArrayList<HarHeader>();
  }

  /**
   * Creates a new <code>HarHeaders</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarHeaders(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    headers = new ArrayList<HarHeader>();

    // Read the content of the pages element
    if (jp.nextToken() != JsonToken.START_ARRAY)
    {
      throw new JsonParseException("[ missing after \"headers\" element "
          + jp.getCurrentName(), jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_ARRAY)
    {
      addHeader(new HarHeader(jp, warnings));
    }
  }

  /**
   * Creates a new <code>HarHeaders</code> object from a database. Retrieves the
   * HarHeader objects that corresponds to the HarLog object with the specified
   * id.
   * 
   * @param config the database configuration
   * @param requestId the requestId this object refers to
   * @param isRequest true if these cookies belong to a request, false if they
   *          belong to a response
   * @throws SQLException if a database error occurs
   */
  public HarHeaders(HarDatabaseConfig config, long requestId, boolean isRequest)
      throws SQLException
  {
    headers = new ArrayList<HarHeader>();
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c.prepareStatement("SELECT id,name,value,comment FROM " + tableName
          + " WHERE ref_id=? AND is_request=?");
      ps.setLong(1, requestId);
      ps.setInt(2, isRequest ? 1 : 0);
      rs = ps.executeQuery();
      while (rs.next())
        addHeader(new HarHeader(config, rs.getLong(1), rs.getString(2),
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
    g.writeArrayFieldStart("headers");
    for (HarHeader header : headers)
      header.writeHar(g);
    g.writeEndArray();
  }

  /**
   * Write this object in the given database referencing the specified
   * requestId.
   * 
   * @param requestId the requestId this object refers to
   * @param config the database configuration
   * @param isRequest true if these cookies belong to a request, false if they
   *          belong to a response
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long requestId, HarDatabaseConfig config,
      boolean isRequest, long logId) throws SQLException
  {
    Connection c = config.getConnection();
    String headerTableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(headerTableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + headerTableName + " (id "
            + config.getDbAutoGeneratedId() + ",name "
            + config.getStringDbType() + ",value " + config.getStringDbType()
            + ",comment " + config.getStringDbType() + ",is_request "
            + config.getSmallIntDbType() + ",ref_id " + config.getLongDbType()
            + ")");
        s.close();
        config.addCreatedTable(headerTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement headerPs = c.prepareStatement("INSERT INTO "
        + headerTableName
        + " (name,value,comment,is_request,ref_id) VALUES (?,?,?,?,?)",
        Statement.RETURN_GENERATED_KEYS);
    try
    {
      for (HarHeader header : headers)
        header.writeJDBC(config, requestId, headerPs, isRequest, logId);
    }
    finally
    {
      try
      {
        if (headerPs != null)
          headerPs.close();
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
   * @param isRequest true if these cookies belong to a request, false if they
   *          belong to a response
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId,
      boolean isRequest) throws SQLException
  {
    String headersTableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(headersTableName))
      return;

    Connection c = config.getConnection();
    PreparedStatement ps = null;
    try
    {
      ps = c.prepareStatement("DELETE FROM " + headersTableName
          + " WHERE ref_id IN (SELECT id FROM " + config.getTablePrefix()
          + (isRequest ? "request" : "response")
          + " WHERE entry_id IN (SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?))");
      ps.setLong(1, logId);
      ps.executeUpdate();
      config.dropTableIfEmpty(c, headersTableName, config);
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
   * Add a new header to the list
   * 
   * @param header the header to add
   */
  public void addHeader(HarHeader header)
  {
    headers.add(header);
  }

  /**
   * Returns the headers value.
   * 
   * @return Returns the headers.
   */
  public List<HarHeader> getHeaders()
  {
    return headers;
  }

  /**
   * Sets the headers value.
   * 
   * @param headers The headers to set.
   */
  public void setHeaders(List<HarHeader> headers)
  {
    this.headers = headers;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    StringBuffer sb = new StringBuffer("  \"headers\": [");
    if (headers != null)
    {
      boolean first = true;
      for (HarHeader header : headers)
      {
        if (first)
          first = false;
        else
          sb.append(", ");
        sb.append(header);
      }
    }
    sb.append("]\n");
    return sb.toString();
  }

}
