using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AxisInstallerPackage.Wins
{
    /// <summary>
    /// Interaction logic for SqlServerInstancesWin.xaml
    /// </summary>
    public partial class SqlServerInstancesWin : Window
    {
        private List<string> servers;
        public string serverName { get; set; }


        public SqlServerInstancesWin(List<string> servers)
        {
            InitializeComponent();

            this.servers = servers;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadInstances();
        }

        private void loadInstances()
        {
            if (servers != null && servers.Count > 0)
            {
                foreach (var server in servers)
                {
                    Instances _name = new Instances();
                    _name.Name = server;
                    lbInstances.Items.Add(server);
                }
                lbInstances.SelectedIndex = 0;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (servers != null && servers.Count > 0)
            {
                this.serverName = lbInstances.SelectedItems[0].ToString();
            }
            this.Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Instances
    {
        public string Name { get; set; }
    }
}