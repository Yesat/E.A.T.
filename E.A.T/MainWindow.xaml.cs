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
        private StyleWindow styleWindow;
        private bool fontBoxHandle = true;

        private List<double> fontSizeList = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        public List<double> FontSizeList
        {
            get
            {
                return this.fontSizeList;
            }
        }

        public MainWindow()
        {
            this.eyeWindow = new EyeTrack(this);
            InitializeComponent();
            this.font_size.ItemsSource = this.fontSizeList;
            this.font_size.SelectedItem = (double)12;
        }

        /**
         * We need this override OnClosed otherwise the eyetrack window is not properly closed
         */
        protected override void OnClosed(EventArgs e)
        {
            this.eyeWindow.Close();
            if (this.spellWindow != null)
            {
                this.spellWindow.Close();
            }
            if (this.styleWindow != null)
            {
                this.styleWindow.Close();
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
                ((App)Application.Current).Host.Commands.Input.SendPanningBegin();
            }
            if (e.Key == Key.Space)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                if (((App)Application.Current).Host.Context.ConnectionState == Tobii.Interaction.Client.ConnectionState.Connected)
                {
                    if (TextEdit.GetHasTentativeActivationFocus()==true)
                    {
                        try
                        {
                            if (this.spellWindow != null)
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
                    else
                    {
                        this.eyeWindow.Visibility = Visibility.Visible;
                        //((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                        this.eyeWindow.Focus();
                        this.eyeWindow.setActif();
                    }
                    
                }
            }

        }

        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)
            {
                ((App)Application.Current).Host.Commands.Input.SendPanningEnd();
            }
            if (e.Key == Key.Space)
            {
                this.eyeWindow.Visibility = Visibility.Hidden;
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

        private void size_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            fontBoxHandle = !cmb.IsDropDownOpen;
            setFontSize();
        }

        private void setFontSize()
        {
            this.TextEdit.Focus();// We need to take the focus for the case where there is no selection (otherwise the font is not take in account when we type text)
            double item = (double)this.font_size.SelectedItem;
            this.TextEdit.Selection.ApplyPropertyValue(Inline.FontSizeProperty, item);
        }

        public void SendCommand(String cmd, String arg)
        {
            switch (cmd)
            {
                case "spell":
                    this.SpellButton_Click(this, null);
                    break;
                case "style":
                    this.styleWindow = new StyleWindow(this);
                    this.styleWindow.Visibility = Visibility.Visible;
                    break;
                case "fontsize":
                    this.TextEdit.Focus();
                    this.TextEdit.Selection.ApplyPropertyValue(Inline.FontSizeProperty, Convert.ToDouble(arg));
                    this.font_size.SelectedItem = Convert.ToDouble(arg);
                    break;
                case "fontfamily":
                    this.TextEdit.Focus();
                    FontFamily nf = new FontFamily(arg);
                    this.TextEdit.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, nf);
                    foreach(ComboBoxItem item in this.font.Items)
                    {
                        if(item.Content.ToString() == arg)
                        {
                            this.font.SelectedItem = item;
                        }
                    }
                    break;
                case "exit":
                    this.Close();
                    break;
                case "save":
                    this.Save_Executed(this, null);
                    break;
                case "load":
                    this.Open_Executed(this, null);
                    break;
                    
            }
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if((bool)this.eyeBox.IsChecked)
            {
                try
                {
                    ((App)Application.Current).Host.EnableConnection();
                    Thickness margin = TextEdit.Margin;
                    margin.Left = 119;
                    TextEdit.Margin = margin;
                    TextEdit.Width = 663;
                    menuRect.Visibility = Visibility.Visible;
                    menuText.Visibility = Visibility.Visible;
                }
                catch (NullReferenceException) { }
                
            }
            else
            {
                try { 
                    ((App)Application.Current).Host.DisableConnection();
                    menuRect.Visibility = Visibility.Hidden;
                    menuText.Visibility = Visibility.Hidden;
                    Thickness margin = TextEdit.Margin;
                    margin.Left = 10;
                    TextEdit.Width = 772;
                    TextEdit.Margin = margin;
                }
                catch (NullReferenceException) { }
        }
        }

        private void TextEdit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GazeArea_Activated(object sender, ActivationRoutedEventArgs e)
        {

        }
    }
}