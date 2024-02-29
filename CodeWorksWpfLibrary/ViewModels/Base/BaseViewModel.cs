using PropertyChanged;
using System.ComponentModel;

namespace CodeWorksWpfLibrary.ViewModels.Base
{
    /// <summary>
    /// A base view model that fires the property changed event as needed
    /// </summary>
    [AddINotifyPropertyChangedInterfaceAttribute]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
