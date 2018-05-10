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
        private SpellCheckWindow spellWindow;
        private bool fontBoxHandle = true;

        public MainWindow()
        {
            eyeWindow = new EyeTrack(this);
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
         * Return the spell checker of the text box
         */
        private SpellCheck GetSpellCheck()
        {
            return this.TextEdit.SpellCheck;
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
                if(this.spellWindow != null)
                {
                    this.spellWindow.Close();
                    this.spellWindow = null;
                }
                this.spellWindow = new SpellCheckWindow(this);
                this.spellWindow.Visibility = Visibility.Visible;
                this.spellWindow.Corrections();
            }
            catch (NullReferenceException)
            {
                return;
            }
            

        }

        private void font_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            fontBoxHandle = !cmb.IsDropDownOpen;
            setFontFamily();
        }

        private void setFontFamily()
        {
            this.TextEdit.Focus();// We need to take the focus for the case where there is no selection (otherwise the font is not take in account when we type text)
            ComboBoxItem item = this.font.SelectedItem as ComboBoxItem;
            FontFamily nf = new FontFamily(item.Content.ToString());
            this.TextEdit.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, nf);
        }

    }
}