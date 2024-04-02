using Azure;
using PoMad.Models;

namespace PoMad.Data
{
    public class DailyDataEntity : Azure.Data.Tables.ITableEntity
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int Calories { get; set; }
        public double Weight { get; set; }
        public bool DidOMAD { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public DailyDataEntity()
        {
            PartitionKey = "DailyData";
            RowKey = Guid.NewGuid().ToString();
        }

        public DailyDataEntity(string userId, DailyData dailyData)
        {
            PartitionKey = "DailyData";
            RowKey = $"{userId}_{dailyData.Date:yyyyMMdd}";
            UserId = userId;
            Date = dailyData.Date.ToUniversalTime();
            Calories = dailyData.Calories;
            Weight = dailyData.Weight;
            DidOMAD = dailyData.DidOMAD;
        }
    }
}
