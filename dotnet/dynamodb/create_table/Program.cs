using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace create_table
{
    class Program
    {
        const string tableName = "AnimalsInventory";

        static void Main(string[] args)
        {
            var config = new AmazonDynamoDBConfig {
                ServiceURL = "http://localhost:4569",
                UseHttp = true,
                AuthenticationRegion = "us-east-1"
            };

            var client = new AmazonDynamoDBClient(config);

            var currentTables = client.ListTablesAsync().Result.TableNames;
            Console.WriteLine("Number of tables: " + currentTables.Count);
            if(!currentTables.Contains(tableName)) {
                var createTableRequest = new CreateTableRequest {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition> {
                        new AttributeDefinition {
                            AttributeName = "Id",
                            // "S" = string, "N" = number, etc.
                            AttributeType = "N"
                        },
                        new AttributeDefinition {
                            AttributeName = "Type",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement> {
                        new KeySchemaElement {
                            AttributeName = "Id",
                            // "HASH" = hash key, "RANGE" = range key.
                            KeyType = "HASH"
                        },
                        new KeySchemaElement {
                            AttributeName = "Type",
                            KeyType = "RANGE"
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput {
                        ReadCapacityUnits = 10,
                        WriteCapacityUnits = 5
                    }
                };

                var response = client.CreateTableAsync(createTableRequest).Result;
                Console.WriteLine("Table created with request ID: " +
                    response.ResponseMetadata.RequestId);
            }
            else {
                Console.WriteLine($"Table with name: {tableName} already exists");
            }
        }
    }
}
