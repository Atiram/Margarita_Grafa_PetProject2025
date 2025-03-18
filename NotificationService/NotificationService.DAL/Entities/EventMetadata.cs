using System.Text.Json;

namespace NotificationService.DAL.Entities;

public class EventMetadata
{
    public string? Metadata { get; set; }

    public T? GetMetadata<T>(string key)
    {
        if (string.IsNullOrEmpty(Metadata))
        {
            return default;
        }

        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(Metadata, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dict != null && dict.TryGetValue(key, out var value))
            {
                var json = JsonSerializer.Serialize(value);
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return default;
        }
        catch
        {
            return default;
        }
    }

    public void SetMetadata<T>(string key, T value)
    {
        var dict = string.IsNullOrEmpty(Metadata)
            ? new Dictionary<string, object>()
            : JsonSerializer.Deserialize<Dictionary<string, object>>(Metadata, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new Dictionary<string, object>();

        dict[key] = value;
        Metadata = JsonSerializer.Serialize(dict);
    }
}