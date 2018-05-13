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
using System.Windows.Shapes;

namespace E.A.T
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StyleWindow : Window
    {
        private MainWindow parent;
        public StyleWindow(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        /**
        * This function detect when a key is press down
        */
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftShift:
                    ((App)Application.Current).Host.Commands.Input.SendPanningBegin();//active eye panning
                    break;

            }
        }

        /**
         * This function detect when a key is release
         */
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftShift:
                    ((App)Application.Current).Host.Commands.Input.SendPanningEnd();//disable eye panning
                    break;
                case Key.Space:
                    ((App)Application.Current).Host.Commands.Input.SendActivation();
                    ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                    break;

            }
        }
    }    
}
