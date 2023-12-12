using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NexusMods.Paths.Trees.Traits.Interfaces;
using Reloaded.Memory.Extensions;

namespace NexusMods.Paths.Trees.Traits;

/// <summary>
///     An interface used by Tree implementations to indicate that they have a child.
/// </summary>
/// <typeparam name="TSelf">The type of the child stored in this FileTree.</typeparam>
public interface IHaveObservableChildren<TSelf> where TSelf : struct, IHaveObservableChildren<TSelf>
{
    /// <summary>
    ///     An observable collection containing all the children of this node.
    /// </summary>
    public ObservableCollection<Box<TSelf>> Children { get; }
}

/// <summary>
///     Trait methods for <see cref="IHaveObservableChildren{TSelf}" />.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IHaveObservableChildrenExtensions
{
    /// <summary>
    ///     True if the current node is a leaf (it has no children).
    /// </summary>
    /// <param name="item">The node to check.</param>
    public static bool IsLeaf<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => item.Item.IsLeaf();

    /// <summary>
    ///     True if the current node is a leaf (it has no children).
    /// </summary>
    /// <param name="item">The node to check.</param>
    public static bool IsLeaf<TSelf>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => item.Children.Count == 0;

    /// <summary>
    ///     Enumerates all child nodes of the current node in a breadth-first manner.
    /// </summary>
    /// <param name="item">The node whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>An IEnumerable of all child nodes of the current node.</returns>
    public static IEnumerable<Box<TSelf>> EnumerateChildrenBfs<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => item.Item.EnumerateChildrenBfs();

    /// <summary>
    ///     Enumerates all child nodes of the current node in a breadth-first manner.
    /// </summary>
    /// <param name="item">The node whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>An IEnumerable of all child nodes of the current node.</returns>
    public static IEnumerable<Box<TSelf>> EnumerateChildrenBfs<TSelf>(this TSelf item) where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        // Return the current item's immediate children first.
        foreach (var child in item.Children)
            yield return child;

        // Then return the children of those children.
        foreach (var child in item.Children)
        foreach (var grandChild in child.Item.EnumerateChildrenBfs())
            yield return grandChild;
    }

    /// <summary>
    ///     Enumerates all child nodes of the current node in a depth-first manner.
    /// </summary>
    /// <param name="item">The node whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>An IEnumerable of all child nodes of the current node.</returns>
    public static IEnumerable<Box<TSelf>> EnumerateChildrenDfs<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => item.Item.EnumerateChildrenDfs();

    /// <summary>
    ///     Enumerates all child nodes of the current node in a depth-first manner.
    /// </summary>
    /// <param name="item">The node whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>An IEnumerable of all child nodes of the current node.</returns>
    public static IEnumerable<Box<TSelf>> EnumerateChildrenDfs<TSelf>(this TSelf item) where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        foreach (var child in item.Children)
        {
            yield return child;
            foreach (var grandChild in child.Item.EnumerateChildrenDfs())
                yield return grandChild;
        }
    }

    /// <summary>
    ///     Enumerates all child nodes of the current node in a breadth-first manner that satisfy a given filter.
    /// </summary>
    /// <param name="item">The node, wrapped in a Box, whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">The type of the filter struct.</typeparam>
    /// <returns>An IEnumerable of all filtered child nodes of the current node.</returns>
    [ExcludeFromCodeCoverage] // Wrapper
    public static IEnumerable<Box<TSelf>> EnumerateChildrenFilteredBfs<TSelf, TFilter>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<Box<TSelf>>
        => item.Item.EnumerateChildrenFilteredBfs<TSelf, TFilter>();

    /// <summary>
    ///     Enumerates all child nodes of the current node in a breadth-first manner that satisfy a given filter.
    /// </summary>
    /// <param name="item">The node whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">The type of the filter struct.</typeparam>
    /// <returns>An IEnumerable of all filtered child nodes of the current node.</returns>
    public static IEnumerable<Box<TSelf>> EnumerateChildrenFilteredBfs<TSelf, TFilter>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<Box<TSelf>>
    {
        // Return the current item's immediate children first if they match the filter.
        foreach (var child in item.Children)
            if (TFilter.Match(child))
                yield return child;

        // Then return the filtered children of those children.
        foreach (var child in item.Children)
        foreach (var grandChild in child.Item.EnumerateChildrenFilteredBfs<TSelf, TFilter>())
            yield return grandChild;
    }

    /// <summary>
    ///     Enumerates all child nodes of the current node in a depth-first manner that satisfy a given filter.
    /// </summary>
    /// <param name="item">The node, wrapped in a Box, whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">The type of the filter struct.</typeparam>
    /// <returns>An IEnumerable of all filtered child nodes of the current node.</returns>
    [ExcludeFromCodeCoverage] // Wrapper
    public static IEnumerable<Box<TSelf>> EnumerateChildrenFilteredDfs<TSelf, TFilter>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<Box<TSelf>>
        => item.Item.EnumerateChildrenFilteredDfs<TSelf, TFilter>();

    /// <summary>
    ///     Enumerates all child nodes of the current node in a depth-first manner that satisfy a given filter.
    /// </summary>
    /// <param name="item">The node whose children are to be enumerated.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">The type of the filter struct.</typeparam>
    /// <returns>An IEnumerable of all filtered child nodes of the current node.</returns>
    public static IEnumerable<Box<TSelf>> EnumerateChildrenFilteredDfs<TSelf, TFilter>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<Box<TSelf>>
    {
        foreach (var child in item.Children)
        {
            if (TFilter.Match(child))
                yield return child;

            foreach (var grandChild in child.Item.EnumerateChildrenFilteredDfs<TSelf, TFilter>())
                yield return grandChild;
        }
    }

    /// <summary>
    ///     Counts the number of children that match the given filter under this node.
    /// </summary>
    /// <param name="item">The node whose children are to be counted.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <returns>The total count of children that match the filter under this node.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // Wrapper
    public static int CountChildren<TSelf, TFilter>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
        => item.Item.CountChildren<TSelf, TFilter>();

    /// <summary>
    ///     Counts the number of children that match the given filter under this node.
    /// </summary>
    /// <param name="item">The node whose children are to be counted.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <returns>The total count of children that match the filter under this node.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountChildren<TSelf, TFilter>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
    {
        var result = 0;
        item.CountChildrenRecursive<TSelf, TFilter>(ref result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CountChildrenRecursive<TSelf, TFilter>(this TSelf item, ref int accumulator)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
    {
        foreach (var child in item.Children)
        {
            var matchesFilter = TFilter.Match(child.Item);
            accumulator += Unsafe.As<bool, byte>(ref matchesFilter); // Branchless increment.
            child.Item.CountChildrenRecursive<TSelf, TFilter>(ref accumulator);
        }
    }

    /// <summary>
    ///     Counts the number of direct child nodes of the current node.
    /// </summary>
    /// <param name="item">The node whose children are to be counted.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>The count of direct child nodes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountChildren<TSelf>(this Box<TSelf> item) where TSelf : struct, IHaveObservableChildren<TSelf>
        => item.Item.CountChildren();

    /// <summary>
    ///     Counts the number of direct child nodes of the current node.
    /// </summary>
    /// <param name="item">The node whose children are to be counted.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>The count of direct child nodes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountChildren<TSelf>(this TSelf item) where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        var result = 0;
        item.CountChildrenRecursive(ref result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CountChildrenRecursive<TSelf>(this TSelf item, ref int accumulator)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        accumulator += item.Children.Count;
        foreach (var child in item.Children) // <= lowered to 'for loop' because array.
            child.Item.CountChildrenRecursive(ref accumulator);
    }

    /// <summary>
    ///     Recursively returns all the child items of this node selected by the given selector.
    /// </summary>
    /// <param name="item">The boxed node whose child items to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TSelector">The type of the selector.</typeparam>
    /// <returns>An array of all the selected child items of this node.</returns>
    [ExcludeFromCodeCoverage]
    public static TResult[] GetChildrenRecursive<TSelf, TResult, TSelector>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
        => item.Item.GetChildrenRecursive<TSelf, TResult, TSelector>();

    /// <summary>
    ///     Recursively returns all the child items of this node selected by the given selector.
    /// </summary>
    /// <param name="item">The node whose child items to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TSelector">The type of the selector.</typeparam>
    /// <returns>An array of all the selected child items of this node.</returns>
    public static TResult[] GetChildrenRecursive<TSelf, TResult, TSelector>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
    {
        var totalValues = item.CountChildren();
        var results = GC.AllocateUninitializedArray<TResult>(totalValues);
        var index = 0;
        GetChildrenRecursiveUnsafe<TSelf, TResult, TSelector>(item, results, ref index);
        return results;
    }

    /// <summary>
    ///     Helper method to populate child items recursively.
    /// </summary>
    /// <param name="item">The current node.</param>
    /// <param name="buffer">
    ///     The span to fill with child items.
    ///     Should be at least as big as <see cref="IHaveBoxedChildrenExtensions.CountChildren{TSelf}(Box{TSelf})"/>
    /// </param>
    /// <param name="index">The current index in the array.</param>
    public static void GetChildrenRecursiveUnsafe<TSelf, TResult, TSelector>(this TSelf item, Span<TResult> buffer, ref int index)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
    {
        // Populate breadth first. Improved cache locality helps here.
        foreach (var child in item.Children)
            buffer.DangerousGetReferenceAt(index++) = TSelector.Select(child.Item);

        foreach (var child in item.Children)
            GetChildrenRecursiveUnsafe<TSelf, TResult, TSelector>(child.Item, buffer, ref index);
    }

    /// <summary>
    ///     Recursively returns all the child items of this node that match the given filter, transformed by the selector.
    /// </summary>
    /// <param name="item">The boxed node whose child items to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TSelector">The type of the selector.</typeparam>
    /// <returns>An array of all the transformed child items of this node that match the filter.</returns>
    public static TResult[] GetChildrenRecursive<TSelf, TResult, TFilter, TSelector>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
        => item.Item.GetChildrenRecursive<TSelf, TResult, TFilter, TSelector>();

    /// <summary>
    /// Recursively returns all the child items of this node that match the given filter, transformed by the selector.
    /// </summary>
    /// <param name="item">The node whose child items to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TSelector">The type of the selector.</typeparam>
    /// <returns>An array of all the transformed child items of this node that match the filter.</returns>
    public static TResult[] GetChildrenRecursive<TSelf, TResult, TFilter, TSelector>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
    {
        var totalChildren = item.CountChildren<TSelf, TFilter>();
        var results = GC.AllocateUninitializedArray<TResult>(totalChildren);
        var index = 0;
        GetChildrenRecursiveUnsafe<TSelf, TResult, TFilter, TSelector>(item, results, ref index);
        return results;
    }

    /// <summary>
    ///     Helper method to populate transformed child items recursively, filtered by the given filter.
    /// </summary>
    /// <param name="item">The current node.</param>
    /// <param name="buffer">The span to fill with transformed child items.</param>
    /// <param name="index">The current index in the span.</param>
    public static void GetChildrenRecursiveUnsafe<TSelf, TResult, TFilter, TSelector>(this Box<TSelf> item,
        Span<TResult> buffer, ref int index)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
        => item.Item.GetChildrenRecursiveUnsafe<TSelf, TResult, TFilter, TSelector>(buffer, ref index);

    /// <summary>
    ///     Helper method to populate transformed child items recursively, filtered by the given filter.
    /// </summary>
    /// <param name="item">The current node.</param>
    /// <param name="buffer">The span to fill with transformed child items.</param>
    /// <param name="index">The current index in the span.</param>
    public static void GetChildrenRecursiveUnsafe<TSelf, TResult, TFilter, TSelector>(this TSelf item, Span<TResult> buffer, ref int index)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
        where TSelector : struct, ISelector<TSelf, TResult>
    {
        foreach (var child in item.Children)
        {
            if (TFilter.Match(child.Item))
                buffer.DangerousGetReferenceAt(index++) = TSelector.Select(child.Item);

            GetChildrenRecursiveUnsafe<TSelf, TResult, TFilter, TSelector>(child.Item, buffer, ref index);
        }
    }

    /// <summary>
    ///     Recursively returns all the children of this node.
    /// </summary>
    /// <param name="item">The node whose children to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>An array of all the children of this node.</returns>
    public static Box<TSelf>[] GetChildrenRecursive<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf> => item.Item.GetChildrenRecursive();

    /// <summary>
    ///     Recursively returns all the children of this node.
    /// </summary>
    /// <param name="item">The node whose children to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <returns>An array of all the children of this node.</returns>
    public static Box<TSelf>[] GetChildrenRecursive<TSelf>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        var totalChildren = item.CountChildren();
        var children = GC.AllocateUninitializedArray<Box<TSelf>>(totalChildren);
        var index = 0;
        GetChildrenRecursiveUnsafe(item, children, ref index);
        return children;
    }

    /// <summary>
    ///     Recursively returns all the children of this node (unsafe / no bounds checks).
    /// </summary>
    /// <param name="item">The current node.</param>
    /// <param name="childrenSpan">
    ///     The span representing the array to fill with children.
    ///     Should be at least as long as value returned by <see cref="CountChildren{TSelf}(Box{TSelf})"/>
    /// </param>
    /// <param name="index">The current index in the span.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetChildrenRecursiveUnsafe<TSelf>(this TSelf item, Span<Box<TSelf>> childrenSpan, ref int index)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        // Populate breadth first. Improved cache locality helps here.
        foreach (var child in item.Children)
            childrenSpan.DangerousGetReferenceAt(index++) = child;

        foreach (var child in item.Children)
            GetChildrenRecursiveUnsafe(child.Item, childrenSpan, ref index);
    }

    /// <summary>
    ///     Recursively returns all the children of this node that match the given filter.
    /// </summary>
    /// <param name="item">The node whose children to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">Filter to apply that decides which children should be returned.</typeparam>
    /// <returns>An array of all the children of this node that match the filter.</returns>
    [ExcludeFromCodeCoverage] // Wrapper
    public static Box<TSelf>[] GetChildrenRecursive<TSelf, TFilter>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
        => item.Item.GetChildrenRecursive();

    /// <summary>
    ///     Recursively returns all the children of this node that match the given filter.
    /// </summary>
    /// <param name="item">The node whose children to obtain.</param>
    /// <typeparam name="TSelf">The type of child node.</typeparam>
    /// <typeparam name="TFilter">Filter to apply that decides which children should be returned.</typeparam>
    /// <returns>An array of all the children of this node that match the filter.</returns>
    public static Box<TSelf>[] GetChildrenRecursive<TSelf, TFilter>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
    {
        var totalChildren = item.CountChildren<TSelf, TFilter>();
        var children = GC.AllocateUninitializedArray<Box<TSelf>>(totalChildren);
        var index = 0;
        GetChildrenRecursiveUnsafe<TSelf, TFilter>(item, children, ref index);
        return children;
    }

    /// <summary>
    ///     Recursively returns all the children of this node (unsafe / no bounds checks).
    /// </summary>
    /// <param name="item">The current node.</param>
    /// <param name="childrenSpan">
    ///     The span representing the array to fill with children.
    ///     Should be at least as long as the value returned by <see cref="CountChildren{TSelf,TFilter}(NexusMods.Paths.Trees.Box{TSelf})"/>
    /// </param>
    /// <param name="index">The current index in the span.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetChildrenRecursiveUnsafe<TSelf, TFilter>(this TSelf item, Span<Box<TSelf>> childrenSpan, ref int index)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        where TFilter : struct, IFilter<TSelf>
    {
        foreach (var child in item.Children)
        {
            if (TFilter.Match(child.Item))
                childrenSpan.DangerousGetReferenceAt(index++) = child;

            GetChildrenRecursiveUnsafe<TSelf, TFilter>(child.Item, childrenSpan, ref index);
        }
    }

    /// <summary>
    ///     Counts the number of leaf nodes (nodes with no children) of the current node.
    /// </summary>
    /// <typeparam name="TSelf">The type of the child node.</typeparam>
    /// <param name="item">The boxed node whose leaf nodes are to be counted.</param>
    /// <returns>The count of leaf nodes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountLeaves<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => CountLeaves(item.Item);

    /// <summary>
    ///     Counts the number of leaf nodes (nodes with no children) under the current node.
    /// </summary>
    /// <typeparam name="TSelf">The type of the child node.</typeparam>
    /// <param name="item">The node whose leaf nodes are to be counted.</param>
    /// <returns>The total count of leaf nodes under the current node.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountLeaves<TSelf>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        var leafCount = 0;
        CountLeavesRecursive(item, ref leafCount);
        return leafCount;
    }

    /// <summary>
    ///     Helper method to recursively count leaf nodes.
    /// </summary>
    /// <typeparam name="TSelf">The type of the child node.</typeparam>
    /// <param name="item">The node to start counting from.</param>
    /// <param name="leafCount">Accumulator for the count of leaf nodes.</param>
    private static void CountLeavesRecursive<TSelf>(TSelf item, ref int leafCount)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        foreach (var child in item.Children)
        {
            if (child.Item.IsLeaf())
                leafCount++;
            else
                CountLeavesRecursive(child.Item, ref leafCount);
        }
    }

    /// <summary>
    ///     Recursively finds and returns all leaf nodes under the current node.
    /// </summary>
    /// <typeparam name="TSelf">The type of the child node.</typeparam>
    /// <param name="item">The boxed node whose leaf nodes are to be found.</param>
    /// <returns>An array of all leaf nodes under the current node.</returns>
    public static Box<TSelf>[] GetLeaves<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => GetLeaves(item.Item);

    /// <summary>
    ///     Recursively finds and returns all leaf nodes under the current node.
    /// </summary>
    /// <typeparam name="TSelf">The type of the child node.</typeparam>
    /// <param name="item">The node whose leaf nodes are to be found.</param>
    /// <returns>An array of all leaf nodes under the current node.</returns>
    public static Box<TSelf>[] GetLeaves<TSelf>(this TSelf item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        var totalLeaves = item.CountLeaves();
        var leaves = GC.AllocateUninitializedArray<Box<TSelf>>(totalLeaves);
        var index = 0;
        GetLeavesUnsafe(item, leaves, ref index);
        return leaves;
    }

    /// <summary>
    ///     Helper method to populate leaf nodes recursively without bounds checking.
    /// </summary>
    /// <typeparam name="TSelf">The type of the child node.</typeparam>
    /// <param name="item">The current node to find leaf nodes from.</param>
    /// <param name="leavesSpan">The span to fill with leaf nodes.</param>
    /// <param name="index">Current index in the span, used internally for recursion.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetLeavesUnsafe<TSelf>(TSelf item, Span<Box<TSelf>> leavesSpan, ref int index)
        where TSelf : struct, IHaveObservableChildren<TSelf>
    {
        foreach (var child in item.Children)
        {
            if (child.IsLeaf())
                leavesSpan.DangerousGetReferenceAt(index++) = child;
            else
                GetLeavesUnsafe(child.Item, leavesSpan, ref index);
        }
    }

    /// <summary>
    ///     Retrieves the observable collection containing all the children of the current node.
    /// </summary>
    /// <param name="item">The boxed node whose children are to be retrieved.</param>
    /// <typeparam name="TSelf">The type of the child node stored in this FileTree.</typeparam>
    /// <returns>An observable collection of the direct children of the current node.</returns>
    [ExcludeFromCodeCoverage] // Wrapper
    public static ObservableCollection<Box<TSelf>> Children<TSelf>(this Box<TSelf> item)
        where TSelf : struct, IHaveObservableChildren<TSelf>
        => item.Item.Children;
}
