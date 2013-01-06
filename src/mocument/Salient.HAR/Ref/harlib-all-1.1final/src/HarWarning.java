/**
 * BenchLab: Internet Scale Benchmarking.
 * Copyright (C) 2010-2012 Emmanuel Cecchet.
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

import org.codehaus.jackson.JsonLocation;

/**
 * This class defines a HarWarning
 * 
 * @author <a href="mailto:manu@frogthinker.org">Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarWarning
{
  private String       message;
  private JsonLocation location;

  /**
   * Creates a new <code>HarWarning</code> object
   * 
   * @param message warning message
   */
  public HarWarning(String message)
  {
    this.message = message;
  }

  /**
   * Creates a new <code>HarWarning</code> object
   * 
   * @param message warning message
   * @param location location of the warning
   */
  public HarWarning(String message, JsonLocation location)
  {
    this.message = message;
    this.location = location;
  }

  /**
   * Returns the message value.
   * 
   * @return Returns the message.
   */
  public String getMessage()
  {
    return message;
  }

  /**
   * Sets the message value.
   * 
   * @param message The message to set.
   */
  public void setMessage(String message)
  {
    this.message = message;
  }

  /**
   * Returns the location value.
   * 
   * @return Returns the location.
   */
  public JsonLocation getLocation()
  {
    return location;
  }

  /**
   * Sets the location value.
   * 
   * @param location The location to set.
   */
  public void setLocation(JsonLocation location)
  {
    this.location = location;
  }

  /**
   * @see java.lang.Object#toString()
   */
  @Override
  public String toString()
  {
    return "Line:" + location.getLineNr() + ", Column: "
        + location.getColumnNr() + " - " + message;
  }

}
