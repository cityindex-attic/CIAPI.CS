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
 * This class defines a HarRequest
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarRequest
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "request";

  private String          method;
  private String          url;
  private String          httpVersion;
  private HarCookies      cookies;
  private HarHeaders      headers;
  private HarQueryString  queryString;
  private HarPostData     postData;
  private Long            headersSize;
  private Long            bodySize;
  private String          comment;

  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarRequest</code> object
   * 
   * @param method method [string] - Request method (GET, POST, ...).
   * @param url url [string] - Absolute URL of the request (fragments are not
   *          included).
   * @param httpVersion httpVersion [string] - Request HTTP Version.
   * @param cookies cookies [array] - List of cookie objects.
   * @param headers headers [array] - List of header objects.
   * @param queryString queryString [array] - List of param name/values parsed
   *          from a query string.
   * @param postData postData [object, optional] - Posted data info.
   * @param headersSize headersSize [number] - Total number of bytes from the
   *          start of the HTTP request message until (and including) the double
   *          CRLF before the body. Set to -1 if the info is not available.
   * @param bodySize bodySize [number] - Size of the request body (POST data
   *          payload) in bytes. Set to -1 if the info is not available.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarRequest(String method, String url, String httpVersion,
      HarCookies cookies, HarHeaders headers, HarQueryString queryString,
      HarPostData postData, long headersSize, long bodySize, String comment)
  {
    this.method = method;
    this.url = url;
    this.httpVersion = httpVersion;
    this.cookies = cookies;
    this.headers = headers;
    this.queryString = queryString;
    this.postData = postData;
    this.headersSize = headersSize;
    this.bodySize = bodySize;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarRequest</code> object with all mandatory parameters
   * 
   * @param method method [string] - Request method (GET, POST, ...).
   * @param url url [string] - Absolute URL of the request (fragments are not
   *          included).
   * @param httpVersion httpVersion [string] - Request HTTP Version.
   * @param cookies cookies [array] - List of cookie objects.
   * @param headers headers [array] - List of header objects.
   * @param queryString queryString [array] - List of param name/values parsed
   *          from a query string.
   * @param headersSize headersSize [number] - Total number of bytes from the
   *          start of the HTTP request message until (and including) the double
   *          CRLF before the body. Set to -1 if the info is not available.
   * @param bodySize bodySize [number] - Size of the request body (POST data
   *          payload) in bytes. Set to -1 if the info is not available.
   */
  public HarRequest(String method, String url, String httpVersion,
      HarCookies cookies, HarHeaders headers, HarQueryString queryString,
      long headersSize, long bodySize)
  {
    this.method = method;
    this.url = url;
    this.httpVersion = httpVersion;
    this.cookies = cookies;
    this.headers = headers;
    this.queryString = queryString;
    this.headersSize = headersSize;
    this.bodySize = bodySize;
  }

  /**
   * Creates a new <code>HarRequest</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarRequest(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"request\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("method".equals(name))
        setMethod(jp.getText());
      else if ("url".equals(name))
        setUrl(jp.getText());
      else if ("httpVersion".equals(name))
        setHttpVersion(jp.getText());
      else if ("cookies".equals(name))
        setCookies(new HarCookies(jp, warnings));
      else if ("headers".equals(name))
        setHeaders(new HarHeaders(jp, warnings));
      else if ("queryString".equals(name))
        setQueryString(new HarQueryString(jp, warnings));
      else if ("postData".equals(name))
        setPostData(new HarPostData(jp, warnings));
      else if ("headersSize".equals(name))
        setHeadersSize(jp.getValueAsLong());
      else if ("bodySize".equals(name))
        setBodySize(jp.getValueAsLong());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in request element", jp.getCurrentLocation());
      }
    }
    if (method == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing name field in request element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing name field in request element",
            jp.getCurrentLocation());
    }
    if (url == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing url field in request element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing url field in request element",
            jp.getCurrentLocation());
    }
    if (httpVersion == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing httpVersion field in request element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing httpVersion field in request element",
            jp.getCurrentLocation());
    }
    if (cookies == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing cookies field in request element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing cookies field in request element", jp.getCurrentLocation());
    }
    if (headers == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing headers field in request element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing headers field in request element", jp.getCurrentLocation());
    }
    if (queryString == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing queryString field in request element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing queryString field in request element",
            jp.getCurrentLocation());
    }
    if (headersSize == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing headersSize field in request element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing headersSize field in request element",
            jp.getCurrentLocation());
    }
    if (bodySize == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing bodySize field in request element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing bodySize field in request element",
            jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarRequest</code> object from a database. Retrieves the
   * HarRequest objects that corresponds to the specified entry id.
   * 
   * @param config the database configuration to use
   * @param entryId the entry id to read
   * @throws SQLException if a database error occurs
   */
  public HarRequest(HarDatabaseConfig config, long entryId) throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT id,method,url,http_version,header_size,body_size,comment FROM "
              + tableName + " WHERE entry_id=?");
      ps.setLong(1, entryId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarRequest for entry id " + entryId
            + " found in database");
      long requestId = rs.getLong(1);
      setMethod(rs.getString(2));
      setUrl(rs.getString(3));
      setHttpVersion(rs.getString(4));
      setHeadersSize(rs.getLong(5));
      setBodySize(rs.getLong(6));
      setComment(rs.getString(7));
      cookies = new HarCookies(config, requestId, true);
      headers = new HarHeaders(config, requestId, true);
      queryString = new HarQueryString(config, requestId);
      try
      {
        postData = new HarPostData(config, requestId);
      }
      catch (SQLException ignore)
      { // Optional parameter
        postData = null;
      }
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARREQUEST, requestId);
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
    g.writeObjectFieldStart("request");
    g.writeStringField("method", method);
    g.writeStringField("url", url);
    g.writeStringField("httpVersion", httpVersion);
    cookies.writeHar(g);
    headers.writeHar(g);
    queryString.writeHar(g);
    if (postData != null)
      postData.writeHar(g);
    g.writeNumberField("headersSize", headersSize);
    g.writeNumberField("bodySize", bodySize);
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
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(tableName))
    {
      try
      {
        Statement s = c.createStatement();

        s.executeUpdate("CREATE TABLE " + tableName + " (id "
            + config.getDbAutoGeneratedId() + ",method "
            + config.getStringDbType() + ",url " + config.getStringDbType()
            + ",http_version " + config.getStringDbType() + ",header_size "
            + config.getLongDbType() + ",body_size " + config.getLongDbType()
            + ",comment " + config.getStringDbType() + ",entry_id "
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
    long requestId;
    try
    {
      ps = c
          .prepareStatement(
              "INSERT INTO "
                  + tableName
                  + " (method,url,http_version,header_size,body_size,comment,entry_id) VALUES (?,?,?,?,?,?,?)",
              Statement.RETURN_GENERATED_KEYS);
      ps.setString(1, method);
      ps.setString(2, url);
      ps.setString(3, httpVersion);
      ps.setLong(4, headersSize);
      ps.setLong(5, bodySize);
      if (comment == null)
        ps.setNull(6, Types.LONGVARCHAR);
      else
        ps.setString(6, comment);
      ps.setLong(7, entryId);
      ps.executeUpdate();
      rs = ps.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarRequest entry");
      requestId = rs.getLong(1);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARREQUEST, requestId, logId);
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
    cookies.writeJDBC(requestId, config, true, logId);
    headers.writeJDBC(requestId, config, true, logId);
    queryString.writeJDBC(requestId, config, logId);
    if (postData != null)
      postData.writeJDBC(requestId, config, logId);
  }

  /**
   * Delete objects in the database referencing the specified logId.
   * 
   * @param config the database configuration
   * @param logId the logId this object refers to
   * @param dropTables true if tables must be dropped
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId,
      boolean dropTables) throws SQLException
  {
    cookies.deleteFromJDBC(config, logId, true);
    headers.deleteFromJDBC(config, logId, true);
    queryString.deleteFromJDBC(config, logId);
    if (postData != null)
      postData.deleteFromJDBC(config, logId);

    if (dropTables)
    {
      Connection c = config.getConnection();
      String entriesTableName = config.getTablePrefix() + HarEntry.TABLE_NAME;
      String requestTableName = config.getTablePrefix() + TABLE_NAME;

      PreparedStatement ps = null;
      try
      {
        ps = c.prepareStatement("DELETE FROM " + requestTableName
            + " WHERE entry_id IN (SELECT entry_id FROM " + entriesTableName
            + " WHERE log_id=?)");
        ps.setLong(1, logId);
        ps.executeUpdate();
        config.dropTableIfEmpty(c, requestTableName, config);
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
  }

  /**
   * Returns the method value.
   * 
   * @return Returns the method.
   */
  public String getMethod()
  {
    return method;
  }

  /**
   * Sets the method value.
   * 
   * @param method The method to set.
   */
  public void setMethod(String method)
  {
    this.method = method;
  }

  /**
   * Returns the url value.
   * 
   * @return Returns the url.
   */
  public String getUrl()
  {
    return url;
  }

  /**
   * Sets the url value.
   * 
   * @param url The url to set.
   */
  public void setUrl(String url)
  {
    this.url = url;
  }

  /**
   * Returns the httpVersion value.
   * 
   * @return Returns the httpVersion.
   */
  public String getHttpVersion()
  {
    return httpVersion;
  }

  /**
   * Sets the httpVersion value.
   * 
   * @param httpVersion The httpVersion to set.
   */
  public void setHttpVersion(String httpVersion)
  {
    this.httpVersion = httpVersion;
  }

  /**
   * Returns the cookies value.
   * 
   * @return Returns the cookies.
   */
  public HarCookies getCookies()
  {
    return cookies;
  }

  /**
   * Sets the cookies value.
   * 
   * @param cookies The cookies to set.
   */
  public void setCookies(HarCookies cookies)
  {
    this.cookies = cookies;
  }

  /**
   * Returns the headers value.
   * 
   * @return Returns the headers.
   */
  public HarHeaders getHeaders()
  {
    return headers;
  }

  /**
   * Sets the headers value.
   * 
   * @param headers The headers to set.
   */
  public void setHeaders(HarHeaders headers)
  {
    this.headers = headers;
  }

  /**
   * Returns the queryString value.
   * 
   * @return Returns the queryString.
   */
  public HarQueryString getQueryString()
  {
    return queryString;
  }

  /**
   * Sets the queryString value.
   * 
   * @param queryString The queryString to set.
   */
  public void setQueryString(HarQueryString queryString)
  {
    this.queryString = queryString;
  }

  /**
   * Returns the postData value.
   * 
   * @return Returns the postData.
   */
  public HarPostData getPostData()
  {
    return postData;
  }

  /**
   * Sets the postData value.
   * 
   * @param postData The postData to set.
   */
  public void setPostData(HarPostData postData)
  {
    this.postData = postData;
  }

  /**
   * Returns the headersSize value.
   * 
   * @return Returns the headersSize.
   */
  public long getHeadersSize()
  {
    return headersSize;
  }

  /**
   * Sets the headersSize value.
   * 
   * @param headersSize The headersSize to set.
   */
  public void setHeadersSize(long headersSize)
  {
    this.headersSize = headersSize;
  }

  /**
   * Returns the bodySize value.
   * 
   * @return Returns the bodySize.
   */
  public long getBodySize()
  {
    return bodySize;
  }

  /**
   * Sets the bodySize value.
   * 
   * @param bodySize The bodySize to set.
   */
  public void setBodySize(long bodySize)
  {
    this.bodySize = bodySize;
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
    return "\"request\": { \"method\": \"" + method + "\", \"url\": \"" + url
        + "\", \"httpVersion\": \"" + httpVersion + "\", " + cookies + ", "
        + headers + ", " + queryString + ", " + postData
        + ",  \"headersSize\": " + headersSize + ",  \"bodySize\": " + bodySize
        + ",  \"comment\": \"" + comment + "\", " + customFields + "}\n";
  }

}
