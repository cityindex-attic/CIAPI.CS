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

import java.text.ParseException;
import java.util.Date;

import junit.framework.TestCase;
import edu.umass.cs.benchlab.har.ISO8601DateFormatter;

/**
 * This class defines a ISO8601DateFormatterTest
 * Note: this test may fail depending of your timezone.
 * 
 * @author <a href="mailto:cecchet@cs.umass.edu>Emmanuel Cecchet</a>
 * @version 1.0
 */
public class ISO8601DateFormatterTest extends TestCase
{
  private static final long   DATE_IN_MS    = 1296860016453L;
  private static final String EXPECTED_DATE = "2011-02-04T17:53:36.453-05:00";

  /**
   * Test method for
   * {@link edu.umass.cs.benchlab.har.ISO8601DateFormatter#parseDate(java.lang.String)}
   * .
   */
  public void testParseDate()
  {
    assertEquals(EXPECTED_DATE, ISO8601DateFormatter
        .format(new Date(DATE_IN_MS)));
  }

  /**
   * Test method for
   * {@link edu.umass.cs.benchlab.har.ISO8601DateFormatter#format(java.util.Date)}
   * .
   */
  public void testFormat()
  {
    try
    {
      assertEquals(DATE_IN_MS, ISO8601DateFormatter.parseDate(EXPECTED_DATE)
          .getTime());
    }
    catch (ParseException e)
    {
      e.printStackTrace();
      fail("Parsing failed"); // TODO
    }
  }
}
