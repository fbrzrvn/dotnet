using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

AmazonDynamoDBClient dynamoDb = new();

QueryRequest queryRequest = new()
{
    TableName              = "movies",
    IndexName              = "year-rotten-index",
    KeyConditionExpression = "ReleaseYear = :v_Year and RottenTomatoesPercentage >= :v_Rotten",
    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
    {
        { ":v_Year", new AttributeValue { N = "2018" } }, { ":v_Rotten", new AttributeValue { N = "88" } }
    }
};

QueryResponse? response = await dynamoDb.QueryAsync(queryRequest);

foreach (Dictionary<string, AttributeValue> itemAttribute in response.Items)
{
    Document? document = Document.FromAttributeMap(itemAttribute);
    string?   json     = document.ToJsonPretty();

    Console.Write(json);
}