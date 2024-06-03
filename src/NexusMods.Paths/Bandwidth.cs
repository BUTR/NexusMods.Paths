using NexusMods.Paths.Extensions;
using TransparentValueObjects;

namespace NexusMods.Paths;

/// <summary>
/// Represents bandwidth in bytes per second.
/// </summary>
[ValueObject<ulong>]
public readonly partial struct Bandwidth : IAugmentWith<JsonAugment>
{
    /// <inheritdoc />
    public override string ToString() => Value.ToFileSizeString("/sec");
}
