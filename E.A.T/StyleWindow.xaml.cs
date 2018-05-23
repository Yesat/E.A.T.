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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StyleWindow : Window
    {
        private MainWindow parent;
        public StyleWindow(MainWindow parent)
        {
            this.parent = parent;            
            InitializeComponent();
            String usedFontFamily = ((ComboBoxItem)this.parent.font.SelectedItem).Name;
            String usedFontSize = this.parent.TextEdit.FontSize.ToString();
            Style style = this.FindResource("ItemList") as Style;
            this.fontsize.ItemContainerStyle = style;
            /**
             * Creation of the list of size
             */
            foreach (double size in this.parent.FontSizeList)
            {
                //Creation of the item
                TextBlock word = new TextBlock();
                word.Text = size.ToString();
                word.SetIsActivatable(true);
                word.SetIsTentativeFocusEnabled(true);
                word.SetActivatedCommand(new ItemCommand(this, "style"));
                word.FontSize = 18;
                //Add it to the itemlist
                this.fontsize.Items.Add(word);
                if (word.Text == usedFontSize)
                {
                    this.fontsize.SelectedItem = word;
                }
                
                
            }
            foreach(ListBoxItem item in this.fontfamily.Items)
            {
                if(item.Name == usedFontFamily)
                {
                    this.fontfamily.SelectedItem = item;
                }
            }
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
        }

        /**
        * Overide of the base OnClosed
        * We disable the activation mode of tobii
        */
        protected override void OnClosed(EventArgs e)
        {
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOff();
            base.OnClosed(e);
        }

        /**
        * This function detect when a key is press down
        */
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.RightAlt:
                    if (!e.IsRepeat) {
                        ((App)Application.Current).Host.Commands.Input.SendPanningBegin();//active eye panning
                    }
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
                case Key.RightAlt:
                    ((App)Application.Current).Host.Commands.Input.SendPanningEnd();//disable eye panning
                    break;
                case Key.Space:
                    ((App)Application.Current).Host.Commands.Input.SendActivation();
                    ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                    break;

            }
        }

        private void fontFamilyActiv(object sender, ActivationRoutedEventArgs e)
        {
            this.fontfamily.SelectedItem = ((ListBoxItem)sender);
        }

        public void fontSizeActiv(object sender)
        {
            this.fontsize.SelectedItem = ((ActivatedArgs)sender).Interactor.Element;
        }

        private void ValidButton(object sender, ActivationRoutedEventArgs e)
        {
            string bt_name = ((Button)sender).Name;

            switch (bt_name)
            {
                case "bt_ok":
                    String family = ((String)((ListBoxItem)this.fontfamily.SelectedItem).Content);
                    String size = ((TextBlock)(this.fontsize.SelectedItem)).Text.ToString();
                    this.parent.SendCommand("fontsize",size);
                    this.parent.SendCommand("fontfamily", family);
                    this.Close();
                    break;
                case "bt_quit":
                    this.Close();
                    break;
            }

        }
    }    
}
