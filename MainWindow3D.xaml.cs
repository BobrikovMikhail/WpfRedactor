using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WpfLab1
{
    public partial class MainWindow3D : Window
    {
        private Point3D curStartPoint;
        private bool isDrawing;
        private LinesVisual3D currentLine;

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
    }
}
