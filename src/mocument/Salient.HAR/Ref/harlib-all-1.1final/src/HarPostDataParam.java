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
 * This class defines a HarPostDataParam
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarPostDataParam extends AbstractNameValueComment
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "post_params";

  private String          fileName;
  private String          contentType;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarPostDataParam</code> object
   * 
   * @param name name [string] - name of a posted parameter.
   * @param value value [string, optional] - value of a posted parameter or
   *          content of a posted file.
   * @param fileName fileName [string, optional] - name of a posted file.
   * @param contentType contentType [string, optional] - content type of a
   *          posted file.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarPostDataParam(String name, String value, String fileName,
      String contentType, String comment)
  {
    super(name, value, comment);
    this.fileName = fileName;
    this.contentType = contentType;
  }

  /**
   * Creates a new <code>HarPostDataParam</code> object from the database
   * 
   * @param config
   * @param postDataParamId
   * @param name
   * @param value
   * @param fileName
   * @param contentType
   * @param comment
   * @throws SQLException
   */
  public HarPostDataParam(HarDatabaseConfig config, long postDataParamId,
      String name, String value, String fileName, String contentType,
      String comment) throws SQLException
  {
    this(name, value, fileName, contentType, comment);
    this.customFields.readCustomFieldsJDBC(config,
        HarCustomFields.Type.HARPOSTDATAPARAM, postDataParamId);
  }

  /**
   * Creates a new <code>HarPostDataParam</code> object with the mandatory
   * fields
   * 
   * @param name name [string] - name of a posted parameter.
   * @param value value [string, optional] - value of a posted parameter or
   *          content of a posted file.
   */
  public HarPostDataParam(String name, String value)
  {
    super(name, value);
  }

  /**
   * Creates a new <code>HarHeader</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarPostDataParam(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    super(jp);

    // Read the content of the log element
    if (jp.getCurrentToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"params\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("name".equals(name))
        setName(jp.getText());
      else if ("value".equals(name))
        setValue(jp.getText());
      else if ("fileName".equals(name))
        setFileName(jp.getText());
      else if ("contentType".equals(name))
        setContentType(jp.getText());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
        throw new JsonParseException("Unrecognized field '" + name
            + "' in params element", jp.getCurrentLocation());
    }
    if (getName() == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing name field in postData element",
            jp.getCurrentLocation()));
      else
        throw new JsonParseException("Missing name field in postData element",
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
    if (getValue() != null)
      g.writeStringField("value", getValue());
    if (getFileName() != null)
      g.writeStringField("fileName", fileName);
    if (getContentType() != null)
      g.writeStringField("contentType", contentType);
    if (getComment() != null)
      g.writeStringField("comment", getComment());
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified id.
   * 
   * @param id the id this object refers to
   * @param ps PreparedStatement to write data
   * @throws SQLException if a database access error occurs
   */
  @Override
  public void writeJDBC(HarDatabaseConfig config, long id,
      PreparedStatement ps, long logId) throws SQLException
  {
    ps.setString(1, getName());
    if (getValue() == null)
      ps.setNull(2, Types.LONGVARCHAR);
    else
      ps.setString(2, getValue());
    if (fileName == null)
      ps.setNull(3, Types.LONGVARCHAR);
    else
      ps.setString(3, fileName);
    if (contentType == null)
      ps.setNull(4, Types.LONGVARCHAR);
    else
      ps.setString(4, contentType);
    if (getComment() == null)
      ps.setNull(5, Types.LONGVARCHAR);
    else
      ps.setString(5, getComment());
    ps.setLong(6, id);
    ps.executeUpdate();
    ResultSet rs = ps.getGeneratedKeys();
    if (!rs.next())
      throw new SQLException(
          "The database did not generate a key for an HarPage entry");
    long postDataParamId = rs.getLong(1);
    this.customFields.writeCustomFieldsJDBC(config,
        HarCustomFields.Type.HARPOSTDATAPARAM, postDataParamId, logId);
  }

  /**
   * Returns the fileName value.
   * 
   * @return Returns the fileName.
   */
  public String getFileName()
  {
    return fileName;
  }

  /**
   * Sets the fileName value.
   * 
   * @param fileName The fileName to set.
   */
  public void setFileName(String fileName)
  {
    this.fileName = fileName;
  }

  /**
   * Returns the contentType value.
   * 
   * @return Returns the contentType.
   */
  public String getContentType()
  {
    return contentType;
  }

  /**
   * Sets the contentType value.
   * 
   * @param contentType The contentType to set.
   */
  public void setContentType(String contentType)
  {
    this.contentType = contentType;
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
   * @see edu.umass.cs.benchlab.har.AbstractNameValueComment#toString()
   */
  @Override
  public String toString()
  {
    return "{ \"name\": \"" + getName() + "\", \"value\": \"" + getValue()
        + "\", \"fileName\": \"" + getFileName() + "\", \"contentType\": \""
        + getContentType() + "\", \"comment\": \"" + getComment() + "\", "
        + customFields + " }";
  }

}
