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
    /// Interaction logic for SpellCheck.xaml
    /// </summary>
    public partial class SpellCheck : Window
    {
        private SpellingError spErr;

        public SpellCheck()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOff(); ;
            base.OnClosed(e);
        }

        public void ToCorrect(SpellingError spErr)
        {
            ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
            this.spErr = spErr;
            this.suggestions.Items.Clear();
            int i = 0;
            Style style = this.FindResource("ItemList") as Style;

            this.suggestions.ItemContainerStyle = style;
            //Creation of the item list
            foreach (string sugg in this.spErr.Suggestions)
            {
                TextBlock word = new TextBlock();
                word.Text = sugg;
                word.Name = "sugg" + i.ToString();//Id of the suggestion
                word.SetIsActivatable(true);
                word.SetIsTentativeFocusEnabled(true);
                word.SetActivatedCommand(new ItemCommand(this));
                this.suggestions.Items.Add(word);
                Console.WriteLine(sugg);
                i++;
            }
        }

        /**
         * Callback for the item selection by eye
         */
        public void ItemOfList(object sender)
        {
            string itemName = ((ActivatedArgs)sender).Interactor.Element.Name;
            this.suggestions.SelectedItem = ((ActivatedArgs)sender).Interactor.Element;

        }

        public void ClickButton(object sender, RoutedEventArgs e)
        {
            this.SpellButton(sender, null);
        }
        /**
         * Trigger with the user validate a button on the spelling window
         */
        public void SpellButton(object sender, ActivationRoutedEventArgs e)
        {
            string bt_name = ((Button)sender).Name;
            switch (bt_name)
            {
                case "bt_quit": //Quit the splelling correction
                    this.Close();
                    break;
                case "bt_ignore": //Ignore on go to the next available error
                    this.Close();
                    break;
                case "bt_ok": //Validation of the correction
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
            switch (e.Key)
            {
                case Key.LeftShift:
                    ((App)Application.Current).Host.Commands.Input.SendPanningBegin();
                    break;

            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftShift:
                    ((App)Application.Current).Host.Commands.Input.SendPanningEnd();
                    break;
                case Key.Space:
                    ((App)Application.Current).Host.Commands.Input.SendActivation();
                    ((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
                    break;

            }
        }
    }

    /**
     * This class is use to put eye interaction on the item list
     */
    public class ItemCommand : ICommand
    {
        private SpellCheck vm;
        public ItemCommand(SpellCheck vm)
        {
            this.vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            vm.ItemOfList(parameter);
        }
    }
}
