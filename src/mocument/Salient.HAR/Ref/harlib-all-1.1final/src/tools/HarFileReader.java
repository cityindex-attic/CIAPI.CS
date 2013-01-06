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

package edu.umass.cs.benchlab.har.tools;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.List;

import org.codehaus.jackson.JsonFactory;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.codehaus.jackson.JsonToken;

import edu.umass.cs.benchlab.har.HarLog;
import edu.umass.cs.benchlab.har.HarWarning;

/**
 * HarFileReader reads a HAR file into a HarLog object (building all the
 * necessary hierarchy of objects)
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class HarFileReader
{

  /**
   * Read the given file and build the corresponding HarLog object hierarchy in
   * memory.
   * 
   * @param jp The Json parser
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @return HarLog representation of the file
   * @throws JsonParseException if a parsing error occurs
   * @throws IOException if an IO error occurs
   */
  public HarLog readHarFile(JsonParser jp, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("File does not start with {",
          jp.getCurrentLocation());
    }

    try
    {
      HarLog log = new HarLog(jp, warnings);
      return log;
    }
    finally
    {
      jp.close(); // ensure resources get cleaned up timely and properly
    }
  }

  /**
   * Read the given file and build the corresponding HarLog object hierarchy in
   * memory.
   * 
   * @param file The file to read
   * @return HarLog representation of the file
   * @throws JsonParseException if a parsing error occurs
   * @throws IOException if an IO error occurs
   */
  public HarLog readHarFile(File file) throws JsonParseException, IOException
  {
    return readHarFile(file, null);
  }

  /**
   * Read the given file and build the corresponding HarLog object hierarchy in
   * memory.
   * 
   * @param file The file to read
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @return HarLog representation of the file
   * @throws JsonParseException if a parsing error occurs
   * @throws IOException if an IO error occurs
   */
  public HarLog readHarFile(File file, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    JsonFactory f = new JsonFactory();
    JsonParser jp = f.createJsonParser(file);
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("File does not start with {",
          jp.getCurrentLocation());
    }

    try
    {
      HarLog log = new HarLog(jp, warnings);
      return log;
    }
    finally
    {
      jp.close(); // ensure resources get cleaned up timely and properly
    }
  }

  /**
   * Read the given InputStream and build the corresponding HarLog object
   * hierarchy in memory.
   * 
   * @param stream The stream to read from
   * @param warnings null if parser should fail on first error, pointer to
   *          warning list if warnings can be issued for missing fields
   * @return HarLog representation of the file
   * @throws JsonParseException if a parsing error occurs
   * @throws IOException if an IO error occurs
   */
  public HarLog readHarFile(InputStream stream, List<HarWarning> warnings)
      throws JsonParseException, IOException
  {
    JsonFactory f = new JsonFactory();
    JsonParser jp = f.createJsonParser(stream);
    if (jp.nextToken() != JsonToken.START_OBJECT)
    {
      throw new JsonParseException("File does not start with {",
          jp.getCurrentLocation());
    }

    try
    {
      HarLog log = new HarLog(jp, warnings);
      return log;
    }
    finally
    {
      jp.close(); // ensure resources get cleaned up timely and properly
    }
  }

}
