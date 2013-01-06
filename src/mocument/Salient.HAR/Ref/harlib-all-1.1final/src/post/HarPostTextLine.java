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

package edu.umass.cs.benchlab.har.post;

/**
 * This class represents a line of POST data Content-Type: multipart/form-data;
 * A line starts with a delimiter and it is ended with a \r\n\r\n
 * 
 * @author Fabien Mottet
 */
public class HarPostTextLine
{

  final static String   srsn               = "\r\n";
  private static String doubleSrsn         = srsn.concat(srsn);
  private static String contentDisposition = "Content-Disposition: form-data; ";
  private String        boundary           = "";
  private String        name;
  private String        value;

  /**
   * Create a new PostText string The format is like this: "--boundary \r\n
   * Content-Disposition: form-data; name="field_name" \r\n\r\n value \r\n "
   * 
   * @param name
   * @param value
   * @param boundary
   */
  public HarPostTextLine(String name, String value, String boundary)
  {
    this.name = name;
    this.value = value;
    this.boundary = boundary;
  }

  /**
   * Get field name
   * 
   * @return field name
   */
  public String getName()
  {
    return name;
  }

  /**
   * Get field value
   * 
   * @return field value
   */
  public String getValue()
  {
    return value;
  }

  public String toString()
  {
    return "--" + boundary + srsn + contentDisposition + "name=\"" + name
        + "\"" + doubleSrsn + value + srsn;
  }

}
