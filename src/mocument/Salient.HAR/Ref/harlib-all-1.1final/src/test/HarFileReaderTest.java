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

package edu.umass.cs.benchlab.har.test;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;

import junit.framework.TestCase;

import org.codehaus.jackson.JsonFactory;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;
import org.junit.runners.Parameterized.Parameters;

import edu.umass.cs.benchlab.har.HarLog;
import edu.umass.cs.benchlab.har.HarWarning;
import edu.umass.cs.benchlab.har.tools.HarFileReader;

/**
 * This class defines a HarFileReaderTest
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
@RunWith(value = Parameterized.class)
public class HarFileReaderTest extends TestCase
{
  private String fileName;

  /**
   * Creates a new <code>HarFileReaderTest</code> object
   * 
   * @param fileName
   */
  public HarFileReaderTest(String fileName)
  {
    super(fileName);
    this.fileName = fileName;
  }

  /**
   * Returns the list of files to test
   */
  @Parameters
  public static Collection<Object[]> data()
  {
    Object[][] data = new Object[][]{{"test_files/www.frogthinker.org.har"},
        {"test_files/en.wikipedia.org.har"},
        {"test_files/browser-blocking-time.har"},
        {"test_files/google.com.har"}, {"test_files/inline-scripts-block.har"},
        {"test_files/softwareishard.com.har"}, {"test_files/custom-tags.har"},
        {"test_files/missing-timing.har"}, {"test_files/missing-timing2.har"}};
    return Arrays.asList(data);
  }

  /**
   * Test method for
   * {@link edu.umass.cs.benchlab.har.tools.HarFileReader#readHarFile(java.io.File)}
   * .
   */
  @Test
  public void testReadHarFile()
  {
    File f = new File(fileName);
    HarFileReader r = new HarFileReader();
    try
    {
      List<HarWarning> warnings = new ArrayList<HarWarning>();
      HarLog l = r.readHarFile(f, warnings);
      assertNotNull(l);
      for (HarWarning w : warnings)
        System.out.println("File:" + fileName + " - Warning:" + w);
    }
    catch (JsonParseException e)
    {
      e.printStackTrace();
      fail("Parsing error during test");
    }
    catch (IOException e)
    {
      e.printStackTrace();
      fail("IO exception during test");
    }
  }

  @Test
  public void testReadWithCustomJsonParser()
  {
    try
    {
      JsonParser jp = new JsonFactory().createJsonParser(new File(fileName));
      HarFileReader r = new HarFileReader();

      List<HarWarning> warnings = new ArrayList<HarWarning>();
      assertNotNull(r.readHarFile(jp, warnings));
      for (HarWarning w : warnings)
        System.out.println("File:" + fileName + " - Warning:" + w);
    }
    catch (Exception e)
    {
      e.printStackTrace();
      fail("Exception during test");
    }
  }
}
