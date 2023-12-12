using System;
using System.Diagnostics.CodeAnalysis;
using NexusMods.Paths.Trees.Traits;

namespace NexusMods.Paths.Trees;

/// <summary>
///     A boxed element that implements <see cref="IHaveBoxedChildrenWithKey{TKey,TSelf}" />
/// </summary>
/// <remarks>
///     This type exists to help the user avoid having to type the generic arguments over and over again throughout code.
/// </remarks>
[ExcludeFromCodeCoverage]
public class KeyedBox<TKey, TSelf> : Box<TSelf>, IEquatable<KeyedBox<TKey, TSelf>> where TSelf : struct
    where TKey : notnull
{
    /// <summary />
    public static implicit operator TSelf(KeyedBox<TKey, TSelf> box) => box.Item;

    /// <summary />
    public static explicit operator KeyedBox<TKey, TSelf>(TSelf item) => new() { Item = item };

    #region Autogenerated by R#
    /// <inheritdoc />
    public bool Equals(KeyedBox<TKey, TSelf>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Item.Equals(other.Item);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((KeyedBox<TKey, TSelf>)obj);
    }

    /// <inheritdoc />
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Item.GetHashCode();
    #endregion
}
