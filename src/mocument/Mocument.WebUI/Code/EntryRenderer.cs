using System.Collections.Generic;
using System.Web.UI.WebControls;
using Salient.HTTPArchiveModel;
using Content = Salient.HTTPArchiveModel.Content;

namespace Mocument.WebUI.Code
{
    public static class EntryRenderer
    {
        public static Table BuildEntryTable(Entry entry)
        {
            var table = new Table();

            AddSimpleValues(entry, table);

            AddRequest(table, entry.request);
            AddResponse(table, entry.response);

            AddTimings(table, entry.timings, entry.time);

            return table;
        }

        private static void AddResponse(Table table, Response response)
        {
            if (response == null)
            {
                return;
            }
            Table subtable = CreateSubtable("response", table);
            AddTextRow("status", response.status, subtable);

            AddTextRow("statusText", response.statusText, subtable);
            AddTextRow("redirectURL", response.redirectURL, subtable);
            //AddTextRow("httpVersion", response.httpVersion, subtable);

            //AddTextRow("headersSize", response.headersSize, subtable);
            AddTextRow("comment", response.comment, subtable);
            //AddTextRow("bodySize", response.bodySize, subtable);


            AddCookies(subtable, response.cookies);
            AddNameValueTable(subtable, response.headers, "headers");

            Content content = response.content;

            Table cntable = CreateSubtable("content", subtable);

            AddTextRow("mimeType", content.mimeType, cntable);
            AddTextRow("size", content.size, cntable);
            AddTextRow("encoding", content.encoding, cntable);
            AddTextRow("compression", content.compression, cntable);
            AddTextRow("text", content.text, cntable);
            //AddTextRow("comment", content.comment, cntable);
        }

        private static void AddRequest(Table table, Request request)
        {
            if (request == null)
            {
                return;
            }
            Table subtable = CreateSubtable("request", table);
            //AddTextRow("bodySize", request.bodySize, subtable);
            //AddTextRow("comment", request.comment, subtable);
            //AddTextRow("headersSize", request.headersSize, subtable);
            //AddTextRow("host", request.host, subtable);
            AddTextRow("url", request.url, subtable);
            AddTextRow("method", request.method, subtable);

            //AddTextRow("httpVersion", request.httpVersion, subtable);
            //AddTextRow("path", request.path, subtable);

            AddCookies(subtable, request.cookies);
            AddNameValueTable(subtable, request.headers, "headers");
            AddNameValueTable(subtable, request.queryString, "queryString");
            AddPostData(subtable, request.postData);
        }

        private static void AddPostData(Table subtable, PostData postData)
        {
            if (postData == null)
            {
                return;
            }
            Table tbl = CreateSubtable("postData", subtable);
            AddTextRow("mimeType", postData.mimeType, tbl);

            AddTextRow("text", postData.text, tbl);
            AddNameValueTable(tbl, postData.@params, "params");
            //AddTextRow("comment", postData.comment, tbl);
        }

        private static void AddNameValueTable(Table subtable, List<NameValuePair> values, string title)
        {
            if (values == null || values.Count == 0)
            {
                return;
            }
            Table valueTable = CreateSubtable(title, subtable);
            var row = new TableRow();
            valueTable.Rows.Add(row);
            AddHeaderCell(row, "name");
            AddHeaderCell(row, "value");
            //AddHeaderCell(row, "comment");

            foreach (NameValuePair value in values)
            {
                var valuerow = new TableRow();
                valueTable.Rows.Add(valuerow);
                AddContentCell(valuerow, value.name);
                AddContentCell(valuerow, value.value);
                //AddContentCell(valuerow, value.comment);
            }
        }

        private static void AddCookies(Table subtable, List<Cookie> cookies)
        {
            if (cookies == null || cookies.Count == 0)
            {
                return;
            }
            Table cookietable = CreateSubtable("cookies", subtable);
            var row = new TableRow();
            cookietable.Rows.Add(row);
            AddHeaderCell(row, "name");
            AddHeaderCell(row, "value");
            AddHeaderCell(row, "path");
            AddHeaderCell(row, "domain");
            AddHeaderCell(row, "expires");
            AddHeaderCell(row, "httpOnly");
            AddHeaderCell(row, "secure");
            //AddHeaderCell(row, "comment");

            foreach (Cookie cookie in cookies)
            {
                var cookierow = new TableRow();
                cookietable.Rows.Add(cookierow);
                AddContentCell(cookierow, cookie.name);
                AddContentCell(cookierow, cookie.value);
                AddContentCell(cookierow, cookie.path);
                AddContentCell(cookierow, cookie.domain);
                AddContentCell(cookierow, cookie.expires);
                AddContentCell(cookierow, cookie.httpOnly);
                AddContentCell(cookierow, cookie.secure);
                //AddContentCell(cookierow, cookie.comment);
            }
        }

        private static void AddContentCell(TableRow row, object text)
        {
            string value = (text == null ? null : text.ToString());
            row.Cells.Add(new TableCell
                              {
                                  Text = value,
                              });
        }

        private static void AddHeaderCell(TableRow row, string text)
        {
            row.Cells.Add(new TableCell
                              {
                                  CssClass = "HeaderCell",
                                  Text = text,
                              });
        }


        private static void AddTimings(Table table, Timings timings, int time)
        {
            if (timings == null)
            {
                return;
            }

            Table subtable = CreateSubtable("timings", table);

            var row = new TableRow();
            subtable.Rows.Add(row);
            AddTextRow("ssl", timings.ssl, subtable);
            //AddTextRow("blocked", timings.blocked, subtable);
            AddTextRow("dns", timings.dns, subtable);
            AddTextRow("connect", timings.connect, subtable);


            AddTextRow("send", timings.send, subtable);
            AddTextRow("wait", timings.wait, subtable);
            AddTextRow("receive", timings.receive, subtable);
            AddTextRow("TOTAL", time, subtable);
            AddTextRow("comment", timings.comment, subtable);

        }

        private static Table CreateSubtable(string title, Table table)
        {
            var row = new TableRow();
            var cell = new TableCell();
            cell.CssClass = "NameCell";
            cell.Text = title;
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            table.Rows.Add(row);

            var subtable = new Table { CssClass = "subtable" };
            cell.Controls.Add(subtable);
            return subtable;
        }

        private static void AddTextRow(string title, object text, Table table)
        {
            if (text == null)
            {
                return;
            }
            string value = text.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var row = new TableRow();
            TableCell cell;
            cell = new TableCell
                       {
                           CssClass = "NameCell",
                           Text = title
                       };
            row.Cells.Add(cell);

            cell = new TableCell
                       {
                           Text = value
                       };
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        private static void AddSimpleValues(Entry entry, Table table)
        {
            AddTextRow("startedDateTime", entry.startedDateTime, table);
            //AddTextRow("time", entry.time, table);
            //AddTextRow("comment", entry.comment, table);
            //AddTextRow("connection", entry.connection, table);
            //AddTextRow("pageref", entry.pageref, table);
            //AddTextRow("serverIPAddress", entry.serverIPAddress, table);
        }

    }
}