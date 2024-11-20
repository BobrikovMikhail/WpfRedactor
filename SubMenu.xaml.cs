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
using System.Windows.Shapes;

namespace WpfLab1
{
    /// <summary>
    /// Логика взаимодействия для SubMenu.xaml
    /// </summary>
    public partial class SubMenu : Window
    {
       
        public SubMenu()
        {
            InitializeComponent();
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            Transfer.Background = Brushes.Green;
            TransferX.Visibility = Visibility.Visible;
            TransferLabelY.Visibility = Visibility.Visible;
            TransferLabelX.Visibility = Visibility.Visible;
            TransferY.Visibility = Visibility.Visible;
            ScaleBtn.Background = Brushes.LightGray;
            TransferCofirmBtn.Visibility = Visibility.Visible;
            ScaleConfBtn.Visibility = Visibility.Hidden;


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TransferCofirmBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.TransferLine(TransferX.Text, TransferY.Text);
        }

        private void ScaleBtn_Click(object sender, RoutedEventArgs e)
        {
            ScaleConfBtn.Visibility = Visibility.Visible;
          ScaleBtn.Background = Brushes.Green;
            ScaleX.Visibility = Visibility.Visible;
            ScaleY.Visibility = Visibility.Visible;
            Transfer.Background = Brushes.LightGray;
            TransferX.Visibility = Visibility.Hidden;
            TransferLabelY.Visibility = Visibility.Hidden;
            TransferLabelX.Visibility = Visibility.Hidden;
            TransferY.Visibility = Visibility.Hidden;
            TransferCofirmBtn.Visibility = Visibility.Hidden;

        }

        private void ScaleConfBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ScaleLine(ScaleX.Text, ScaleY.Text);
        }

        private void MirrorX_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MirrorX();
        }

        private void MirrorY_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MirrorY();
        }

        private void MirrorSC_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MirroSC();
        }
    }
}
