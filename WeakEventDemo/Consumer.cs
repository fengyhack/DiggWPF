using System;
using System.Windows;

namespace WeakEventDemo
{
    public class Consumer : IWeakEventListener
    {
        public void DoSomething(string message)
        {
            //
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            var args = e as MessageEventArgs;
            DoSomething(args.Message);
            return true;
        }
    }
}
