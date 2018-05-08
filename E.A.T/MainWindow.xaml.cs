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
        private EyeTrack eyeWindow;
        private SpellCheck spellWindow;

        public MainWindow()
        {
            eyeWindow = new EyeTrack();
            InitializeComponent();
        }

        /**
         * We need this override OnClosed otherwise the eyetrack window is not properly closed
         */
        protected override void OnClosed(EventArgs e)
        {
            this.eyeWindow.Close();
            if(this.spellWindow != null)
            {
                this.spellWindow.Close();
            }
            base.OnClosed(e);
        }


        /**
         * This function detect when a key is press down
         */
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                this.eyeWindow.Visibility = Visibility.Visible;
            }
           
        }

        /**
         * This function detect when a key is release
         */
        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                //((App)Application.Current).Host.Commands.Input.SendActivation();
                //((App)Application.Current).Host.Commands.Input.SendActivationModeOff();
                this.eyeWindow.Visibility = Visibility.Hidden;
            }
        }

        private void SpellButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.spellWindow = new SpellCheck();
                this.spellWindow.Visibility = Visibility.Visible;
                this.spellWindow.ToCorrect(this.TextEdit.GetSpellingError(this.TextEdit.CaretPosition));
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No suggestions");
            }
            

        }
    }

}
