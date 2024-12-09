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

namespace GraphicEditor
{
    /// <summary>
    /// Логика взаимодействия для InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public double ZValue { get; private set; }
        public bool IsConfirmed { get; private set; }

        public InputDialog()
        {
            InitializeComponent();
            IsConfirmed = false;
        }
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ZValueTextBox.Text, out double z))
            {
                ZValue = z;
                IsConfirmed = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите числовое значение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
