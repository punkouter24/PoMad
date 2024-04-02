using Azure.Data.Tables;
using Azure;
using PoMad.Data;
using PoMad.Models;

namespace PoMad.Services
{
    public class DailyDataService
    {
        private readonly TableClient _tableClient;

        public DailyDataService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureTableStorage");
            _tableClient = new TableClient(connectionString, "DailyData");
            _tableClient.CreateIfNotExists();
        }

        public async Task<DailyData> GetDailyDataAsync(string userId, DateTime date)
        {
            try
            {
                var dailyDataEntity = await _tableClient.GetEntityAsync<DailyDataEntity>("DailyData", $"{userId}_{date:yyyyMMdd}");
                return new DailyData
                {
                    UserId = dailyDataEntity.Value.UserId,
                    Date = dailyDataEntity.Value.Date,
                    Calories = dailyDataEntity.Value.Calories,
                    Weight = dailyDataEntity.Value.Weight,
                    DidOMAD = dailyDataEntity.Value.DidOMAD
                };
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<List<DailyData>> GetDailyDataForUserAsync(string userId)
        {
            var dailyDataList = new List<DailyData>();

            // Assuming you want to get all entities in the "DailyData" partition
            // and then filter them by RowKey starting with userId on the client side.
            var query = _tableClient.QueryAsync<DailyDataEntity>(e => e.PartitionKey == "DailyData");

            await foreach (var dailyDataEntity in query)
            {
                if (dailyDataEntity.RowKey.StartsWith(userId))
                {
                    dailyDataList.Add(new DailyData
                    {
                        UserId = dailyDataEntity.UserId,
                        Date = dailyDataEntity.Date,
                        Calories = dailyDataEntity.Calories,
                        Weight = dailyDataEntity.Weight,
                        DidOMAD = dailyDataEntity.DidOMAD
                    });
                }
            }

            return dailyDataList;
        }


        public async Task SaveDailyDataAsync(DailyData dailyData)
        {
            var dailyDataEntity = new DailyDataEntity(dailyData.UserId, dailyData);
            await _tableClient.UpsertEntityAsync(dailyDataEntity);
        }
    }
}
