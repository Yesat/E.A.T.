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
using Tobii.Interaction.Wpf;

namespace E.A.T
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

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Click!");
        }

        private void HelloCMD(object sender, HasGazeChangedRoutedEventArgs e)
        {
            Console.WriteLine("From: "+((Shape)sender).Name);
            if (e.HasGaze)
            {
                Console.WriteLine("I am here");
            }
            else
            {
                Console.WriteLine("Good bye");
            }
        }

        private void Hello2(object sender, ActivationRoutedEventArgs e)
        {
            Console.WriteLine("From: " + ((Shape)sender).Name);
            
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
            }
        }

        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivation();
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOff();
            }
        }
    }

}
