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
 * This class defines a HarResponse
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarResponse
{
  /**
   * Database table name where the data is stored
   */
  public static String TABLE_NAME  = "response";

  private Integer      status;
  private String       statusText;
  private String       httpVersion;
  private HarCookies   cookies;
  private HarHeaders   headers;
  private HarContent   content;
  private String       redirectURL;
  private Long         headersSize = new Long(-1);
  private Long         bodySize    = new Long(-1);
  private String       comment;

  /**
   * Creates a new <code>HarResponse</code> object
   * 
   * @param status status [number] - Response status.
   * @param statusText statusText [string] - Response status description.
   * @param httpVersion httpVersion [string] - Response HTTP Version.
   * @param cookies cookies [array] - List of cookie objects.
   * @param headers headers [array] - List of header objects.
   * @param content content [object] - Details about the response body.
   * @param redirectURL redirectURL [string] - Redirection target URL from the
   *          Location response header.
   * @param headersSize headersSize [number]* - Total number of bytes from the
   *          start of the HTTP response message until (and including) the
   *          double CRLF before the body. Set to -1 if the info is not
   *          available.
   * @param bodySize bodySize [number] - Size of the received response body in
   *          bytes. Set to zero in case of responses coming from the cache
   *          (304). Set to -1 if the info is not available.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarResponse(int status, String statusText, String httpVersion,
      HarCookies cookies, HarHeaders headers, HarContent content,
      String redirectURL, long headersSize, long bodySize, String comment)
  {
    this.status = status;
    this.statusText = statusText;
    this.httpVersion = httpVersion;
    this.cookies = cookies;
    this.headers = headers;
    this.content = content;
    this.redirectURL = redirectURL;
    this.headersSize = headersSize;
    this.bodySize = bodySize;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarResponse</code> object with all mandatory fields
   * 
   * @param status status [number] - Response status.
   * @param statusText statusText [string] - Response status description.
   * @param httpVersion httpVersion [string] - Response HTTP Version.
   * @param cookies cookies [array] - List of cookie objects.
   * @param headers headers [array] - List of header objects.
   * @param content content [object] - Details about the response body.
   * @param redirectURL redirectURL [string] - Redirection target URL from the
   *          Location response header.
   */
  public HarResponse(int status, String statusText, String httpVersion,
      HarCookies cookies, HarHeaders headers, HarContent content,
      String redirectURL)
  {
    this.status = status;
    this.statusText = statusText;
    this.httpVersion = httpVersion;
    this.cookies = cookies;
    this.headers = headers;
    this.content = content;
    this.redirectURL = redirectURL;
  }

  /**
   * Creates a new <code>HarResponse</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarResponse(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"response\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("status".equals(name))
        setStatus(jp.getValueAsInt());
      else if ("statusText".equals(name))
        setStatusText(jp.getText());
      else if ("httpVersion".equals(name))
        setHttpVersion(jp.getText());
      else if ("cookies".equals(name))
        setCookies(new HarCookies(jp, warnings));
      else if ("headers".equals(name))
        setHeaders(new HarHeaders(jp, warnings));
      else if ("content".equals(name))
        setContent(new HarContent(jp, warnings));
      else if ("redirectURL".equals(name))
        setRedirectURL(jp.getText());
      else if ("headersSize".equals(name))
        setHeadersSize(jp.getValueAsLong());
      else if ("bodySize".equals(name))
        setBodySize(jp.getValueAsLong());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in response element", jp.getCurrentLocation());
      }
    }
    if (status == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing status field in response element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing status field in response element", jp.getCurrentLocation());
    }
    if (statusText == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing statusText field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing statusText field in response element",
            jp.getCurrentLocation());
    }
    if (httpVersion == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing httpVersion field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing httpVersion field in response element",
            jp.getCurrentLocation());
    }
    if (cookies == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing cookies field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing cookies field in response element",
            jp.getCurrentLocation());
    }
    if (headers == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing headers field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing headers field in response element",
            jp.getCurrentLocation());
    }
    if (content == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing content field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing content field in response element",
            jp.getCurrentLocation());
    }
    if (redirectURL == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing redirectURL field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing redirectURL field in response element",
            jp.getCurrentLocation());
    }
    if (headersSize == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing headersSize field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing headersSize field in response element",
            jp.getCurrentLocation());
    }
    if (bodySize == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing bodySize field in response element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing bodySize field in response element",
            jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarResponse</code> object from a database. Retrieves
   * the HarResponse objects that corresponds to the specified entry id.
   * 
   * @param config the database configuration to use
   * @param entryId the entry id to read
   * @throws SQLException if a database error occurs
   */
  public HarResponse(HarDatabaseConfig config, long entryId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT id,status,status_text,http_version,redirect_url,header_size,body_size,comment FROM "
              + tableName + " WHERE entry_id=?");
      ps.setLong(1, entryId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarResponse for entry id " + entryId
            + " found in database");
      long responseId = rs.getLong(1);
      setStatus((int) rs.getLong(2));
      setStatusText(rs.getString(3));
      setHttpVersion(rs.getString(4));
      setRedirectURL(rs.getString(5));
      setHeadersSize(rs.getLong(6));
      setBodySize(rs.getLong(7));
      setComment(rs.getString(8));
      cookies = new HarCookies(config, responseId, false);
      headers = new HarHeaders(config, responseId, false);
      content = new HarContent(config, responseId);
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
    g.writeObjectFieldStart("response");
    g.writeNumberField("status", status);
    g.writeStringField("statusText", statusText);
    g.writeStringField("httpVersion", httpVersion);
    cookies.writeHar(g);
    headers.writeHar(g);
    content.writeHar(g);
    g.writeStringField("redirectURL", redirectURL);
    g.writeNumberField("headersSize", headersSize);
    g.writeNumberField("bodySize", bodySize);
    if (comment != null)
      g.writeStringField("comment", comment);
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
            + config.getDbAutoGeneratedId() + ",status "
            + config.getLongDbType() + ",status_text "
            + config.getStringDbType() + ",http_version "
            + config.getStringDbType() + ",redirect_url "
            + config.getStringDbType() + ",header_size "
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
    long responseId;
    try
    {
      ps = c
          .prepareStatement(
              "INSERT INTO "
                  + tableName
                  + " (status,status_text,http_version,redirect_url,header_size,body_size,comment,entry_id) VALUES (?,?,?,?,?,?,?,?)",
              Statement.RETURN_GENERATED_KEYS);
      ps.setLong(1, status);
      ps.setString(2, statusText);
      ps.setString(3, httpVersion);
      ps.setString(4, redirectURL);
      ps.setLong(5, headersSize);
      ps.setLong(6, bodySize);
      if (comment == null)
        ps.setNull(7, Types.LONGVARCHAR);
      else
        ps.setString(7, comment);
      ps.setLong(8, entryId);
      ps.executeUpdate();
      rs = ps.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarResponse entry");
      responseId = rs.getLong(1);
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
    cookies.writeJDBC(responseId, config, false, logId);
    headers.writeJDBC(responseId, config, false, logId);
    content.writeJDBC(responseId, config, logId);
  }

  /**
   * Delete objects in the database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param config the database configuration
   * @param dropTables true if tables must be dropped
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId,
      boolean dropTables) throws SQLException
  {
    cookies.deleteFromJDBC(config, logId, false);
    headers.deleteFromJDBC(config, logId, false);
    content.deleteFromJDBC(config, logId);

    if (dropTables)
    {
      Connection c = config.getConnection();
      String entriesTableName = config.getTablePrefix() + HarEntry.TABLE_NAME;
      String responseTableName = config.getTablePrefix() + TABLE_NAME;

      PreparedStatement ps = null;
      try
      {
        ps = c.prepareStatement("DELETE FROM " + responseTableName
            + " WHERE entry_id IN (SELECT entry_id FROM " + entriesTableName
            + " WHERE log_id=?)");
        ps.setLong(1, logId);
        ps.executeUpdate();
        config.dropTableIfEmpty(c, responseTableName, config);
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
   * Returns the status value.
   * 
   * @return Returns the status.
   */
  public int getStatus()
  {
    return status;
  }

  /**
   * Sets the status value.
   * 
   * @param status The status to set.
   */
  public void setStatus(int status)
  {
    this.status = status;
  }

  /**
   * Returns the statusText value.
   * 
   * @return Returns the statusText.
   */
  public String getStatusText()
  {
    return statusText;
  }

  /**
   * Sets the statusText value.
   * 
   * @param statusText The statusText to set.
   */
  public void setStatusText(String statusText)
  {
    this.statusText = statusText;
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
   * Returns the content value.
   * 
   * @return Returns the content.
   */
  public HarContent getContent()
  {
    return content;
  }

  /**
   * Sets the content value.
   * 
   * @param content The content to set.
   */
  public void setContent(HarContent content)
  {
    this.content = content;
  }

  /**
   * Returns the redirectURL value.
   * 
   * @return Returns the redirectURL.
   */
  public String getRedirectURL()
  {
    return redirectURL;
  }

  /**
   * Sets the redirectURL value.
   * 
   * @param redirectURL The redirectURL to set.
   */
  public void setRedirectURL(String redirectURL)
  {
    this.redirectURL = redirectURL;
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
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {

    return "\"response\": {  \"status\": \"" + status
        + "\", \"statusText\": \"" + statusText + "\",  \"httpVersion\": \""
        + httpVersion + "\", " + cookies + ", " + headers + ", " + content
        + ", \"redirectURL\": \"" + redirectURL + "\", \"headersSize\": "
        + headersSize + ", \"bodySize\": " + bodySize + ", \"comment\": \""
        + comment + "\"}\n";
  }

}
