using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab_3_Assembly_Browser
{
    class AssemblyBrowseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action action; 
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action();
        }
        public AssemblyBrowseCommand(Action action)
        {
            this.action = action;
        }
    }
    class AssemblyBrowserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<AssemblyBrowserLib.Nodable> _nodes;

        public ObservableCollection<AssemblyBrowserLib.Nodable> Nodes
        {
            get
            {
               return _nodes;
            }
            set
            {
                _nodes = value;
                OnAssemblyBrowsed();
            }
        }
        public void OnAssemblyBrowsed([CallerMemberName]string path = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(path));
        }
        public ICommand BtnOpenClick
        {
            get
            {
                return new AssemblyBrowseCommand(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Assembly|*.dll";
                    bool openFileResult = (bool)openFileDialog.ShowDialog();
                    if (openFileResult)
                    {
                        String filePath;
                        filePath = openFileDialog.FileName;
                        try
                        {

                            Assembly assemblyFile = Assembly.LoadFrom(filePath);
                            AssemblyBrowserLib.ReadableAssembly readableAssembly = new AssemblyBrowserLib.ReadableAssembly(assemblyFile);
                            var nodes = readableAssembly.GetChildren();
                            this.Nodes = new ObservableCollection<AssemblyBrowserLib.Nodable>(nodes);
                        } catch (Exception e)
                        {
                            //display error message
                        }
                    }
                });
            }
        }
    }
}
