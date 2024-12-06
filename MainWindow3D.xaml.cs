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
        private LinesVisual3D currentLine;
        private static List<Line> lines = new List<Line>();
        MainWindow main = new MainWindow();

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
            //Ось X (красная)
            var xAxis = Create3DAxis(0, 0, 0, 100, 0, 0, Colors.Red);
            //Ось Y (зеленая)
            var yAxis = Create3DAxis(0, 0, 0, 0, 100, 0, Colors.Green);
            //Ось Z (синяя)
            var zAxis = Create3DAxis(0, 0, 0, 0, 0, 100, Colors.Blue);

            //Добавляем оси в 3D сцену
            viewport.Children.Add(xAxis);
            viewport.Children.Add(yAxis);
            viewport.Children.Add(zAxis);
        }

        /*private void AddAxes()
        {
            // Ось X - красная
            Add3DLine(new Point3D(0, 0, 0), new Point3D(100, 0, 0), Colors.Red);

            // Ось Y - зеленая
            Add3DLine(new Point3D(0, 0, 0), new Point3D(0, 100, 0), Colors.Green);

            // Ось Z - синяя, увеличиваем длину для лучшей видимости
            Add3DLine(new Point3D(0, 0, 0), new Point3D(0, 0, 100), Colors.Blue);
        }

        public void Add3DLine(Point3D start, Point3D end, Color color)
        {
            var line = new LinesVisual3D
            {
                Color = color,
                Thickness = 2
            };
            line.Points.Add(start);
            line.Points.Add(end);

            viewport3d.Children.Add(line);
        }*/

        private Point3D ScreenToWorld(Point screenPoint, double z)
        {
            return new Point3D(screenPoint.X, screenPoint.Y, z);
        }

        private void viewport3d_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDrawing = true;
                Point mousePos = e.GetPosition(viewport3d);
                Point3D point3D = ScreenToWorld(mousePos, 0);
                curStartPoint = point3D;
                currentLine = CreateLine3D(curStartPoint, curStartPoint, Colors.Black);
                viewport3d.Children.Add(currentLine);
            }
        }

        private void viewport3d_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && currentLine != null)
            {
                Point mousePos = e.GetPosition(viewport3d);
                Point3D point3D = ScreenToWorld(mousePos, 0);
                UpdateLine3D(currentLine, curStartPoint, point3D);
            }
        }

        private void viewport3d_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
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
        /*private void LoadLinesTo3D()
        {
            List<Line> lines = new List<Line>();
            for (int i = 0; i < main.paintSurface.Children.Count; i++)
            {
                if (main.paintSurface.Children[i] is Line) lines.Add((Line)main.paintSurface.Children[i]);
            }
            if (lines.Count > 0)
            {
                double z = 0; // Константа для Z-координаты

                foreach (Line line in lines)
                {
                    // Получаем координаты начала и конца линии
                    Point start2D = new Point(line.X1, line.Y1);
                    Point end2D = new Point(line.X2, line.Y2);

                    // Конвертируем в 3D точки
                    Point3D start3D = new Point3D(start2D.X, start2D.Y, z);
                    Point3D end3D = new Point3D(end2D.X, end2D.Y, z);

                    // Создаем 3D линию и добавляем ее в 3D viewport
                    LinesVisual3D line3D = AddNewLine3D(start3D, end3D, Colors.Black);
                    viewport3d.Children.Add(line3D);
                }
            }
        }*/
        private void AddLinesOnCanvas()
        {
            foreach (var line in lines)
            {
                var start3D = new Point3D((line.X1 - 767) / 5, (-line.Y1 + 498) / 5, 0);
                var end3D = new Point3D((line.X2 - 767) / 5, (-line.Y2 + 498) / 5, 0);
                /*var start3D = new Point3D(line.X1, -line.Y1, -100);
                var end3D = new Point3D(line.X2, -line.Y2, -100);*/

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

        private LinesVisual3D AddNewLine3D(Point3D start, Point3D end, double thikness, Color color)
        {
            return new LinesVisual3D
            {
                Points = new Point3DCollection { start, end },
                Thickness = thikness,
                Color = color
            };
        }
    }
    
}
