namespace BasicFormats
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Text;

    internal class JSON
    {
        private const int BUILDER_CAPACITY = 0x800;
        protected static JSON instance = new JSON();
        protected int lastErrorIndex = -1;
        private const int TOKEN_COLON = 5;
        private const int TOKEN_COMMA = 6;
        private const int TOKEN_CURLY_CLOSE = 2;
        private const int TOKEN_CURLY_OPEN = 1;
        private const int TOKEN_FALSE = 10;
        private const int TOKEN_NONE = 0;
        private const int TOKEN_NULL = 11;
        private const int TOKEN_NUMBER = 8;
        private const int TOKEN_SQUARED_CLOSE = 4;
        private const int TOKEN_SQUARED_OPEN = 3;
        private const int TOKEN_STRING = 7;
        private const int TOKEN_TRUE = 9;

        protected void EatWhitespace(char[] json, ref int index)
        {
            while (index < json.Length)
            {
                if (" \t\n\r".IndexOf(json[index]) == -1)
                {
                    return;
                }
                index++;
            }
        }

        protected int GetLastIndexOfNumber(char[] json, int index)
        {
            int num = index;
            while (num < json.Length)
            {
                if ("0123456789+-.eE".IndexOf(json[num]) == -1)
                {
                    break;
                }
                num++;
            }
            return (num - 1);
        }

        protected bool IsNumeric(object o)
        {
            try
            {
                double.Parse(o.ToString());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        internal static object JsonDecode(string json)
        {
            if (json == null)
            {
                return null;
            }
            char[] chArray = json.ToCharArray();
            int index = 0;
            bool success = true;
            object obj2 = instance.ParseValue(chArray, ref index, ref success);
            if (success)
            {
                instance.lastErrorIndex = -1;
                return obj2;
            }
            instance.lastErrorIndex = index;
            return obj2;
        }

        internal static string JsonEncode(object json)
        {
            StringBuilder builder = new StringBuilder(0x800);
            if (!instance.SerializeValue(json, builder))
            {
                return null;
            }
            return builder.ToString();
        }

        protected int LookAhead(char[] json, int index)
        {
            int num = index;
            return this.NextToken(json, ref num);
        }

        protected int NextToken(char[] json, ref int index)
        {
            this.EatWhitespace(json, ref index);
            if (index != json.Length)
            {
                char ch = json[index];
                index++;
                switch (ch)
                {
                    case '"':
                        return 7;

                    case ',':
                        return 6;

                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        return 8;

                    case ':':
                        return 5;

                    case '[':
                        return 3;

                    case ']':
                        return 4;

                    case '{':
                        return 1;

                    case '}':
                        return 2;
                }
                index--;
                int num = json.Length - index;
                if ((((num >= 5) && (json[index] == 'f')) && ((json[index + 1] == 'a') && (json[index + 2] == 'l'))) && ((json[index + 3] == 's') && (json[index + 4] == 'e')))
                {
                    index += 5;
                    return 10;
                }
                if ((((num >= 4) && (json[index] == 't')) && ((json[index + 1] == 'r') && (json[index + 2] == 'u'))) && (json[index + 3] == 'e'))
                {
                    index += 4;
                    return 9;
                }
                if ((((num >= 4) && (json[index] == 'n')) && ((json[index + 1] == 'u') && (json[index + 2] == 'l'))) && (json[index + 3] == 'l'))
                {
                    index += 4;
                    return 11;
                }
            }
            return 0;
        }

        protected ArrayList ParseArray(char[] json, ref int index)
        {
            ArrayList list = new ArrayList();
            this.NextToken(json, ref index);
            bool flag = false;
            while (!flag)
            {
                int num = this.LookAhead(json, index);
                if (num == 0)
                {
                    return null;
                }
                if (num == 6)
                {
                    this.NextToken(json, ref index);
                }
                else
                {
                    if (num == 4)
                    {
                        this.NextToken(json, ref index);
                        return list;
                    }
                    bool success = true;
                    object obj2 = this.ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return null;
                    }
                    list.Add(obj2);
                }
            }
            return list;
        }

        protected double ParseNumber(char[] json, ref int index)
        {
            this.EatWhitespace(json, ref index);
            int lastIndexOfNumber = this.GetLastIndexOfNumber(json, index);
            int length = (lastIndexOfNumber - index) + 1;
            char[] destinationArray = new char[length];
            Array.Copy(json, index, destinationArray, 0, length);
            index = lastIndexOfNumber + 1;
            return double.Parse(new string(destinationArray), CultureInfo.InvariantCulture);
        }

        protected Hashtable ParseObject(char[] json, ref int index)
        {
            Hashtable hashtable = new Hashtable();
            this.NextToken(json, ref index);
            bool flag = false;
            while (!flag)
            {
                switch (this.LookAhead(json, index))
                {
                    case 0:
                        return null;

                    case 6:
                    {
                        this.NextToken(json, ref index);
                        continue;
                    }
                    case 2:
                        this.NextToken(json, ref index);
                        return hashtable;
                }
                string str = this.ParseString(json, ref index);
                if (str == null)
                {
                    return null;
                }
                if (this.NextToken(json, ref index) != 5)
                {
                    return null;
                }
                bool success = true;
                object obj2 = this.ParseValue(json, ref index, ref success);
                if (!success)
                {
                    return null;
                }
                hashtable[str] = obj2;
            }
            return hashtable;
        }

        protected string ParseString(char[] json, ref int index)
        {
            StringBuilder builder = new StringBuilder(0x800);
            this.EatWhitespace(json, ref index);
            char ch = json[index++];
            bool flag = false;
            while (!flag)
            {
                if (index == json.Length)
                {
                    break;
                }
                ch = json[index++];
                if (ch == '"')
                {
                    flag = true;
                    break;
                }
                if (ch == '\\')
                {
                    if (index == json.Length)
                    {
                        break;
                    }
                    ch = json[index++];
                    if (ch == '"')
                    {
                        builder.Append('"');
                    }
                    else
                    {
                        if (ch == '\\')
                        {
                            builder.Append('\\');
                            continue;
                        }
                        if (ch == '/')
                        {
                            builder.Append('/');
                            continue;
                        }
                        if (ch == 'b')
                        {
                            builder.Append('\b');
                            continue;
                        }
                        if (ch == 'f')
                        {
                            builder.Append('\f');
                            continue;
                        }
                        if (ch == 'n')
                        {
                            builder.Append('\n');
                            continue;
                        }
                        if (ch == 'r')
                        {
                            builder.Append('\r');
                            continue;
                        }
                        if (ch == 't')
                        {
                            builder.Append('\t');
                        }
                        else if (ch == 'u')
                        {
                            string str;
                            int num = json.Length - index;
                            if (num < 4)
                            {
                                break;
                            }
                            char[] destinationArray = new char[4];
                            Array.Copy(json, index, destinationArray, 0, 4);
                            uint num2 = uint.Parse(new string(destinationArray), NumberStyles.HexNumber);
                            try
                            {
                                str = char.ConvertFromUtf32((int) num2);
                            }
                            catch (Exception)
                            {
                                str = "�";
                            }
                            builder.Append(str);
                            index += 4;
                        }
                    }
                }
                else
                {
                    builder.Append(ch);
                }
            }
            if (!flag)
            {
                return null;
            }
            return builder.ToString();
        }

        protected object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (this.LookAhead(json, index))
            {
                case 1:
                    return this.ParseObject(json, ref index);

                case 3:
                    return this.ParseArray(json, ref index);

                case 7:
                    return this.ParseString(json, ref index);

                case 8:
                    return this.ParseNumber(json, ref index);

                case 9:
                    this.NextToken(json, ref index);
                    return bool.Parse("TRUE");

                case 10:
                    this.NextToken(json, ref index);
                    return bool.Parse("FALSE");

                case 11:
                    this.NextToken(json, ref index);
                    return null;
            }
            success = false;
            return null;
        }

        protected bool SerializeArray(ArrayList anArray, StringBuilder builder)
        {
            builder.Append("[");
            bool flag = true;
            for (int i = 0; i < anArray.Count; i++)
            {
                object obj2 = anArray[i];
                if (!flag)
                {
                    builder.Append(", ");
                }
                if (!this.SerializeValue(obj2, builder))
                {
                    return false;
                }
                flag = false;
            }
            builder.Append("]");
            return true;
        }

        protected void SerializeNumber(double number, StringBuilder builder)
        {
            builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
        }

        protected bool SerializeObject(Hashtable anObject, StringBuilder builder)
        {
            builder.Append("{");
            IDictionaryEnumerator enumerator = anObject.GetEnumerator();
            for (bool flag = true; enumerator.MoveNext(); flag = false)
            {
                string aString = enumerator.Key.ToString();
                object obj2 = enumerator.Value;
                if (!flag)
                {
                    builder.Append(", ");
                }
                this.SerializeString(aString, builder);
                builder.Append(":");
                if (!this.SerializeValue(obj2, builder))
                {
                    return false;
                }
            }
            builder.Append("}");
            return true;
        }

        protected bool SerializeObjectOrArray(object objectOrArray, StringBuilder builder)
        {
            if (objectOrArray is Hashtable)
            {
                return this.SerializeObject((Hashtable) objectOrArray, builder);
            }
            return ((objectOrArray is ArrayList) && this.SerializeArray((ArrayList) objectOrArray, builder));
        }

        protected void SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");
            foreach (char ch in aString.ToCharArray())
            {
                switch (ch)
                {
                    case '"':
                        builder.Append("\\\"");
                        break;

                    case '\\':
                        builder.Append(@"\\");
                        break;

                    case '\b':
                        builder.Append(@"\b");
                        break;

                    case '\f':
                        builder.Append(@"\f");
                        break;

                    case '\n':
                        builder.Append(@"\n");
                        break;

                    case '\r':
                        builder.Append(@"\r");
                        break;

                    case '\t':
                        builder.Append(@"\t");
                        break;

                    default:
                    {
                        int num2 = Convert.ToInt32(ch);
                        if ((num2 >= 0x20) && (num2 <= 0x7e))
                        {
                            builder.Append(ch);
                        }
                        else
                        {
                            builder.Append(@"\u" + Convert.ToString(num2, 0x10).PadLeft(4, '0'));
                        }
                        break;
                    }
                }
            }
            builder.Append("\"");
        }

        protected bool SerializeValue(object value, StringBuilder builder)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else if (value is string)
            {
                this.SerializeString((string) value, builder);
            }
            else if (value is Hashtable)
            {
                this.SerializeObject((Hashtable) value, builder);
            }
            else if (value is ArrayList)
            {
                this.SerializeArray((ArrayList) value, builder);
            }
            else if (this.IsNumeric(value))
            {
                this.SerializeNumber(Convert.ToDouble(value), builder);
            }
            else if ((value is bool) && ((bool) value))
            {
                builder.Append("true");
            }
            else if ((value is bool) && !((bool) value))
            {
                builder.Append("false");
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}

