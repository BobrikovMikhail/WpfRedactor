﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using HelixToolkit.Wpf;

namespace WpfLab1
{
    public partial class MainWindow3D : Window
    {
        private Point3D curStartPoint;
        private bool isDrawing;
        private bool isMoving;
        private LinesVisual3D currentLine;
        private LinesVisual3D selectedLine;
        private static List<Line> lines = new List<Line>();
        MainWindow main = new MainWindow();
        List<LinesVisual3D> LinesFrom2d = new List<LinesVisual3D>();

        public MainWindow3D()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.Width = 1920;
            this.Height = 1080;

            Create3DAxesManual(viewport3d);
        }

        public LinesVisual3D Create3DAxis(int X1, int Y1, int Z1, int X2, int Y2, int Z2, Color color)
        {
            var axes = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(X1, Y1, Z1),
                    new Point3D(X2, Y2, Z2)
                },
                Color = color,
                Thickness = 3
            };
            return axes;
        }

        private void Create3DAxesManual(HelixViewport3D viewport)
        {
            // Ось X (красная)
            var xAxis = Create3DAxis(0, 0, 0, 100, 0, 0, Colors.Red);
            // Ось Y (зеленая)
            var yAxis = Create3DAxis(0, 0, 0, 0, 100, 0, Colors.Green);
            // Ось Z (синяя)
            var zAxis = Create3DAxis(0, 0, 0, 0, 0, 100, Colors.Blue);

            // Добавляем оси в 3D сцену
            viewport.Children.Add(xAxis);
            viewport.Children.Add(yAxis);
            viewport.Children.Add(zAxis);
        }

        private Point3D ScreenToWorld(Point screenPoint, double z)
        {
            return new Point3D(screenPoint.X, screenPoint.Y, z);
        }

        /*private void viewport3d_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePos = e.GetPosition(viewport3d);
                Point3D point3D = ScreenToWorld(mousePos, 0);

                // Если нет выбранной линии, начинаем новую
                if (selectedLine == null)
                {
                    isDrawing = true;
                    curStartPoint = point3D;
                    currentLine = CreateLine3D(curStartPoint, curStartPoint, Colors.Black);
                    viewport3d.Children.Add(currentLine);
                }
                else
                {
                    isMoving = true;
                }
            }

        }

        private void viewport3d_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(viewport3d);
            Point3D point3D = ScreenToWorld(mousePos, 0);

            if (isDrawing && currentLine != null)
            {
                UpdateLine3D(currentLine, curStartPoint, point3D);
            }
            else if (isMoving && selectedLine != null)
            {
                // Обновляем позицию выбранной линии
                Point3D offset = new Point3D(point3D.X - curStartPoint.X, point3D.Y - curStartPoint.Y, point3D.Z - curStartPoint.Z);
                MoveLine3D(selectedLine, offset);
                curStartPoint = point3D;
            }
        }

        private void viewport3d_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
            }

            if (isMoving)
            {
                isMoving = false;
                selectedLine = null;
            }
        }*/

        private LinesVisual3D CreateLine3D(Point3D start, Point3D end, Color color)
        {
            var line = new LinesVisual3D
            {
                Color = color,
                Thickness = 2
            };
            line.Points.Add(start);
            line.Points.Add(end);

            return line;
        }

        private void UpdateLine3D(LinesVisual3D line, Point3D start, Point3D end)
        {
            if (line.Points.Count >= 2)
            {
                line.Points[0] = start;
                line.Points[1] = end;
            }
        }

        private void MoveLine3D(LinesVisual3D line, Point3D offset)
        {
            if (line.Points.Count >= 2)
            {
                line.Points[0] = new Point3D(line.Points[0].X + offset.X, line.Points[0].Y + offset.Y, line.Points[0].Z + offset.Z);
                line.Points[1] = new Point3D(line.Points[1].X + offset.X, line.Points[1].Y + offset.Y, line.Points[1].Z + offset.Z);
            }
        }

        private void AddLinesOnCanvas()
        {
            foreach (var line in lines)
            {
                var start3D = new Point3D((line.X1 - 767) / 5, (-line.Y1 + 498) / 5, 0);
                var end3D = new Point3D((line.X2 - 767) / 5, (-line.Y2 + 498) / 5, 0);

                var line3D = AddNewLine3D(start3D, end3D, 2, Colors.Gray);
                viewport3d.Children.Add(line3D);
                LinesFrom2d.Add(line3D);
            }
        }

        public void LoadLinesTo3D(Canvas paintSurface, double z)
        {
            foreach (var child in paintSurface.Children)
            {
                if (child is Line line)
                {
                    lines.Add(line);
                }
            }
            AddLinesOnCanvas();
            AddDepthTo2DLines(z); // Задайте необходимую глубину
        }

        private LinesVisual3D AddNewLine3D(Point3D start, Point3D end, double thickness, Color color)
        {
            return new LinesVisual3D
            {
                Points = new Point3DCollection { start, end },
                Thickness = thickness,
                Color = color
            };
        }
        private void RolateLines3DX()
        {
            int gradus = 5;
            double radians = -gradus * Math.PI / 180.0; // Угол в радианах
            List<LinesVisual3D> selectedLines = new List<LinesVisual3D>();

            // Выбор всех линий с цветом Gray
            for (int i = 0; i < viewport3d.Children.Count; i++)
            {
                if (viewport3d.Children[i] is LinesVisual3D lineToRolate && lineToRolate.Color == Colors.Gray)
                {
                    selectedLines.Add(lineToRolate);
                }
            }

            // Поворот всех выбранных линий
            foreach (var line in selectedLines)
            {
                var points = line.Points;
                for (int j = 0; j < points.Count; j++)
                {
                    double x = points[j].X;
                    double y = points[j].Y;
                    double z = points[j].Z;

                    // Поворот точки вокруг оси X
                    double newY = y * Math.Cos(radians) - z * Math.Sin(radians);
                    double newZ = y * Math.Sin(radians) + z * Math.Cos(radians);

                    points[j] = new Point3D(x, newY, newZ);
                }
                line.Points = points; // Обновляем точки линии
            }
        }

        private void RolateLines3DY()
        {
            int gradus = 5;
            double radians = -gradus * Math.PI / 180.0; // Угол в радианах
            List<LinesVisual3D> selectedLines = new List<LinesVisual3D>();

            // Выбор всех линий с цветом Gray
            for (int i = 0; i < viewport3d.Children.Count; i++)
            {
                if (viewport3d.Children[i] is LinesVisual3D lineToRolate && lineToRolate.Color == Colors.Gray)
                {
                    selectedLines.Add(lineToRolate);
                }
            }

            // Поворот всех выбранных линий
            foreach (var line in selectedLines)
            {
                var points = line.Points;
                for (int j = 0; j < points.Count; j++)
                {
                    double x = points[j].X;
                    double y = points[j].Y;
                    double z = points[j].Z;

                    // Поворот точки вокруг оси Y
                    double newX = x * Math.Cos(radians) + z * Math.Sin(radians);
                    double newZ = -x * Math.Sin(radians) + z * Math.Cos(radians);

                    points[j] = new Point3D(newX, y, newZ); // Обновляем точки с новыми координатами
                }
                line.Points = points; // Обновляем точки линии
            }
        }



        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
            }
            if (e.Key == Key.F1)
            {
                RolateLines3DX();
            }
            if (e.Key == Key.F2)
            {
                RolateLines3DY();
            }
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
            }
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {

            }
        }
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
        private void TrimetrMatrix(string fiValue, string tetaValue)
        {
            double fiKoef = CheckValue(fiValue);
            double tetaKoef = CheckValue(tetaValue);
            double fi = -fiKoef * Math.PI / 180.0;
            double teta = -tetaKoef * Math.PI / 180.0;
            foreach (var child in viewport3d.Children)
            {
                if (child is LinesVisual3D line3D)
                {
                    if (line3D.Color == Colors.Gray)
                    {


                        //Получаем точки начала и конца 3D линии
                        var start = line3D.Points[0];
                        var end = line3D.Points[1];
                        double detstart = (start.X * Math.Sin(fi) * Math.Cos(teta) - start.Y * Math.Sin(teta) - start.Z * Math.Cos(fi) * Math.Cos(teta)) / 100 + 1;
                        double detend = (end.X * Math.Sin(fi) * Math.Cos(teta) - end.Y * Math.Sin(teta) - end.Z * Math.Cos(fi) * Math.Cos(teta)) / 100 + 1;

                        //Обратное преобразование координат
                        var x1 = (start.X * Math.Cos(fi) + start.Z * Math.Sin(fi)) / detstart;
                        var y1 = (start.X * Math.Sin(fi) * Math.Sin(teta) + start.Y * Math.Cos(teta) - start.Z * Math.Cos(fi) * Math.Sin(teta)) / detstart;
                        var z1 = 0;


                        var x2 = (end.X * Math.Cos(fi) + end.Z * Math.Sin(fi)) / detend;
                        var y2 = (end.X * Math.Sin(fi) * Math.Sin(teta) + end.Y * Math.Cos(teta) - end.Z * Math.Cos(fi) * Math.Sin(teta)) / detend;
                        var z2 = 0;


                        line3D.Points[0] = new Point3D(x1, y1, z1);
                        line3D.Points[1] = new Point3D(x2, y2, z2);
                    }
                }

            }
        }

        private void ChangeZRnd()
        {
            double depth = 10.0; // Глубина, на которую мы будем смещать точки
            List<Point3D> newPoints = new List<Point3D>();

            foreach (var child in viewport3d.Children)
            {
                if (child is LinesVisual3D line3D)
                {
                    if (line3D.Color == Colors.Gray)
                    {
                        var points = line3D.Points;
                        for (int i = 0; i < points.Count; i++)
                        {
                            double x = points[i].X;
                            double y = points[i].Y;
                            double z = points[i].Z;

                            // Создаем две точки с разными значениями Z для каждой исходной точки
                            newPoints.Add(new Point3D(x, y, z));
                            newPoints.Add(new Point3D(x, y, z + depth));
                        }
                        line3D.Points = new Point3DCollection(newPoints);
                    }
                }
            }
        }




        public void DrawHouseUsingLinesWithNodes(HelixViewport3D viewport)
        {
            Random random = new Random();
            int size = random.Next(20, 100);
            //=== Основание дома (куб) ===
            var basePoints = new List<Point3D>
    {
        new Point3D(0, 0, size), //Передний левый нижний
        new Point3D(size, 0, size),  //Передний правый нижний
        new Point3D(size, 0, 0),   //Задний правый нижний
        new Point3D(0, 0, 0),  //Задний левый нижний
        new Point3D(0,  size, size), //Передний левый верхний
        new Point3D(size, size, size),  //Передний правый верхний
        new Point3D(size, size, 0),   //Задний правый верхний
        new Point3D(0, size, 0)   //Задний левый верхний
    };

            //Добавляем линии основания
            AddRectangleWithNodes(viewport, basePoints[0], basePoints[1], basePoints[2], basePoints[3]); //Нижняя грань
            AddRectangleWithNodes(viewport, basePoints[4], basePoints[5], basePoints[6], basePoints[7]); //Верхняя грань
            AddLineWithNodes(viewport, basePoints[0], basePoints[4]); //Передняя левая вертикаль
            AddLineWithNodes(viewport, basePoints[1], basePoints[5]); //Передняя правая вертикаль
            AddLineWithNodes(viewport, basePoints[2], basePoints[6]); //Задняя правая вертикаль
            AddLineWithNodes(viewport, basePoints[3], basePoints[7]); //Задняя левая вертикаль

            /* var roofPeak = new Point3D(20, 70, 20); //Вершина крыши

             AddTriangleWithNodes(viewport, basePoints[4], basePoints[5], roofPeak); //Передний треугольник
             AddTriangleWithNodes(viewport, basePoints[6], basePoints[7], roofPeak); //Задний треугольник
             AddLineWithNodes(viewport, basePoints[4], basePoints[7]); //Левая наклонная
             AddLineWithNodes(viewport, basePoints[5], basePoints[6]); //Правая наклонная*/


        }

        //Вспомогательный метод для добавления линии с узлами
        private void AddLineWithNodes(HelixViewport3D viewport, Point3D start, Point3D end)
        {
            //Добавляем линию
            var line = CreateLine3D(start, end, Colors.Gray);
            viewport.Children.Add(line);

            //Добавляем сферы в начальную и конечную точки линии
            //var startSphere = DrawService.Create3DSphere(start, 1, Colors.LightBlue);
            //var endSphere = DrawService.Create3DSphere(end, 1, Colors.LightBlue);
            //viewport.Children.Add(startSphere);
            //viewport.Children.Add(endSphere);
        }

        //Вспомогательный метод для добавления прямоугольника (4 линии с узлами)
        private void AddRectangleWithNodes(HelixViewport3D viewport, Point3D p1, Point3D p2, Point3D p3, Point3D p4)
        {
            AddLineWithNodes(viewport, p1, p2); //Верхняя линия
            AddLineWithNodes(viewport, p2, p3); //Правая линия
            AddLineWithNodes(viewport, p3, p4); //Нижняя линия
            AddLineWithNodes(viewport, p4, p1); //Левая линия
        }

        private void AddFigureToViewport(Point3D[] points, (int, int)[] edges, Color color)
        {
            var lines = new LinesVisual3D { Color = color, Thickness = 1 };

            foreach (var edge in edges)
            {
                lines.Points.Add(points[edge.Item1]);
                lines.Points.Add(points[edge.Item2]);
            }

            viewport3d.Children.Add(lines);
        }
        //Вспомогательный метод для добавления треугольника (3 линии с узлами)
        private void AddTriangleWithNodes(HelixViewport3D viewport, Point3D p1, Point3D p2, Point3D p3)
        {
            AddLineWithNodes(viewport, p1, p2); //Первая сторона
            AddLineWithNodes(viewport, p2, p3); //Вторая сторона
            AddLineWithNodes(viewport, p3, p1); //Третья сторона
        }

        private void AddPyramide(HelixViewport3D viewport)
        {
            Random random = new Random();
            int size = random.Next(20, 100);
            var basePoints = new List<Point3D>
            {
                new Point3D(0, 0, size), //Передний левый нижний
                new Point3D(size, 0, size),  //Передний правый нижний
                new Point3D(size, 0, 0),   //Задний правый нижний
                new Point3D(0, 0, 0),  //Задний левый нижний
            };
            Random rnd = new Random();
            int YKoef = random.Next(40, 100);
            var roofPeak = new Point3D(20, YKoef, 20); //Вершина крыши

            AddTriangleWithNodes(viewport, basePoints[0], basePoints[1], roofPeak); //Передний треугольник
            AddTriangleWithNodes(viewport, basePoints[2], basePoints[3], roofPeak); //Задний треугольник
            AddLineWithNodes(viewport, basePoints[0], basePoints[3]); //Левая наклонная
            AddLineWithNodes(viewport, basePoints[1], basePoints[2]); //
        }


        public void AddParallelePiped(HelixViewport3D viewport)
        {
            Random random = new Random();
            int size = random.Next(20, 60);
            int secondSize = random.Next(80, 100);
            //=== Основание дома (куб) ===
            var basePoints = new List<Point3D>
    {
        new Point3D(0, 0, secondSize), //Передний левый нижний
        new Point3D(size, 0, secondSize),  //Передний правый нижний
        new Point3D(size, 0, 0),   //Задний правый нижний 
        new Point3D(0, 0, 0),  //Задний левый нижний
        new Point3D(0,  size, secondSize), //Передний левый верхний
        new Point3D(size, size, secondSize),  //Передний правый верхний
        new Point3D(size, size, 0),   //Задний правый верхний
        new Point3D(0, size, 0)   //Задний левый верхний
    };

            //Добавляем линии основания
            AddRectangleWithNodes(viewport, basePoints[0], basePoints[1], basePoints[2], basePoints[3]); //Нижняя грань
            AddRectangleWithNodes(viewport, basePoints[4], basePoints[5], basePoints[6], basePoints[7]); //Верхняя грань
            AddLineWithNodes(viewport, basePoints[0], basePoints[4]); //Передняя левая вертикаль
            AddLineWithNodes(viewport, basePoints[1], basePoints[5]); //Передняя правая вертикаль
            AddLineWithNodes(viewport, basePoints[2], basePoints[6]); //Задняя правая вертикаль
            AddLineWithNodes(viewport, basePoints[3], basePoints[7]); //Задняя левая вертикаль

            /* var roofPeak = new Point3D(20, 70, 20); //Вершина крыши

             AddTriangleWithNodes(viewport, basePoints[4], basePoints[5], roofPeak); //Передний треугольник
             AddTriangleWithNodes(viewport, basePoints[6], basePoints[7], roofPeak); //Задний треугольник
             AddLineWithNodes(viewport, basePoints[4], basePoints[7]); //Левая наклонная
             AddLineWithNodes(viewport, basePoints[5], basePoints[6]); //Правая наклонная*/


        }

        private void ChangeZ_Click(object sender, RoutedEventArgs e)
        {
            List<LinesVisual3D> LinesOnViewport = new List<LinesVisual3D>();
            foreach (var item in viewport3d.Children)
            {
                if (item is LinesVisual3D line)
                {
                    if (line.Color == Colors.Gray)
                    {
                        LinesOnViewport.Add(line);
                    }

                }
            }
            for (int i = 0; i < LinesOnViewport.Count; i++)
            {
                viewport3d.Children.Remove(LinesOnViewport[i]);
            }


            DrawHouseUsingLinesWithNodes(viewport3d);




        }

        private void TrimetrMatr_Click(object sender, RoutedEventArgs e)
        {
            string fi = FItb.Text;
            string teta = Tetatb.Text;
            TrimetrMatrix(fi, teta);
        }

        private void ChangeZ1_Click(object sender, RoutedEventArgs e)
        {
            double zKoef = CheckValue(Ztext.Text);
            if (LinesFrom2d.Count > 0)
            {
                for (int i = 0; i < LinesFrom2d.Count; i++)
                {
                    var points = LinesFrom2d[i].Points;
                    for (int j = 0; j < points.Count; j++)
                    {
                        double x = points[j].X;
                        double y = points[j].Y;
                        double z = points[j].Z;
                        z += zKoef;
                        points[j] = new Point3D(x, y, z);
                    }
                    LinesFrom2d[i].Points = points;
                }
            }
        }

        private void Pyramide_Click(object sender, RoutedEventArgs e)
        {

            List<LinesVisual3D> LinesOnViewport = new List<LinesVisual3D>();
            foreach (var item in viewport3d.Children)
            {
                if (item is LinesVisual3D line)
                {
                    if (line.Color == Colors.Gray)
                    {
                        LinesOnViewport.Add(line);
                    }

                }
            }
            for (int i = 0; i < LinesOnViewport.Count; i++)
            {
                viewport3d.Children.Remove(LinesOnViewport[i]);
            }
            AddPyramide(viewport3d);
        }

        private void Parallelepiped_Click(object sender, RoutedEventArgs e)
        {
            List<LinesVisual3D> LinesOnViewport = new List<LinesVisual3D>();
            foreach (var item in viewport3d.Children)
            {
                if (item is LinesVisual3D line)
                {
                    if (line.Color == Colors.Gray)
                    {
                        LinesOnViewport.Add(line);
                    }

                }
            }
            for (int i = 0; i < LinesOnViewport.Count; i++)
            {
                viewport3d.Children.Remove(LinesOnViewport[i]);
            }
            AddParallelePiped(viewport3d);

        }
        private void AddDepthTo2DLines(double depth)
        {
            foreach (var line in LinesFrom2d)
            {
                var start = line.Points[0];
                var end = line.Points[1];

                var startBack = new Point3D(start.X, start.Y, start.Z + depth);
                var endBack = new Point3D(end.X, end.Y, end.Z + depth);

                var frontLine = CreateLine3D(start, end, Colors.Gray);
                var backLine = CreateLine3D(startBack, endBack, Colors.Gray);
                var connectingLine1 = CreateLine3D(start, startBack, Colors.Gray);
                var connectingLine2 = CreateLine3D(end, endBack, Colors.Gray);

                viewport3d.Children.Add(frontLine);
                viewport3d.Children.Add(backLine);
                viewport3d.Children.Add(connectingLine1);
                viewport3d.Children.Add(connectingLine2);
            }
        }

        private void Smesh_click(object sender, RoutedEventArgs e)
        {
            /*if (IsCountObjectsZero3D())
           {
               return;
           }*/
            double x = CheckValue(FItb_Копировать2.Text);
            double y = CheckValue(FItb_Копировать1.Text);
            double z = CheckValue(FItb_Копировать3.Text);

            foreach (var child in viewport3d.Children)
            {
                if (child is LinesVisual3D line3D)
                {
                    if (line3D.Color == Colors.Gray)
                    {
                        //Получаем точки начала и конца 3D линии
                        var start = line3D.Points[0];
                        var end = line3D.Points[1];

                        //Обратное преобразование координат
                        var x1 = start.X + x;
                        var y1 = start.Y + y;
                        var z1 = start.Z + z;

                        var x2 = end.X + x;
                        var y2 = end.Y + y;
                        var z2 = end.Z + z;


                        line3D.Points[0] = new Point3D(x1, y1, z1);
                        line3D.Points[1] = new Point3D(x2, y2, z2);
                    }
                }

            }
        }
        private Point3D startPoint3D;
        private bool isFirstPointSet3D = false;
        private LinesVisual3D current3DLine;

        private void viewport3d_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем позицию мыши
            Point mousePosition = e.GetPosition(viewport3d);

            // Выполняем хит-тест
            var hitResult = VisualTreeHelper.HitTest(viewport3d.Viewport, mousePosition);

            if (hitResult is RayMeshGeometry3DHitTestResult rayHitResult)
            {
                // Получаем точку пересечения
                var hitPoint = rayHitResult.PointHit;

                if (!isFirstPointSet3D)
                {
                    // Сохраняем первую точку и устанавливаем флаг
                    startPoint3D = hitPoint;
                    isFirstPointSet3D = true;

                    /*// Добавляем сферу для первой точки
                    var startSphere = DrawService.Create3DSphere(startPoint3D, 1, Colors.LightBlue);
                    viewport3d.Children.Add(startSphere);*/
                }
                else
                {
                    // Вторая точка и завершение линии
                    var endPoint3D = hitPoint;
                    current3DLine = CreateLine3D(startPoint3D, endPoint3D, Colors.Gray);
                    viewport3d.Children.Add(current3DLine);

                    /*// Добавляем сферу для второй точки
                    var endSphere = DrawService.Create3DSphere(endPoint3D, 1, Colors.LightBlue);
                    viewport3d.Children.Add(endSphere);*/

                    // Сброс флага для начала новой линии
                    isFirstPointSet3D = false;
                }
            }
        }
        private void viewport3d_MouseMove(object sender, MouseEventArgs e)
        {
            /*Point mousePos = e.GetPosition(viewport3d);
            Point3D point3D = ScreenToWorld(mousePos, 0);

            if (isDrawing && currentLine != null)
            {
                UpdateLine3D(currentLine, curStartPoint, point3D);
            }
            else if (isMoving && selectedLine != null)
            {
                // Обновляем позицию выбранной линии
                Point3D offset = new Point3D(point3D.X - curStartPoint.X, point3D.Y - curStartPoint.Y, point3D.Z - curStartPoint.Z);
                MoveLine3D(selectedLine, offset);
                curStartPoint = point3D;
            }*/
        }

        private void viewport3d_MouseUp(object sender, MouseButtonEventArgs e)
        {
            /*if (isDrawing)
            {
                isDrawing = false;
            }

            if (isMoving)
            {
                isMoving = false;
                selectedLine = null;
            }*/
        }

    }


}
