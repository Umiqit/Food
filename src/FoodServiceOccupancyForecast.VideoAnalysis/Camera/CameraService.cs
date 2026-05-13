using System.Threading.Tasks;

namespace FoodServiceOccupancyForecast.VideoAnalysis.Camera
{
    public class CameraService
    {
        public Task<byte[]> CaptureFrameAsync(string cameraUrl)
        {
            // RTSP/ONVIF camera integration placeholder
            return Task.FromResult(new byte[0]);
        }
    }
}
