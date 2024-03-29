﻿using CompanyManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompanyManagement.UI.TechLead
{
    /// <summary>
    /// Interaction logic for TechLeadProjectPage.xaml
    /// </summary>
    public partial class TechLeadProjectPage : Page
    {
        public TechLeadProjectPage()
        {
            InitializeComponent();
            DataContext = new TechLeadProjectViewModel();
        }
        private void ProjectButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            NavigationService.Navigate(new TechLeadStagePage());
        }
    }
}
