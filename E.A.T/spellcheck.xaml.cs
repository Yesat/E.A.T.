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
    public partial class SpellCheck : Window
    {
        private SpellingError spErr;

        public SpellCheck()
        {
            InitializeComponent();
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
        }

        protected override void OnClosed(EventArgs e)
        {
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOff(); ;
            base.OnClosed(e);
        }

        public void ToCorrect(SpellingError spErr)
        {
            this.spErr = spErr;
            this.suggestions.Items.Clear();
            int i = 0;
            foreach (string sugg in this.spErr.Suggestions)
            {
                TextBlock word = new TextBlock();
                word.Text = sugg;
                word.Name = "sugg" + i.ToString();
                this.suggestions.Items.Add(word);
                Console.WriteLine(sugg);
                i++;
            }
        }

        public void SpellButton(object sender, ActivationRoutedEventArgs e)
        {
            string bt_name = ((Rectangle)sender).Name;
            Console.WriteLine(bt_name);
            switch (bt_name)
            {
                case "bt_quit":
                    this.Close();
                    break;
                case "bt_ignore":
                    this.Close();
                    break;
                case "bt_ok":
                    if(this.suggestions.SelectedItem != null)
                    {
                        string choice = ((TextBlock)this.suggestions.SelectedItem).Text;
                        this.spErr.Correct(choice);
                        this.Close();
                    }
                    break;

            }
            
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ((App)Application.Current).Host.Commands.Input.SendActivation();
                ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
            }
        }
    }
}
