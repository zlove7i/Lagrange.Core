using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class RecordSegment(string url)
{
    public RecordSegment() : this("") { }

    [JsonPropertyName("file")] public string Url { get; set; } = url;
}

[SegmentSubscriber(typeof(RecordEntity), "record")]
public partial class RecordSegment : ISegment
{
    public IMessageEntity ToEntity() => new RecordEntity(Url);
    
    public void Build(MessageBuilder builder, ISegment segment)
    {
        if (segment is RecordSegment recordSegment and not { Url: "" } && CommonResolver.Resolve(recordSegment.Url) is { } image)
        {
            builder.Record(image);
        }
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not RecordEntity recordEntity) throw new ArgumentException("Invalid entity type.");

        return new RecordSegment(recordEntity.AudioUrl);
    }
}