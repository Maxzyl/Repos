using System;
using System.Windows.Input;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class CommandViewModel : NotifyBase
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            this.DisplayName = displayName;
            this.Command = command;
        }

        public ICommand Command { get; private set; }

        public string DisplayName { get; set; }
    }
}