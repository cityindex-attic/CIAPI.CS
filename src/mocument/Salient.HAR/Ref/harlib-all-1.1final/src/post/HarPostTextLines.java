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

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Contains all lines off a postData field. Note: this is only for form-data
 * with Content-Type: multipart/form-data
 * 
 * @author Fabien Mottet
 */
public class HarPostTextLines
{
  private String                     start    = "";
  private String                     boundary = "---------------------------12655665101227410282340966306";
  private long                       contentLength;

  private ArrayList<HarPostTextLine> lines;

  /**
   * Creates a HarPostTextLines containing what is inside String s.
   * HarPostTextLines can then be explored to get an easier access to data.
   * TODO: handle content length
   * 
   * @param s A string corresponding to HAR postData.text with the format
   *          multipart/form-data
   */
  public HarPostTextLines(String s)
  {
    this(-1);

    // Check start
    assert (s.startsWith("Content-Type: multipart/form-data;"));

    // Get boundary
    Pattern p = Pattern.compile(".*boundary=(\\-+\\d+)\\r\\n.*");
    Matcher m = p.matcher(s);

    if (m.find())
    {
      boundary = m.group(1);
    }
    else
    {
      // TODO: nice exception
      System.err.println("Error in detecting boundary in Raw Trace\n");
      System.exit(-1);
    }

    // Boundaries except the first one has 2 extra hyphens.
    String[] tokens = s.split("--" + boundary);

    // Split each line in key values
    for (String string : tokens)
    {
      String pattern = "\\r\\nContent-Disposition: form-data; name=\"(.*)\"";
      p = Pattern.compile(pattern);
      m = p.matcher(string);
      if (m.find())
      {
        String fieldName = m.group(1);
        String[] tmpS = string.split(fieldName + "\\\"");
        String fieldValue = tmpS[1];
        // remove head and tail
        fieldValue = fieldValue.substring(4, fieldValue.length() - 2);
        this.addLine(fieldName, fieldValue);
      }
      else
      {
        // ignore unmatching lines
      }
    }

  }

  /**
   * Creates a new <code>HarPostTextLines</code> object
   * 
   * @param contentLength
   */
  public HarPostTextLines(long contentLength)
  {
    this.start = "Content-Type: multipart/form-data; boundary=";
    this.contentLength = contentLength;
    this.lines = new ArrayList<HarPostTextLine>();
  }

  /**
   * Creates a new <code>HarPostTextLines</code> object
   * 
   * @param contentLength
   * @param boundary
   */
  public HarPostTextLines(long contentLength, String boundary)
  {
    this(contentLength);
    this.boundary = boundary;
  }

  /**
   * Add a new line
   * 
   * @param name Key
   * @param value
   */
  public void addLine(String name, String value)
  {
    HarPostTextLine hptl = new HarPostTextLine(name, value, boundary);
    lines.add(hptl);
  }

  /**
   * Returns the contentLength value.
   * 
   * @return Returns the contentLength.
   */
  public long getContentLength()
  {
    return contentLength;
  }

  /**
   * Sets the contentLength value.
   * 
   * @param contentLength The contentLength to set.
   */
  public void setContentLength(long contentLength)
  {
    this.contentLength = contentLength;
  }

  /**
   * Gets Lines
   * 
   * @return Lines in ArrayList Format
   */
  public List<HarPostTextLine> getLines()
  {
    return lines;
  }

  public String toString()
  {
    String out = start + boundary + HarPostTextLine.srsn + "Content-Length: "
        + contentLength + HarPostTextLine.srsn + HarPostTextLine.srsn;

    java.util.Iterator<HarPostTextLine> i = lines.iterator();
    while (i.hasNext())
    {
      out = out + i.next().toString();
    }

    out = out + "--" + boundary + "--" + HarPostTextLine.srsn;

    return out;
  }

}
