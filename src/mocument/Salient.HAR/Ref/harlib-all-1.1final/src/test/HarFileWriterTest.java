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
import java.util.Arrays;
import java.util.Collection;

import junit.framework.TestCase;

import org.codehaus.jackson.JsonEncoding;
import org.codehaus.jackson.JsonFactory;
import org.codehaus.jackson.JsonGenerator;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.JsonParser;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;
import org.junit.runners.Parameterized.Parameters;

import edu.umass.cs.benchlab.har.HarLog;
import edu.umass.cs.benchlab.har.tools.HarFileReader;
import edu.umass.cs.benchlab.har.tools.HarFileWriter;

/**
 * Reads HAR files in tests_files, write them back with a .test extension, read
 * them again and compare the results with the original reading.
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
@RunWith(value = Parameterized.class)
public class HarFileWriterTest extends TestCase
{
  private String fileName;

  /**
   * Creates a new <code>HarFileWriterTest</code> object
   * 
   * @param fileName
   */
  public HarFileWriterTest(String fileName)
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
        {"test_files/softwareishard.com.har"}, {"test_files/custom-tags.har"}};
    return Arrays.asList(data);
  }

  /**
   * Test method for
   * {@link edu.umass.cs.benchlab.har.tools.HarFileWriter#writeHarFile(edu.umass.cs.benchlab.har.HarLog, java.io.File)}
   * .
   */
  @Test
  public void testWriteHarFile()
  {
    File f = new File(fileName);
    HarFileReader r = new HarFileReader();
    HarFileWriter w = new HarFileWriter();
    try
    {
      System.out.println("Reading " + fileName);
      HarLog l = r.readHarFile(f);
      System.out.println("Writing " + fileName + ".test");
      File f2 = new File(fileName + ".test");
      w.writeHarFile(l, f2);
      System.out.println("Writing done, reading again from " + fileName
          + ".test");
      HarLog l2 = r.readHarFile(f2);
      System.out.println("Comparing " + fileName + " with " + fileName
          + ".test");
      assertEquals(l.toString(), l2.toString());
      System.out.println("Success. Deleting " + fileName + ".test");
      f2.delete();
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
  public void testWriteWithCustomJsonGeneratorAndParser()
  {
    HarFileReader r = new HarFileReader();
    HarFileWriter w = new HarFileWriter();
    try
    {
      File f = new File(fileName);
      JsonParser jp = new JsonFactory().createJsonParser(f);

      System.out.println("Reading " + fileName);
      HarLog l = r.readHarFile(jp, null);
      System.out.println("Writing " + fileName + ".test");

      File file2 = new File(fileName + ".test");
      JsonGenerator g2 = new JsonFactory().createJsonGenerator(file2,
          JsonEncoding.UTF8);
      JsonParser jp2 = new JsonFactory().createJsonParser(file2);
      w.writeHarFile(l, g2);
      System.out.println("Writing done, reading again from " + fileName
          + ".test");
      HarLog l2 = r.readHarFile(jp2, null);
      System.out.println("Comparing " + fileName + " with " + fileName
          + ".test");
      assertEquals(l.toString(), l2.toString());
      System.out.println("Success. Deleting " + fileName + ".test");
      file2.delete();
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

}
