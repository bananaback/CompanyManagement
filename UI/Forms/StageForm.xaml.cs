using CompanyManagement.ViewModels;
using System.Windows;

namespace CompanyManagement.UI.Forms
{
    /// <summary>
    /// Interaction logic for StageForm.xaml
    /// </summary>
    public partial class StageForm : Window
    {
        public StageForm(StagePageViewModel spvm)
        {
            InitializeComponent();
            this.DataContext = spvm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
