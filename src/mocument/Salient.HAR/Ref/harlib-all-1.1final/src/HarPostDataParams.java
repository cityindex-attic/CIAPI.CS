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
import java.util.ArrayList;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarPostDataParams
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarPostDataParams
{
  private List<HarPostDataParam> postDataParams = new ArrayList<HarPostDataParam>();

  /**
   * Creates a new <code>HarPostDataParams</code> object
   */
  public HarPostDataParams()
  {
  }

  /**
   * Creates a new <code>HarPostDataParams</code> object from a JsonParser
   * already positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarPostDataParams(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the pages element
    if (jp.nextToken() != JsonToken.START_ARRAY)
    {
      throw new JsonParseException("[ missing after \"params\" element "
          + jp.getCurrentName(), jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_ARRAY)
    {
      addPostDataParam(new HarPostDataParam(jp, warnings));
    }
  }

  /**
   * Creates a new <code>HarPostDataParams</code> object from a database.
   * Retrieves the HarPostDataParam objects that corresponds to the HarLog
   * object with the specified id.
   * 
   * @param config the database configuration
   * @param requestId the requestId this object refers to
   * @throws SQLException if a database error occurs
   */
  public HarPostDataParams(HarDatabaseConfig config, long requestId)
      throws SQLException
  {
    Connection c = config.getConnection();
    String tableName = config.getTablePrefix() + HarPostDataParam.TABLE_NAME;
    PreparedStatement ps = null;
    ResultSet rs = null;
    try
    {
      ps = c
          .prepareStatement("SELECT id,name,value,filename,content_type,comment FROM "
              + tableName + " WHERE post_id=?");
      ps.setLong(1, requestId);
      rs = ps.executeQuery();
      while (rs.next())
        addPostDataParam(new HarPostDataParam(config, rs.getLong(1),
            rs.getString(2), rs.getString(3), rs.getString(4), rs.getString(5),
            rs.getString(6)));
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
    g.writeArrayFieldStart("params");
    for (HarPostDataParam param : postDataParams)
      param.writeHar(g);
    g.writeEndArray();
  }

  /**
   * Write this object in the given database referencing the specified
   * requestId.
   * 
   * @param postId the post id this object refers to
   * @param paramsPs PreparedStatement to write post params data
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(HarDatabaseConfig config, long postId,
      PreparedStatement paramsPs, long logId) throws SQLException
  {
    for (HarPostDataParam param : postDataParams)
      param.writeJDBC(config, postId, paramsPs, logId);
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
    Connection c = config.getConnection();
    String postDataParamName = config.getTablePrefix()
        + HarPostDataParam.TABLE_NAME;
    if (!config.isCreatedTable(postDataParamName))
      return;

    PreparedStatement ps = null;
    try
    {
      ps = c.prepareStatement("DELETE FROM " + postDataParamName
          + " WHERE post_id IN " + "(SELECT id FROM " + config.getTablePrefix()
          + "post" + " WHERE id IN" + "(SELECT entry_id FROM "
          + config.getTablePrefix() + "entries WHERE log_id=?))");
      ps.setLong(1, logId);
      ps.executeUpdate();
      config.dropTableIfEmpty(c, postDataParamName, config);
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
   * Add a new postDataParam to the list
   * 
   * @param postDataParam the postDataParam to add
   */
  public void addPostDataParam(HarPostDataParam postDataParam)
  {
    postDataParams.add(postDataParam);
  }

  /**
   * Returns the postDataParams value.
   * 
   * @return Returns the postDataParams.
   */
  public List<HarPostDataParam> getPostDataParams()
  {
    return postDataParams;
  }

  /**
   * Sets the postDataParams value.
   * 
   * @param postDataParams The postDataParams to set.
   */
  public void setPostDataParams(List<HarPostDataParam> postDataParams)
  {
    this.postDataParams = postDataParams;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    StringBuffer sb = new StringBuffer("  \"params\": [");
    if (postDataParams != null)
    {
      boolean first = true;
      for (HarPostDataParam param : postDataParams)
      {
        if (first)
          first = false;
        else
          sb.append(", ");
        sb.append(param);
      }
    }
    sb.append("]\n");
    return sb.toString();
  }

}
