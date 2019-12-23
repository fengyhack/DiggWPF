using System;

namespace WeakEventDemo
{
    public class Producer
    {
        public event EventHandler<MessageEventArgs> MessageProduced;

        public Producer()
        {
            //
        }

        public void CreateOne(string message)
        {
            MessageProduced?.Invoke(this, new MessageEventArgs(message));
        }
    }
}
