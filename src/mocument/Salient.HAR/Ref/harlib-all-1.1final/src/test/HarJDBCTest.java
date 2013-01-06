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
import java.sql.SQLException;
import java.util.Arrays;
import java.util.Collection;

import junit.framework.TestCase;

import org.codehaus.jackson.JsonParseException;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;
import org.junit.runners.Parameterized.Parameters;

import edu.umass.cs.benchlab.har.HarDatabaseConfig;
import edu.umass.cs.benchlab.har.HarLog;
import edu.umass.cs.benchlab.har.tools.HarFileReader;

/**
 *Read files from test_files, write them to an embedded Derby database, read
 * them back and compare with the original reading from the file.
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
@RunWith(value = Parameterized.class)
public class HarJDBCTest extends TestCase
{
  private String fileName;

  /**
   * Creates a new <code>HarJDBCTest</code> object
   * 
   * @param fileName
   */
  public HarJDBCTest(String fileName)
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
        {"test_files/softwareishard.com.har"},{"test_files/custom-tags.har"}};
    return Arrays.asList(data);
  }

/**
   * Test method for
   * {@link edu.umass.cs.benchlab.har.HarLog#writeJDBC(HarDatabaseConfig) and
   * 
   * @link edu.umass.cs.benchlab.har.HarLog#HarLog(HarDatabaseConfig)}.
   */
  @Test
  public void testHarJDBC()
  {
    File f = new File(fileName);
    HarFileReader r = new HarFileReader();
    try
    {
      HarLog l = r.readHarFile(f);
      System.out.println("Reading " + fileName + " done, writing now");
      HarDatabaseConfig config = new HarDatabaseConfig(
          "org.apache.derby.jdbc.EmbeddedDriver",
          "jdbc:derby:harlibtest;create=true", "SA", "", "writetest", null,
          null, null, null, null);
      long id = l.writeJDBC(config);
      System.out.println("Writing done, reading again");
      HarLog l2 = new HarLog(config, id);
      assertEquals(l.toString(), l2.toString());
      try
      {
        l.deleteFromJDBC();
        fail("Deleting an object in the database that comes from a file should fail");
      }
      catch (SQLException e)
      {
        // This is not a database object, expected to fail
      }
      l2.deleteFromJDBC();
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
    catch (SQLException e)
    {
      e.printStackTrace();
      fail("SQL exception during test");
    }
  }

}
