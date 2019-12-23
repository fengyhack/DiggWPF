using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WeakEventDemo
{
    public partial class MainWindow : Window, IWeakEventListener
    {
        private bool isStarted;
        private Producer producer;

        public MainWindow()
        {
            InitializeComponent();

            isStarted = false;
            producer = new Producer();
            MessageEventManager.AddListener(producer, this);            
        }

        /// <summary>
        /// Implement <see cref="IWeakEventListener"/>
        /// </summary>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            listBox.Items.Insert(0, (e as MessageEventArgs).Message);
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isStarted)
            {
                listBox.Items.Clear();
                button.Content = "Start";
                isStarted = false;
            }
            else
            {
                button.Content = "Stop";
                isStarted = true;
                Task.Run(() =>
                {
                    while (isStarted)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            producer.CreateOne(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                        });
                        Thread.Sleep(200);
                    }
                });                
            }
        }
    }
}
