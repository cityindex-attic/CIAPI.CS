namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text;

    internal class CollectionsSupport
    {
        public static bool Add(ICollection c, object obj)
        {
            bool flag = false;
            try
            {
                MethodInfo method = c.GetType().GetMethod("Add");
                if (method == null)
                {
                    method = c.GetType().GetMethod("add");
                }
                int num = (int) method.Invoke(c, new object[] { obj });
                if (num >= 0)
                {
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public static void Clear(ICollection c)
        {
            try
            {
                MethodInfo method = c.GetType().GetMethod("Clear");
                if (method == null)
                {
                    method = c.GetType().GetMethod("clear");
                }
                method.Invoke(c, new object[0]);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static bool Contains(ICollection c, object obj)
        {
            bool flag = false;
            try
            {
                MethodInfo method = c.GetType().GetMethod("Contains");
                if (method == null)
                {
                    method = c.GetType().GetMethod("contains");
                }
                flag = (bool) method.Invoke(c, new object[] { obj });
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public static bool ContainsAll(ICollection target, ICollection c)
        {
            IEnumerator enumerator = c.GetEnumerator();
            bool flag = false;
            try
            {
                MethodInfo method = target.GetType().GetMethod("containsAll");
                if (method != null)
                {
                    return (bool) method.Invoke(target, new object[] { c });
                }
                method = target.GetType().GetMethod("Contains");
                while (enumerator.MoveNext())
                {
                    if (!(flag = (bool) method.Invoke(target, new object[] { enumerator.Current })))
                    {
                        return flag;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public static void Copy(IList SourceList, IList TargetList)
        {
            for (int i = 0; i < SourceList.Count; i++)
            {
                TargetList[i] = SourceList[i];
            }
        }

        public static void Fill(IList List, object Element)
        {
            for (int i = 0; i < List.Count; i++)
            {
                List[i] = Element;
            }
        }

        public static bool Remove(ICollection c, object obj)
        {
            bool flag = false;
            try
            {
                MethodInfo method = c.GetType().GetMethod("remove");
                if (method != null)
                {
                    method.Invoke(c, new object[] { obj });
                    return flag;
                }
                flag = (bool) c.GetType().GetMethod("Contains").Invoke(c, new object[] { obj });
                c.GetType().GetMethod("Remove").Invoke(c, new object[] { obj });
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public static void Shuffle(IList List)
        {
            Random randomList = new Random((int) DateTime.Now.Ticks);
            Shuffle(List, randomList);
        }

        public static void Shuffle(IList List, Random RandomList)
        {
            object obj2 = null;
            int num = 0;
            for (int i = 0; i < List.Count; i++)
            {
                num = RandomList.Next(List.Count);
                obj2 = List[i];
                List[i] = List[num];
                List[num] = obj2;
            }
        }

        public static object[] ToArray(ICollection c)
        {
            int num = 0;
            object[] objArray = new object[c.Count];
            IEnumerator enumerator = c.GetEnumerator();
            while (enumerator.MoveNext())
            {
                objArray[num++] = enumerator.Current;
            }
            return objArray;
        }

        public static object[] ToArray(ICollection c, object[] objects)
        {
            int num = 0;
            object[] objArray = (object[]) Array.CreateInstance(objects.GetType().GetElementType(), c.Count);
            IEnumerator enumerator = c.GetEnumerator();
            while (enumerator.MoveNext())
            {
                objArray[num++] = enumerator.Current;
            }
            if (objects.Length >= c.Count)
            {
                objArray.CopyTo(objects, 0);
            }
            return objArray;
        }

        public static string ToString(ICollection c)
        {
            StringBuilder builder = new StringBuilder();
            if (c != null)
            {
                int index = 0;
                object[] objArray = new object[c.Count];
                IEnumerator enumerator = c.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    objArray[index++] = enumerator.Current;
                }
                bool flag = ((c is BitArray) || (c is IDictionary)) || ((objArray.Length > 0) && (objArray[0] is DictionaryEntry));
                for (index = 0; index < objArray.Length; index++)
                {
                    object obj2 = objArray[index];
                    if (obj2 == null)
                    {
                        builder.Append("null");
                    }
                    else if (obj2 is DictionaryEntry)
                    {
                        DictionaryEntry entry = (DictionaryEntry) obj2;
                        builder.Append(entry.Key);
                        builder.Append("=");
                        entry = (DictionaryEntry) obj2;
                        builder.Append(entry.Value);
                    }
                    else
                    {
                        builder.Append(obj2);
                    }
                    if (index < (objArray.Length - 1))
                    {
                        builder.Append(", ");
                    }
                }
                if (flag)
                {
                    builder.Insert(0, "{");
                    builder.Append("}");
                }
                else
                {
                    builder.Insert(0, "[");
                    builder.Append("]");
                }
            }
            else
            {
                builder.Insert(0, "null");
            }
            return builder.ToString();
        }

        private class CompareCharValues : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.CompareOrdinal((string) x, (string) y);
            }
        }
    }
}

