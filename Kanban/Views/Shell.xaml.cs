using Kanban.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;

namespace Kanban.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell 
    {
        public Shell()
        {
            InitializeComponent();
            Closing += Shell_Closing;
        }

        private void Shell_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = (ShellViewModel)DataContext;
            if (vm.HasChanges)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    vm.SaveCommand.Execute();
                }
            }
            
        }
    }
}
