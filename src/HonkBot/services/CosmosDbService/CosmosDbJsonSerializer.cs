using System.Text.Json;
using Azure.Core.Serialization;
using Microsoft.Azure.Cosmos;

namespace HonkBot.Services;

/// <summary>
/// A custom serializer to serialize and deserialize data for the CosmosDB service.
/// </summary>
public class CosmosDbJsonSerializer : CosmosSerializer
{
    private readonly JsonObjectSerializer jsonSerializer;

    /// <summary>
    /// Default constructor for the <see cref="CosmosDbJsonSerializer"/> class.
    /// </summary>
    /// <param name="jsonSerializerOptions"></param>
    public CosmosDbJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        jsonSerializer = new(jsonSerializerOptions);
    }

    /// <inheritdoc cref="Microsoft.Azure.Cosmos.CosmosSerializer.FromStream"/>
    public override T FromStream<T>(Stream stream)
    {
        using (stream)
        {
            if (stream.CanSeek
                   && stream.Length == 0)
            {
                return default!;
            }

            if (typeof(Stream).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)stream;
            }

            return (T)jsonSerializer.Deserialize(
                stream: stream,
                returnType: typeof(T),
                cancellationToken: default
            )!;
        }
    }

    /// <inheritdoc cref="Microsoft.Azure.Cosmos.CosmosSerializer.ToStream"/>
    public override Stream ToStream<T>(T input)
    {
        MemoryStream streamPayload = new();

        jsonSerializer.Serialize(
            stream: streamPayload,
            value: input,
            inputType: typeof(T),
            cancellationToken: default
        );
        streamPayload.Position = 0;

        return streamPayload;
    }
}