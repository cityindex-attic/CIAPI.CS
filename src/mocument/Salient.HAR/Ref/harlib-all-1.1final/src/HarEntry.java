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
import java.sql.Timestamp;
import java.sql.Types;
import java.text.ParseException;
import java.util.Date;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarEntry
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarEntry implements Comparable<HarEntry>
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "entries";

  private String          pageRef;
  private Date            startedDateTime;
  private Long            time;
  private HarRequest      request;
  private HarResponse     response;
  private HarCache        cache;
  private HarEntryTimings timings;
  private String          serverIPAddress;
  private String          connection;
  private String          comment;

  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarEntry</code> object
   * 
   * @param startedDateTime startedDateTime [string] - Date and time stamp of
   *          the request start (ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD).
   * @param time time [number] - Total elapsed time of the request in
   *          milliseconds. This is the sum of all timings available in the
   *          timings object (i.e. not including -1 values) .
   * @param request request [object] - Detailed info about the request.
   * @param response response [object] - Detailed info about the response.
   * @param cache cache [object] - Info about cache usage.
   * @param timings timings [object] - Detailed timing info about
   *          request/response round trip.
   */
  public HarEntry(Date startedDateTime, long time, HarRequest request,
      HarResponse response, HarCache cache, HarEntryTimings timings)
  {
    this.startedDateTime = startedDateTime;
    this.time = time;
    this.request = request;
    this.response = response;
    this.cache = cache;
    this.timings = timings;
  }

  /**
   * Creates a new <code>HarEntry</code> object
   * 
   * @param pageRef pageref [string, unique, optional] - Reference to the parent
   *          page. Leave out this field if the application does not support
   *          grouping by pages.
   * @param startedDateTime startedDateTime [string] - Date and time stamp of
   *          the request start (ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD).
   * @param time time [number] - Total elapsed time of the request in
   *          milliseconds. This is the sum of all timings available in the
   *          timings object (i.e. not including -1 values) .
   * @param request request [object] - Detailed info about the request.
   * @param response response [object] - Detailed info about the response.
   * @param cache cache [object] - Info about cache usage.
   * @param timings timings [object] - Detailed timing info about
   *          request/response round trip.
   * @param serverIPAddress serverIPAddress [string, optional] (new in 1.2) - IP
   *          address of the server that was connected (result of DNS
   *          resolution).
   * @param connection connection [string, optional] (new in 1.2) - Unique ID of
   *          the parent TCP/IP connection, can the client port number. Note
   *          that a port number doesn't have to be unique identifier in cases
   *          where the port is shared for more connections. If the port isn't
   *          available for the application, any other unique connection ID can
   *          be used instead (e.g. connection index). Leave out this field if
   *          the application doesn't support this info.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarEntry(String pageRef, Date startedDateTime, long time,
      HarRequest request, HarResponse response, HarCache cache,
      HarEntryTimings timings, String serverIPAddress, String connection,
      String comment)
  {
    this.pageRef = pageRef;
    this.startedDateTime = startedDateTime;
    this.time = time;
    this.request = request;
    this.response = response;
    this.cache = cache;
    this.timings = timings;
    this.serverIPAddress = serverIPAddress;
    this.connection = connection;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarEntry</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarEntry(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.getCurrentToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"entries\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("pageref".equals(name))
        setPageRef(jp.getText());
      else if ("startedDateTime".equals(name))
      {
        try
        {
          if (jp.getCurrentName().equals(jp.getText()))
            jp.nextToken();
          setStartedDateTime(ISO8601DateFormatter.parseDate(jp.getText()));
        }
        catch (ParseException e)
        {
          throw new JsonParseException("Invalid date format '" + jp.getText()
              + "'", jp.getCurrentLocation());
        }
      }
      else if ("time".equals(name))
        setTime(jp.getValueAsLong());
      else if ("request".equals(name))
        setRequest(new HarRequest(jp, warnings));
      else if ("response".equals(name))
        setResponse(new HarResponse(jp, warnings));
      else if ("cache".equals(name))
        setCache(new HarCache(jp, warnings));
      else if ("timings".equals(name))
        setTimings(new HarEntryTimings(jp, warnings));
      else if ("serverIPAddress".equals(name))
        setServerIPAddress(jp.getText());
      else if ("connection".equals(name))
        setConnection(jp.getText());
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
    if (startedDateTime == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing startedDateTime field in entries element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing startedDateTime field in entries element",
            jp.getCurrentLocation());
    }
    if (time == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing time field in entries element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing time field in entries element",
            jp.getCurrentLocation());
    }
    if (request == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing request field in entries element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing request field in entries element", jp.getCurrentLocation());
    }
    if (response == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing response field in entries element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing response field in entries element",
            jp.getCurrentLocation());
    }
    if (cache == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing cache field in entries element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException("Missing cache field in entries element",
            jp.getCurrentLocation());
    }
    if (timings == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing timings field in entries element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing timings field in entries element", jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarEntry</code> object from a database. Retrieves the
   * HarEntry objects that corresponds to the specified page id.
   * 
   * @param config the database configuration to use
   * @param entryId the entry id to read
   * @throws SQLException if a database error occurs
   */
  public HarEntry(HarDatabaseConfig config, long entryId) throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + "entries";
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT page_ref,start_date,time,server_ip,connexion,comment FROM "
              + tableName + " WHERE entry_id=?");
      ps.setLong(1, entryId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarEntry for page id " + entryId
            + " found in database");
      setPageRef(rs.getString(1));
      setStartedDateTime(rs.getTimestamp(2));
      setTime(rs.getLong(3));
      setServerIPAddress(rs.getString(4));
      setConnection(rs.getString(5));
      setComment(rs.getString(6));
      request = new HarRequest(config, entryId);
      response = new HarResponse(config, entryId);
      try
      {
        cache = new HarCache(config, entryId);
      }
      catch (SQLException e)
      { // Cache is empty
        cache = new HarCache();
      }
      timings = new HarEntryTimings(config, entryId);
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARENTRY, entryId);
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
    g.writeStartObject();
    if (pageRef != null)
      g.writeStringField("pageref", pageRef);
    g.writeStringField("startedDateTime",
        ISO8601DateFormatter.format(startedDateTime));
    g.writeNumberField("time", time);
    request.writeHar(g);
    response.writeHar(g);
    if (cache != null)
      cache.writeHar(g);
    timings.writeHar(g);
    if (serverIPAddress != null)
      g.writeStringField("serverIPAddress", serverIPAddress);
    if (connection != null)
      g.writeStringField("connection", connection);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param entryPs PreparedStatement to write entry data
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long logId, PreparedStatement entryPs,
      HarDatabaseConfig config) throws SQLException
  {
    ResultSet rs = null;
    try
    {
      entryPs.setString(1, pageRef);
      entryPs.setTimestamp(2, new Timestamp(startedDateTime.getTime()));
      entryPs.setLong(3, time);
      entryPs.setString(4, serverIPAddress);
      entryPs.setString(5, connection);
      if (comment == null)
        entryPs.setNull(6, Types.LONGVARCHAR);
      else
        entryPs.setString(6, comment);
      entryPs.setLong(7, logId);
      entryPs.executeUpdate();
      rs = entryPs.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarEntry row");
      long entryId = rs.getLong(1);
      request.writeJDBC(entryId, config, logId);
      response.writeJDBC(entryId, config, logId);
      if (cache != null)
        cache.writeJDBC(entryId, config, logId);
      timings.writeJDBC(entryId, config, logId);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARENTRY, entryId, logId);
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
    }
  }

  /**
   * Delete entries objects in the database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param config the database configuration
   * @param dropTables true if tables must be dropped
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId,
      boolean dropTables) throws SQLException
  {
    request.deleteFromJDBC(config, logId, dropTables);
    response.deleteFromJDBC(config, logId, dropTables);
    if (dropTables)
      return;
    if (cache != null)
      cache.deleteFromJDBC(config, logId);
    timings.deleteFromJDBC(config, logId);
  }

  /**
   * Returns the pageRef value.
   * 
   * @return Returns the pageRef.
   */
  public String getPageRef()
  {
    return pageRef;
  }

  /**
   * Sets the pageRef value.
   * 
   * @param pageRef The pageRef to set.
   */
  public void setPageRef(String pageRef)
  {
    this.pageRef = pageRef;
  }

  /**
   * Returns the startedDateTime value.
   * 
   * @return Returns the startedDateTime.
   */
  public Date getStartedDateTime()
  {
    return startedDateTime;
  }

  /**
   * Sets the startedDateTime value.
   * 
   * @param startedDateTime The startedDateTime to set.
   */
  public void setStartedDateTime(Date startedDateTime)
  {
    this.startedDateTime = startedDateTime;
  }

  /**
   * Returns the time value.
   * 
   * @return Returns the time.
   */
  public long getTime()
  {
    return time;
  }

  /**
   * Sets the time value.
   * 
   * @param time The time to set.
   */
  public void setTime(long time)
  {
    this.time = time;
  }

  /**
   * Returns the request value.
   * 
   * @return Returns the request.
   */
  public HarRequest getRequest()
  {
    return request;
  }

  /**
   * Sets the request value.
   * 
   * @param request The request to set.
   */
  public void setRequest(HarRequest request)
  {
    this.request = request;
  }

  /**
   * Returns the response value.
   * 
   * @return Returns the response.
   */
  public HarResponse getResponse()
  {
    return response;
  }

  /**
   * Sets the response value.
   * 
   * @param response The response to set.
   */
  public void setResponse(HarResponse response)
  {
    this.response = response;
  }

  /**
   * Returns the cache value.
   * 
   * @return Returns the cache.
   */
  public HarCache getCache()
  {
    return cache;
  }

  /**
   * Sets the cache value.
   * 
   * @param cache The cache to set.
   */
  public void setCache(HarCache cache)
  {
    this.cache = cache;
  }

  /**
   * Returns the timings value.
   * 
   * @return Returns the timings.
   */
  public HarEntryTimings getTimings()
  {
    return timings;
  }

  /**
   * Sets the timings value.
   * 
   * @param timings The timings to set.
   */
  public void setTimings(HarEntryTimings timings)
  {
    this.timings = timings;
  }

  /**
   * Returns the serverIPAddress value.
   * 
   * @return Returns the serverIPAddress.
   */
  public String getServerIPAddress()
  {
    return serverIPAddress;
  }

  /**
   * Sets the serverIPAddress value.
   * 
   * @param serverIPAddress The serverIPAddress to set.
   */
  public void setServerIPAddress(String serverIPAddress)
  {
    this.serverIPAddress = serverIPAddress;
  }

  /**
   * Returns the connection value.
   * 
   * @return Returns the connection.
   */
  public String getConnection()
  {
    return connection;
  }

  /**
   * Sets the connection value.
   * 
   * @param connection The connection to set.
   */
  public void setConnection(String connection)
  {
    this.connection = connection;
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
    return "{ \"pageref\": \"" + pageRef + "\",  \"startedDateTime\": \""
        + ISO8601DateFormatter.format(startedDateTime) + "\", \"time\": \""
        + time + "\", " + request + ", " + response + ", " + cache + ", "
        + timings + ", \"serverIPAddress\": \"" + serverIPAddress
        + "\", \"connection\": \"" + connection + "\", \"comment\": \""
        + comment + "\", " + customFields + "}\n";
  }

  /**
   * @see java.lang.Comparable#compareTo(java.lang.Object)
   */
  @Override
  public int compareTo(HarEntry o)
  {
    return startedDateTime.compareTo(o.getStartedDateTime());
  }

}
