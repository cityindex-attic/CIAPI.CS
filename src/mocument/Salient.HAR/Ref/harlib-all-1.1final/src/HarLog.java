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
import java.sql.CallableStatement;
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
 * This class defines a HarLog
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarLog
{
  /**
   * Database table name where the data is stored
   */
  public static String      TABLE_NAME   = "log";

  private String            version      = "1.2";
  private HarCreator        creator;
  private HarBrowser        browser;
  private HarPages          pages;
  private HarEntries        entries;
  private HarCustomFields   customFields = new HarCustomFields();
  private String            comment;

  private HarDatabaseConfig harDbConfig;
  private long              dbLogId;

  /**
   * Creates a new <code>HarLog</code> object
   * 
   * @param version Version number of the format (by default 1.2)
   * @param creator Name and version info of the log creator application
   * @param browser Name and version info of used browser (optional)
   * @param comment A comment provided by the user or the application (optional)
   */
  public HarLog(String version, HarCreator creator, HarBrowser browser,
      String comment)
  {
    this.version = version;
    this.creator = creator;
    this.browser = browser;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarLog</code> version 1.2
   * 
   * @param creator Name and version info of the log creator application
   */
  public HarLog(HarCreator creator)
  {
    this.creator = creator;
  }

  /**
   * Creates a new <code>HarLog</code> object from a JsonParser stream
   * 
   * @param jp the JsonParson which first token must point to the log element
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws IOException
   * @throws JsonParseException
   */
  public HarLog(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    if (jp.nextToken() == JsonToken.END_OBJECT)
      throw new JsonParseException("Empty file", jp.getCurrentLocation());

    if (!"log".equals(jp.getCurrentName()))
      throw new JsonParseException("First element must be \"log\"",
          jp.getCurrentLocation());

    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"log\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("version".equals(name))
        setVersion(jp.getText());
      else if ("creator".equals(name))
        setCreator(new HarCreator(jp,warnings));
      else if ("browser".equals(name))
        setBrowser(new HarBrowser(jp,warnings));
      else if ("pages".equals(name))
        setPages(new HarPages(jp,warnings));
      else if ("entries".equals(name))
        setEntries(new HarEntries(jp,warnings));
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        customFields.addHarCustomFields(name, jp);
      else
        throw new JsonParseException("Unrecognized field '" + name
            + "' in log element", jp.getCurrentLocation());
    }
    if (version == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing version field in log element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing version field in log element",
            jp.getCurrentLocation());
    }
    if (creator == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing create field in log element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing create field in log element",
            jp.getCurrentLocation());
    }
    if (entries == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing entries field in log element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing entries field in log element",
            jp.getCurrentLocation());
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
    g.writeObjectFieldStart("log");
    g.writeStringField("version", version);
    creator.writeHar(g);
    if (browser != null)
      browser.writeHar(g);
    if (pages != null)
      pages.writeHar(g);
    entries.writeHar(g);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Creates a new <code>HarLog</code> object from a database. If the database
   * contains multiple HarLog object, only the first one is retrieved
   * 
   * @param config the database configuration to use
   * @throws SQLException if a database error occurs
   */
  public HarLog(HarDatabaseConfig config) throws SQLException
  {
    readJDBC(config, 1L);
  }

  /**
   * Creates a new <code>HarLog</code> object from a database. Retrieves the
   * HarLog object with the specified id.
   * 
   * @param config the database configuration to use
   * @param logId the HarLog id to read
   * @throws SQLException if a database error occurs
   */
  public HarLog(HarDatabaseConfig config, long logId) throws SQLException
  {
    readJDBC(config, logId);
  }

  /**
   * If this object was read from a database, it will be deleted from it using
   * this method.
   * 
   * @throws SQLException if the object was not retrieved from the database or
   *           an error occurs while deleting it
   */
  public void deleteFromJDBC() throws SQLException
  {
    if (harDbConfig == null)
      throw new SQLException("This object was not retrieved from the database");

    Connection c = null;
    PreparedStatement ps = null;
    String tableName = harDbConfig.getTablePrefix() + TABLE_NAME;
    try
    {
      c = harDbConfig.getConnection();
      ps = c.prepareStatement("DELETE FROM " + tableName + " WHERE id=?");
      ps.setLong(1, dbLogId);
      if (ps.executeUpdate() != 1)
        throw new SQLException("No HarLog with id " + dbLogId
            + " found in database");
      creator.deleteFromJDBC(harDbConfig, dbLogId);
      if (browser != null)
        browser.deleteFromJDBC(harDbConfig, dbLogId);
      if (pages != null)
        pages.deleteFromJDBC(harDbConfig, dbLogId);
      entries.deleteFromJDBC(harDbConfig, dbLogId);
      this.customFields.deleteFromJDBC(harDbConfig, dbLogId);
      harDbConfig.dropTableIfEmpty(c, tableName, harDbConfig);
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
      harDbConfig.closeConnection(c);
    }
  }

  private void readJDBC(HarDatabaseConfig config, long logId)
      throws SQLException
  {
    this.harDbConfig = config;
    this.dbLogId = logId;
    String tableName = config.getTablePrefix() + "log";
    Connection c = null;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      c = config.getConnection();
      ps = c.prepareStatement("SELECT version,comment FROM " + tableName
          + " WHERE id=?");
      ps.setLong(1, logId);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No HarLog with id " + logId
            + " found in database");
      setVersion(rs.getString(1));
      setComment(rs.getString(2));
      creator = new HarCreator(config, logId);
      try
      {
        browser = new HarBrowser(config, logId);
      }
      catch (SQLException ignore)
      { // Optional element
        browser = null;
      }
      try
      {
        pages = new HarPages(config, logId);
      }
      catch (SQLException ignore)
      { // Optional element
        pages = null;
      }
      entries = new HarEntries(config, logId);
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARLOG, logId);
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
   * Write this object to a database according to the given configuration
   * 
   * @param config the database configuration to use
   * @return the id of the HarLog in the database
   * @throws SQLException if a database error occurs
   */
  public long writeJDBC(HarDatabaseConfig config) throws SQLException
  {
    Connection c = config.getConnection();
    c.setAutoCommit(false);
    c.setSavepoint();
    c.setTransactionIsolation(Connection.TRANSACTION_READ_COMMITTED);
    String tableName = config.getTablePrefix() + "log";
    if (!config.isCreatedTable(tableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + tableName + " (id "
            + config.getDbAutoGeneratedId() + ", version "
            + config.getStringDbType() + ",comment " + config.getStringDbType()
            + ")");
        s.close();
        config.addCreatedTable(tableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement ps = null;
    ResultSet rs = null;
    long logId;
    try
    {
      ps = c.prepareStatement("INSERT INTO " + tableName
          + " (version,comment) VALUES (?,?)", Statement.RETURN_GENERATED_KEYS);
      ps.setString(1, version);
      if (comment == null)
        ps.setNull(2, Types.LONGVARCHAR);
      else
        ps.setString(2, comment);
      ps.executeUpdate();
      rs = ps.getGeneratedKeys();
      if (!rs.next())
        throw new SQLException(
            "The database did not generate a key for an HarLog entry");
      logId = rs.getLong(1);
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

      creator.writeJDBC(logId, config);
      if (browser != null)
        browser.writeJDBC(logId, config);
      if (pages != null)
        pages.writeJDBC(logId, config);
      entries.writeJDBC(logId, config);
      this.customFields.writeCustomFieldsJDBC(config,
          HarCustomFields.Type.HARLOG, logId, logId);
      // warning: we assume a single connection c per harlog write.
      c.commit();
      // Restore autocommit
      c.setAutoCommit(true);
      // DB flush on disk
      CallableStatement cs = c
          .prepareCall("CALL SYSCS_UTIL.SYSCS_CHECKPOINT_DATABASE()");
      cs.execute();
      cs.close();
    }
    catch (SQLException e)
    {
      c.rollback();
      throw e;
    }
    finally
    {
      config.closeConnection(c);
    }

    return logId;
  }

  /**
   * Returns the version value.
   * 
   * @return Returns the version.
   */
  public String getVersion()
  {
    return version;
  }

  /**
   * Sets the version value.
   * 
   * @param version The version to set.
   */
  public void setVersion(String version)
  {
    this.version = version;
  }

  /**
   * Returns the creator value.
   * 
   * @return Returns the creator.
   */
  public HarCreator getCreator()
  {
    return creator;
  }

  /**
   * Sets the creator value.
   * 
   * @param creator The creator to set.
   */
  public void setCreator(HarCreator creator)
  {
    this.creator = creator;
  }

  /**
   * Returns the browser value.
   * 
   * @return Returns the browser.
   */
  public HarBrowser getBrowser()
  {
    return browser;
  }

  /**
   * Sets the browser value.
   * 
   * @param browser The browser to set.
   */
  public void setBrowser(HarBrowser browser)
  {
    this.browser = browser;
  }

  /**
   * Returns the pages value.
   * 
   * @return Returns the pages.
   */
  public HarPages getPages()
  {
    return pages;
  }

  /**
   * Sets the pages value.
   * 
   * @param pages The pages to set.
   */
  public void setPages(HarPages pages)
  {
    this.pages = pages;
  }

  /**
   * Returns the entries value.
   * 
   * @return Returns the entries.
   */
  public HarEntries getEntries()
  {
    return entries;
  }

  /**
   * Sets the entries value.
   * 
   * @param entries The entries to set.
   */
  public void setEntries(HarEntries entries)
  {
    this.entries = entries;
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
    StringBuffer sb = new StringBuffer();
    sb.append("{ \"log\": { \"version\": \"" + version + "\", " + creator + ","
        + browser + ", " + pages + ", " + entries + ", " + " \"comment\": "
        + "\"" + comment + "\" }, " + customFields + " }\n");
    return sb.toString();
  }
}
