/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/28/2011
 * Time: 10:55
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using MVVM;
using System;
using System.Windows.Input;

namespace RamTune.UI
{
    /// <summary>
    /// Description of RelayCommand.
    /// </summary>
    public class RelayCommand : ICommand
    {
	    #region Fields
	
	    readonly Action<object> _execute;
	    readonly Func<object, bool> _canExecute;        
	
	    #endregion // Fields
	
	    #region Constructors
	
	    public RelayCommand(Action<object> execute)
	    : this(execute, null)
	    {
	    }
	
	    public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
	    {
	        if (execute == null)
	            throw new ArgumentNullException("execute");
	
	        _execute = execute;
	        _canExecute = canExecute;           
	    }
	    #endregion // Constructors
	
	    #region ICommand Members
	
	    public bool CanExecute(object parameter)
	    {
	        return _canExecute == null ? true : _canExecute(parameter);
	    }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
	        _execute(parameter);
	    }
	
	    #endregion // ICommand Members
	}

	public static class CommandHelper
	{
		/// <summary>
		/// Creates a new DelegateCommand the first time it is called and then re-use that command.
		/// </summary>
		/// <param name="cmd">The reference of the command object to initialize.</param>
		/// <param name="execute">The method this command will execute.</param>
		/// <param name="canExecute">The method returning whether command can execute.</param>
		/// <returns>The initialized command object.</returns>
		public static RelayCommand InitCommand(this ViewModelBase obj, ref RelayCommand cmd, Action<object> execute, Func<object, bool> canExecute)
		{
			if (cmd == null)
				cmd = new RelayCommand(execute, canExecute);
			return cmd;
		}
	}

	}
