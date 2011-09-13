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
            this.items = (string[]) items.Clone();
            this.fields = (string[]) fields.Clone();
        }

        public override object Clone()
        {
            return base.MemberwiseClone();
        }

        private static string GetGroupName(string[] items)
        {
            StringBuilder strGroup = new StringBuilder(items[0]);
            for (int i = 1; i < items.Length; i++)
            {
                strGroup.Append(' ');
                strGroup.Append(items[i]);
            }
            return strGroup.ToString();
        }

        private static string GetSchemaName(string[] fields)
        {
            StringBuilder strSchema = new StringBuilder(fields[0]);
            for (int i = 1; i < fields.Length; i++)
            {
                strSchema.Append(' ');
                strSchema.Append(fields[i]);
            }
            return strSchema.ToString();
        }

        public override void SetRange(int start, int end)
        {
        }

        public virtual string[] Fields
        {
            get
            {
                return (string[]) this.fields.Clone();
            }
        }

        public override string Group
        {
            get
            {
                return base.group;
            }
        }

        public virtual string[] Items
        {
            get
            {
                return (string[]) this.items.Clone();
            }
        }

        public override string Schema
        {
            get
            {
                return base.schema;
            }
        }
    }
}

