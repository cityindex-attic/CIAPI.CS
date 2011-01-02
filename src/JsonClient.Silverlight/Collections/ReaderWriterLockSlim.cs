using System;
using System.Threading;

namespace CIAPI.Core.Collections
{
    /// <summary>
    /// modified fx source (c) microsoft - for now.
    /// if we can determine that CIAPI does not require recursive ReaderWriterLock we can use Vance's implementation
    /// in readerWriterDemo.cs
    /// </summary>
    public class ReaderWriterLockSlim : IDisposable
    {
        private readonly bool _fIsReentrant;
        private bool _fDisposed;
        private bool _fNoWaiters;
        private bool _fUpgradeThreadHoldingRead;
        private int _myLock;
        private uint _numReadWaiters;
        private uint _numUpgradeWaiters;
        private uint _numWriteUpgradeWaiters;
        private uint _numWriteWaiters;
        private uint _owners;
        private EventWaitHandle _readEvent;
        private ReaderWriterCount[] _rwc;
        private EventWaitHandle _upgradeEvent;
        private int _upgradeLockOwnerId;
        private EventWaitHandle _waitUpgradeEvent;
        private EventWaitHandle _writeEvent;
        private int _writeLockOwnerId;

        // Methods

        public ReaderWriterLockSlim()
            : this(LockRecursionPolicy.NoRecursion)
        {
        }

        public ReaderWriterLockSlim(LockRecursionPolicy recursionPolicy)
        {
            if (recursionPolicy == LockRecursionPolicy.SupportsRecursion)
            {
                _fIsReentrant = true;
            }
            InitializeThreadCounts();
        }

        public int CurrentReadCount
        {
            get
            {
                var numReaders = (int) GetNumReaders();
                if (_upgradeLockOwnerId != -1)
                {
                    return (numReaders - 1);
                }
                return numReaders;
            }
        }

        public bool IsReadLockHeld
        {
            get { return (RecursiveReadCount > 0); }
        }

        public bool IsUpgradeableReadLockHeld
        {
            get { return (RecursiveUpgradeCount > 0); }
        }

        public bool IsWriteLockHeld
        {
            get { return (RecursiveWriteCount > 0); }
        }

        public LockRecursionPolicy RecursionPolicy
        {
            get
            {
                if (_fIsReentrant)
                {
                    return LockRecursionPolicy.SupportsRecursion;
                }
                return LockRecursionPolicy.NoRecursion;
            }
        }

        public int RecursiveReadCount
        {
            get
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                int readercount = 0;


                EnterMyLock();
                ReaderWriterCount threadRwCount = GetThreadRwCount(managedThreadId, true);
                if (threadRwCount != null)
                {
                    readercount = threadRwCount.readercount;
                }
                ExitMyLock();

                return readercount;
            }
        }

        public int RecursiveUpgradeCount
        {
            get
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (_fIsReentrant)
                {
                    int upgradecount = 0;


                    EnterMyLock();
                    ReaderWriterCount threadRwCount = GetThreadRwCount(managedThreadId, true);
                    if (threadRwCount != null)
                    {
                        upgradecount = threadRwCount.rc.Upgradecount;
                    }
                    ExitMyLock();


                    return upgradecount;
                }
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    return 1;
                }
                return 0;
            }
        }

        public int RecursiveWriteCount
        {
            get
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                int writercount = 0;
                if (_fIsReentrant)
                {
                    EnterMyLock();
                    ReaderWriterCount threadRwCount = GetThreadRwCount(managedThreadId, true);
                    if (threadRwCount != null)
                    {
                        writercount = threadRwCount.rc.Writercount;
                    }
                    ExitMyLock();

                    return writercount;
                }
                if (managedThreadId == _writeLockOwnerId)
                {
                    return 1;
                }
                return 0;
            }
        }

        public int WaitingReadCount
        {
            get { return (int) _numReadWaiters; }
        }

        public int WaitingUpgradeCount
        {
            get { return (int) _numUpgradeWaiters; }
        }

        public int WaitingWriteCount
        {
            get { return (int) _numWriteWaiters; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private void ClearUpgraderWaiting()
        {
            _owners &= 0xdfffffff;
        }

        private void ClearWriterAcquired()
        {
            _owners &= 0x7fffffff;
        }

        private void ClearWritersWaiting()
        {
            _owners &= 0xbfffffff;
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_fDisposed)
                {
                    throw new ObjectDisposedException(null);
                }
                if (((WaitingReadCount > 0) || (WaitingUpgradeCount > 0)) || (WaitingWriteCount > 0))
                {
                    throw new SynchronizationLockException("SynchronizationLockException_IncorrectDispose");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_IncorrectDispose"));
                }
                if ((IsReadLockHeld || IsUpgradeableReadLockHeld) || IsWriteLockHeld)
                {
                    throw new SynchronizationLockException("SynchronizationLockException_IncorrectDispose");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_IncorrectDispose"));
                }
                if (_writeEvent != null)
                {
                    _writeEvent.Close();
                    _writeEvent = null;
                }
                if (_readEvent != null)
                {
                    _readEvent.Close();
                    _readEvent = null;
                }
                if (_upgradeEvent != null)
                {
                    _upgradeEvent.Close();
                    _upgradeEvent = null;
                }
                if (_waitUpgradeEvent != null)
                {
                    _waitUpgradeEvent.Close();
                    _waitUpgradeEvent = null;
                }
                _fDisposed = true;
            }
        }

        private void EnterMyLock()
        {
            if (Interlocked.CompareExchange(ref _myLock, 1, 0) != 0)
            {
                EnterMyLockSpin();
            }
        }

        private void EnterMyLockSpin()
        {
            int processorCount = Environment.ProcessorCount;
            int num2 = 0;
            while (true)
            {
                if ((num2 < 10) && (processorCount > 1))
                {
                    Thread.SpinWait(20*(num2 + 1));
                }
                else if (num2 < 15)
                {
                    Thread.Sleep(0);
                }
                else
                {
                    Thread.Sleep(1);
                }
                if ((_myLock == 0) && (Interlocked.CompareExchange(ref _myLock, 1, 0) == 0))
                {
                    return;
                }
                num2++;
            }
        }

        public void EnterReadLock()
        {
            TryEnterReadLock(-1);
        }

        public void EnterUpgradeableReadLock()
        {
            TryEnterUpgradeableReadLock(-1);
        }

        public void EnterWriteLock()
        {
            TryEnterWriteLock(-1);
        }

        private void ExitAndWakeUpAppropriateWaiters()
        {
            if (_fNoWaiters)
            {
                ExitMyLock();
            }
            else
            {
                ExitAndWakeUpAppropriateWaitersPreferringWriters();
            }
        }

        private void ExitAndWakeUpAppropriateWaitersPreferringWriters()
        {
            bool flag = false;
            bool flag2 = false;
            uint numReaders = GetNumReaders();
            if ((_fIsReentrant && (_numWriteUpgradeWaiters > 0)) && (_fUpgradeThreadHoldingRead && (numReaders == 2)))
            {
                ExitMyLock();
                _waitUpgradeEvent.Set();
            }
            else if ((numReaders == 1) && (_numWriteUpgradeWaiters > 0))
            {
                ExitMyLock();
                _waitUpgradeEvent.Set();
            }
            else if ((numReaders == 0) && (_numWriteWaiters > 0))
            {
                ExitMyLock();
                _writeEvent.Set();
            }
            else if (numReaders >= 0)
            {
                if ((_numReadWaiters == 0) && (_numUpgradeWaiters == 0))
                {
                    ExitMyLock();
                }
                else
                {
                    if (_numReadWaiters != 0)
                    {
                        flag2 = true;
                    }
                    if ((_numUpgradeWaiters != 0) && (_upgradeLockOwnerId == -1))
                    {
                        flag = true;
                    }
                    ExitMyLock();
                    if (flag2)
                    {
                        _readEvent.Set();
                    }
                    if (flag)
                    {
                        _upgradeEvent.Set();
                    }
                }
            }
            else
            {
                ExitMyLock();
            }
        }

        private void ExitMyLock()
        {
            _myLock = 0;
        }

        public void ExitReadLock()
        {
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
// ReSharper disable RedundantAssignment
            ReaderWriterCount threadRwCount = null;
// ReSharper restore RedundantAssignment
            EnterMyLock();
            threadRwCount = GetThreadRwCount(managedThreadId, true);
            if (!_fIsReentrant)
            {
                if (threadRwCount == null)
                {
                    ExitMyLock();
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedRead");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedRead"));
                }
            }
            else
            {
                if ((threadRwCount == null) || (threadRwCount.readercount < 1))
                {
                    ExitMyLock();
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedRead");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedRead"));
                }
                if (threadRwCount.readercount > 1)
                {
                    threadRwCount.readercount--;
                    ExitMyLock();
                    return;
                }
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    _fUpgradeThreadHoldingRead = false;
                }
            }
            _owners--;
            threadRwCount.readercount--;
            ExitAndWakeUpAppropriateWaiters();
        }

        public void ExitUpgradeableReadLock()
        {
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (!_fIsReentrant)
            {
                if (managedThreadId != _upgradeLockOwnerId)
                {
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedUpgrade");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedUpgrade"));
                }
                EnterMyLock();
            }
            else
            {
                EnterMyLock();
                ReaderWriterCount threadRwCount = GetThreadRwCount(managedThreadId, true);
                if (threadRwCount == null)
                {
                    ExitMyLock();
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedUpgrade");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedUpgrade"));
                }
                RecursiveCounts rc = threadRwCount.rc;
                if (rc.Upgradecount < 1)
                {
                    ExitMyLock();
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedUpgrade");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedUpgrade"));
                }
                rc.Upgradecount--;
                if (rc.Upgradecount > 0)
                {
                    ExitMyLock();
                    return;
                }
                _fUpgradeThreadHoldingRead = false;
            }
            _owners--;
            _upgradeLockOwnerId = -1;
            ExitAndWakeUpAppropriateWaiters();
        }

        public void ExitWriteLock()
        {
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (!_fIsReentrant)
            {
                if (managedThreadId != _writeLockOwnerId)
                {
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedWrite");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedWrite"));
                }
                EnterMyLock();
            }
            else
            {
                EnterMyLock();
                ReaderWriterCount threadRwCount = GetThreadRwCount(managedThreadId, false);
                if (threadRwCount == null)
                {
                    ExitMyLock();
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedWrite");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedWrite"));
                }
                RecursiveCounts rc = threadRwCount.rc;
                if (rc.Writercount < 1)
                {
                    ExitMyLock();
                    throw new SynchronizationLockException("SynchronizationLockException_MisMatchedWrite");
                    //throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedWrite"));
                }
                rc.Writercount--;
                if (rc.Writercount > 0)
                {
                    ExitMyLock();
                    return;
                }
            }
            ClearWriterAcquired();
            _writeLockOwnerId = -1;
            ExitAndWakeUpAppropriateWaiters();
        }

        private uint GetNumReaders()
        {
            return (_owners & 0xfffffff);
        }

        private ReaderWriterCount GetThreadRwCount(int id, bool dontAllocate)
        {
            int index = id & 0xff;
            ReaderWriterCount count = null;
            if (_rwc[index] == null)
            {
                if (dontAllocate)
                {
                    return null;
                }
                _rwc[index] = new ReaderWriterCount(_fIsReentrant);
            }
            if (_rwc[index].threadid == id)
            {
                return _rwc[index];
            }
            if (IsRwEntryEmpty(_rwc[index]) && !dontAllocate)
            {
                if (_rwc[index].next == null)
                {
                    _rwc[index].threadid = id;
                    return _rwc[index];
                }
                count = _rwc[index];
            }
            ReaderWriterCount next = _rwc[index].next;
            while (next != null)
            {
                if (next.threadid == id)
                {
                    return next;
                }
                if ((count == null) && IsRwEntryEmpty(next))
                {
                    count = next;
                }
                next = next.next;
            }
            if (dontAllocate)
            {
                return null;
            }
            if (count == null)
            {
                next = new ReaderWriterCount(_fIsReentrant)
                           {
                               threadid = id,
                               next = _rwc[index].next
                           };
                _rwc[index].next = next;
                return next;
            }
            count.threadid = id;
            return count;
        }

        private void InitializeThreadCounts()
        {
            _rwc = new ReaderWriterCount[0x100];
            _upgradeLockOwnerId = -1;
            _writeLockOwnerId = -1;
        }

        private static bool IsRwEntryEmpty(ReaderWriterCount rwc)
        {
            return ((rwc.threadid == -1) ||
                    (((rwc.readercount == 0) && (rwc.rc == null)) ||
                     (((rwc.readercount == 0) && (rwc.rc.Writercount == 0)) && (rwc.rc.Upgradecount == 0))));
        }

        private static bool IsRwHashEntryChanged(ReaderWriterCount lrwc, int id)
        {
            return (lrwc.threadid != id);
        }

        private bool IsWriterAcquired()
        {
            return ((_owners & 0xbfffffff) == 0);
        }

        private void LazyCreateEvent(ref EventWaitHandle waitEvent, bool makeAutoResetEvent)
        {
            EventWaitHandle handle;
            ExitMyLock();
            if (makeAutoResetEvent)
            {
                handle = new AutoResetEvent(false);
            }
            else
            {
                handle = new ManualResetEvent(false);
            }
            EnterMyLock();
            if (waitEvent == null)
            {
                waitEvent = handle;
            }
            else
            {
                handle.Close();
            }
        }

        private void SetUpgraderWaiting()
        {
            _owners |= 0x20000000;
        }

        private void SetWriterAcquired()
        {
            _owners |= 0x80000000;
        }

        private void SetWritersWaiting()
        {
            _owners |= 0x40000000;
        }

        private static void SpinWait(int spinCount)
        {
            if ((spinCount < 5) && (Environment.ProcessorCount > 1))
            {
                Thread.SpinWait(20*spinCount);
            }
            else if (spinCount < 0x11)
            {
                Thread.Sleep(0);
            }
            else
            {
                Thread.Sleep(1);
            }
        }

        public bool TryEnterReadLock(int millisecondsTimeout)
        {
            bool flag = false;
            try
            {
                flag = TryEnterReadLockCore(millisecondsTimeout);
            }
            finally
            {
                if (!flag)
                {
                }
            }
            return flag;
        }

        public bool TryEnterReadLock(TimeSpan timeout)
        {
            var totalMilliseconds = (long) timeout.TotalMilliseconds;
            if ((totalMilliseconds < -1L) || (totalMilliseconds > 0x7fffffffL))
            {
                throw new ArgumentOutOfRangeException("timeout");
            }
            var millisecondsTimeout = (int) timeout.TotalMilliseconds;
            return TryEnterReadLock(millisecondsTimeout);
        }

        private bool TryEnterReadLockCore(int millisecondsTimeout)
        {
            if (millisecondsTimeout < -1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }
            if (_fDisposed)
            {
                throw new ObjectDisposedException(null);
            }
// ReSharper disable RedundantAssignment
            ReaderWriterCount lrwc = null;
// ReSharper restore RedundantAssignment
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (!_fIsReentrant)
            {
                if (managedThreadId == _writeLockOwnerId)
                {
                    throw new LockRecursionException("LockRecursionException_ReadAfterWriteNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_ReadAfterWriteNotAllowed"));
                }
                EnterMyLock();
                lrwc = GetThreadRwCount(managedThreadId, false);
                if (lrwc.readercount > 0)
                {
                    ExitMyLock();
                    throw new LockRecursionException("LockRecursionException_RecursiveReadNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_RecursiveReadNotAllowed"));
                }
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    lrwc.readercount++;
                    _owners++;
                    ExitMyLock();
                    return true;
                }
            }
            else
            {
                EnterMyLock();
                lrwc = GetThreadRwCount(managedThreadId, false);
                if (lrwc.readercount > 0)
                {
                    lrwc.readercount++;
                    ExitMyLock();
                    return true;
                }
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    lrwc.readercount++;
                    _owners++;
                    ExitMyLock();
                    _fUpgradeThreadHoldingRead = true;
                    return true;
                }
                if (managedThreadId == _writeLockOwnerId)
                {
                    lrwc.readercount++;
                    _owners++;
                    ExitMyLock();
                    return true;
                }
            }
            int spinCount = 0;
            Label_013D:
            if (_owners < 0xffffffe)
            {
                _owners++;
                lrwc.readercount++;
            }
            else
            {
                if (spinCount < 20)
                {
                    ExitMyLock();
                    if (millisecondsTimeout == 0)
                    {
                        return false;
                    }
                    spinCount++;
                    SpinWait(spinCount);
                    EnterMyLock();
                    if (IsRwHashEntryChanged(lrwc, managedThreadId))
                    {
                        lrwc = GetThreadRwCount(managedThreadId, false);
                    }
                }
                else if (_readEvent == null)
                {
                    LazyCreateEvent(ref _readEvent, false);
                    if (IsRwHashEntryChanged(lrwc, managedThreadId))
                    {
                        lrwc = GetThreadRwCount(managedThreadId, false);
                    }
                }
                else
                {
                    bool flag = WaitOnEvent(_readEvent, ref _numReadWaiters, millisecondsTimeout);
                    if (!flag)
                    {
                        return false;
                    }
                    if (IsRwHashEntryChanged(lrwc, managedThreadId))
                    {
                        lrwc = GetThreadRwCount(managedThreadId, false);
                    }
                }
                goto Label_013D;
            }
            ExitMyLock();
            return true;
        }

        public bool TryEnterUpgradeableReadLock(int millisecondsTimeout)
        {
            bool flag = false;
            try
            {
                flag = TryEnterUpgradeableReadLockCore(millisecondsTimeout);
            }
            finally
            {
                if (!flag)
                {
                }
            }
            return flag;
        }

        public bool TryEnterUpgradeableReadLock(TimeSpan timeout)
        {
            var totalMilliseconds = (long) timeout.TotalMilliseconds;
            if ((totalMilliseconds < -1L) || (totalMilliseconds > 0x7fffffffL))
            {
                throw new ArgumentOutOfRangeException("timeout");
            }
            var millisecondsTimeout = (int) timeout.TotalMilliseconds;
            return TryEnterUpgradeableReadLock(millisecondsTimeout);
        }

        private bool TryEnterUpgradeableReadLockCore(int millisecondsTimeout)
        {
            ReaderWriterCount threadRwCount;
            if (millisecondsTimeout < -1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }
            if (_fDisposed)
            {
                throw new ObjectDisposedException(null);
            }
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (!_fIsReentrant)
            {
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    throw new LockRecursionException("LockRecursionException_RecursiveUpgradeNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_RecursiveUpgradeNotAllowed"));
                }
                if (managedThreadId == _writeLockOwnerId)
                {
                    throw new LockRecursionException("LockRecursionException_UpgradeAfterWriteNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_UpgradeAfterWriteNotAllowed"));
                }
                EnterMyLock();
                threadRwCount = GetThreadRwCount(managedThreadId, true);
                if ((threadRwCount != null) && (threadRwCount.readercount > 0))
                {
                    ExitMyLock();
                    throw new LockRecursionException("LockRecursionException_UpgradeAfterReadNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_UpgradeAfterReadNotAllowed"));
                }
            }
            else
            {
                EnterMyLock();
                threadRwCount = GetThreadRwCount(managedThreadId, false);
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    threadRwCount.rc.Upgradecount++;
                    ExitMyLock();
                    return true;
                }
                if (managedThreadId == _writeLockOwnerId)
                {
                    _owners++;
                    _upgradeLockOwnerId = managedThreadId;
                    threadRwCount.rc.Upgradecount++;
                    if (threadRwCount.readercount > 0)
                    {
                        _fUpgradeThreadHoldingRead = true;
                    }
                    ExitMyLock();
                    return true;
                }
                if (threadRwCount.readercount > 0)
                {
                    ExitMyLock();
                    throw new LockRecursionException("LockRecursionException_UpgradeAfterReadNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_UpgradeAfterReadNotAllowed"));
                }
            }
            int spinCount = 0;
            Label_0139:
            if ((_upgradeLockOwnerId == -1) && (_owners < 0xffffffe))
            {
                _owners++;
                _upgradeLockOwnerId = managedThreadId;
            }
            else
            {
                if (spinCount < 20)
                {
                    ExitMyLock();
                    if (millisecondsTimeout == 0)
                    {
                        return false;
                    }
                    spinCount++;
                    SpinWait(spinCount);
                    EnterMyLock();
                    goto Label_0139;
                }
                if (_upgradeEvent == null)
                {
                    LazyCreateEvent(ref _upgradeEvent, true);
                    goto Label_0139;
                }
                if (WaitOnEvent(_upgradeEvent, ref _numUpgradeWaiters, millisecondsTimeout))
                {
                    goto Label_0139;
                }
                return false;
            }
            if (_fIsReentrant)
            {
                if (IsRwHashEntryChanged(threadRwCount, managedThreadId))
                {
                    threadRwCount = GetThreadRwCount(managedThreadId, false);
                }
                threadRwCount.rc.Upgradecount++;
            }
            ExitMyLock();
            return true;
        }

        public bool TryEnterWriteLock(int millisecondsTimeout)
        {
            bool flag = false;
            try
            {
                flag = TryEnterWriteLockCore(millisecondsTimeout);
            }
            finally
            {
                if (!flag)
                {
                }
            }
            return flag;
        }

        public bool TryEnterWriteLock(TimeSpan timeout)
        {
            var totalMilliseconds = (long) timeout.TotalMilliseconds;
            if ((totalMilliseconds < -1L) || (totalMilliseconds > 0x7fffffffL))
            {
                throw new ArgumentOutOfRangeException("timeout");
            }
            var millisecondsTimeout = (int) timeout.TotalMilliseconds;
            return TryEnterWriteLock(millisecondsTimeout);
        }

        private bool TryEnterWriteLockCore(int millisecondsTimeout)
        {
            ReaderWriterCount threadRwCount;
            if (millisecondsTimeout < -1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }
            if (_fDisposed)
            {
                throw new ObjectDisposedException(null);
            }
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            bool flag = false;
            if (!_fIsReentrant)
            {
                if (managedThreadId == _writeLockOwnerId)
                {
                    throw new LockRecursionException("LockRecursionException_RecursiveWriteNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_RecursiveWriteNotAllowed"));
                }
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    flag = true;
                }
                EnterMyLock();
                threadRwCount = GetThreadRwCount(managedThreadId, true);
                if ((threadRwCount != null) && (threadRwCount.readercount > 0))
                {
                    ExitMyLock();
                    throw new LockRecursionException("LockRecursionException_WriteAfterReadNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_WriteAfterReadNotAllowed"));
                }
            }
            else
            {
                EnterMyLock();
                threadRwCount = GetThreadRwCount(managedThreadId, false);
                if (managedThreadId == _writeLockOwnerId)
                {
                    threadRwCount.rc.Writercount++;
                    ExitMyLock();
                    return true;
                }
                if (managedThreadId == _upgradeLockOwnerId)
                {
                    flag = true;
                }
                else if (threadRwCount.readercount > 0)
                {
                    ExitMyLock();
                    throw new LockRecursionException("LockRecursionException_WriteAfterReadNotAllowed");
                    //throw new LockRecursionException(SR.GetString("LockRecursionException_WriteAfterReadNotAllowed"));
                }
            }
            int spinCount = 0;
            Label_00EC:
            if (IsWriterAcquired())
            {
                SetWriterAcquired();
            }
            else
            {
                if (flag)
                {
                    uint numReaders = GetNumReaders();
                    if (numReaders == 1)
                    {
                        SetWriterAcquired();
                        goto Label_01DD;
                    }
                    if ((numReaders == 2) && (threadRwCount != null))
                    {
                        if (IsRwHashEntryChanged(threadRwCount, managedThreadId))
                        {
                            threadRwCount = GetThreadRwCount(managedThreadId, false);
                        }
                        if (threadRwCount.readercount > 0)
                        {
                            SetWriterAcquired();
                            goto Label_01DD;
                        }
                    }
                }
                if (spinCount < 20)
                {
                    ExitMyLock();
                    if (millisecondsTimeout == 0)
                    {
                        return false;
                    }
                    spinCount++;
                    SpinWait(spinCount);
                    EnterMyLock();
                    goto Label_00EC;
                }
                if (flag)
                {
                    if (_waitUpgradeEvent != null)
                    {
                        if (!WaitOnEvent(_waitUpgradeEvent, ref _numWriteUpgradeWaiters, millisecondsTimeout))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        LazyCreateEvent(ref _waitUpgradeEvent, true);
                    }
                    goto Label_00EC;
                }
                if (_writeEvent == null)
                {
                    LazyCreateEvent(ref _writeEvent, true);
                    goto Label_00EC;
                }
                if (WaitOnEvent(_writeEvent, ref _numWriteWaiters, millisecondsTimeout))
                {
                    goto Label_00EC;
                }
                return false;
            }
            Label_01DD:
            if (_fIsReentrant)
            {
                if (IsRwHashEntryChanged(threadRwCount, managedThreadId))
                {
                    threadRwCount = GetThreadRwCount(managedThreadId, false);
                }
                threadRwCount.rc.Writercount++;
            }
            ExitMyLock();
            _writeLockOwnerId = managedThreadId;
            return true;
        }

        private bool WaitOnEvent(EventWaitHandle waitEvent, ref uint numWaiters, int millisecondsTimeout)
        {
            waitEvent.Reset();
            numWaiters++;
            _fNoWaiters = false;
            if (_numWriteWaiters == 1)
            {
                SetWritersWaiting();
            }
            if (_numWriteUpgradeWaiters == 1)
            {
                SetUpgraderWaiting();
            }
            bool flag = false;
            ExitMyLock();
            try
            {
                flag = waitEvent.WaitOne(millisecondsTimeout);
                //flag = waitEvent.WaitOne(millisecondsTimeout, false);
            }
            finally
            {
                EnterMyLock();
                numWaiters--;
                if (((_numWriteWaiters == 0) && (_numWriteUpgradeWaiters == 0)) &&
                    ((_numUpgradeWaiters == 0) && (_numReadWaiters == 0)))
                {
                    _fNoWaiters = true;
                }
                if (_numWriteWaiters == 0)
                {
                    ClearWritersWaiting();
                }
                if (_numWriteUpgradeWaiters == 0)
                {
                    ClearUpgraderWaiting();
                }
                if (!flag)
                {
                    ExitMyLock();
                }
            }
            return flag;
        }

        // Properties
    }
}