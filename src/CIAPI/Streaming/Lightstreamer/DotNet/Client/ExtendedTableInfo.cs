namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Text;

    public class ExtendedTableInfo : SimpleTableInfo
    {
        internal string[] fields;
        internal string[] items;

        public ExtendedTableInfo(string[] items, string mode, string[] fields, bool snapshot) : base(GetGroupName(items), mode, GetSchemaName(fields), snapshot)
        {
            this.items = items;
            this.fields = fields;
        }

        private static string GetGroupName(string[] items)
        {
            StringBuilder builder = new StringBuilder(items[0]);
            for (int i = 1; i < items.Length; i++)
            {
                builder.Append(' ');
                builder.Append(items[i]);
            }
            return builder.ToString();
        }

        private static string GetSchemaName(string[] fields)
        {
            StringBuilder builder = new StringBuilder(fields[0]);
            for (int i = 1; i < fields.Length; i++)
            {
                builder.Append(' ');
                builder.Append(fields[i]);
            }
            return builder.ToString();
        }

        public override void SetRange(int start, int end)
        {
            base.start = start;
            base.end = end;
        }
    }
}

