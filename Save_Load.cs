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
        [XmlElement("Content")]
        public string Content { get; set; }  // Для хранения содержимого Button, Label


        public Save_Load CreateDrawableObject(string type, List<Point> points, string strokeColor, double strokeThickness, string content)
        {
            return new Save_Load
            {
                Type = type,
                Points = new List<Point>(points),
                StrokeColor = strokeColor,
                StrokeThickness = strokeThickness,
                Content = content
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
                        ((SolidColorBrush)line.Stroke).Color.ToString(),
                        line.StrokeThickness,
                        null  // Нет содержимого для Line
                    ));
                }
                else if (child is Button button)
                {
                    var topLeft = new Point(Canvas.GetLeft(button), Canvas.GetTop(button));
                    drawableObjects.Add(CreateDrawableObject(
                        "Button",
                        new List<Point> { topLeft },
                        null,  // Нет цвета обводки для Button
                        0,     // Нет толщины обводки для Button
                        button.Content.ToString()  // Содержимое Button
                    ));
                }
                else if (child is Label label)
                {
                    var position = new Point(Canvas.GetLeft(label), Canvas.GetTop(label));
                    drawableObjects.Add(CreateDrawableObject(
                        "Label",
                        new List<Point> { position },
                        null,  // Нет цвета обводки для Label
                        0,     // Нет толщины обводки для Label
                        label.Content.ToString()  // Содержимое Label
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
            // Очистка Canvas перед загрузкой новых объектов
            objectsCanvas.Children.Clear();

            // Десериализация объектов из XML файла
            var drawableObjects = DeserializeFromXml(filePath);

            foreach (var drawableObject in drawableObjects)
            {
                switch (drawableObject.Type)
                {
                    case "Line":
                        if (drawableObject.Points.Count >= 2)
                        {
                            var line = new Line
                            {
                                X1 = drawableObject.Points[0].X,
                                Y1 = drawableObject.Points[0].Y,
                                X2 = drawableObject.Points[1].X,
                                Y2 = drawableObject.Points[1].Y,
                                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(drawableObject.StrokeColor)),
                                StrokeThickness = drawableObject.StrokeThickness
                            };
                            objectsCanvas.Children.Add(line);
                        }
                        break;
                    case "Button":
                        if (drawableObject.Points.Count >= 2)
                        {
                            var button = new Button
                            {
                                Content = drawableObject.Content,
                                Width = drawableObject.Points[1].X - drawableObject.Points[0].X,
                                Height = drawableObject.Points[1].Y - drawableObject.Points[0].Y
                            };
                            Canvas.SetLeft(button, drawableObject.Points[0].X);
                            Canvas.SetTop(button, drawableObject.Points[0].Y);
                            objectsCanvas.Children.Add(button);
                        }
                        break;
                    case "Label":
                        if (drawableObject.Points.Count >= 1)
                        {
                            var label = new Label
                            {
                                Content = drawableObject.Content
                            };
                            Canvas.SetLeft(label, drawableObject.Points[0].X);
                            Canvas.SetTop(label, drawableObject.Points[0].Y);
                            objectsCanvas.Children.Add(label);
                        }
                        break;
                        // Добавьте дополнительные случаи для других UI элементов, если необходимо
                }
            }
        }

        // Метод для десериализации объектов из XML файла
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