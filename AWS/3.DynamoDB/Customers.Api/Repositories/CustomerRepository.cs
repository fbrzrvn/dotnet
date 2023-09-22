using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;

namespace Customers.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string          _tableName = "customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;

        string                              customerAsJson       = JsonSerializer.Serialize(customer);
        Dictionary<string, AttributeValue>? customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        PutItemRequest createItemRequest = new()
        {
            TableName           = _tableName,
            Item                = customerAsAttributes,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)"
        };

        PutItemResponse? response = await _dynamoDb.PutItemAsync(createItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        GetItemRequest getItemRequest = new()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        GetItemResponse? response = await _dynamoDb.GetItemAsync(getItemRequest);

        if (response.Item.Count == 0)
        {
            return null;
        }

        Document? itemAsDocument = Document.FromAttributeMap(response.Item);

        return JsonSerializer.Deserialize<CustomerDto>(itemAsDocument.ToJson());
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        QueryRequest queryRequest = new()
        {
            TableName              = _tableName,
            IndexName              = "email-id-index",
            KeyConditionExpression = "Email = :v_Email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":v_Email", new AttributeValue { S = email } }
            }
        };

        QueryResponse? response = await _dynamoDb.QueryAsync(queryRequest);

        if (response.Items.Count == 0)
        {
            return null;
        }

        Document? itemDocument = Document.FromAttributeMap(response.Items[0]);

        return JsonSerializer.Deserialize<CustomerDto>(itemDocument.ToJson());
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        ScanRequest scanRequest = new() { TableName = _tableName };

        ScanResponse? response = await _dynamoDb.ScanAsync(scanRequest);

        return response.Items.Select(
            x =>
            {
                string? json = Document.FromAttributeMap(x).ToJson();

                return JsonSerializer.Deserialize<CustomerDto>(json);
            }
        )!;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer, DateTime requestStarted)
    {
        customer.UpdatedAt = DateTime.UtcNow;

        string                              customerAsJson       = JsonSerializer.Serialize(customer);
        Dictionary<string, AttributeValue>? customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        PutItemRequest updateItemRequest = new()
        {
            TableName           = _tableName,
            Item                = customerAsAttributes,
            ConditionExpression = "UpdatedAt < :requestStarted",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":requestStarted", new AttributeValue { S = requestStarted.ToString("O") } }
            }
        };

        PutItemResponse? response = await _dynamoDb.PutItemAsync(updateItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        DeleteItemRequest deleteItemRequest = new()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        DeleteItemResponse? response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}