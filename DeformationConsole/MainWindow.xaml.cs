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

namespace DeformationConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

/*        static void Main(string[] args)
        {
            Console.WriteLine("NamedPipe client started.");

            using (var pipe = new NamedPipeClientStream(".", "deformablepipe", PipeDirection.InOut))
            {
                try
                {
                    pipe.Connect(3000);
                    Console.WriteLine("NamedPipe client connected.");

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
                catch (Exception)
                {
                    Console.WriteLine("Connect failed."); ;
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
*/