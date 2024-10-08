﻿using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using TestTask.Domain.Entites;
using TestTask.Infrastructure.Inerfaces.Repositories;

namespace TestTask.Infrastructure.Implementations.Repositories;

public class StorageRepository : IStorageRepository
{
    private const int _maxQuery = 3601;
    private readonly TableClient _tableClient;

    public StorageRepository(IConfiguration configuration)
    {
        _tableClient = new TableClient(configuration["Values:AzureWebJobsStorage"], configuration["AzureStorageName"]);
        _tableClient.CreateIfNotExists();
    }

    public IList<WeatherLog> GetLogByDataRange(DateTimeOffset from, DateTimeOffset to,
        CancellationToken cancellationToken)
    {
        return _tableClient
            .Query<WeatherLog>(cancellationToken: cancellationToken)
            .Where(log => log.Timestamp >= from && log.Timestamp <= to)
            .Take(_maxQuery)
            .ToList();
    }

    public async Task<Response> SaveWeatherDataAsync(WeatherLog data, CancellationToken cancellationToken)
    {
        return await _tableClient.AddEntityAsync(data, cancellationToken);
    }
}