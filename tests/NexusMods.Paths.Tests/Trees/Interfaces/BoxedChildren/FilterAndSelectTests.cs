using NexusMods.Paths.Trees;
using NexusMods.Paths.Trees.Traits;
using NexusMods.Paths.Trees.Traits.Interfaces;

namespace NexusMods.Paths.Tests.Trees.Interfaces.BoxedChildren;

// ReSharper disable once InconsistentNaming
public class FilterAndSelectTests
{
    [Fact]
    public void GetChildItems_ShouldReturnAllValuesRecursively()
    {
        // Arrange
        var grandChild1 = TestTree.Create(null, 3);
        var grandChild2 = TestTree.Create(null, 4);
        var child1 = TestTree.Create(new[] { grandChild1 }, 1);
        var child2 = TestTree.Create(new[] { grandChild2 }, 2);
        var root = TestTree.Create(new[] { child1, child2 });

        // Act
        var values = root.GetChildrenRecursive<TestTree, int, ValuesOverTwoFilter<TestTree>, ValueSelector<TestTree, int>>();

        // Assert
        values.Should().Equal(3, 4);
    }

    [Fact]
    public void GetChildItemsUnsafe_ShouldReturnAllValuesRecursively()
    {
        // Arrange
        var grandChild1 = TestTree.Create(null, 3);
        var grandChild2 = TestTree.Create(null, 4);
        var child1 = TestTree.Create(new[] { grandChild1 }, 1);
        var child2 = TestTree.Create(new[] { grandChild2 }, 2);
        var root = TestTree.Create(new[] { child1, child2 });

        var buffer = new int[2];
        var index = 0;

        // Act
        root.GetChildrenRecursiveUnsafe<TestTree, int, ValuesOverTwoFilter<TestTree>, ValueSelector<TestTree, int>>(buffer, ref index);

        // Assert
        buffer.Should().Equal(3, 4);
    }

    private struct TestTree : IHaveBoxedChildren<TestTree>, IHaveValue<int>
    {
        public Box<TestTree>[] Children { get; private init; }
        public int Value { get; private init; }

        public static Box<TestTree> Create(Box<TestTree>[]? children, int value = default)
        {
            return (Box<TestTree>)new TestTree()
            {
                Children = children ?? Array.Empty<Box<TestTree>>(),
                Value = value
            };
        }
    }

    private struct ValuesOverTwoFilter<TSelf> : IFilter<TSelf> where TSelf : struct, IHaveValue<int>
    {
        public static bool Match(TSelf item) => item.Value > 2;
    }

    private struct ValueSelector<TSelf, TValue> : ISelector<TSelf, TValue> where TSelf : struct, IHaveValue<TValue>
    {
        public static TValue Select(TSelf item) => item.Value;
    }
}
