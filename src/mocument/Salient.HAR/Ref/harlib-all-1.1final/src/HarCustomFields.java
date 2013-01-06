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
 * Initial developer(s): Fabien Mottet.
 * Contributor(s): Gregory Bobak.
 */

package edu.umass.cs.benchlab.har;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Set;

import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

/**
 * This class defines a HAR custom fields. From the specifications, each HAR
 * file can contain custom fields. A custom field is made of a (name, value)
 * string pair. Manipulating HarCustomFields in an object is done this way: <br>
 * 
 * <pre>
 * {@code
 * HarLog hl = new HarLog(config) ;
 * //Add or set :
 * hl.getCustomFields().addCustomField("name", "value");
 * //Get :
 * String value = hl.getCustomFields().getCustomFieldValue("name");
 * }
 * </pre>
 * 
 * @author Fabien Mottet
 * @version 1.0
 */
public class HarCustomFields
{

  /**
   * Defines constants for HarCustomFields database entry
   */
  public enum Type
  {
    HARLOG, HARPAGE, HARENTRY, HARREQUEST, HARCONTENT, HARCOOKIE, HARBROWSER, HARCACHE, HARCACHEREQUEST, HARENTRYTIMING, HARPAGETIMING, HARCREATOR, HARPOSTDATA, HARPOSTDATAPARAM, HARHEADER, HARQUERYSTRING
  }

  /**
   * Database table name used to store custom fields
   */
  public static String        TABLE_NAME = "customfields";
  private Map<String, String> customFields;

  /**
   * Creates a new <code>HarCustomFields</code> object
   */
  public HarCustomFields()
  {
    this.customFields = new HashMap<String, String>();
  }

  /**
   * Returns the customFields map.
   * 
   * @return Returns the customFields.
   */
  protected Map<String, String> getCustomFields()
  {
    return customFields;
  }

  /**
   * Sets the customFields value.
   * 
   * @param customFields The customFields to set.
   */
  protected void setCustomFields(Map<String, String> customFields)
  {
    this.customFields = customFields;
  }

  /**
   * Adds a new customField to this object
   * 
   * @param name the name of the custom field. It must start with an underscore
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @throws IOException
   */
  public void addHarCustomFields(String name, JsonParser jp) throws IOException
  {
    // TODO: handle multiple level custom fields
    JsonToken jt = jp.nextToken();
    if (jt == JsonToken.START_OBJECT) // complex custom object
    {

      jt = jp.nextToken();
      while (jt != JsonToken.END_OBJECT)
      {
        String newName = jp.getCurrentName();
        if (newName.startsWith("_"))
        {
          String childName = name + newName;
          this.addHarCustomFields(childName, jp);
        }
        else if (jt == JsonToken.FIELD_NAME)
        {
          String childName = name + "_" + jp.getCurrentName();
          this.addHarCustomFields(childName, jp);
        }
        jt = jp.nextToken();
      }
    }
    else
    {
      String value = jp.getText();
      customFields.put(name, value);
    }
  }

  /**
   * Writes this object on a JsonGenerator stream
   * 
   * @param g a JsonGenerator
   * @throws IOException
   */
  public void writeHar(JsonGenerator g) throws IOException
  {
    Set<Map.Entry<String, String>> s = customFields.entrySet();
    for (Entry<String, String> entry : s)
    {
      g.writeStringField(entry.getKey(), entry.getValue());
    }
  }

  /**
   * Writes this object in the given database referencing the specified logId.
   * 
   * @param config the database configuration
   * @param HarTypeFrom The type of HAR object
   * @param HarIdFrom The id of the HAR object associated to the customFields
   * @param HarLogId The Id of the parent log
   * @throws SQLException if a database access error occurs
   */
  public long writeCustomFieldsJDBC(HarDatabaseConfig config,
      HarCustomFields.Type HarTypeFrom, long HarIdFrom, long HarLogId)
      throws SQLException
  {
    // TODO
    Connection c = config.getConnection();
    // create table if it does not exit
    String tableName = config.getTablePrefix() + TABLE_NAME;
    if (!config.isCreatedTable(tableName))
    {
      try
      {
        Statement s = c.createStatement();
        s.executeUpdate("CREATE TABLE " + tableName + " (id "
            + config.getDbAutoGeneratedId() + ", HarTypeFrom INTEGER"
            + ", HarIdFrom BIGINT" + ", customName " + config.getStringDbType()
            + ", value " + config.getStringDbType() + ",log_id "
            + config.getLongDbType() + ")");
        s.close();
        config.addCreatedTable(tableName);
      }
      catch (Exception ignore)
      { // Database table probably already
        // exists
      }
    }
    long nbInserted = 0;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement(
              "INSERT INTO "
                  + tableName
                  + " (HarTypeFrom,HarIdFrom,customName,value, log_id) VALUES (?,?,?,?,?)",
              Statement.RETURN_GENERATED_KEYS);

      // Insert whole Map
      Set<Map.Entry<String, String>> s = this.customFields.entrySet();
      for (Entry<String, String> entry : s)
      {
        ps.setLong(1, HarTypeFrom.ordinal());
        ps.setLong(2, HarIdFrom);
        ps.setString(3, entry.getKey());
        ps.setString(4, entry.getValue());
        ps.setLong(5, HarLogId);
        ps.executeUpdate();
        nbInserted++;
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

    return nbInserted;
  }

  /**
   * Reads the customFields from database
   * 
   * @param config the database configuration
   * @param harTypeFrom The type of the HAR object associated to the
   *          customFields
   * @param harIdFrom The id of the HAR object associated to the customFields
   * @throws SQLException
   */
  public void readCustomFieldsJDBC(HarDatabaseConfig config,
      HarCustomFields.Type harTypeFrom, long harIdFrom) throws SQLException
  {
    String tableName = config.getTablePrefix() + TABLE_NAME;
    Connection c = null;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      c = config.getConnection();
      ps = c.prepareStatement("SELECT customName,value FROM " + tableName
          + " WHERE HarTypeFrom=? AND HarIdFrom=?");
      ps.setLong(1, harTypeFrom.ordinal());
      ps.setLong(2, harIdFrom);
      rs = ps.executeQuery();
      while (rs.next())
      {
        String key = rs.getString(1);
        String value = rs.getString(2);
        this.customFields.put(key, value);
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
   * Delete all custom fields objects in the database referencing the specified
   * logId.
   * 
   * @param config the database configuration
   * @param dbLogId the Id of the parent log
   * @throws SQLException
   */
  public void deleteFromJDBC(HarDatabaseConfig config, long dbLogId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;

    PreparedStatement ps = null;
    try
    {
      // Delete timings first
      ps = c.prepareStatement("DELETE FROM " + tableName + " WHERE log_id = ?");
      ps.setLong(1, dbLogId);
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
   * Adds a custom field to HarCustomFields object
   * 
   * @param name Name for custom field
   * @param value value to set for the custom field
   */
  public void addCustomField(String name, String value)
  {
    this.customFields.put(name, value);
  }

  /**
   * Gets custom field value by name
   * 
   * @param name Name of custom field to get
   * @return the associated value or null if not present
   */
  public String getCustomFieldValue(String name)
  {
    return this.customFields.get(name);
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    Set<Map.Entry<String, String>> s = this.customFields.entrySet();

    StringBuilder builder = new StringBuilder();
    builder.append("");
    Iterator<Entry<String, String>> i = s.iterator();
    while (i.hasNext())
    {
      Entry<String, String> entry = i.next();
      builder.append("\"" + entry.getKey() + "\" : \"" + entry.getValue()
          + "\"");
      if (!i.hasNext())
      {
        break;
      }
      builder.append(", ");
    }
    return builder.toString();
  }

}
