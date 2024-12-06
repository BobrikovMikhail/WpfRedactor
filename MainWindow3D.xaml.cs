using System;
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
       // List<LinesVisual3D> selectedLines = new List<LinesVisual3D>();

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

        private void viewport3d_MouseDown(object sender, MouseButtonEventArgs e)
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
        }

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
            }
        }

        public void LoadLinesTo3D(Canvas paintSurface)
        {
            foreach (var child in paintSurface.Children)
            {
                if (child is Line line)
                {
                    lines.Add(line);
                }
            }
            AddLinesOnCanvas();
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
            if(e.Key == Key.F2)
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
    }
}
