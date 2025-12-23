using idou.Core.Domain;
using idou.Core.Domain.Enums;
using idou.Core.Engine;
using idou.Core.Mapping;

namespace idou.Core.Tests.Engine;

public sealed class ChangePipelineTests
{
    [Fact]
    public void MapAndTransform_WhenBatchIsNull_ThrowsArgumentNullException()
    {
        var pipeline = new ChangePipeline();

        Assert.Throws<ArgumentNullException>(() =>
            pipeline.MapAndTransform(null!, new RecordingMappingPlan(), []));
    }

    [Fact]
    public void MapAndTransform_WhenMappingPlanIsNull_ThrowsArgumentNullException()
    {
        var pipeline = new ChangePipeline();

        Assert.Throws<ArgumentNullException>(() =>
            pipeline.MapAndTransform(Batch(), null!, []));
    }

    [Fact]
    public void MapAndTransform_WhenTransformersIsNull_ThrowsArgumentNullException()
    {
        var pipeline = new ChangePipeline();

        Assert.Throws<ArgumentNullException>(() =>
            pipeline.MapAndTransform(Batch(), new RecordingMappingPlan(), null!));
    }

    [Fact]
    public void MapAndTransform_PreservesNextCheckpoint()
    {
        var pipeline = new ChangePipeline();
        var checkpoint = Checkpoint("cp-123");

        var input = Batch(
            events: [Upsert(Type("User"), Key("u1"), payload: Record(Type("User"), Key("u1")))],
            nextCheckpoint: checkpoint
        );

        var result = pipeline.MapAndTransform(input, new DefaultMappingPlan(), Array.Empty<ITransformer>());

        Assert.Equal(checkpoint, result.NextCheckpoint);
    }

    [Fact]
    public void MapAndTransform_MapsTypeAndKey_OnEventAndPayload()
    {
        var pipeline = new ChangePipeline();

        var sourceType = Type("User");
        var sourceKey = Key("u1");
        var payload = Record(sourceType, sourceKey);

        var input = Upsert(sourceType, sourceKey, payload);

        var mappedType = Type("Person");
        var mappedKey = Key("p-100");

        var mappingPlan = new RecordingMappingPlan(
            mapEntityType: _ => mappedType,
            mapKey: (_, _) => mappedKey,
            mapRecord: r => r
        );

        var batch = Batch(input);

        var result = pipeline.MapAndTransform(batch, mappingPlan, Array.Empty<ITransformer>());
        var evt = result.Events.Single();

        Assert.Equal(mappedType, evt.Type);
        Assert.Equal(mappedKey, evt.Key);

        Assert.NotNull(evt.Payload);
        Assert.Equal(mappedType, evt.Payload!.Type);
        Assert.Equal(mappedKey, evt.Payload!.Key);

        Assert.Equal(1, mappingPlan.MapEntityTypeCalls);
        Assert.Equal(1, mappingPlan.MapKeyCalls);
        Assert.Equal(1, mappingPlan.MapRecordCalls);
    }

    [Fact]
    public void MapAndTransform_WhenPayloadIsNull_DoesNotCallMapRecord_AndStillMapsTypeAndKey()
    {
        var pipeline = new ChangePipeline();

        var sourceType = Type("User");
        var sourceKey = Key("u1");

        var input = Delete(sourceType, sourceKey); // payload null, typically delete

        var mappedType = Type("Person");
        var mappedKey = Key("p-100");

        var mappingPlan = new RecordingMappingPlan(
            mapEntityType: _ => mappedType,
            mapKey: (_, _) => mappedKey,
            mapRecord: _ => throw new Exception("MapRecord must not be called when payload is null")
        );

        var batch = Batch(input);

        var result = pipeline.MapAndTransform(batch, mappingPlan, Array.Empty<ITransformer>());
        var evt = result.Events.Single();

        Assert.Equal(mappedType, evt.Type);
        Assert.Equal(mappedKey, evt.Key);
        Assert.Null(evt.Payload);

        Assert.Equal(1, mappingPlan.MapEntityTypeCalls);
        Assert.Equal(1, mappingPlan.MapKeyCalls);
        Assert.Equal(0, mappingPlan.MapRecordCalls);
    }

    [Fact]
    public void MapAndTransform_AppliesTransformersInOrder_PerEvent()
    {
        var pipeline = new ChangePipeline();

        var input = Upsert(Type("User"), Key("u1"), Record(Type("User"), Key("u1")));
        var batch = Batch(input);

        var mappingPlan = new DefaultMappingPlan();

        var t1 = new RecordingTransformer();
        var t2 = new RecordingTransformer();
        var transformers = new ITransformer[] { t1, t2 };

        _ = pipeline.MapAndTransform(batch, mappingPlan, transformers);

        Assert.Equal(1, t1.CallCount);
        Assert.Equal(1, t2.CallCount);

        Assert.Same(t1.LastOutput!, t2.LastInput!);
    }

    [Fact]
    public void MapAndTransform_FiltersOutNoOp_UpsertWithNullPayload()
    {
        var pipeline = new ChangePipeline();

        var noop = Upsert(Type("User"), Key("u1"), payload: null);

        var nonNoop = Delete(Type("User"), Key("u2")); // payload null but not Upsert

        var batch = Batch(noop, nonNoop);

        var result = pipeline.MapAndTransform(batch, new DefaultMappingPlan(), Array.Empty<ITransformer>());

        Assert.Single(result.Events);
        Assert.Equal(ChangeOperation.Delete, result.Events[0].Operation);
    }

    [Fact]
    public void MapAndTransform_IfTransformerTurnsEventIntoNoOp_ItIsFilteredOut()
    {
        var pipeline = new ChangePipeline();

        var input = Upsert(Type("User"), Key("u1"), Record(Type("User"), Key("u1")));
        var batch = Batch(input);

        var mappingPlan = new DefaultMappingPlan();

        var makeNoOp = new LambdaTransformer(_ =>
            Upsert(Type("User"), Key("u1"), payload: null)
        );

        var result = pipeline.MapAndTransform(batch, mappingPlan, [makeNoOp]);

        Assert.Empty(result.Events);
    }

    private sealed class RecordingMappingPlan : IMappingPlan
    {
        private readonly Func<EntityType, EntityType> _mapEntityType;
        private readonly Func<EntityType, EntityKey, EntityKey> _mapKey;
        private readonly Func<EntityRecord, EntityRecord> _mapRecord;

        public int MapEntityTypeCalls { get; private set; }
        public int MapKeyCalls { get; private set; }
        public int MapRecordCalls { get; private set; }

        public RecordingMappingPlan(
            Func<EntityType, EntityType>? mapEntityType = null,
            Func<EntityType, EntityKey, EntityKey>? mapKey = null,
            Func<EntityRecord, EntityRecord>? mapRecord = null)
        {
            _mapEntityType = mapEntityType ?? (t => t);
            _mapKey = mapKey ?? ((_, k) => k);
            _mapRecord = mapRecord ?? (r => r);
        }

        public EntityType MapEntityType(EntityType sourceType)
        {
            MapEntityTypeCalls++;
            return _mapEntityType(sourceType);
        }

        public EntityKey MapKey(EntityType sourceType, EntityKey sourceKey)
        {
            MapKeyCalls++;
            return _mapKey(sourceType, sourceKey);
        }

        public EntityRecord MapRecord(EntityRecord sourceRecord)
        {
            MapRecordCalls++;
            return _mapRecord(sourceRecord);
        }
    }

    private sealed class RecordingTransformer : ITransformer
    {
        public int CallCount { get; private set; }
        public ChangeEvent? LastInput { get; private set; }
        public ChangeEvent? LastOutput { get; private set; }

        public ChangeEvent Transform(ChangeEvent change)
        {
            CallCount++;
            LastInput = change;

            var output = change with { };
            LastOutput = output;
            return output;
        }
    }

    private sealed class LambdaTransformer(Func<ChangeEvent, ChangeEvent> fn) : ITransformer
    {
        public ChangeEvent Transform(ChangeEvent change) => fn(change);
    }

    private static EntityType Type(string value) => new(value);
    private static EntityKey Key(string value) => new(value);

    private static EntityRecord Record(EntityType type, EntityKey key)
        => new()
        {
            Type = type,
            Key = key,
            Attributes = new Dictionary<string, object?>()
        };

    private static ChangeEvent Upsert(EntityType type, EntityKey key, EntityRecord? payload)
        => new()
        {
            Operation = ChangeOperation.Upsert,
            Type = type,
            Key = key,
            Payload = payload
        };

    private static ChangeEvent Delete(EntityType type, EntityKey key)
        => new()
        {
            Operation = ChangeOperation.Delete,
            Type = type,
            Key = key,
            Payload = null
        };

    private static Checkpoint Checkpoint(string value) => new(value);

    private static ChangeBatch Batch(params ChangeEvent[] events)
        => Batch(events, nextCheckpoint: Checkpoint("cp-default"));

    private static ChangeBatch Batch(IEnumerable<ChangeEvent> events, Checkpoint nextCheckpoint) =>
        new(events.ToList(), nextCheckpoint);
}