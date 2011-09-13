namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections.Generic;

    internal class SequencesHandler
    {
        private IDictionary<string, SequenceHandler> sequences = new Dictionary<string, SequenceHandler>();

        internal SequenceHandler GetSequence(string sequence)
        {
            if (!this.sequences.ContainsKey(sequence))
            {
                SequenceHandler sh = new SequenceHandler();
                this.sequences[sequence] = sh;
            }
            return this.sequences[sequence];
        }

        internal IEnumerator<KeyValuePair<string, SequenceHandler>> Reset()
        {
            IEnumerator<KeyValuePair<string, SequenceHandler>> result = this.sequences.GetEnumerator();
            this.sequences = new Dictionary<string, SequenceHandler>();
            return result;
        }
    }
}

