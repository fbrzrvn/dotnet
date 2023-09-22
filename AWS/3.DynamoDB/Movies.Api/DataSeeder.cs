using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace Movies.Api;

public class DataSeeder
{
    public async Task ImportDataAsync()
    {
        AmazonDynamoDBClient dynamoDb = new();
        string[]             lines    = await File.ReadAllLinesAsync("./movies.csv");

        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }

            string   line       = lines[i];
            string[] commaSplit = line.Split(",");

            string title          = commaSplit[0];
            int    year           = int.Parse(commaSplit[1]);
            int    ageRestriction = int.Parse(commaSplit[2]);
            int    rottenTomatoes = int.Parse(commaSplit[3]);

            Movie movie = new()
            {
                Id                       = Guid.NewGuid(),
                Title                    = title,
                ReleaseYear              = year,
                AgeRestriction           = ageRestriction,
                RottenTomatoesPercentage = rottenTomatoes
            };

            string                              movieAsJson    = JsonSerializer.Serialize(movie);
            Document?                           itemDocument   = Document.FromJson(movieAsJson);
            Dictionary<string, AttributeValue>? itemAttributes = itemDocument.ToAttributeMap();

            PutItemRequest createItemRequest = new() { TableName = "movies", Item = itemAttributes };

            await dynamoDb.PutItemAsync(createItemRequest);
            await Task.Delay(300);
        }
    }
}