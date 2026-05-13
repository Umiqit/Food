using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.VideoAnalysis.AI
{
    public class PersonDetector
    {
        public Task<int> DetectPeopleCountAsync(byte[] imageData)
        {
            // AI model integration placeholder
            return Task.FromResult(0);
        }
    }
}
