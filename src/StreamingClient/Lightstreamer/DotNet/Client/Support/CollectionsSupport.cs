namespace Lightstreamer.DotNet.Client.Support
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Text;

    public class CollectionsSupport
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

        public static bool AddAll(ICollection target, ICollection c)
        {
            IEnumerator enumerator = new ArrayList(c).GetEnumerator();
            bool flag = false;
            try
            {
                MethodInfo method = target.GetType().GetMethod("addAll");
                if (method != null)
                {
                    return (bool) method.Invoke(target, new object[] { c });
                }
                method = target.GetType().GetMethod("Add");
                while (enumerator.MoveNext())
                {
                    bool flag2 = ((int) method.Invoke(target, new object[] { enumerator.Current })) >= 0;
                    flag = flag ? flag : flag2;
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

        public static object Max(ICollection Collection, IComparer Comparator)
        {
            ArrayList list;
            InvalidOperationException exception;
            if (((ArrayList) Collection).IsReadOnly)
            {
                throw new NotSupportedException();
            }
            if ((Comparator == null) || (Comparator is Comparer))
            {
                try
                {
                    list = new ArrayList(Collection);
                    list.Sort();
                }
                catch (InvalidOperationException exception1)
                {
                    exception = exception1;
                    throw new InvalidCastException(exception.Message);
                }
                return list[Collection.Count - 1];
            }
            try
            {
                list = new ArrayList(Collection);
                list.Sort(Comparator);
            }
            catch (InvalidOperationException exception2)
            {
                exception = exception2;
                throw new InvalidCastException(exception.Message);
            }
            return list[Collection.Count - 1];
        }

        public static object Min(ICollection Collection, IComparer Comparator)
        {
            ArrayList list;
            InvalidOperationException exception;
            if (((ArrayList) Collection).IsReadOnly)
            {
                throw new NotSupportedException();
            }
            if ((Comparator == null) || (Comparator is Comparer))
            {
                try
                {
                    list = new ArrayList(Collection);
                    list.Sort();
                }
                catch (InvalidOperationException exception1)
                {
                    exception = exception1;
                    throw new InvalidCastException(exception.Message);
                }
                return list[0];
            }
            try
            {
                list = new ArrayList(Collection);
                list.Sort(Comparator);
            }
            catch (InvalidOperationException exception2)
            {
                exception = exception2;
                throw new InvalidCastException(exception.Message);
            }
            return list[0];
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

        public static bool RemoveAll(ICollection target, ICollection c)
        {
            ArrayList list = ToArrayList(c);
            IEnumerator enumerator = list.GetEnumerator();
            try
            {
                MethodInfo method = target.GetType().GetMethod("removeAll");
                if (method != null)
                {
                    method.Invoke(target, new object[] { list });
                }
                else
                {
                    method = target.GetType().GetMethod("Remove");
                    MethodInfo info2 = target.GetType().GetMethod("Contains");
                    while (enumerator.MoveNext())
                    {
                        goto Label_0088;
                    Label_006C:;
                        method.Invoke(target, new object[] { enumerator.Current });
                    Label_0088:;
                        if ((bool) info2.Invoke(target, new object[] { enumerator.Current }))
                        {
                            goto Label_006C;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return true;
        }

        public static bool RetainAll(ICollection target, ICollection c)
        {
            IEnumerator enumerator = new ArrayList(target).GetEnumerator();
            ArrayList list = new ArrayList(c);
            try
            {
                MethodInfo method = c.GetType().GetMethod("retainAll");
                if (method != null)
                {
                    method.Invoke(target, new object[] { c });
                }
                else
                {
                    method = c.GetType().GetMethod("Remove");
                    while (enumerator.MoveNext())
                    {
                        if (!list.Contains(enumerator.Current))
                        {
                            method.Invoke(target, new object[] { enumerator.Current });
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return true;
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

        public static void Sort(IList list, IComparer Comparator)
        {
            InvalidOperationException exception;
            if (((ArrayList) list).IsReadOnly)
            {
                throw new NotSupportedException();
            }
            if ((Comparator == null) || (Comparator is Comparer))
            {
                try
                {
                    ((ArrayList) list).Sort();
                }
                catch (InvalidOperationException exception1)
                {
                    exception = exception1;
                    throw new InvalidCastException(exception.Message);
                }
            }
            else
            {
                try
                {
                    ((ArrayList) list).Sort(Comparator);
                }
                catch (InvalidOperationException exception2)
                {
                    exception = exception2;
                    throw new InvalidCastException(exception.Message);
                }
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

        public static ArrayList ToArrayList(ICollection c)
        {
            ArrayList list = new ArrayList();
            IEnumerator enumerator = c.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }

        public static string ToString(ICollection c)
        {
            StringBuilder builder = new StringBuilder();
            if (c != null)
            {
                ArrayList list = new ArrayList(c);
                bool flag = (((c is BitArray) || (c is Hashtable)) || ((c is IDictionary) || (c is NameValueCollection))) || ((list.Count > 0) && (list[0] is DictionaryEntry));
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == null)
                    {
                        builder.Append("null");
                    }
                    else if (!flag)
                    {
                        builder.Append(list[i]);
                    }
                    else
                    {
                        DictionaryEntry entry;
                        flag = true;
                        if (c is NameValueCollection)
                        {
                            builder.Append(((NameValueCollection) c).GetKey(i));
                        }
                        else
                        {
                            entry = (DictionaryEntry) list[i];
                            builder.Append(entry.Key);
                        }
                        builder.Append("=");
                        if (c is NameValueCollection)
                        {
                            builder.Append(((NameValueCollection) c).GetValues(i)[0]);
                        }
                        else
                        {
                            entry = (DictionaryEntry) list[i];
                            builder.Append(entry.Value);
                        }
                    }
                    if (i < (list.Count - 1))
                    {
                        builder.Append(", ");
                    }
                }
                if (flag && (c is ArrayList))
                {
                    flag = false;
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

