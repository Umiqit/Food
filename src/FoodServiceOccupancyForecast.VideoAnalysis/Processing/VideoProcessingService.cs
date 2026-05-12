using FoodServiceOccupancyForecast.VideoAnalysis.AI;
using FoodServiceOccupancyForecast.VideoAnalysis.Camera;

namespace FoodServiceOccupancyForecast.VideoAnalysis.Processing;

public class VideoProcessingService
{
    private readonly PeopleDetector _detector;
    private readonly Dictionary<string, CameraClient> _cameras;

    public VideoProcessingService()
    {
        _detector = new PeopleDetector();
        _cameras = new Dictionary<string, CameraClient>();
    }

    public void AddCamera(string cameraId, string rtspUrl)
    {
        _cameras[cameraId] = new CameraClient(rtspUrl);
    }

    public bool ConnectCamera(string cameraId)
    {
        if (_cameras.TryGetValue(cameraId, out var camera))
            return camera.Connect();
        return false;
    }

    public int GetPeopleCount(string cameraId)
    {
        if (!_cameras.TryGetValue(cameraId, out var camera) || !camera.IsConnected)
            return 0;

        var frame = camera.CaptureFrame();
        if (frame == null) return 0;

        return _detector.DetectPeople(frame);
    }

    public Dictionary<string, int> GetAllZonesCount()
    {
        var result = new Dictionary<string, int>();
        foreach (var cam in _cameras)
        {
            result[cam.Key] = GetPeopleCount(cam.Key);
        }
        return result;
    }
}
