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
 * This class defines a HarCookie
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarCookie
{
  /**
   * Database table name where the data is stored
   */
  public static String    TABLE_NAME   = "cookies";

  private String          name;
  private String          value;
  private String          path;
  private String          domain;
  private Date            expires;
  private Boolean         httpOnly;
  private Boolean         secure;
  private String          comment;
  private String          version;
  private String          maxAge;
  private HarCustomFields customFields = new HarCustomFields();

  /**
   * Creates a new <code>HarCookie</code> object
   * 
   * @param name name [string] - The name of the cookie.
   * @param value value [string] - The cookie value.
   * @param path path [string, optional] - The path pertaining to the cookie.
   * @param domain domain [string, optional] - The host of the cookie.
   * @param expires expires [string, optional] - Cookie expiration time. (ISO
   *          8601 - YYYY-MM-DDThh:mm:ss.sTZD, e.g.
   *          2009-07-24T19:20:30.123+02:00).
   * @param httpOnly httpOnly [boolean, optional] - Set to true if the cookie is
   *          HTTP only, false otherwise.
   * @param secure secure [boolean, optional] (new in 1.2) - True if the cookie
   *          was transmitted over ssl, false otherwise.
   * @param comment comment [string, optional] (new in 1.2) - A comment provided
   *          by the user or the application.
   */
  public HarCookie(String name, String value, String path, String domain,
      Date expires, boolean httpOnly, boolean secure, String comment)
  {
    this.name = name;
    this.value = value;
    this.path = path;
    this.domain = domain;
    this.expires = expires;
    this.httpOnly = httpOnly;
    this.secure = secure;
    this.comment = comment;
  }

  /**
   * Creates a new <code>HarCookie</code> object with mandatory fields
   * 
   * @param name name [string] - The name of the cookie.
   * @param value value [string] - The cookie value.
   */
  public HarCookie(String name, String value)
  {
    this.name = name;
    this.value = value;
  }

  /**
   * Creates a new <code>HarCookie</code> object from a JsonParser already
   * positioned at the beginning of the element content
   * 
   * @param jp a JsonParser already positioned at the beginning of the element
   *          content
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @throws JsonParseException
   * @throws IOException
   */
  public HarCookie(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    // Read the content of the log element
    if (jp.getCurrentToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("{ missing after \"cookies\" element",
          jp.getCurrentLocation());
    }

    while (jp.nextToken() != JsonToken.END_OBJECT)
    {
      String name = jp.getCurrentName();
      if ("name".equals(name))
        setName(jp.getText());
      else if ("value".equals(name))
        setValue(jp.getText());
      else if ("path".equals(name))
        setPath(jp.getText());
      else if ("domain".equals(name))
        setDomain(jp.getText());
      else if ("expires".equals(name))
      {
        try
        {
          if (jp.getCurrentName().equals(jp.getText()))
            jp.nextToken();
          if (jp.getText().contains("NaN"))
          {
            setExpires(new Date(0));
          }
          else
          {
            setExpires(ISO8601DateFormatter.parseDate(jp.getText()));
          }
        }
        catch (ParseException e)
        {
          if (warnings != null)
            warnings.add(new HarWarning("Invalid date format '" + jp.getText()
                + "'", jp.getCurrentLocation()));
          else
            throw new JsonParseException("Invalid date format '" + jp.getText()
                + "'", jp.getCurrentLocation());
        }
      }
      else if ("httpOnly".equals(name))
        setHttpOnly(jp.getValueAsBoolean());
      else if ("secure".equals(name))
        setSecure(jp.getValueAsBoolean());
      else if ("comment".equals(name))
        setComment(jp.getText());
      else if ("version".equals(name))
        setVersion(jp.getText());
      else if ("max-age".equals(name))
        setMaxAge(jp.getText());
      else if (name.startsWith("_"))
        this.customFields.addHarCustomFields(name, jp);
      else
      {
        throw new JsonParseException("Unrecognized field '" + name
            + "' in page element", jp.getCurrentLocation());
      }
    }
    if (name == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing name field in cookie element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing name field in cookie element",
            jp.getCurrentLocation());
    }
    if (value == null)
    {
      if (warnings != null)
        warnings.add(new HarWarning("Missing value field in cookie element", jp
            .getCurrentLocation()));
      else
        throw new JsonParseException("Missing value field in cookie element",
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
    g.writeStringField("name", name);
    g.writeStringField("value", value);
    if (path != null)
      g.writeStringField("path", path);
    if (domain != null)
      g.writeStringField("domain", domain);
    if (expires != null)
      g.writeStringField("expires", ISO8601DateFormatter.format(expires));
    if (httpOnly != null)
      g.writeBooleanField("httpOnly", httpOnly);
    if (secure != null)
      g.writeBooleanField("secure", secure);
    if (comment != null)
      g.writeStringField("comment", comment);
    if (version != null)
      g.writeStringField("version", version);
    if (maxAge != null)
      g.writeStringField("maxAge", maxAge);
    this.customFields.writeHar(g);
    g.writeEndObject();
  }

  /**
   * Write this object in the given database referencing the specified logId.
   * 
   * @param requestId the requestId this object refers to
   * @param cookiePs PreparedStatement to write cookie data
   * @param isRequest true if these cookies belong to a request, false if they
   *          belong to a response
   * @throws SQLException if a database access error occurs
   */
  public void writeJDBC(HarDatabaseConfig config, long logId, long requestId,
      PreparedStatement cookiePs, boolean isRequest) throws SQLException
  {
    cookiePs.setString(1, name);
    cookiePs.setString(2, value);
    if (path == null)
      cookiePs.setNull(3, Types.LONGVARCHAR);
    else
      cookiePs.setString(3, path);
    if (domain == null)
      cookiePs.setNull(4, Types.LONGVARCHAR);
    else
      cookiePs.setString(4, domain);
    if (expires == null)
      cookiePs.setNull(5, Types.TIMESTAMP);
    else
      cookiePs.setTimestamp(5, new Timestamp(expires.getTime()));
    if (httpOnly == null)
      cookiePs.setNull(6, Types.SMALLINT);
    else
      cookiePs.setInt(6, httpOnly ? 1 : 0);
    if (secure == null)
      cookiePs.setNull(7, Types.SMALLINT);
    else
      cookiePs.setInt(7, secure ? 1 : 0);
    if (comment == null)
      cookiePs.setNull(8, Types.LONGVARCHAR);
    else
      cookiePs.setString(8, comment);
    cookiePs.setInt(9, isRequest ? 1 : 0);
    if (version == null)
      cookiePs.setNull(10, Types.LONGVARCHAR);
    else
      cookiePs.setString(10, version);
    if (maxAge == null)
      cookiePs.setNull(11, Types.LONGVARCHAR);
    else
      cookiePs.setString(11, maxAge);
    cookiePs.setLong(12, requestId);
    cookiePs.executeUpdate();
    ResultSet rs = cookiePs.getGeneratedKeys();
    if (!rs.next())
      throw new SQLException(
          "The database did not generate a key for an HarPage entry");
    long cookieId = rs.getLong(1);
    this.customFields.writeCustomFieldsJDBC(config,
        HarCustomFields.Type.HARCOOKIE, cookieId, logId);
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
   * Returns the path value.
   * 
   * @return Returns the path.
   */
  public String getPath()
  {
    return path;
  }

  /**
   * Sets the path value.
   * 
   * @param path The path to set.
   */
  public void setPath(String path)
  {
    this.path = path;
  }

  /**
   * Returns the domain value.
   * 
   * @return Returns the domain.
   */
  public String getDomain()
  {
    return domain;
  }

  /**
   * Sets the domain value.
   * 
   * @param domain The domain to set.
   */
  public void setDomain(String domain)
  {
    this.domain = domain;
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
   * Returns the httpOnly value.
   * 
   * @return Returns the httpOnly.
   */
  public boolean isHttpOnly()
  {
    return httpOnly;
  }

  /**
   * Sets the httpOnly value.
   * 
   * @param httpOnly The httpOnly to set.
   */
  public void setHttpOnly(boolean httpOnly)
  {
    this.httpOnly = httpOnly;
  }

  /**
   * Returns the secure value.
   * 
   * @return Returns the secure.
   */
  public boolean isSecure()
  {
    return secure;
  }

  /**
   * Sets the secure value.
   * 
   * @param secure The secure to set.
   */
  public void setSecure(boolean secure)
  {
    this.secure = secure;
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
   * Returns the maxAge value.
   * 
   * @return Returns the maxAge.
   */
  public String getMaxAge()
  {
    return maxAge;
  }

  /**
   * Sets the maxAge value.
   * 
   * @param maxAge The maxAge to set.
   */
  public void setMaxAge(String maxAge)
  {
    this.maxAge = maxAge;
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
    return "{ \"name\": \"" + name + "\", \"value\": \"" + value
        + "\", \"path\": \"" + path + "\", \"domain\": \"" + domain
        + "\", \"expires\": \"" + ISO8601DateFormatter.format(expires)
        + "\", \"httpOnly\": " + httpOnly + ", \"secure\": " + secure
        + ", \"comment\": \"" + comment + "\", " + customFields + "}\n";
  }

}
