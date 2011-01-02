namespace CIAPI.Core.Collections
{
    internal class ReaderWriterCount
    {
        // Fields
        public ReaderWriterCount next;
        public RecursiveCounts rc;
        public int readercount;
        public int threadid = -1;

        // Methods
        public ReaderWriterCount(bool fIsReentrant)
        {
            if (fIsReentrant)
            {
                rc = new RecursiveCounts();
            }
        }
    }
}