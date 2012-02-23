using System;

namespace Lightstreamer.DotNet.Client.Test {

	public class TestTableListenerForExtended : IHandyTableListener {

        public TestTableListenerForExtended() { }

        private string NotifyUpdate(IUpdateInfo update)
        {
            return update.Snapshot ? "snapshot" : "update";
        }

        private string NotifyValue(IUpdateInfo update, string fldName)
        {
            string newValue = update.GetNewValue(fldName);
            string notify = " " + fldName + " = " + (newValue != null ? newValue : "null");
            if (update.IsValueChanged(fldName))
            {
                string oldValue = update.GetOldValue(fldName);
                notify += " (was " + (oldValue != null ? oldValue : "null") + ")";
            }
            return notify;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            Console.WriteLine(NotifyUpdate(update) +
                            " for " + itemName + ":" +
                            NotifyValue(update, "last_price") +
                            NotifyValue(update, "time") +
                            NotifyValue(update, "pct_change"));
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
            Console.WriteLine("end of snapshot for " + itemName);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            Console.WriteLine(lostUpdates + " updates lost for " + itemName);
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
            Console.WriteLine("unsubscr " + itemName);
        }

        public void OnUnsubscrAll()
        {
            Console.WriteLine("unsubscr table");
        }
    }

	public class TestTableListenerForMultiple : IHandyTableListener {
        public TestTableListenerForMultiple() { }

        private string NotifyUpdate(IUpdateInfo update)
        {
            return update.Snapshot ? "snapshot" : "update";
        }

        private string NotifyValue(IUpdateInfo update, string fldName)
        {
            string newValue = update.GetNewValue(fldName);
            string notify = " " + fldName + " = " + (newValue != null ? newValue : "null");
            if (update.IsValueChanged(fldName))
            {
                string oldValue = update.GetOldValue(fldName);
                notify += " (was " + (oldValue != null ? oldValue : "null") + ")";
            }
            return notify;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            Console.WriteLine(NotifyUpdate(update) +
                            " for " + itemName + ":" +
                            NotifyValue(update, "last_price") +
                            NotifyValue(update, "time") +
                            NotifyValue(update, "pct_change"));
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
            Console.WriteLine("end of snapshot for " + itemName);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            Console.WriteLine(lostUpdates + " updates lost for " + itemName);
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
            Console.WriteLine("unsubscr " + itemName);
        }

        public void OnUnsubscrAll()
        {
            Console.WriteLine("OnUnsubscrAll invoked (???)");
        }
    }

	public class TestTableListenerForSimple : IHandyTableListener {

        public TestTableListenerForSimple() { }

        private string NotifyUpdate(IUpdateInfo update)
        {
            return update.Snapshot ? "snapshot" : "update";
        }

        private string NotifyValue(IUpdateInfo update, int fldPos, string fldText)
        {
            string newValue = update.GetNewValue(fldPos);
            string notify = " " + fldText + " = " + (newValue != null ? newValue : "null");
            if (update.IsValueChanged(fldPos))
            {
                string oldValue = update.GetOldValue(fldPos);
                notify += " (was " + (oldValue != null ? oldValue : "null") + ")";
            }
            return notify;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            Console.WriteLine(NotifyUpdate(update) +
                            " for " + itemPos + ":" +
                            NotifyValue(update, 1, "last_price") +
                            NotifyValue(update, 2, "time") +
                            NotifyValue(update, 3, "pct_change"));
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
            Console.WriteLine("end of snapshot for " + itemPos);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            Console.WriteLine(lostUpdates + " updates lost for " + itemPos);
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
            Console.WriteLine("unsubscr " + itemPos);
        }

        public void OnUnsubscrAll()
        {
            Console.WriteLine("unsubscr table");
        }
    }

    public class TestPortfolioListenerForExtended : IHandyTableListener
    {

        public TestPortfolioListenerForExtended() { }

        private string NotifyUpdate(IUpdateInfo update)
        {
            return update.Snapshot ? "snapshot" : "update";
        }

        private string NotifyValue(IUpdateInfo update, string fldName)
        {
            string newValue = update.GetNewValue(fldName);
            string notify = " " + fldName + " = " + (newValue != null ? newValue : "null");
            if (update.IsValueChanged(fldName))
            {
                string oldValue = update.GetOldValue(fldName);
                notify += " (was " + (oldValue != null ? oldValue : "null") + ")";
            }
            return notify;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            Console.WriteLine(NotifyUpdate(update) +
                            " for " + itemName + ":" +
                            " key = " + update.GetNewValue("key") +
                            " command = " + update.GetNewValue("command") +
                            NotifyValue(update, "qty"));
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
            Console.WriteLine("end of snapshot for " + itemName);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            Console.WriteLine(lostUpdates + " updates lost for " + itemName);
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
            Console.WriteLine("unsubscr " + itemName);
        }

        public void OnUnsubscrAll()
        {
            Console.WriteLine("unsubscr table");
        }
    }

    public class TestPortfolioListenerForSimple : IHandyTableListener
    {

        public TestPortfolioListenerForSimple() { }

        private string NotifyUpdate(IUpdateInfo update)
        {
            return update.Snapshot ? "snapshot" : "update";
        }

        private string NotifyValue(IUpdateInfo update, int fldPos, string fldText)
        {
            string newValue = update.GetNewValue(fldPos);
            string notify = " " + fldText + " = " + (newValue != null ? newValue : "null");
            if (update.IsValueChanged(fldPos))
            {
                string oldValue = update.GetOldValue(fldPos);
                notify += " (was " + (oldValue != null ? oldValue : "null") + ")";
            }
            return notify;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            Console.WriteLine(NotifyUpdate(update) +
                            " for " + itemPos + ":" +
                            " key = " + update.GetNewValue(1) +
                            " command = " + update.GetNewValue(2) +
                            NotifyValue(update, 3, "qty"));
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
            Console.WriteLine("end of snapshot for " + itemPos);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            Console.WriteLine(lostUpdates + " updates lost for " + itemPos);
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
            Console.WriteLine("unsubscr " + itemPos);
        }

        public void OnUnsubscrAll()
        {
            Console.WriteLine("unsubscr table");
        }
    }
}
