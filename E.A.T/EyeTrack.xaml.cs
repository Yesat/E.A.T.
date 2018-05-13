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
using Tobii.Interaction.Wpf;

namespace E.A.T
{
    /// <summary>
    /// Interaction logic for EyeTrack.xaml
    /// </summary>
    public partial class EyeTrack : Window
    {
        private MainWindow parent;
        public EyeTrack(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            //((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
        }

        /**
         * Overide of the base OnClosed
         * We disable the activation mode of tobii
         */
        protected override void OnClosed(EventArgs e)
        {
            if (((App)Application.Current).Host.Context.ConnectionState == Tobii.Interaction.Client.ConnectionState.Connected)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOff();
            }
            base.OnClosed(e);
        }

        public void setActif()
        {
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
        }
        /**
        * This function detect when a key is press down
        */
        private void EyeTrack_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                //((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
            }
        }

        /**
         * This function detect when a key is release
         */
        private void EyeTrack_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivation();
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOff();
                this.Visibility = Visibility.Hidden;
                //this.Close();
            }
        }

        private void activation(object sender, ActivationRoutedEventArgs e)
        {
            String name = ((Rectangle)sender).Name;
            switch (name)
            {
                case "save":
                    this.parent.SendCommand("save",null);
                    break;
                case "load":
                    this.parent.SendCommand("load", null);
                    break;
                case "style":
                    this.parent.SendCommand("style", null);
                    break;
                case "spell":
                    this.parent.SendCommand("spell", null);
                    break;
                case "exit":
                    this.parent.SendCommand("exit", null); ;
                    break;
            }
        }

        public MainWindow GetParent()
        {
            return this.parent;
        }
    }
}
