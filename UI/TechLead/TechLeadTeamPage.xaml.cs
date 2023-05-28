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
    /// Interaction logic for TechLeadTeamPage.xaml
    /// </summary>
    public partial class TechLeadTeamPage : Page
    {
        public TechLeadTeamPage()
        {
            InitializeComponent();
            this.DataContext = new TechLeadTeamViewModel();
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void TeamItemClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            NavigationService.Navigate(new TechLeadTeamMemberPage());
        }
    }
}
