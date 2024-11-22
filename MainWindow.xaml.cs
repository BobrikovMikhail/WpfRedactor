using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfLab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //bool isDeletePressed = false;
        Point previousPoint = new Point();
        int point_index = 0;
        //static SubMenu wind = null;
        static Line curLine = null;
        string mode = "Create";
        bool mbPress;
        Point curPoint = new Point();
        static private List<Line> selectedShapes = new List<Line>();
        int cnt = 0;
        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
        string currentFilePath = null;
        public MainWindow()
        {
            InitializeComponent();
            CreateBtn.Background = Brushes.Green;
            this.WindowState = WindowState.Maximized;

        }

        /*private void paintSurface_MouseDown(object sender, MouseButtonEventArgs ev)
        {
            if (mode == "Create")
            {
                curPoint = ev.GetPosition(this);
                previousPoint = curPoint; // Инициализация предыдущей точки
                curLine = null;

                foreach (Line line in paintSurface.Children.OfType<Line>())
                {
                   *//* double distance = DistanceFromPointToLine(curPoint, new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
                    if (distance < 10) // Проверка на клик по линии
                    {
                        curLine = line;
                        point_index = -1; // Устанавливаем point_index в -1 для перемещения всей линии
                        break;
                    }*//*
                     if (Math.Abs(line.X1 - curPoint.X) < 15 && Math.Abs(line.Y1 - curPoint.Y + 63) < 15)
                    {
                        curLine = line;
                        point_index = 0;
                        break;
                    }
                    else if (Math.Abs(line.X2 - curPoint.X) < 15 && Math.Abs(line.Y2 - curPoint.Y + 63) < 15)
                    {
                        curLine = line;
                        point_index = 1;
                        break;
                    }
                }
                mbPress = true;
                if (ev.LeftButton == MouseButtonState.Pressed) { curPoint = ev.GetPosition(this); }
                LabelXY.Content = "X: " + curPoint.X.ToString() + " " + "Y: " + curPoint.Y.ToString();
            }
            else if (mode == "Edit")
            {
                if (ev.RightButton == MouseButtonState.Pressed)
                {
                    curLine = null;
                    foreach (var rect in paintSurface.Children.OfType<System.Windows.Shapes.Rectangle>().ToList())
                    {
                        paintSurface.Children.Remove(rect);
                    }
                    return;
                }

                if (ev.LeftButton == MouseButtonState.Pressed) curPoint = ev.GetPosition(this);
                LabelXY.Content = "X: " + curPoint.X.ToString() + ";" + "Y: " + curPoint.Y.ToString();
                Line line;
                mbPress = true;
                double x_coor = -1; double y_coor = -1;
                foreach (Line line1 in paintSurface.Children.OfType<Line>())
                {
                    line = line1;
                    if (Math.Abs(line.X1 - ev.GetPosition(this).X) < 15 && Math.Abs(ev.GetPosition(this).Y - 63 - line.Y1) < 15)
                    {
                        point_index = 0;
                        x_coor = line.X1;
                        y_coor = line.Y1;
                        curLine = line;
                    }
                    else if (Math.Abs(line.X2 - ev.GetPosition(this).X) < 15 && Math.Abs(ev.GetPosition(this).Y - 63 - line.Y2) < 15)
                    {
                        point_index = 1;
                        x_coor = line.X2;
                        y_coor = line.Y2;
                        curLine = line;
                    }
                    else // Проверка на клик по самой линии
                    {
                        bool IsBelong = CheckPoint(curPoint, new Point(line1.X1, line1.Y1), new Point(line1.X2, line1.Y2));
                        if (IsBelong == true) // Проверка на клик по линии
                        {
                            curLine = line1;
                            point_index = -1; // Устанавливаем point_index в -1 для перемещения всей линии
                            curLine.StrokeDashArray = new DoubleCollection { 2, 2 }; // Устанавливаем пунктирный стиль
                            break;
                            //}
                        }
                    }
                }
            }
        }*/



        private double DistanceFromPointToLine(Point p, Point lineStart, Point lineEnd, MouseButtonEventArgs ev)
        {
            double d = 0;
            /*if (lineEnd.X - lineStart.X == 0)
            {
                //LabelDist.Content = $"X: " + lineStart.X + ", " + "Y: " + 0;
                //d = (lineStart.X - p.Y)/Math.Sqrt(lineStart.X * lineStart.X + 1); как тут посчитать? нужно ли учитывать случай х == 0
                return d;
            }*/
            double k = (lineEnd.Y - lineStart.Y) / (lineEnd.X - lineStart.X);
            double b = lineStart.Y - k * lineStart.X;
            //if(ev.OriginalSource is Line) equationLabel.Content = $"y = {Math.Round(k, 2)}x +{Math.Round(b, 2)}";
            d = Math.Abs((k * p.X - p.Y + b)) / Math.Sqrt(k * k + 1);
            LabelDistToLine.Content = $"расст-e {d}";
            return d;
        }



        private void paintSurface_MouseDown(object sender, MouseButtonEventArgs ev)
        {
            if (mode == "Create")
            {

                mbPress = true;
                if (ev.LeftButton == MouseButtonState.Pressed) { curPoint = ev.GetPosition(this); }
                LabelXY.Content = "X: " + curPoint.X.ToString() + " " + "Y: " + curPoint.Y.ToString();
            }
            else if (mode == "Edit")
            {
                if (ev.RightButton == MouseButtonState.Pressed)
                {
                    curLine = null;
                    foreach (var rect in paintSurface.Children.OfType<System.Windows.Shapes.Rectangle>().ToList())
                    {
                        paintSurface.Children.Remove(rect);
                    }
                    return;
                }

                if (ev.LeftButton == MouseButtonState.Pressed)
                {
                    curPoint = ev.GetPosition(this);
                }
                LabelXY.Content = "X: " + curPoint.X.ToString() + " ; " + "Y: " + curPoint.Y.ToString();

                foreach (Line line in paintSurface.Children.OfType<Line>())
                {
                    LabelDist.Content = line.X1 + " ; " + line.Y1;
                    LineLabel.Content = line.X2 + " ; " + line.Y2;

                    if (Math.Abs(line.X1 - ev.GetPosition(this).X) < 15 && Math.Abs(ev.GetPosition(this).Y - line.Y1) < 15)
                    {
                        point_index = 0;
                        curLine = line;
                    }
                    else if (Math.Abs(line.X2 - ev.GetPosition(this).X) < 15 && Math.Abs(ev.GetPosition(this).Y - line.Y2) < 15)
                    {
                        point_index = 1;
                        curLine = line;
                    }
                    else
                    {
                        double distance = DistanceFromPointToLine(curPoint, new Point(line.X1, line.Y1), new Point(line.X2, line.Y2), ev);
                        if (distance < 10)
                        {
                            curLine = line;
                            point_index = -1;
                            curLine.StrokeDashArray = new DoubleCollection { 2, 2 };
                            equationWrite(curPoint, new Point(curLine.X1, curLine.Y1), new Point(curLine.X2, curLine.Y2));


                            bool isShiftPressed = Keyboard.IsKeyDown(Key.LeftShift);
                            ContLabel.Content = selectedShapes.Count;
                            SelectObject(curLine, isShiftPressed);
                            //DeleteObject(curLine, isDeletePressed);
                            break;
                        }
                    }


                }

                mbPress = true;
                previousPoint = ev.GetPosition(this);
            }
        }

        /*public void DeleteObj(Line currrentLine, bool isDelPres)
        {
            if(currrentLine != null && isDelPres == true)
            {
                currrentLine = null;
                paintSurface.Children.OfType<Line> = null;
            }
        }*/

        public void DeleteObject(Line curLine)
        {

            if (selectedShapes.Count == 0)
            {
                paintSurface.Children.Remove(curLine);
            }
            else
            {
                for (int i = 0; i < selectedShapes.Count; i++)
                {
                    paintSurface.Children.Remove(selectedShapes[i]);
                }
                selectedShapes.Clear();

            }
            return;
        }

        public static void SelectObject(Line CurLine, bool isShiftPressed)
        {
            // Получаем объект, по которому кликнули
            //var ivent = e.OriginalSource as Shape;

            // Проверяем, что был клик именно по линии
            //selectedShapes.Clear();
            if (isShiftPressed)
            {
                // Если линия уже выделена, снимаем выделение
                if (selectedShapes.Contains(CurLine))
                {
                    int index = selectedShapes.IndexOf(CurLine);
                    //ivent.Stroke = originalShapesStrokes[index]; // Вернуть исходный цвет линии
                    selectedShapes.RemoveAt(index);
                    CurLine.Stroke = Brushes.Black;
                    //originalShapesStrokes.RemoveAt(index);
                }
                else
                {
                    // Добавляем линию в список выделенных и сохраняем ее исходный цвет
                    selectedShapes.Add(CurLine);
                    //riginalShapesStrokes.Add(ivent.Stroke);

                    // Меняем цвет на выделенный
                    CurLine.Stroke = Brushes.Blue;
                }
            }
            /*else
            {
                // Если Shift не нажат, снимаем все предыдущие выделения
                ClearSelection();

               *//* // Добавляем новую линию в выделение
                selectedShapes.Add(CurLine);
                //originalShapesStrokes.Add(ivent.Stroke);

                // Меняем цвет на выделенный
                CurLine.Stroke = Brushes.Blue;*//*
            }*/

            /*else if (!isShiftPressed)
            {
                // Снимаем все выделения, если щелчок был не по линии и Shift не нажат
                ClearSelection();
            }*/

        }
        public static void ClearSelection()
        {
            // Восстанавливаем исходные цвета для всех выделенных линий
            for (int i = 0; i < selectedShapes.Count; i++)
            {
                selectedShapes[i].Stroke = Brushes.Black;
            }

            // Очищаем списки
            selectedShapes.Clear();
            //originalShapesStrokes.Clear();
        }

        private void equationWrite(Point p, Point lineStart, Point lineEnd)
        {
            double d = 0;
            /*if (lineEnd.X - lineStart.X == 0)
            {
                //LabelDist.Content = $"X: " + lineStart.X + ", " + "Y: " + 0;
                //d = (lineStart.X - p.Y)/Math.Sqrt(lineStart.X * lineStart.X + 1); как тут посчитать? нужно ли учитывать случай х == 0
                return d;
            }*/
            double k = (lineEnd.Y - lineStart.Y) / (lineEnd.X - lineStart.X);
            double b = lineStart.Y - k * lineStart.X;
            //LabelDist.Content = $"y = + {k}x +{b}";
            if (b < 0) equationLabel.Content = $"y = {Math.Round(k, 2)}x {Math.Round(b, 2)}";
            else equationLabel.Content = $"y = {Math.Round(k, 2)}x + {Math.Round(b, 2)}";
            //d = Math.Abs((k * p.X - p.Y + b)) / Math.Sqrt(k * k + 1);
            //LabelDistToLine.Content = $"расст-e {d}";
            return;
        }
        /* private bool CheckPoint(Point p, Line line)
         {
             double tolerance = 5; // Допустимая погрешность

             Point Start = new Point(line.X1, line.Y1);
             Point End = new Point(line.X2, line.Y2);

             // Проверка вертикальной линии
             if (Math.Abs(Start.X - End.X) < tolerance)
             {
                 return Math.Abs(p.X - Start.X) < tolerance &&
                        p.Y >= Math.Min(Start.Y, End.Y) && p.Y <= Math.Max(Start.Y, End.Y);
             }

             // Проверка горизонтальной линии
             if (Math.Abs(Start.Y - End.Y) < tolerance)
             {
                 return Math.Abs(p.Y - Start.Y) < tolerance &&
                        p.X >= Math.Min(Start.X, End.X) && p.X <= Math.Max(Start.X, End.X);
             }

             // Проверка наклонной линии
             double k = (End.Y - Start.Y) / (End.X - Start.X);
             double b = Start.Y - k * Start.X;
             double yOnLine = k * p.X + b;

             bool isWithinLineSegment = p.X >= Math.Min(Start.X, End.X) && p.X <= Math.Max(Start.X, End.X) &&
                                        p.Y >= Math.Min(Start.Y, End.Y) && p.Y <= Math.Max(Start.Y, End.Y);

             return Math.Abs(p.Y - yOnLine) < tolerance && isWithinLineSegment;
         }*/
        /*private bool CheckPoint(Point p, Point StartP, Point LastP)
        {
            double tolerance = 5; // Допустимая погрешность
            double dx = LastP.X - StartP.X;
            double dy = LastP.Y - StartP.Y;

            if (Math.Abs(dx) < tolerance) // Вертикальная линия
            {
                return Math.Abs(p.X - StartP.X) < tolerance &&
                       p.Y >= Math.Min(StartP.Y, LastP.Y) && p.Y <= Math.Max(StartP.Y, LastP.Y);
            }

            double k = dy / dx;
            double b = StartP.Y - k * StartP.X;
            double yOnLine = k * p.X + b;

            return Math.Abs(p.Y - yOnLine) < tolerance;
        }
*/
        private void paintSurface_MouseUp(object sender, MouseButtonEventArgs ev)
        {
            mbPress = false;
            if (curLine != null)
            {
                curLine.StrokeDashArray = null;
            }
            //curLine = null;
            point_index = -1;
            cnt++;
        }

        private void paintSurface_MouseMove(object sender, MouseEventArgs ev)
        {
            if (mode == "Create")
            {
                /* double marker_x = -1;
                 double marker_y = -1;*/
                if (ev.LeftButton == MouseButtonState.Pressed)
                {

                    /*    if (curLine != null)
                        {
                            LineLabel.Content = curLine;
                            if (point_index == 0)
                            {
                                curLine.X1 = ev.GetPosition(this).X;
                                curLine.Y1 = ev.GetPosition(this).Y - 63;
                                marker_x = curLine.X1;
                                marker_y = curLine.Y1;
                            }
                            else
                            {
                                curLine.X2 = ev.GetPosition(this).X;
                                curLine.Y2 = ev.GetPosition(this).Y - 63;
                                marker_x = curLine.X2;
                                marker_y = curLine.Y2;
                            }
    */
                    /*for (int i = 0; i < 10; i++)
                    {
                        foreach (System.Windows.Shapes.Rectangle rect1 in paintSurface.Children.OfType<Rectangle>())
                        {
                            paintSurface.Children.Remove(rect1);
                            break;
                        }*/

                    /*  System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                      rect.Stroke = new SolidColorBrush(Colors.Red);
                      rect.Fill = new SolidColorBrush(Colors.Transparent);
                      rect.Width = 10;
                      rect.Height = 10;
                      Canvas.SetLeft(rect, marker_x - 5);
                      Canvas.SetTop(rect, marker_y - 5);
                      paintSurface.Children.Add(rect);
                      return;
                  }*/
                    /*}
                    else
                    {*/
                    Line line = null;
                    foreach (Line line1 in paintSurface.Children.OfType<Line>())
                    {
                        if (line1.Name == "line" + cnt)
                        {
                            line = line1;
                            break;
                        }
                    }

                    if (line == null)
                    {
                        line = new Line();
                        line.Stroke = SystemColors.WindowFrameBrush;
                        line.X1 = curPoint.X;
                        line.Y1 = curPoint.Y;
                        line.X2 = ev.GetPosition(this).X;
                        line.Y2 = ev.GetPosition(this).Y;

                        curPoint = ev.GetPosition(this);
                        line.Name = "line" + cnt;

                        paintSurface.Children.Add(line);
                    }
                    else
                    {
                        line.X2 = ev.GetPosition(this).X;
                        line.Y2 = ev.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }
                }

                /* if (marker_x != -1)
                 {
                     //System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                     rect.Stroke = new SolidColorBrush(Colors.Red);
                     rect.Fill = new SolidColorBrush(Colors.Transparent);
                     rect.Width = 10;
                     rect.Height = 10;
                     Canvas.SetLeft(rect, marker_x - 5);
                     Canvas.SetTop(rect, marker_y - 5);
                     paintSurface.Children.Add(rect);
                 }
                 else if (ev.LeftButton != MouseButtonState.Pressed)
                 {
                     for (int i = 0; i < 10; i++)
                     {
                         foreach (System.Windows.Shapes.Rectangle rect in paintSurface.Children.OfType<Rectangle>())
                         {
                             paintSurface.Children.Remove(rect);
                             curLine = null;
                             paintSurface.InvalidateVisual();
                             break;
                         }
                     }
                 }*/
                //}
            }
            else if (mode == "Edit" && curLine != null)
            {

                double marker_x = -1;
                double marker_y = -1;
                Point currentPoint = ev.GetPosition(this);
                if (ev.LeftButton == MouseButtonState.Pressed)
                {
                    if (curLine != null)
                    {

                        //LineLabel.Content = curLine;
                        if (point_index == 0)
                        {
                            curLine.X1 = currentPoint.X;
                            curLine.Y1 = currentPoint.Y;
                            marker_x = curLine.X1;
                            marker_y = curLine.Y1;
                        }
                        else if (point_index == 1)
                        {
                            curLine.X2 = currentPoint.X;
                            curLine.Y2 = currentPoint.Y;
                            marker_x = curLine.X2;
                            marker_y = curLine.Y2;
                        }
                        else if (point_index == -1)
                        {
                            double offsetX = currentPoint.X - previousPoint.X;
                            double offsetY = currentPoint.Y - previousPoint.Y;
                            if (selectedShapes.Count != 0)
                            {
                                for (int i = 0; i < selectedShapes.Count; i++)
                                {
                                    curLine = (Line)selectedShapes[i];
                                    curLine.X1 += offsetX;
                                    curLine.Y1 += offsetY;
                                    curLine.X2 += offsetX;
                                    curLine.Y2 += offsetY;
                                }
                            }

                            else
                            {


                                curLine.X1 += offsetX;
                                curLine.Y1 += offsetY;
                                curLine.X2 += offsetX;
                                curLine.Y2 += offsetY;
                            }
                        }





                        previousPoint = currentPoint; // Обновляем предыдущую точку
                        paintSurface.InvalidateVisual(); // Обновляем область рисования

                        //paintSurface.InvalidateVisual();

                        for (int i = 0; i < 10; i++)
                        {
                            foreach (System.Windows.Shapes.Rectangle rect1 in paintSurface.Children.OfType<Rectangle>())
                            {
                                paintSurface.Children.Remove(rect1);
                                break;
                            }

                            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                            rect.Stroke = new SolidColorBrush(Colors.Red);
                            rect.Fill = new SolidColorBrush(Colors.Transparent);
                            rect.Width = 10;
                            rect.Height = 10;
                            Canvas.SetLeft(rect, marker_x - 5);
                            Canvas.SetTop(rect, marker_y - 5);
                            paintSurface.Children.Add(rect);
                            return;
                        }
                    }
                    else
                    {
                        Line line = null;
                        foreach (Line line1 in paintSurface.Children.OfType<Line>())
                        {
                            if (line1.Name == "line" + cnt)
                            {
                                line = line1;
                                break;
                            }
                        }
                        line.X2 = ev.GetPosition(this).X;
                        line.Y2 = ev.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();

                    }

                    if (marker_x != -1)
                    {
                        //System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                        rect.Stroke = new SolidColorBrush(Colors.Red);
                        rect.Fill = new SolidColorBrush(Colors.Transparent);
                        rect.Width = 10;
                        rect.Height = 10;
                        Canvas.SetLeft(rect, marker_x - 5);
                        Canvas.SetTop(rect, marker_y - 5);
                        paintSurface.Children.Add(rect);
                    }
                    else if (ev.LeftButton != MouseButtonState.Pressed)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            foreach (System.Windows.Shapes.Rectangle rect in paintSurface.Children.OfType<Rectangle>())
                            {
                                paintSurface.Children.Remove(rect);
                                curLine = null;
                                paintSurface.InvalidateVisual();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            Color color = Color.FromArgb(255, 14, 73, 183);
            SolidColorBrush brush = new SolidColorBrush(color);
            mode = "Create";
            CreateBtn.Background = Brushes.Green;
            EditBtn.Background = brush;
            Operations.Background = brush;
        }

        private void EditBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Color color = Color.FromArgb(255, 14, 73, 183);
            SolidColorBrush brush = new SolidColorBrush(color);
            mode = "Create";
            mode = "Edit";
            EditBtn.Background = Brushes.Green;
            CreateBtn.Background = brush;
            Operations.Background = brush;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteObject(curLine);
            }

            if (e.Key == Key.Escape)
            {
                if (selectedShapes != null)
                {
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        selectedShapes[i].Stroke = Brushes.Black;
                    }
                    selectedShapes.Clear();
                }
            }
        }

        private void Operations_Click(object sender, RoutedEventArgs e)
        {
            SubMenu wind = new SubMenu();
            Color color = Color.FromArgb(255, 14, 73, 183);
            SolidColorBrush brush = new SolidColorBrush(color);
            Operations.Background = Brushes.Green;
            EditBtn.Background = brush;
            CreateBtn.Background = brush;

            wind.ShowDialog();
        }
        //Проверка числового значения из TextBox
        /* public static double CheckValue(TextBox t)
         {
             // Попробуем разобрать значения из текстбоксов
             bool isVal1Valid = double.TryParse(t.Text, out double val1);
             //bool isVal2Valid = double.TryParse(t2.Text, out double val2);

             // Проверяем, были ли оба значения корректно разобраны
             if (!isVal1Valid)
             {
                 MessageBox.Show("Ошибка: введите число", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
             }

         }*/
        private static double CheckValue(string countString)
        {
            if (!string.IsNullOrWhiteSpace(countString))
            {
                double number;
                bool isOkType;
                isOkType = double.TryParse(countString, out number);
                if (!isOkType)
                {
                    MessageBox.Show("Ошибка: введите число", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
                return number;
            }
            else
            {
                MessageBox.Show("Ошибка: введите число", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return 0;
        }

        public static void TransferLine(string XKof, string YKof)
        {
            double XKoef = CheckValue(XKof);
            double YKoef = CheckValue(YKof);
            YKoef *= -1;
            //CheckValue(wind.TransferX, wind.TransferY);
            if (XKoef == 0 && YKoef == 0)
            {
                return;
            }
            if (selectedShapes.Count != 0)
            {
                if (XKoef == 0 && YKoef != 0)
                {
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        selectedShapes[i].Y1 += YKoef;
                        selectedShapes[i].Y2 += YKoef;
                    }

                }

                else if (XKoef != 0 && YKoef == 0)
                {
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        selectedShapes[i].X1 += XKoef;
                        selectedShapes[i].X2 += XKoef;
                    }

                }
                else if (XKoef != 0 && YKoef != 0)
                {
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        selectedShapes[i].X1 += XKoef;
                        selectedShapes[i].X2 += XKoef;
                        selectedShapes[i].Y1 += YKoef;
                        selectedShapes[i].Y2 += YKoef;
                    }
                }
            }
            else
            {
                if (curLine != null)
                {
                    if (XKoef == 0 && YKoef != 0)
                    {
                        curLine.Y1 += YKoef;
                        curLine.Y2 += YKoef;
                    }
                    else if (XKoef != 0 && YKoef == 0)
                    {
                        curLine.X1 += XKoef;
                        curLine.X2 += XKoef;
                    }
                    else if (YKoef != 0 && YKoef != 0)
                    {
                        curLine.X1 += XKoef;
                        curLine.X2 += XKoef;
                        curLine.Y1 += YKoef;
                        curLine.Y2 += YKoef;
                    }
                }
            }

        }

        /* public static void ScaleLine(string XK, string YK)
         {
             double XKoef = CheckValue(XK);
             double YKoef = CheckValue(YK);
             YKoef *= -1;
             //CheckValue(wind.TransferX, wind.TransferY);
             if (XKoef == 0 && YKoef == 0)
             {
                 return;
             }
             if (selectedShapes.Count != 0)
             {
                 if (XKoef == 0 && YKoef != 0)
                 {
                     for (int i = 0; i < selectedShapes.Count; i++)
                     {
                         selectedShapes[i].Y1 *= YKoef;
                         selectedShapes[i].Y2 *= YKoef;
                     }
                 }

                 else if (XKoef != 0 && YKoef == 0)
                 {
                     for (int i = 0; i < selectedShapes.Count; i++)
                     {
                         selectedShapes[i].X1 *= XKoef;
                         selectedShapes[i].X2 *= XKoef;
                     }
                 }
                 else if (XKoef != 0 && YKoef != 0)
                 {
                     for (int i = 0; i < selectedShapes.Count; i++)
                     {
                         selectedShapes[i].X1 *= XKoef;
                         selectedShapes[i].X2 *= XKoef;
                         selectedShapes[i].Y1 *= YKoef;
                         selectedShapes[i].Y2 *= YKoef;
                     }
                 }
             }
             else
             {
                 if (curLine != null)
                 {
                     if (XKoef == 0 && YKoef != 0)
                     {
                         curLine.Y1 *= YKoef;
                         curLine.Y2 *= YKoef;
                     }
                     else if (XKoef != 0 && YKoef == 0)
                     {
                         curLine.X1 *= XKoef;
                         curLine.X2 *= XKoef;
                     }
                     else if (YKoef != 0 && YKoef != 0)
                     {
                         curLine.X1 *= XKoef;
                         curLine.X2 *= XKoef;
                         curLine.Y1 *= YKoef;
                         curLine.Y2 *= YKoef;
                     }
                 }
             }
         }*/
        public static void ScaleLine(string XK, string YK)
        {
            double XKoef = CheckValue(XK);
            double YKoef = CheckValue(YK);

            // Если коэффициенты равны нулю, выход из метода
            if (XKoef == 0 && YKoef == 0)
            {
                return;
            }

            if (selectedShapes.Count != 0)
            {
                foreach (var line in selectedShapes)
                {
                    ScaleSingleLine(line, XKoef, YKoef);
                }
            }
            else if (curLine != null)
            {
                ScaleSingleLine(curLine, XKoef, YKoef);
            }
        }

        private static void ScaleSingleLine(Line line, double XKoef, double YKoef)
        {
            // Найдем центр линии
            //double centerX = (line.X1 + line.X2) / 2; 


            double centerX = 767; double centerY = 424;
            //double centerY = (line.Y1 + line.Y2) / 2;

            // Масштабируем координаты относительно центра
            if (XKoef != 0)
            {
                line.X1 = centerX + (line.X1 - centerX) * XKoef;
                line.X2 = centerX + (line.X2 - centerX) * XKoef;

                /*line.X1 = line.X1 + 767;
                line.X2 = line.X2 + 767;*/

            }
            if (YKoef != 0)
            {
                /*line.Y1 = 424 - line.Y1;
                line.Y2 = 424 - line.Y2;*/

                line.Y1 = (centerY + (line.Y1 - centerY) * YKoef);
                line.Y2 = (centerY + (line.Y2 - centerY) * YKoef);
            }
        }

        public static void MirrorX()
        {
            double centerY = 424;
            if (selectedShapes.Count != 0)
            {
                for (int i = 0; i < selectedShapes.Count; i++)
                {
                    // Зеркалирование с учетом смещения
                    selectedShapes[i].Y1 = centerY - (selectedShapes[i].Y1 - centerY);
                    selectedShapes[i].Y2 = centerY - (selectedShapes[i].Y2 - centerY);
                }
            }
            else if (curLine != null)
            {
                // Зеркалирование с учетом смещения
                curLine.Y1 = centerY - (curLine.Y1 - centerY);
                curLine.Y2 = centerY - (curLine.Y2 - centerY);
            }
        }
        public static void MirrorY()
        {
            double centerX = 767;
            if (selectedShapes.Count != 0)
            {
                for (int i = 0; i < selectedShapes.Count; i++)
                {
                    // Зеркалирование с учетом смещения
                    selectedShapes[i].X1 = centerX - (selectedShapes[i].X1 - centerX);
                    selectedShapes[i].X2 = centerX - (selectedShapes[i].X2 - centerX);
                }
            }
            else if (curLine != null)
            {
                // Зеркалирование с учетом смещения
                curLine.X1 = centerX - (curLine.X1 - centerX);
                curLine.X2 = centerX - (curLine.X2 - centerX);
            }

        }
        public static void MirroSC()
        {
            double centerX = 767; double centerY = 424;
            if (selectedShapes.Count != 0)
            {
                for (int i = 0; i < selectedShapes.Count; i++)
                {
                    // Зеркалирование с учетом смещения
                    selectedShapes[i].X1 = centerX - (selectedShapes[i].X1 - centerX);
                    selectedShapes[i].X2 = centerX - (selectedShapes[i].X2 - centerX);
                    selectedShapes[i].Y1 = centerY - (selectedShapes[i].Y1 - centerY);
                    selectedShapes[i].Y2 = centerY - (selectedShapes[i].Y2 - centerY);

                }
            }
            else if (curLine != null)
            {
                // Зеркалирование с учетом смещения
                curLine.X1 = centerX - (curLine.X1 - centerX);
                curLine.X2 = centerX - (curLine.X2 - centerX);
                curLine.Y1 = centerY - (curLine.Y1 - centerY);
                curLine.Y2 = centerY - (curLine.Y2 - centerY);
            }
        }

        public static void Rotate(string angle)
        {
            double gradus = CheckValue(angle); // Преобразуем строку в число
            double centerX = 767;
            double centerY = 424;
            if (gradus > 0 && gradus <= 360)
            {
                double radians = -gradus * Math.PI / 180.0; // Переводим градусы в радианы


                if (selectedShapes.Count != 0)
                {
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        // Получаем координаты линии
                        double x1 = selectedShapes[i].X1;
                        double y1 = selectedShapes[i].Y1;
                        double x2 = selectedShapes[i].X2;
                        double y2 = selectedShapes[i].Y2;


                        // Смещаем координаты к центру вращения
                        x1 -= centerX;
                        y1 -= centerY;
                        x2 -= centerX;
                        y2 -= centerY;

                        // Применяем матрицу поворота
                        double newX1 = x1 * Math.Cos(radians) - y1 * Math.Sin(radians);
                        double newY1 = x1 * Math.Sin(radians) + y1 * Math.Cos(radians);
                        double newX2 = x2 * Math.Cos(radians) - y2 * Math.Sin(radians);
                        double newY2 = x2 * Math.Sin(radians) + y2 * Math.Cos(radians);

                        // Возвращаем координаты обратно
                        newX1 += centerX;
                        newY1 += centerY;
                        newX2 += centerX;
                        newY2 += centerY;

                        // Обновляем координаты линии
                        selectedShapes[i].X1 = newX1;
                        selectedShapes[i].Y1 = newY1;
                        selectedShapes[i].X2 = newX2;
                        selectedShapes[i].Y2 = newY2;
                    }
                }
                else if (curLine != null)
                {
                    double x1 = curLine.X1;
                    double y1 = curLine.Y1;
                    double x2 = curLine.X2;
                    double y2 = curLine.Y2;




                    // Смещаем координаты к центру вращения
                    x1 -= centerX;
                    y1 -= centerY;
                    x2 -= centerX;
                    y2 -= centerY;
                    double newX1 = x1 * Math.Cos(radians) - y1 * Math.Sin(radians);
                    double newY1 = x1 * Math.Sin(radians) + y1 * Math.Cos(radians);
                    double newX2 = x2 * Math.Cos(radians) - y2 * Math.Sin(radians);
                    double newY2 = x2 * Math.Sin(radians) + y2 * Math.Cos(radians);

                    newX1 += centerX;
                    newY1 += centerY;
                    newX2 += centerX;
                    newY2 += centerY;

                    curLine.X1 = newX1; curLine.X2 = newX2; curLine.Y1 = newY1; curLine.Y2 = newY2;

                }
            }
        }

        /* public static void Rolation(string grade)
         {
             double gradus = CheckValue(grade);
             if(gradus>0 && gradus <= 360)
             {
                 if(selectedShapes.Count != 0)
                 {
                     for(int i =0; i< selectedShapes.Count; i++)
                     {
                         selectedShapes[i].X1 *= Math.Cos(gradus);
                         selectedShapes[i].Y1 *= Math.Sin(gradus);

                         selectedShapes[i].X2 *= -Math.Sin(gradus);
                         selectedShapes[i].Y2 *= Math.Cos(gradus);
                     }
                 }
             }
         }*/
        /*        public static void MirrorX()
                {
                    double centerY = 424;
                    if (selectedShapes.Count != 0)
                    {
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            // Зеркалирование с учетом смещения
                            selectedShapes[i].Y1 = centerY + (selectedShapes[i].Y1 *=-1);
                            selectedShapes[i].Y2 = centerY + (selectedShapes[i].Y2 *= -1);
                        }
                    }
                    else if (curLine != null)
                    {
                        // Зеркалирование с учетом смещения
                        curLine.Y1 = centerY + (curLine.Y1 *= -1);
                        curLine.Y2 = centerY + (curLine.Y2 *= -1);
                    }
                }
                public static void MirrorY()
                {
                    double centerX = 767;
                    if (selectedShapes.Count != 0)
                    {
                        for (int i = 0; i < selectedShapes.Count; i++)
                        {
                            // Зеркалирование с учетом смещения
                            selectedShapes[i].X1 = centerX - (selectedShapes[i].X1 - centerX);
                            selectedShapes[i].X2 = centerX - (selectedShapes[i].X2 - centerX);
                        }
                    }
                    else if (curLine != null)
                    {
                        // Зеркалирование с учетом смещения
                        curLine.X1 = centerX - (curLine.X1 - centerX);
                        curLine.X2 = centerX - (curLine.X2 - centerX);
                    }

                }*/



        /*  private void XYCanvas_Loaded(object sender, RoutedEventArgs e)
          {

          }*/

        private void XYCanvas_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Проверим, что метод отрисовки осей выполняется
            Console.WriteLine("Отрисовка осей началась");

            // Размеры канваса
            double canvasWidth = XYCanvas.ActualWidth;
            double canvasHeight = XYCanvas.ActualHeight;
            Console.WriteLine($"Размеры канваса: {canvasWidth}x{canvasHeight}");

            // Позиции осей (по центру)
            double centerX = canvasWidth / 2;
            double centerY = canvasHeight / 2;

            // Оси X и Y
            DrawAxis(centerX, centerY, canvasWidth, canvasHeight);
        }

        private void DrawAxis(double centerX, double centerY, double canvasWidth, double canvasHeight)
        {
            // Горизонтальная ось X
            Line xLine = new Line
            {
                X1 = 0,  // Начало оси X
                X2 = canvasWidth,  // Конец оси X
                Y1 = centerY,  // Позиция по Y (центр)
                Y2 = centerY,  // Позиция по Y (центр)
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            XYCanvas.Children.Add(xLine);

            // Вертикальная ось Y
            Line yLine = new Line
            {
                X1 = centerX,  // Позиция по X (центр)
                X2 = centerX,  // Позиция по X (центр)
                Y1 = 0,  // Начало оси Y
                Y2 = canvasHeight,  // Конец оси Y
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            XYCanvas.Children.Add(yLine);

            // Стрелка на оси X
            Polygon xArrow = new Polygon
            {
                Points = new PointCollection
        {
            new Point(canvasWidth - 10, centerY - 5),
            new Point(canvasWidth, centerY),
            new Point(canvasWidth - 10, centerY + 5)
        },
                Stroke = Brushes.Black,
                Fill = Brushes.Black
            };
            XYCanvas.Children.Add(xArrow);

            // Подпись "X" на оси X
            TextBlock xLabel = new TextBlock
            {
                Text = "X",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(xLabel, canvasWidth - 20);  // Правильное позиционирование по оси X
            Canvas.SetTop(xLabel, centerY + 10);  // Правильное позиционирование по оси Y
            XYCanvas.Children.Add(xLabel);

            // Стрелка на оси Y
            Polygon yArrow = new Polygon
            {
                Points = new PointCollection
        {
            new Point(centerX - 5, 10),
            new Point(centerX, 0),
            new Point(centerX + 5, 10)
        },
                Stroke = Brushes.Black,
                Fill = Brushes.Black
            };
            XYCanvas.Children.Add(yArrow);

            // Подпись "Y" на оси Y
            TextBlock yLabel = new TextBlock
            {
                Text = "Y",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(yLabel, centerX + 10);  // Правильное позиционирование по оси X
            Canvas.SetTop(yLabel, 10);  // Правильное позиционирование по оси Y
            XYCanvas.Children.Add(yLabel);
        }

        private void Save(string filePath)
        {
            var drawableObject = new Save_Load();
            drawableObject.SaveObjectsToFile(filePath, paintSurface);
            MessageBox.Show("Файл успешно сохранен", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml",
                DefaultExt = ".xml",
                Title = "Сохранить проект как"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                currentFilePath = saveFileDialog.FileName;
                Save(currentFilePath);
            }
        }


            private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveAs();
            }
            else
            {
                Save(currentFilePath);
            }
        }


        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var drawableObject = new Save_Load();
                drawableObject.LoadObjectsFromFile(openFileDialog.FileName, paintSurface);
                //очистка списка выделенных групп
                selectedShapes.Clear();
                currentFilePath = openFileDialog.FileName;
            }
        }

        private void SaveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        /* private void paintSurface_Loaded(object sender, RoutedEventArgs e)
         {
             int StartX = 2030 / 2;
             int StartY = 1080 / 2;
             Line XLine = new Line { X1 = 0, X2 = 2030, Y1 = 540, Y2 = 540 };
             XLine.StrokeThickness = 10;
             XLine.Stroke = Brushes.Black;
             //Line Y = new Line {X1 = 
             paintSurface.Children.Add(XLine);
         }*/
    }
}