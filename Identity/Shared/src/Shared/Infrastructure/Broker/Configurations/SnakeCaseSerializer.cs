namespace Shared.Infrastructure.Broker.Configurations;

using System.Text;
using KafkaFlow;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class SnakeCaseSerializer : ISerializer
{
    private const int _defaultBufferSize = 1024;

    private static readonly UTF8Encoding           _utf8NoBom = new(false);
    private readonly        JsonSerializerSettings _settings;

    public SnakeCaseSerializer()
    {
        var contractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

        _settings = new JsonSerializerSettings
        {
            ContractResolver = contractResolver, Formatting = Formatting.Indented
        };
    }

    /// <inheritdoc />
    public Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
        using var sw = new StreamWriter(
            output,
            _utf8NoBom,
            _defaultBufferSize,
            true
        );
        var serializer = JsonSerializer.CreateDefault(_settings);

        serializer.Serialize(sw, message);

        return Task.CompletedTask;
    }

    public Task<object?> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        using var sr = new StreamReader(
            input,
            _utf8NoBom,
            true,
            _defaultBufferSize,
            true
        );

        var serializer         = JsonSerializer.CreateDefault(_settings);
        var deserializedObject = serializer.Deserialize(sr, type);

        return Task.FromResult(deserializedObject);
    }
}
