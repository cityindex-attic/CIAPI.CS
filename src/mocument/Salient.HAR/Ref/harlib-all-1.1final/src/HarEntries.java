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
 * Contributor(s): ______________________.
 */

package edu.umass.cs.benchlab.har;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarEntrys
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarEntries
{
  private List<HarEntry> entries;

  /**
   * Creates a new <code>HarEntrys</code> object
   */
  public HarEntries()
  {
    entries = new ArrayList<HarEntry>();
  }

  /**
   * Creates a new <code>HarEntries</code> objectfrom a JsonParser already
   * positioned at the beginning of the element content. Note that entries are
   * sorted automatically at the end of the parsing.
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarEntries(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    entries = new ArrayList<HarEntry>();

    // Read the content of the entries element
    if (jp.nextToken() != JsonToken.START_ARRAY)
    {
      throw new JsonParseException("[ missing after \"entries\" element "
          + jp.getCurrentName(), jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_ARRAY)
    {
      addEntry(new HarEntry(jp, warnings));
    }

    // don't want to sort for now...
    // Collections.sort(entries);
  }

  /**
   * Creates a new <code>HarEntries</code> object from a database. Retrieves the
   * HarEntry objects that corresponds to the HarLog object with the specified
   * id.
   * 
   * @param config the database configuration to use
   * @param logId the HarLog id to read
   * @throws SQLException if a database error occurs
   */
  public HarEntries(HarDatabaseConfig config, long logId) throws SQLException
  {
    entries = new ArrayList<HarEntry>();
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + "entries";
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c.prepareStatement("SELECT entry_id FROM " + tableName
          + " WHERE log_id=?");
      ps.setLong(1, logId);
      rs = ps.executeQuery();
      // We use the same connection after this, bufferizes entries logid.
      List<Long> l = new ArrayList<Long>();
      while (rs.next())
        l.add(rs.getLong(1));

      for (Iterator<Long> iterator = l.iterator(); iterator.hasNext();)
      {
        Long long1 = iterator.next();
        addEntry(new HarEntry(config, long1));
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
    g.writeArrayFieldStart("entries");
    for (HarEntry entry : entries)
      entry.writeHar(g);
    g.writeEndArray();
  }

  /**
   * Write this object in the given database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(long logId, HarDatabaseConfig config)
      throws SQLException
  {
    Connection c = config.getConnection();
    String entriesTableName = config.getTablePrefix() + HarEntry.TABLE_NAME;
    if (!config.isCreatedTable(entriesTableName))
    {
      try
      { // Check if entries table exists
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + entriesTableName + " (entry_id "
            + config.getDbAutoGeneratedId() + ",page_ref "
            + config.getStringDbType() + ", start_date "
            + config.getTimestampDbType() + ",time " + config.getLongDbType()
            + ",server_ip " + config.getStringDbType() + ",connexion "
            + config.getStringDbType() + ",comment " + config.getStringDbType()
            + ",log_id " + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(entriesTableName);
      }
      catch (Exception ignore)
      { // Database table probably already exists
      }
    }
    PreparedStatement entryPs = c
        .prepareStatement(
            "INSERT INTO "
                + entriesTableName
                + " (page_ref,start_date,time,server_ip,connexion,comment,log_id) VALUES (?,?,?,?,?,?,?)",
            Statement.RETURN_GENERATED_KEYS);
    try
    {
      for (HarEntry entry : entries)
        entry.writeJDBC(logId, entryPs, config);
    }
    finally
    {
      try
      {
        if (entryPs != null)
          entryPs.close();
      }
      catch (Exception ignore)
      {
      }
      config.closeConnection(c);
    }
  }

  /**
   * Delete entries objects in the database referencing the specified logId.
   * 
   * @param logId the logId this object refers to
   * @param config the database configuration
   * @throws SQLException if a database access error occurs
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long logId)
      throws SQLException
  {
    // First delete the content
    for (HarEntry entry : entries)
      entry.deleteFromJDBC(config, logId, false);

    // Now drop the tables
    for (HarEntry entry : entries)
    {
      entry.deleteFromJDBC(config, logId, true);
      break;
    }

    config.deleteFromTable(logId, config, "entries");
  }

  /**
   * Add a new entry to the list
   * 
   * @param entry the entry to add
   */
  public void addEntry(HarEntry entry)
  {
    entries.add(entry);
  }

  /**
   * Remove an entry from the list
   * 
   * @param entry the entry to remove
   */
  public void removeEntry(HarEntry entry)
  {
    entries.remove(entry);
  }

  /**
   * Returns the entries value.
   * 
   * @return Returns the entries.
   */
  public List<HarEntry> getEntries()
  {
    return entries;
  }

  /**
   * Sets the entries value.
   * 
   * @param entries The entries to set.
   */
  public void setEntries(List<HarEntry> entries)
  {
    this.entries = entries;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    StringBuffer sb = new StringBuffer("  \"entries\": [");
    if (entries != null)
    {
      boolean first = true;
      for (HarEntry entry : entries)
      {
        if (first)
          first = false;
        else
          sb.append(", ");
        sb.append(entry);
      }
    }
    sb.append("]");
    return sb.toString();
  }

}
