using idou.Core.Domain;
using idou.Core.Domain.Enums;
using idou.Core.Mapping;

namespace idou.Core.Engine;

public sealed class ChangePipeline
{
    public ChangeBatch MapAndTransform(
        ChangeBatch batch,
        IMappingPlan mappingPlan,
        IReadOnlyList<ITransformer> transformers)
    {
        ArgumentNullException.ThrowIfNull(batch);
        ArgumentNullException.ThrowIfNull(mappingPlan);
        ArgumentNullException.ThrowIfNull(transformers);

        var nextCheckpoint = batch.NextCheckpoint;

        var output = batch.Events
            .Select(input => MapEvent(input, mappingPlan))
            .Select(mappedChange =>
                transformers.Aggregate(mappedChange, (change, transformer) => transformer.Transform(change)))
            .Where(current => !IsNoOp(current))
            .ToList();

        return new ChangeBatch(output, nextCheckpoint);
    }

    private static ChangeEvent MapEvent(ChangeEvent input, IMappingPlan mappingPlan)
    {
        var targetType = mappingPlan.MapEntityType(input.Type);
        var targetKey = mappingPlan.MapKey(input.Type, input.Key);

        if (input.Payload is null)
        {
            return input with
            {
                Type = targetType,
                Key = targetKey,
                Payload = null
            };
        }

        var mappedRecord = mappingPlan.MapRecord(input.Payload);
        var targetPayload = mappedRecord with
        {
            Type = targetType,
            Key = targetKey
        };

        return input with
        {
            Type = targetType,
            Key = targetKey,
            Payload = targetPayload
        };
    }

    private static bool IsNoOp(ChangeEvent change)
    {
        return change is { Operation: ChangeOperation.Upsert, Payload: null };
    }
}