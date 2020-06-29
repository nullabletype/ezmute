using System;
using System.Windows;
using System.Windows.Input;

namespace ezmute.Tools
{

    internal class ToggleMuteCommand : ICommand
    {
        // Disable warning about not being used as its required by the interface.
        #pragma warning disable CS0067
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        internal delegate void ToggleMute(object sender, RoutedEventArgs e);
        private ToggleMute toggleDel;

        public ToggleMuteCommand(ToggleMute del)
        {
            toggleDel = del;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            toggleDel(null, null);
        }
        
    }
}
