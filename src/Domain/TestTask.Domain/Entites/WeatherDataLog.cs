﻿using Azure;
using Azure.Data.Tables;

namespace TestTask.Domain.Entites;

public class WeatherLog : ITableEntity
{
    public WeatherLog()
    {
    }

    public WeatherLog(string rowKey, int statusCode)
    {
        (RowKey, StatusCode) = (rowKey, statusCode);
    }

    public int StatusCode { get; set; }

    public string PartitionKey { get; set; } = "London";
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}