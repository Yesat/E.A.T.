using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Tobii.Interaction;
using Tobii.Interaction.Framework;
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

        //Mouse interaction for textbox scroll
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);//Set the position of the mouse

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);//Similate a mouse event

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);//Return the position of the mouse

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        //

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

            if (e.Key == Key.RightAlt)//Panning
            {
                ((App)Application.Current).Host.Commands.Input.SendPanningBegin();
            }
            if (e.Key == Key.RightShift)//Eye menu or spell check
            {
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                if (((App)Application.Current).Host.Context.ConnectionState == Tobii.Interaction.Client.ConnectionState.Connected)//Check if eye tracker is enable
                {
                    if (TextEdit.GetHasTentativeActivationFocus()==true)//If user look at text box, open spell check
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
                    else//Otherwise open the eye menu
                    {
                        this.eyeWindow.Visibility = Visibility.Visible;
                        this.eyeWindow.Focus();
                        this.eyeWindow.setActif();
                    }
                    
                }
            }

        }

        /**
         * This function detect when a key is release
         */
        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightAlt)//Panning
            {
                ((App)Application.Current).Host.Commands.Input.SendPanningEnd();
                //Get the eye position
                GazePointDataStream pointStream = ((App)Application.Current).Host.Streams.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);
                pointStream.IsEnabled = true;
                pointStream.Next += (s, eye) => EyeCaret(pointStream, eye);
            }
            if (e.Key == Key.RightShift)//Eye menu
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

        /*
         * Open the spell checker when the user click the button
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

        /*
         * Change the font family when the dopdown menu is closed
         */
        private void font_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            fontBoxHandle = !cmb.IsDropDownOpen;
            setFontFamily();
        }

        /*
         * Change the font size when the dopdown menu is closed
         */
        private void size_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            fontBoxHandle = !cmb.IsDropDownOpen;
            setFontSize();
        }

        /*
         * Set the font family of the text box
         */
        private void setFontFamily()
        {
            this.TextEdit.Focus();// We need to take the focus for the case where there is no selection (otherwise the font is not take in account when we type text)
            ComboBoxItem item = this.font.SelectedItem as ComboBoxItem;
            FontFamily nf = new FontFamily(item.Content.ToString());
            this.TextEdit.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, nf);
        }
       
        /*
         * Set the font size of the text box
         */
        private void setFontSize()
        {
            this.TextEdit.Focus();// We need to take the focus for the case where there is no selection (otherwise the font is not take in account when we type text)
            double item = (double)this.font_size.SelectedItem;
            this.TextEdit.Selection.ApplyPropertyValue(Inline.FontSizeProperty, item);
        }

        /*
         * Execute the command that a child window send
         */
        public void SendCommand(String cmd, String arg)
        {
            switch (cmd)
            {
                case "spell"://Open spell check
                    this.SpellButton_Click(this, null);
                    break;
                case "style"://Open style window
                    this.styleWindow = new StyleWindow(this);
                    this.styleWindow.Visibility = Visibility.Visible;
                    break;
                case "fontsize"://Change the font size
                    this.TextEdit.Focus();
                    this.TextEdit.Selection.ApplyPropertyValue(Inline.FontSizeProperty, Convert.ToDouble(arg));
                    this.font_size.SelectedItem = Convert.ToDouble(arg);
                    break;
                case "fontfamily"://Change the font family
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
                case "exit"://Exit the program
                    this.Close();
                    break;
                case "save"://Save the curent document
                    this.Save_Executed(this, null);
                    break;
                case "load"://Open an other document
                    this.Open_Executed(this, null);
                    break;
                    
            }
        }

        /**
         * This function monitor the modification of the eyetrack check box
         * Resize the textbox and menu and enable/disable the eyetrack
         */
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if((bool)this.eyeBox.IsChecked)
            {
                try
                {
                    ((App)Application.Current).Host.EnableConnection();
                    Thickness margin = TextEdit.Margin;
                    margin.Left = 142;
                    TextEdit.Margin = margin;
                    TextEdit.Width = 640;
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
                    TextEdit.Width = 769;
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

        /**
         * This function take the gaze position and do a mouse left click at this position do reset the caret position
         */
        private void EyeCaret(GazePointDataStream stream, StreamData<GazePointData> eye)
        {
            stream.IsEnabled = false;
            //Get the gaze position
            uint X = (uint)eye.Data.X;
            uint Y = (uint)eye.Data.Y;
            POINT lpPoint;
            //Get mouse position
            GetCursorPos(out lpPoint);
            int oldX = lpPoint.X;
            int oldY = lpPoint.Y;
            SetCursorPos((int)eye.Data.X, (int)eye.Data.Y);//Move the mouse where we gaze
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);//left click
            SetCursorPos(oldX, oldY);//Reset the mouse at the old position


        }
    }
}