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

        /*
         * Enable the activation node of tobii
         */
        public void setActif()
        {
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
        }
        /**
        * This function detect when a key is press down
        */
        private void EyeTrack_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift)
            {
                //((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
            }
        }

        /**
         * This function detect when a key is release
         */
        private void EyeTrack_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift)//HIde the eye menu
            {
                ((App)Application.Current).Host.Commands.Input.SendActivation();
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOff();
                this.Visibility = Visibility.Hidden;
            }
        }

        /*
         * Get the option selected by user and perform an action
         */
        private void activation(object sender, ActivationRoutedEventArgs e)
        {
            String name = ((Rectangle)sender).Name;
            switch (name)
            {
                case "save"://Save the document
                    this.parent.SendCommand("save",null);
                    break;
                case "load"://Open an other document
                    this.parent.SendCommand("load", null);
                    break;
                case "style"://Open style window
                    this.parent.SendCommand("style", null);
                    break;
                case "spell"://Open spell checker
                    this.parent.SendCommand("spell", null);
                    break;
                case "exit"://Exit the application
                    this.parent.SendCommand("exit", null); ;
                    break;
            }
        }

        /*
         * Return the parent window of the object
         */
        public MainWindow GetParent()
        {
            return this.parent;
        }
    }
}
