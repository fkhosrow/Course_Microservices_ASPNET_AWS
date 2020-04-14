using AdvertiseApi.Models;
using Amazon.DynamoDBv2.DataModel;

using System;

namespace AdvertiseApi.Services
{
    [DynamoDBTable("Advertisements")]
    public class AdvertiseModelDb
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public string Price { get; set; }

        [DynamoDBProperty]
        public AdvertiseStatus Status { get; set; }
    }
}
