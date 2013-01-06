using System;
using System.Collections.Generic;

namespace Mocument.Model
{
    [Serializable]
    public class Catalog
    {
        public bool Locked { get; set; }
        public Catalog()
        {
            Tapes = new List<Tape>();
        }

        public List<Tape> Tapes { get; private set; }
    }
}