using CodeWorksWpfLibrary.Interfaces;
using CodeWorksWpfLibrary.ViewModels;
using System.Windows;

namespace CodeWorksWpfLibrary.Views
{
    /// <summary>
    /// Interaction logic for SelectJobView.xaml
    /// </summary>
    public partial class SelectJobView : Window
    {
        public SelectJobView()
        {
            InitializeComponent();

            this.DataContext = new SelectJobViewModel();

            Loaded += SelectJobView_Loaded;
        }

        private void SelectJobView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindow viewModel)
            {
                viewModel.Close += () =>
                {
                    this.Close();
                };

                viewModel.Cancel += () =>
                {
                    this.Close();
                };
            }
        }
    }
}
