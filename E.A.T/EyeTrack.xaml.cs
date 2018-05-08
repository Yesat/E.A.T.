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
        public EyeTrack()
        {
            this.Visibility = Visibility.Hidden;
            InitializeComponent();
        }

        /**
        * This function detect when a key is press down
        */
        private void EyeTrack_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                this.Visibility = Visibility.Visible;
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
            }
        }
    }
}
