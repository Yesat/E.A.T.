using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace E.A.T
{
    /**
     * This class is use to put eye interaction on the item list
     */
    public class ItemCommand : ICommand
    {
        private Window vm;
        private String type;
        public ItemCommand(Window vm, String type)
        {
            this.vm = vm;
            this.type = type;
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
            switch (this.type)
            {
                case "spell":
                    ((SpellCheckWindow)vm).ItemOfList(parameter);
                    break;
                case "style":
                    ((StyleWindow)vm).fontSizeActiv(parameter);
                    break;
            }
        }
    }
}
