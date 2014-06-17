using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO.Pipes;
using System.IO;

namespace DeformationConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Thread client = null;
        
        public MainWindow()
        {
            InitializeComponent();
            logText("Awaiting connection...");
        }

        /// <summary>
        /// Connect button event handler
        /// Connect to named pipe and start processing thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            using (var pipe = new NamedPipeClientStream(".", "deformablepipe", PipeDirection.InOut))
            {
                var tmp = this.Cursor;
                try
                {
                    this.Cursor = Cursors.Wait;
                    pipe.Connect(2000);
                    this.Cursor = tmp;
                    logText("Connect established");
                    client = new Thread(() => pipeThread(pipe));
                    client.Start();
                    connectedEnableGUI();
                }
                catch (Exception)
                {
                    connectionStatusTextBox.Text = "Connection status: Timeout";
                    logText("Connect timeout");
                    this.Cursor = tmp;
                }
            }
        }


        /// <summary>
        /// Enable GUI elements on named pipe connection
        /// </summary>
        private void connectedEnableGUI()
        {
            connectionStatusTextBox.Foreground = Brushes.DarkOliveGreen;
            connectionStatusTextBox.Text = "Connection status: Connected";
            logText("Clien thread started, processing messages");
            modifyLabel.IsEnabled = true;
            modifyGrid.IsEnabled = true;
        }

        /// <summary>
        /// Add log text to the log area
        /// </summary>
        /// <param name="m"></param>
        private void logText(string m)
        {
            logTextBox.Text = "> [" + DateTime.Now.ToLongTimeString() + "] " + m + "\n" + logTextBox.Text;
        }

        /// <summary>
        /// Send and receive messages on the pipe
        /// </summary>
        private void pipeThread(NamedPipeClientStream pipe)
        {
            using (StreamWriter sr = new StreamWriter(pipe))
            {
                while (true)
                {
                    sr.WriteLine(Console.ReadLine());
                    sr.Flush();
                    Console.WriteLine("result: [" + readPipe(pipe) + "]");
                }
            }
        }

        /// <summary>
        /// Read message from pipe buffer, then convert it to string.
        /// </summary>
        /// <param name="pipe">The input pipe to read from.</param>
        /// <returns>The pipe buffer contents in string representation.</returns>
        private static string readPipe(NamedPipeClientStream pipe)
        {
            byte[] message = new byte[512];
            List<byte> text = new List<byte>();
            pipe.Read(message, 0, 512);
            foreach(var b in message)
            {
                if (b != 0)
                {
                    text.Add(b);
                }
            }
            return System.Text.Encoding.ASCII.GetString(text.ToArray());
        }
    }
}