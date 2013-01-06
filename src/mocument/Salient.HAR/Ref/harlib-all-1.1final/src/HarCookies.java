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
 * This class defines a HarCookies
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarCookies
{
  private List<HarCookie> cookies;

  /**
   * Creates a new <code>HarCookies</code> object
   */
  public HarCookies()
  {
    cookies = new ArrayList<HarCookie>();
  }

  /**
   * Creates a new <code>HarCookies</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarCookies(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    cookies = new ArrayList<HarCookie>();

    // Read the content of the pages element
    if (jp.nextToken() != JsonToken.START_ARRAY)
    {
      throw new JsonParseException("[ missing after \"cookies\" element "
          + jp.getCurrentName(), jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_ARRAY)
    {
      addCookie(new HarCookie(jp, warnings));
    }
  }

  /**
   * Creates a new <code>HarCookies</code> object from a database. Retrieves the
   * HarCookie objects that corresponds to the HarLog object with the specified
   * id.
   * 
   * @param config the database configuration to use
   * @param requestId the HarLog id to read
   * @throws SQLException if a database error occurs
   */
  public HarCookies(HarDatabaseConfig config, long requestId, boolean isRequest)
      throws SQLException
  {
    cookies = new ArrayList<HarCookie>();
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + HarCookie.TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT cookie_id,name,value,path,domain,expires,http_only,secure,comment,version,maxage FROM "
              + tableName + " WHERE ref_id=? AND is_request=?");
      ps.setLong(1, requestId);
      ps.setInt(2, isRequest ? 1 : 0);
      rs = ps.executeQuery();
      while (rs.next())
      {
        long cookieId = rs.getLong(1);
        HarCookie cookie = new HarCookie(rs.getString(2), rs.getString(3));
        cookie.setPath(rs.getString(4));
        cookie.setDomain(rs.getString(5));
        cookie.setExpires(rs.getTimestamp(6));
        boolean httpOnly = rs.getInt(7) == 1 ? true : false;
        if (!rs.wasNull())
          cookie.setHttpOnly(httpOnly);
        boolean secure = rs.getInt(8) == 1 ? true : false;
        if (!rs.wasNull())
          cookie.setSecure(secure);
        cookie.setComment(rs.getString(9));
        cookie.setVersion(rs.getString(10));
        cookie.setMaxAge(rs.getString(11));
        addCookie(cookie);
        cookie.getCustomFields().readCustomFieldsJDBC(config,
            HarCustomFields.Type.HARCOOKIE, cookieId);
      }
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
    g.writeArrayFieldStart("cookies");
    for (HarCookie cookie : cookies)
      cookie.writeHar(g);
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
    String pageTableName = config.getTablePrefix() + HarCookie.TABLE_NAME;
    if (!config.isCreatedTable(pageTableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + pageTableName + " (cookie_id "
            + config.getDbAutoGeneratedId() + ",name "
            + config.getStringDbType() + ",value " + config.getStringDbType()
            + ",path " + config.getStringDbType() + ",domain "
            + config.getStringDbType() + ",expires "
            + config.getTimestampDbType() + ",http_only "
            + config.getSmallIntDbType() + ",secure "
            + config.getSmallIntDbType() + ",comment "
            + config.getStringDbType() + ",is_request "
            + config.getSmallIntDbType() + ",version "
            + config.getStringDbType() + ",maxage " + config.getStringDbType()
            + ",ref_id " + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(pageTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement cookiePs = c
        .prepareStatement(
            "INSERT INTO "
                + pageTableName
                + " (name,value,path,domain,expires,http_only,secure,comment,is_request,version,maxage,ref_id) VALUES (?,?,?,?,?,?,?,?,?,?,?,?)",
            Statement.RETURN_GENERATED_KEYS);
    try
    {
      for (HarCookie cookie : cookies)
        cookie.writeJDBC(config, logId, requestId, cookiePs, isRequest);
    }
    finally
    {
      try
      {
        if (cookiePs != null)
          cookiePs.close();
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
    String cookieTableName = config.getTablePrefix() + HarCookie.TABLE_NAME;
    if (!config.isCreatedTable(cookieTableName))
      return;

    Connection c = config.getConnection();
    PreparedStatement cookiePs = null;
    try
    {
      cookiePs = c.prepareStatement("DELETE FROM " + cookieTableName
          + " WHERE ref_id IN (SELECT id FROM " + config.getTablePrefix()
          + (isRequest ? "request" : "response")
          + " WHERE entry_id IN (SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?))");
      cookiePs.setLong(1, logId);
      cookiePs.executeUpdate();
      config.dropTableIfEmpty(c, cookieTableName, config);
    }
    finally
    {
      try
      {
        if (cookiePs != null)
          cookiePs.close();
      }
      catch (Exception ignore)
      {
      }
      config.closeConnection(c);
    }
  }

  /**
   * Add a new cookie to the list
   * 
   * @param cookie the cookie to add
   */
  public void addCookie(HarCookie cookie)
  {
    cookies.add(cookie);
  }

  /**
   * Returns the cookies value.
   * 
   * @return Returns the cookies.
   */
  public List<HarCookie> getCookies()
  {
    return cookies;
  }

  /**
   * Sets the cookies value.
   * 
   * @param cookies The cookies to set.
   */
  public void setCookies(List<HarCookie> cookies)
  {
    this.cookies = cookies;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    StringBuffer sb = new StringBuffer("  \"cookies\": [");
    if (cookies != null)
    {
      boolean first = true;
      for (HarCookie cookie : cookies)
      {
        if (first)
          first = false;
        else
          sb.append(", ");
        sb.append(cookie);
      }
    }
    sb.append("]\n");
    return sb.toString();
  }

}
