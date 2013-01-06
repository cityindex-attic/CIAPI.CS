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
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Types;

import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;

import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * This class defines a AbstractNameValueComment
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public abstract class AbstractNameValueComment
{
  private String name;
  private String value;
  private String comment;

  /**
   * Creates a new <code>AbstractNameValueComment</code> object
   * 
   * @param name
   * @param value
   * @param comment
   */
  public AbstractNameValueComment(String name, String value, String comment)
  {
    this.name = name;
    this.value = value;
    this.comment = comment;
  }

  /**
   * Creates a new <code>AbstractNameValueComment</code> object
   * 
   * @param name
   * @param value
   */
  public AbstractNameValueComment(String name, String value)
  {
    this.name = name;
    this.value = value;
  }

  /**
   * Empty constructor
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @throws JsonParseException
   * @throws IOException
   */
  public AbstractNameValueComment(JsonParser jp) throws JsonParseException,
      IOException
  {
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
    g.writeStringField("name", name);
    g.writeStringField("value", value);
    if (comment != null)
      g.writeStringField("comment", comment);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified id.
   * 
   * @param id the id this object refers to
   * @param ps PreparedStatement to write data
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(HarDatabaseConfig config, long id, PreparedStatement ps,  long logId) throws SQLException
  {
    ps.setString(1, name);
    ps.setString(2, value);
    if (comment == null)
      ps.setNull(3, Types.LONGVARCHAR);
    else
      ps.setString(3, comment);
    ps.setLong(4, id);
    ps.executeUpdate();
  }

  /**
   * Returns the name value.
   * 
   * @return Returns the name.
   */
  public String getName()
  {
    return name;
  }

  /**
   * Sets the name value.
   * 
   * @param name The name to set.
   */
  public void setName(String name)
  {
    this.name = name;
  }

  /**
   * Returns the value value.
   * 
   * @return Returns the value.
   */
  public String getValue()
  {
    return value;
  }

  /**
   * Sets the value value.
   * 
   * @param value The value to set.
   */
  public void setValue(String value)
  {
    this.value = value;
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
    return "{ \"name\": \"" + name + "\", \"value\": \"" + value
        + "\", \"comment\": \"" + comment + "\" }";
  }

}
