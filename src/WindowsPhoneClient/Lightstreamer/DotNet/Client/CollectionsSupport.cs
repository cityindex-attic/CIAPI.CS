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
            bool added = false;
            try
            {
                MethodInfo method = c.GetType().GetMethod("Add");
                if (method == null)
                {
                    method = c.GetType().GetMethod("add");
                }
                int index = (int) method.Invoke(c, new object[] { obj });
                if (index >= 0)
                {
                    added = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return added;
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool Contains(ICollection c, object obj)
        {
            bool contains = false;
            try
            {
                MethodInfo method = c.GetType().GetMethod("Contains");
                if (method == null)
                {
                    method = c.GetType().GetMethod("contains");
                }
                contains = (bool) method.Invoke(c, new object[] { obj });
            }
            catch (Exception e)
            {
                throw e;
            }
            return contains;
        }

        public static bool ContainsAll(ICollection target, ICollection c)
        {
            IEnumerator e = c.GetEnumerator();
            bool contains = false;
            try
            {
                MethodInfo method = target.GetType().GetMethod("containsAll");
                if (method != null)
                {
                    return (bool) method.Invoke(target, new object[] { c });
                }
                method = target.GetType().GetMethod("Contains");
                while (e.MoveNext())
                {
                    if (!(contains = (bool) method.Invoke(target, new object[] { e.Current })))
                    {
                        return contains;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contains;
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
            bool changed = false;
            try
            {
                MethodInfo method = c.GetType().GetMethod("remove");
                if (method != null)
                {
                    method.Invoke(c, new object[] { obj });
                    return changed;
                }
                changed = (bool) c.GetType().GetMethod("Contains").Invoke(c, new object[] { obj });
                c.GetType().GetMethod("Remove").Invoke(c, new object[] { obj });
            }
            catch (Exception e)
            {
                throw e;
            }
            return changed;
        }

        public static void Shuffle(IList List)
        {
            Random RandomList = new Random((int) DateTime.Now.Ticks);
            Shuffle(List, RandomList);
        }

        public static void Shuffle(IList List, Random RandomList)
        {
            object source = null;
            int target = 0;
            for (int i = 0; i < List.Count; i++)
            {
                target = RandomList.Next(List.Count);
                source = List[i];
                List[i] = List[target];
                List[target] = source;
            }
        }

        public static object[] ToArray(ICollection c)
        {
            int index = 0;
            object[] objects = new object[c.Count];
            IEnumerator e = c.GetEnumerator();
            while (e.MoveNext())
            {
                objects[index++] = e.Current;
            }
            return objects;
        }

        public static object[] ToArray(ICollection c, object[] objects)
        {
            int index = 0;
            object[] objs = (object[]) Array.CreateInstance(objects.GetType().GetElementType(), c.Count);
            IEnumerator e = c.GetEnumerator();
            while (e.MoveNext())
            {
                objs[index++] = e.Current;
            }
            if (objects.Length >= c.Count)
            {
                objs.CopyTo(objects, 0);
            }
            return objs;
        }

        public static string ToString(ICollection c)
        {
            StringBuilder s = new StringBuilder();
            if (c != null)
            {
                int index = 0;
                object[] l = new object[c.Count];
                IEnumerator e = c.GetEnumerator();
                while (e.MoveNext())
                {
                    l[index++] = e.Current;
                }
                bool isDictionary = ((c is BitArray) || (c is IDictionary)) || ((l.Length > 0) && (l[0] is DictionaryEntry));
                for (index = 0; index < l.Length; index++)
                {
                    object elem = l[index];
                    if (elem == null)
                    {
                        s.Append("null");
                    }
                    else if (elem is DictionaryEntry)
                    {
                        DictionaryEntry key = (DictionaryEntry) elem;
                        s.Append(key.Key);
                        s.Append("=");
                        key = (DictionaryEntry) elem;
                        s.Append(key.Value);
                    }
                    else
                    {
                        s.Append(elem);
                    }
                    if (index < (l.Length - 1))
                    {
                        s.Append(", ");
                    }
                }
                if (isDictionary)
                {
                    s.Insert(0, "{");
                    s.Append("}");
                }
                else
                {
                    s.Insert(0, "[");
                    s.Append("]");
                }
            }
            else
            {
                s.Insert(0, "null");
            }
            return s.ToString();
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

