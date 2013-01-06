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
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarCacheRequest
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarCacheRequest
{
  /**
   * Database table name where the data is stored
   */
  public static String           TABLE_NAME   = "cache";

  private Date                   expires;
  private Date                   lastAccess;
  private String                 eTag;
  private Integer                hitCount;
  private String                 comment;

  private final SimpleDateFormat SDF          = new SimpleDateFormat(
                                                  "yyyy-MM-dd'T'HH:mm:ss.S");
  private boolean                isBefore;
  private HarCustomFields        customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarCacheRequest</code> object
   * 
   * @param expires expires [string, optional] - Expiration time of the cache
   *          entry.
   * @param lastAccess lastAccess [string] - The last time the cache entry was
   *          opened.
   * @param tag eTag [string] - Etag
   * @param hitCount hitCount [number] - The number of times the cache entry has
   *          been opened.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   * @param isBefore true if this is a beforeCache entry, false for an
   *          afterCache
   */
  public HarCacheRequest(Date expires, Date lastAccess, String tag,
      int hitCount, String comment, boolean isBefore)
  {
    this.expires = expires;
    this.lastAccess = lastAccess;
    eTag = tag;
    this.hitCount = hitCount;
    this.comment = comment;
    this.isBefore = isBefore;
  }

  /**
   * Creates a new <code>HarCacheRequest</code> object
   * 
   * @param lastAccess
   * @param tag
   * @param hitCount
   * @param isBefore true if this is a beforeCache entry, false for an
   *          afterCache
   */
  public HarCacheRequest(Date lastAccess, String tag, int hitCount,
      boolean isBefore)
  {
    this.lastAccess = lastAccess;
    eTag = tag;
    this.hitCount = hitCount;
    this.isBefore = isBefore;
  }

  /**
   * Creates a new <code>HarCacheRequest</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @param isBefore true if this is a beforeCache entry, false for an
   *          afterCache
   * @throws JsonParseException
   * @throws IOException
   */
  public HarCacheRequest(JsonParser jp, boolean isBefore,
      List<HarWarning> warnings) throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      if ("null".equals(jp.getText()))
        return;
      throw new JsonParseException("{ missing after \"" + getObjectTag()
          + "\" element", jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("eTag".equals(name))
        setETag(jp.getText());
      else if ("expires".equals(name))
      {
        try
        {
          if (jp.getCurrentName().equals(jp.getText()))
            jp.nextToken();
          setExpires(ISO8601DateFormatter.parseDate(jp.getText()));
        }
        catch (ParseException e)
        {
          // Fallback try 1.2 spec without timezone
          try
          {
            setExpires(SDF.parse(jp.getText()));
          }
          catch (ParseException e1)
          {
            if (warnings != null)
              warnings.add(new HarWarning("Invalid date format '"
                  + jp.getText() + "'", jp.getCurrentLocation()));
            else
              throw new JsonParseException("Invalid date format '"
                  + jp.getText() + "'", jp.getCurrentLocation());
          }
        }
      }
      else if ("lastAccess".equals(name))
      {
        try
        {
          if (jp.getCurrentName().equals(jp.getText()))
            jp.nextToken();
          setLastAccess(ISO8601DateFormatter.parseDate(jp.getText()));
        }
        catch (ParseException e)
        {
          // Fallback try 1.2 spec without timezone
          try
          {
            setExpires(SDF.parse(jp.getText()));
          }
          catch (ParseException e1)
          {
            if (warnings != null)
              warnings.add(new HarWarning("Invalid date format '"
                  + jp.getText() + "'", jp.getCurrentLocation()));
            else
              throw new JsonParseException("Invalid date format '"
                  + jp.getText() + "'", jp.getCurrentLocation());
          }
        }
      }
      else if ("hitCount".equals(name))
        setHitCount(jp.getValueAsInt());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name + "' in "
            + getObjectTag() + " element", jp.getCurrentLocation());
      }
    }
    if (lastAccess == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing lastAccess field in cache element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing lastAccess field in cache element",
            jp.getCurrentLocation());
    }
    if (eTag == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing eTag field in cache element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing eTag field in cache element",
            jp.getCurrentLocation());
    }
    if (hitCount == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing hitCount field in cache element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException("Missing hitCount field in cache element",
            jp.getCurrentLocation());
    }
  }

  /**
   * Creates a new <code>HarCacheRequest</code> object from a database.
   * Retrieves the HarCacheRequest objects that corresponds to the specified
   * cache id.
   * 
   * @param config the database configuration to use
   * @param cacheId the cache id that we refer to
   * @param isBefore true if this is a beforeCache entry, false for an
   *          afterCache
   * @throws SQLException if a database error occurs
   */
  public HarCacheRequest(HarDatabaseConfig config, long cacheId,
      boolean isBefore) throws SQLException
  {
    this.isBefore = isBefore;
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT expires,last_access,etag,hit_count,comment FROM "
              + tableName + " WHERE cache_id=? AND is_before=?");
      ps.setLong(1, cacheId);
      ps.setInt(2, isBefore ? 1 : 0);
      rs = ps.executeQuery();
      if (!rs.next())
        throw new SQLException("No " + getObjectTag()
            + " HarCacheRequest for cache id " + cacheId + " found in database");
      setExpires(rs.getTimestamp(1));
      setLastAccess(rs.getTimestamp(2));
      setETag(rs.getString(3));
      setHitCount((int) rs.getLong(4));
      setComment(rs.getString(5));
      this.customFields.readCustomFieldsJDBC(config,
          HarCustomFields.Type.HARCACHEREQUEST, cacheId);
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
    if (eTag == null)
    { // If mandatory field is null, it was a null object
      g.writeNullField(getObjectTag());
      return;
    }

    g.writeObjectFieldStart(getObjectTag());
    if (expires != null)
      g.writeStringField("expires", ISO8601DateFormatter.format(expires));
    g.writeStringField("lastAccess", ISO8601DateFormatter.format(lastAccess));
    g.writeStringField("eTag", eTag);
    g.writeNumberField("hitCount", hitCount);
    if (comment != null)
      g.writeStringField("comment", comment);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * TODO: getObjectTag definition.
   * 
   * @return
   */
  private String getObjectTag()
  {
    return isBefore ? "beforeRequest" : "afterRequest";
  }

  /**
   * Write this object to a database according to the given configuration
   * 
   * @param cacheId the cache id this object refers to
   * @param cachePs PreparedStatement to write data to
   * @throws SQLException if a database error occurs
   */
  public void writeJDBC(long cacheId, HarDatabaseConfig config,
      PreparedStatement cachePs, long logId) throws SQLException
  {
    if (eTag == null)
    { // If mandatory field is null, it was a null object
      cachePs.setNull(1, Types.TIMESTAMP);
      cachePs.setNull(2, Types.TIMESTAMP);
      cachePs.setNull(3, Types.LONGVARCHAR);
      cachePs.setNull(4, Types.BIGINT);
    }
    else
    {
      cachePs.setTimestamp(1, new Timestamp(expires.getTime()));
      cachePs.setTimestamp(2, new Timestamp(lastAccess.getTime()));
      cachePs.setString(3, eTag);
      cachePs.setLong(4, hitCount);
    }
    if (comment == null)
      cachePs.setNull(5, Types.LONGVARCHAR);
    else
      cachePs.setString(5, comment);
    cachePs.setInt(6, isBefore ? 1 : 0);
    cachePs.setLong(7, cacheId);
    cachePs.executeUpdate();

    this.customFields.writeCustomFieldsJDBC(config,
        HarCustomFields.Type.HARCACHEREQUEST, cacheId, logId);
  }

  /**
   * Returns the expires value.
   * 
   * @return Returns the expires.
   */
  public Date getExpires()
  {
    return expires;
  }

  /**
   * Sets the expires value.
   * 
   * @param expires The expires to set.
   */
  public void setExpires(Date expires)
  {
    this.expires = expires;
  }

  /**
   * Returns the lastAccess value.
   * 
   * @return Returns the lastAccess.
   */
  public Date getLastAccess()
  {
    return lastAccess;
  }

  /**
   * Sets the lastAccess value.
   * 
   * @param lastAccess The lastAccess to set.
   */
  public void setLastAccess(Date lastAccess)
  {
    this.lastAccess = lastAccess;
  }

  /**
   * Returns the eTag value.
   * 
   * @return Returns the eTag.
   */
  public String getETag()
  {
    return eTag;
  }

  /**
   * Sets the eTag value.
   * 
   * @param tag The eTag to set.
   */
  public void setETag(String tag)
  {
    eTag = tag;
  }

  /**
   * Returns the hitCount value.
   * 
   * @return Returns the hitCount.
   */
  public int getHitCount()
  {
    return hitCount;
  }

  /**
   * Sets the hitCount value.
   * 
   * @param hitCount The hitCount to set.
   */
  public void setHitCount(int hitCount)
  {
    this.hitCount = hitCount;
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
    return "\"" + getObjectTag() + "\":{ \"expires\": \""
        + ISO8601DateFormatter.format(expires) + "\",\"lastAccess\": \""
        + ISO8601DateFormatter.format(lastAccess) + "\",\"eTag\": \"" + eTag
        + "\",\"hitCount\": " + hitCount + ",\"comment\": " + "\"" + comment
        + "\" } }\n";
  }

}
