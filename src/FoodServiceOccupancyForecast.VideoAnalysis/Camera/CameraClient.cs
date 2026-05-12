namespace FoodServiceOccupancyForecast.VideoAnalysis.Camera;

public class CameraClient
{
    private readonly string _rtspUrl;
    private bool _isConnected;

    public CameraClient(string rtspUrl)
    {
        _rtspUrl = rtspUrl;
    }

    public bool Connect()
    {
        // Mock connection
        _isConnected = true;
        return _isConnected;
    }

    public void Disconnect()
    {
        _isConnected = false;
    }

    public byte[]? CaptureFrame()
    {
        if (!_isConnected) return null;
        // Mock: return empty frame (in real project use OpenCV/FFmpeg)
        return new byte[640 * 480 * 3]; // RGB frame
    }

    public bool IsConnected => _isConnected;
}
