using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksWpfLibrary.ViewModels.Base
{
    /// <summary>
    /// A base view model that fires the property changed event as needed
    /// </summary>
    [AddINotifyPropertyChangedInterfaceAttribute]
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
