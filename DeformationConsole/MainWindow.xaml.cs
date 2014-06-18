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

        private NamedPipeClientStream pipe = null;
        
        public MainWindow()
        {
            InitializeComponent();
            logText("Awaiting connection...");
        }


        #region connecting

        /// <summary>
        /// Connect button event handler
        /// Connect to named pipe and start processing thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            var tmp = this.Cursor;
            pipe = new NamedPipeClientStream(".", "deformablepipe", PipeDirection.InOut);
            try
            {
                pipe.Connect(2000);
                this.Cursor = tmp;
                logText("Connection established");
                // #TODO
                //client = new Thread(() => pipeThread(pipe));
                //client.Start();
                connectedEnableGUI();
            }
            catch (Exception)
            {
                connectionStatusTextBox.Text = "Connection status: Timeout";
                logText("Connection timeout");
                this.ClearValue(MainWindow.CursorProperty);
            }
        }

        /// <summary>
        /// Enable GUI elements on named pipe connection
        /// </summary>
        private void connectedEnableGUI()
        {
            connectionSection.IsEnabled = false;            
            connectionStatusTextBox.Foreground = Brushes.DarkOliveGreen;
            connectionStatusTextBox.Text = "Connection status: Connected";
            logText("Client thread started, processing messages");
            modifySection.IsEnabled = true;
        }

        #endregion



        #region private_methods

        /// <summary>
        /// Add log text to the log area
        /// </summary>
        /// <param name="m"></param>
        private void logText(string m)
        {
            logTextBox.Text = "> [" + DateTime.Now.ToLongTimeString() + "] " + m + "\n" + logTextBox.Text;
        }

        /// <summary>
        /// Send command via named pipe
        /// </summary>
        /// <param name="cmd"></param>
        private void sendCommand(string cmd)
        {
            StreamWriter sr = new StreamWriter(pipe);
            sr.WriteLine(cmd);
            sr.Flush();
            logText(readPipe(pipe));
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

        #endregion



        #region buttonclick_events

        /// <summary>
        /// Stiffness command handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stiffnessButton_Click(object sender, RoutedEventArgs e)
        {
            float value;
            if (float.TryParse(stiffnessTextBox.Text, out value))
            {
                stiffnessTextBox.BorderBrush = Brushes.Green;
                sendCommand("set stiffness " + value.ToString());
            }
            else
            {
                stiffnessTextBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// Damping command handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dampingButton_Click(object sender, RoutedEventArgs e)
        {
            float value;
            if (float.TryParse(dampingTextBox.Text, out value))
            {
                dampingTextBox.BorderBrush = Brushes.Green;
                sendCommand("set damping " + value.ToString());
            }
            else
            {
                dampingTextBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// Gravity command handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gravityButton_Click(object sender, RoutedEventArgs e)
        {
            float value;
            if (float.TryParse(gravityTextBox.Text, out value))
            {
                gravityTextBox.BorderBrush = Brushes.Green;
                sendCommand("set gravity " + value.ToString());
            }
            else
            {
                gravityTextBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// Light position command handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lightPosButton_Click(object sender, RoutedEventArgs e)
        {
            float value;
            if (float.TryParse(lightPosTextBox.Text, out value))
            {
                lightPosTextBox.BorderBrush = Brushes.Green;
                sendCommand("set lightpos " + value.ToString());
            }
            else
            {
                lightPosTextBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// Light colour command handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lightColButton_Click(object sender, RoutedEventArgs e)
        {
            float value;
            if (float.TryParse(lightColTextBox.Text, out value))
            {
                lightColTextBox.BorderBrush = Brushes.Green;
                sendCommand("set lightcol " + value.ToString());
            }
            else
            {
                lightColTextBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// Add bunny command handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBunnyButton_Click(object sender, RoutedEventArgs e)
        {
            int value;
            if (int.TryParse(addBunnyTextBox.Text, out value))
            {
                addBunnyTextBox.BorderBrush = Brushes.Green;
                sendCommand("add bunny " + value.ToString());
            }
            else
            {
                addBunnyTextBox.BorderBrush = Brushes.Red;
            }
        }

        #endregion



        #region textchanged_events

        /// <summary>
        /// Stiffness text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stiffnessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            stiffnessTextBox.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Damping text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dampingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            dampingTextBox.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Gravity text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gravityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            gravityTextBox.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Light position text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lightPosTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lightPosTextBox.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Light colour text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lightColTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lightColTextBox.ClearValue(TextBox.BorderBrushProperty);
        }

        /// <summary>
        /// Add bunny text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBunnyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addBunnyTextBox.ClearValue(TextBox.BorderBrushProperty);
        }

        #endregion
    
    }
}