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
 * This class defines a HarEntryTimings
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarEntryTimings
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "entry_timings";

  private Long            blocked;
  private Long            dns;
  private Long            connect;
  private Long            send;
  private Long            wait;
  private Long            receive;
  private Long            ssl;
  private String          comment;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarEntryTimings</code> object
   * 
   * @param blocked blocked [number, optional] - Time spent in a queue waiting
   *          for a network connection. Use -1 if the timing does not apply to
   *          the current request.
   * @param dns dns [number, optional] - DNS resolution time. The time required
   *          to resolve a host name. Use -1 if the timing does not apply to the
   *          current request.
   * @param connect connect [number, optional] - Time required to create TCP
   *          connection. Use -1 if the timing does not apply to the current
   *          request.
   * @param send send [number] - Time required to send HTTP request to the
   *          server.
   * @param wait wait [number] - Waiting for a response from the server.
   * @param receive receive [number] - Time required to read entire response
   *          from the server (or cache).
   * @param ssl ssl [number, optional] (new in 1.2) - Time required for SSL/TLS
   *          negotiation. If this field is defined then the time is also
   *          included in the connect field (to ensure backward compatibility
   *          with HAR 1.1). Use -1 if the timing does not apply to the current
   *          request.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarEntryTimings(long blocked, long dns, long connect, long send,
      long wait, long receive, long ssl, String comment)
  {
    this.blocked = blocked;
    this.dns = dns;
    this.connect = connect;
    this.send = send;
    this.wait = wait;
    this.receive = receive;
    this.ssl = ssl;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarEntryTimings</code> object with mandatory fields
   * 
   * @param send send [number] - Time required to send HTTP request to the
   *          server.
   * @param wait wait [number] - Waiting for a response from the server.
   * @param receive receive [number] - Time required to read entire response
   *          from the server (or cache).
   */
  public HarEntryTimings(long send, long wait, long receive)
  {
    this.send = send;
    this.wait = wait;
    this.receive = receive;
  }

  /**
   * Creates a new <code>HarEntryTimings</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarEntryTimings(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"timings\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("blocked".equals(name))
        setBlocked(jp.getValueAsLong());
      else if ("dns".equals(name))
        setDns(jp.getValueAsLong());
      else if ("connect".equals(name))
        setConnect(jp.getValueAsLong());
      else if ("send".equals(name))
        setSend(jp.getValueAsLong());
      else if ("wait".equals(name))
        setWait(jp.getValueAsLong());
      else if ("receive".equals(name))
        setReceive(jp.getValueAsLong());
      else if ("ssl".equals(name))
        setSsl(jp.getValueAsLong());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in timings element", jp.getCurrentLocation());
      }
    }
    if (send == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing send field in timings element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing send field in timings element",
            jp.getCurrentLocation());
    }
    if (wait == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing wait field in timings element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing wait field in timings element",
            jp.getCurrentLocation());
    }
    if (receive == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing receive field in timings element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing receive field in timings element", jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarEntryTimings</code> object from a database.
   * Retrieves the HarEntryTimings objects that corresponds to the specified
   * entry id.
   * 
   * @param config the database configuration to use
   * @param entryId the entry id to read
   * @throws SQLException if a database error occurs
   */
  public HarEntryTimings(HarDatabaseConfig config, long entryId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT id,blocked,dns,conect,send,wait,receive,ssl,comment FROM "
              + tableName + " WHERE entry_id=?");
      ps.setLong(1, entryId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarEntryTimings for entry id " + entryId
            + " found in database");
      long timingId = rs.getLong(1);
      setBlocked(rs.getLong(2));
      if (rs.wasNull())
        blocked = null;
      setDns(rs.getLong(3));
      if (rs.wasNull())
        dns = null;
      setConnect(rs.getLong(4));
      if (rs.wasNull())
        connect = null;
      setSend(rs.getLong(5));
      setWait(rs.getLong(6));
      setReceive(rs.getLong(7));
      setSsl(rs.getLong(8));
      if (rs.wasNull())
        ssl = null;
      setComment(rs.getString(9));
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARENTRYTIMING, timingId);
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
    g.writeObjectFieldStart("timings");
    if (blocked != null)
      g.writeNumberField("blocked", blocked);
    if (dns != null)
      g.writeNumberField("dns", dns);
    if (connect != null)
      g.writeNumberField("connect", connect);
    g.writeNumberField("send", send);
    g.writeNumberField("wait", wait);
    g.writeNumberField("receive", receive);
    if (ssl != null)
      g.writeNumberField("ssl", ssl);
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
      { // Note that conect is misspelled on purpose to avoid database reserved
        // keywords
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + tableName + " (id "
            + config.getDbAutoGeneratedId() + ",blocked "
            + config.getLongDbType() + ",dns " + config.getLongDbType()
            + ",conect " + config.getLongDbType() + ",send "
            + config.getLongDbType() + ",wait " + config.getLongDbType()
            + ",receive " + config.getLongDbType() + ",ssl "
            + config.getLongDbType() + ",comment " + config.getStringDbType()
            + ",entry_id " + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(tableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement ps = null;
    try
    {
      ps = c
          .prepareStatement(
              "INSERT INTO "
                  + tableName
                  + " (blocked,dns,conect,send,wait,receive,ssl,comment,entry_id) VALUES (?,?,?,?,?,?,?,?,?)",
              PreparedStatement.RETURN_GENERATED_KEYS);
      if (blocked == null)
        ps.setNull(1, Types.BIGINT);
      else
        ps.setLong(1, blocked);
      if (dns == null)
        ps.setNull(2, Types.BIGINT);
      else
        ps.setLong(2, dns);
      if (connect == null)
        ps.setNull(3, Types.BIGINT);
      else
        ps.setLong(3, connect);
      ps.setLong(4, send);
      ps.setLong(5, wait);
      ps.setLong(6, receive);
      if (ssl == null)
        ps.setNull(7, Types.BIGINT);
      else
        ps.setLong(7, ssl);
      if (comment == null)
        ps.setNull(8, Types.LONGVARCHAR);
      else
        ps.setString(8, comment);
      ps.setLong(9, entryId);
      ps.executeUpdate();
      ResultSet rs = ps.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarEntry row");
      long timingId = rs.getLong(1);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARENTRYTIMING, timingId, logId);
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
   * @param logId the logId this object refers to
   * @param config the database configuration
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
          + " WHERE entry_id IN (SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?)");
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
   * Returns the blocked value.
   * 
   * @return Returns the blocked.
   */
  public long getBlocked()
  {
    return blocked;
  }

  /**
   * Sets the blocked value.
   * 
   * @param blocked The blocked to set.
   */
  public void setBlocked(long blocked)
  {
    this.blocked = blocked;
  }

  /**
   * Returns the dns value.
   * 
   * @return Returns the dns.
   */
  public long getDns()
  {
    return dns;
  }

  /**
   * Sets the dns value.
   * 
   * @param dns The dns to set.
   */
  public void setDns(long dns)
  {
    this.dns = dns;
  }

  /**
   * Returns the connect value.
   * 
   * @return Returns the connect.
   */
  public long getConnect()
  {
    return connect;
  }

  /**
   * Sets the connect value.
   * 
   * @param connect The connect to set.
   */
  public void setConnect(long connect)
  {
    this.connect = connect;
  }

  /**
   * Returns the send value.
   * 
   * @return Returns the send.
   */
  public long getSend()
  {
    return send;
  }

  /**
   * Sets the send value.
   * 
   * @param send The send to set.
   */
  public void setSend(long send)
  {
    this.send = send;
  }

  /**
   * Returns the wait value.
   * 
   * @return Returns the wait.
   */
  public long getWait()
  {
    return wait;
  }

  /**
   * Sets the wait value.
   * 
   * @param wait The wait to set.
   */
  public void setWait(long wait)
  {
    this.wait = wait;
  }

  /**
   * Returns the receive value.
   * 
   * @return Returns the receive.
   */
  public long getReceive()
  {
    return receive;
  }

  /**
   * Sets the receive value.
   * 
   * @param receive The receive to set.
   */
  public void setReceive(long receive)
  {
    this.receive = receive;
  }

  /**
   * Returns the ssl value.
   * 
   * @return Returns the ssl.
   */
  public long getSsl()
  {
    return ssl;
  }

  /**
   * Sets the ssl value.
   * 
   * @param ssl The ssl to set.
   */
  public void setSsl(long ssl)
  {
    this.ssl = ssl;
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
    return "\"timings\": { \"blocked\": " + blocked + ", \"dns\": " + dns
        + ", \"connect\": " + connect + ", \"send\": " + send + ", \"wait\": "
        + wait + ", \"receive\": " + receive + ", \"ssl\": " + ssl
        + ", \"comment\": " + "\"" + comment + "\", " + customFields + "}\n";
  }

}
