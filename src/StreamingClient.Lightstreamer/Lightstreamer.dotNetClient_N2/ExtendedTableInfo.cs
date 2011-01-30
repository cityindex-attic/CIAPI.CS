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

        [Obsolete("Use the Fields property instead of the GetFields method.")]
        public virtual string[] GetFields()
        {
            return (string[]) this.fields.Clone();
        }

        [Obsolete("Use the Group property instead of the GetGroup method.")]
        public override string GetGroup()
        {
            return base.group;
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

        [Obsolete("Use the Items property instead of the GetItems method.")]
        public virtual string[] GetItems()
        {
            return (string[]) this.items.Clone();
        }

        [Obsolete("Use the Schema property instead of the GetSchema method.")]
        public override string GetSchema()
        {
            return base.schema;
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
        }

        public virtual string[] Fields
        {
            get
            {
                return this.GetFields();
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
                return this.GetItems();
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

