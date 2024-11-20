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
            RolateBtn.Background = Brushes.LightGray;
            MirrorSC.Background = Brushes.LightGray;
            MirrorX.Background = Brushes.LightGray;
            MirrorY.Background = Brushes.LightGray;
            TransferCofirmBtn.Visibility = Visibility.Visible;
            ScaleConfBtn.Visibility = Visibility.Hidden;
            RolateConfBtn.Visibility = Visibility.Hidden;
            ScaleX.Visibility = Visibility.Hidden;
            ScaleY.Visibility = Visibility.Hidden;
            RolateTb.Visibility = Visibility.Hidden;



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
            RolateBtn.Background = Brushes.LightGray;
            MirrorSC.Background = Brushes.LightGray;
            MirrorX.Background = Brushes.LightGray;
            MirrorY.Background = Brushes.LightGray;
            TransferX.Visibility = Visibility.Hidden;
            TransferLabelY.Visibility = Visibility.Hidden;
            TransferLabelX.Visibility = Visibility.Hidden;
            TransferY.Visibility = Visibility.Hidden;
            TransferCofirmBtn.Visibility = Visibility.Hidden;
            ScaleConfBtn.Visibility = Visibility.Hidden;
            RolateConfBtn.Visibility = Visibility.Hidden;
            ScaleConfBtn.Visibility = Visibility.Visible;
            RolateTb.Visibility = Visibility.Hidden;

        }

        private void ScaleConfBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ScaleLine(ScaleX.Text, ScaleY.Text);
        }

        private void MirrorX_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MirrorX();
            MirrorX.Background = Brushes.Green;
            MirrorY.Background = Brushes.LightGray;
            MirrorSC.Background = Brushes.LightGray;
        }

        private void MirrorY_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MirrorY();
            MirrorY.Background = Brushes.Green;
            MirrorSC.Background = Brushes.LightGray;
            MirrorX.Background = Brushes.LightGray;
        }

        private void MirrorSC_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MirroSC();
            MirrorSC.Background = Brushes.Green;
            MirrorY.Background = Brushes.LightGray;
            MirrorX.Background = Brushes.LightGray;
        }

        private void RolateBtn_Click(object sender, RoutedEventArgs e)
        {
           
            RolateTb.Visibility = Visibility.Visible;
            RolateBtn.Background = Brushes.Green;
            RolateConfBtn.Visibility = Visibility.Visible;

            //убираем видимость элементов других операций
            Transfer.Background = Brushes.LightGray;
            ScaleBtn.Background = Brushes.LightGray;
            MirrorSC.Background = Brushes.LightGray;
            MirrorX.Background = Brushes.LightGray;
            MirrorY.Background = Brushes.LightGray;

            TransferX.Visibility = Visibility.Hidden;
            TransferLabelY.Visibility = Visibility.Hidden;
            TransferLabelX.Visibility = Visibility.Hidden;
            TransferY.Visibility = Visibility.Hidden;
            TransferCofirmBtn.Visibility = Visibility.Hidden;
            ScaleConfBtn.Visibility = Visibility.Hidden;
            ScaleConfBtn.Visibility = Visibility.Hidden;
            ScaleX.Visibility = Visibility.Hidden;
            ScaleY.Visibility = Visibility.Hidden;

        }

        private void RolateConfBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Rotate(RolateTb.Text);
        }
    }
}
