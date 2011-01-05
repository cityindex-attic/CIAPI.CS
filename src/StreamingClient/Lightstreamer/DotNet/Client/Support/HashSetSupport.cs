namespace Lightstreamer.DotNet.Client.Support
{
    using System;
    using System.Collections;

    [Serializable]
    public class HashSetSupport : ArrayList, ISetSupport, IList, ICollection, IEnumerable
    {
        public HashSetSupport()
        {
        }

        public HashSetSupport(ICollection c)
        {
            this.AddAll(c);
        }

        public HashSetSupport(int capacity) : base(capacity)
        {
        }

        public virtual bool Add(object obj)
        {
            bool flag;
            if (!(flag = this.Contains(obj)))
            {
                base.Add(obj);
            }
            return !flag;
        }

        public bool AddAll(ICollection c)
        {
            IEnumerator enumerator = new ArrayList(c).GetEnumerator();
            bool flag = false;
            while (enumerator.MoveNext())
            {
                if (this.Add(enumerator.Current))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public override object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}

