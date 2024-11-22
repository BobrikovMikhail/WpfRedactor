using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfLab1
{
    public class Save_Load
    {
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlArray("Points")]
        [XmlArrayItem("Point")]
        public List<Point> Points { get; set; }

        [XmlElement("StrokeColor")]
        public string StrokeColor { get; set; }

        [XmlElement("StrokeThickness")]
        public double StrokeThickness { get; set; }

        public Save_Load CreateDrawableObject(string type, List<Point> points,double strokeThickness, string content)
        {
            return new Save_Load
            {
                Type = type,
                Points = new List<Point>(points),
                StrokeColor = "Black",
                StrokeThickness = strokeThickness
            };
        }

        public void SaveObjectsToFile(string filePath, Canvas objectsCanvas)
        {
            var drawableObjects = new List<Save_Load>();
            foreach (var child in objectsCanvas.Children)
            {
                if (child is Line line)
                {
                    drawableObjects.Add(CreateDrawableObject(
                        "Line",
                        new List<Point> { new Point(line.X1, line.Y1), new Point(line.X2, line.Y2) },
                       
                        line.StrokeThickness,
                        null  // Нет содержимого для Line
                    ));
                }
            }

            // Сериализация в XML
            var serializer = new XmlSerializer(typeof(List<Save_Load>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, drawableObjects);
            }
        }

        public void LoadObjectsFromFile(string filePath, Canvas objectsCanvas)
        {
            // Удаляем только линии перед загрузкой новых объектов
            var linesToRemove = objectsCanvas.Children.OfType<Line>().ToList();
            foreach (var line in linesToRemove)
            {
                objectsCanvas.Children.Remove(line);
            }

            // Десериализация объектов из XML файла
            var drawableObjects = DeserializeFromXml(filePath);

            foreach (var drawableObject in drawableObjects)
            {
                if (drawableObject.Type == "Line" && drawableObject.Points.Count >= 2)
                {
                    var line = new Line
                    {
                        X1 = drawableObject.Points[0].X,
                        Y1 = drawableObject.Points[0].Y,
                        X2 = drawableObject.Points[1].X,
                        Y2 = drawableObject.Points[1].Y,
                        Stroke = Brushes.Black,
                        StrokeThickness = drawableObject.StrokeThickness
                    };
                    objectsCanvas.Children.Add(line);
                }
            }
        }

        private List<Save_Load> DeserializeFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Save_Load>));
            using (var reader = new StreamReader(filePath))
            {
                return (List<Save_Load>)serializer.Deserialize(reader);
            }
        }
    }
}