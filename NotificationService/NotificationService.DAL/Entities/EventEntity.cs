using NotificationService.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace NotificationService.DAL.Entities;
public class EventEntity
{
    public Guid Id { get; set; }
    public EventType? Type { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    private string? _metadata;

    public string? Metadata
    {
        get => _metadata;
        set => _metadata = value;
    }

    [NotMapped]
    public Dictionary<string, object> MetadataDict
    {
        get => string.IsNullOrEmpty(_metadata)
            ? new Dictionary<string, object>()
            : JsonSerializer.Deserialize<Dictionary<string, object>>(_metadata)
              ?? new Dictionary<string, object>();
        set => _metadata = JsonSerializer.Serialize(value);
    }

    public T? GetMetadata<T>(string key)
    {
        if (MetadataDict.TryGetValue(key, out var value))
        {
            var json = JsonSerializer.Serialize(value);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        return default;
    }

    public void SetMetadata<T>(string key, T value)
    {
        var dict = MetadataDict;
        dict[key] = value;
        MetadataDict = dict;
    }
}
