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
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;
import java.util.List;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a HarQueryParam
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarQueryParam extends AbstractNameValueComment
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "query";

  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarQueryParam</code> object
   * 
   * @param name name of the query parameter
   * @param value value of the query parameter
   * @param comment optional comment provided by the user or the application
   */
  public HarQueryParam(String name, String value, String comment)
  {
    super(name, value, comment);
  }

  /**
   * Creates a new <code>HarQueryParam</code> object from the database
   * 
   * @param config
   * @param queryParamId
   * @param name
   * @param value
   * @param comment
   * @throws SQLException
   */
  public HarQueryParam(HarDatabaseConfig config, long queryParamId,
      String name, String value, String comment) throws SQLException
  {
    super(name, value, comment);
    this.customFields.readCustomFieldsJDBC(config,
        HarCustomFields.Type.HARQUERYSTRING, queryParamId);
  }

  /**
   * Creates a new <code>HarQueryParam</code> object
   * 
   * @param name name of the query parameter
   * @param value value of the query parameter
   */
  public HarQueryParam(String name, String value)
  {
    super(name, value);
  }

  /**
   * Creates a new <code>HarQueryParam</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarQueryParam(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    super(jp);

    // Read the content of the log element
    if (jp.getCurrentToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"queryString\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("name".equals(name))
        setName(jp.getText());
      else if ("value".equals(name))
        setValue(jp.getText());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in queryString element", jp.getCurrentLocation());
      }
    }
    if (getName() == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing name field in queryString element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing name field in queryString element",
            jp.getCurrentLocation());
    }
    if (getValue() == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning(
            "Missing value field in queryString element", jp
                .getCurrentLocation()));
      else
        throw new JsonParseException(
            "Missing value field in queryString element",
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
    g.writeStartObject();
    g.writeStringField("name", getName());
    g.writeStringField("value", getValue());
    if (getComment() != null)
      g.writeStringField("comment", getComment());
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  public void writeJDBC(HarDatabaseConfig config, long id,
      PreparedStatement ps, long logId) throws SQLException
  {
    ps.setString(1, getName());
    ps.setString(2, getValue());
    if (getComment() == null)
      ps.setNull(3, Types.LONGVARCHAR);
    else
      ps.setString(3, getComment());
    ps.setLong(4, id);
    ps.executeUpdate();
    ResultSet rs = ps.getGeneratedKeys();
    if (!rs.next())
      throw new SQLException(
          "The database did not generate a key for an HarPage entry");
    long queryParamId = rs.getLong(1);
    this.customFields.writeCustomFieldsJDBC(config,
        HarCustomFields.Type.HARQUERYSTRING, queryParamId, logId);
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
    return "{ \"name\": \"" + getName() + "\", \"value\": \"" + getValue()
        + "\", \"comment\": \"" + getComment() + "\", " + customFields + " }";
  }

}
