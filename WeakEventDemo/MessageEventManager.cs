using System;
using System.Windows;
using System.IO;

namespace WeakEventDemo
{
    public class MessageEventManager : WeakEventManager
    {
        private static StreamWriter textLogWriter;

        private MessageEventManager()
        {
            textLogWriter = new StreamWriter($"{DateTime.Now:yyyy-MM-dd}.log", false);
            textLogWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] Logger Created");
        }

        public static void AddListener(object source, IWeakEventListener listener)
        {
            if(source == null || listener == null)
            {
                throw new ArgumentNullException();
            }
            Current.ProtectedAddListener(source, listener);
            textLogWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] AddListener");
        }

        public static void RemoveListener(object source, IWeakEventListener listener)
        {
            if (source == null || listener == null)
            {
                throw new ArgumentNullException();
            }
            Current.ProtectedRemoveListener(source, listener);
            textLogWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] RemoveListener");
        }

        protected override void StartListening(object source)
        {
            var producer = (Producer)source;
            producer.MessageProduced += Producer_MessageProduced;
            textLogWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] StartListening");
        }

        protected override void StopListening(object source)
        {
            var producer = (Producer)source;
            producer.MessageProduced -= Producer_MessageProduced;
            textLogWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] StopListening");
            textLogWriter?.Close();
            if (textLogWriter != null)
            {
                textLogWriter = null;
            }
        }

        private static MessageEventManager Current
        {
            get
            {
                var type = typeof(MessageEventManager);
                var manager = GetCurrentManager(type);
                if(manager == null)
                {
                    manager = new MessageEventManager();
                    SetCurrentManager(type, manager);
                }
                return (MessageEventManager)manager;
            }
        }

        private void Producer_MessageProduced(object sender, MessageEventArgs e)
        {
            DeliverEvent(sender, e);
            textLogWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] DeliverEvent, {e.Message}");
        }
    }
}
