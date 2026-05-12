using System.Drawing;

namespace FoodServiceOccupancyForecast.VideoAnalysis.AI;

public class PeopleDetector
{
    // Mock implementation - in real project would use ONNX/YOLO model
    public int DetectPeople(byte[] imageData)
    {
        // Simulate detection with random variance around expected count
        var random = new Random();
        return random.Next(0, 50); // Mock: 0-50 people detected
    }

    public List<DetectionBox> DetectPeopleWithBoxes(byte[] imageData)
    {
        var random = new Random();
        var count = random.Next(0, 20);
        var boxes = new List<DetectionBox>();
        for (int i = 0; i < count; i++)
        {
            boxes.Add(new DetectionBox
            {
                X = random.Next(0, 640),
                Y = random.Next(0, 480),
                Width = random.Next(30, 100),
                Height = random.Next(50, 150),
                Confidence = random.NextDouble() * 0.5 + 0.5
            });
        }
        return boxes;
    }
}

public class DetectionBox
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Confidence { get; set; }
}
