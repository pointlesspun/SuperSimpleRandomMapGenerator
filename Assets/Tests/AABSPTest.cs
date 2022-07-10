using NUnit.Framework;
using UnityEngine;

public class AABSPTest
{
    /// <summary>
    /// Test the constructor of a AABSP, see if all the resulting property values are as expected
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        var rect = new RectInt(0, 0, 2, 2);
        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);

        Assert.IsTrue(tree.Root != null);
        Assert.IsTrue(tree.Root.Rectangle.Equals(rect));
        Assert.IsTrue(tree.Root.Depth == 0);
        Assert.IsTrue(tree.Root.SplitAxis == axis);
        Assert.IsTrue(tree.Root.Left == null);
        Assert.IsTrue(tree.Root.Right == null);
        Assert.IsTrue(tree.LeafNodes[0] == tree.Root);
    }

    /// <summary>
    /// Run one iteration see if the root has been split
    /// </summary>
    [Test]
    public void TestSingleIteration()
    {
        var rect = new RectInt(0, 0, 2, 2);
        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);
        
        AABSP.GenerateRandomTree(tree, 1, 1, 1);

        Assert.IsTrue(tree.Root != null);
        Assert.IsTrue(tree.Root.Rectangle.Equals(rect));
        Assert.IsTrue(tree.Root.Left != null);
        Assert.IsTrue(tree.Root.Right != null);
        Assert.IsTrue(tree.LeafNodes.Count == 2);
        Assert.IsTrue(tree.LeafNodes.Contains(tree.Root.Left));
        Assert.IsTrue(tree.LeafNodes.Contains(tree.Root.Right));
    }

    /// <summary>
    /// Run one iteration see if the root has been split
    /// </summary>
    [Test]
    public void TestWithIterationLimit()
    {
        var rect = new RectInt(0, 0, 2048, 2048);
        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);

        // fix the seed so no random issues can occur
        Random.InitState(42);

        // run  iterations so that both children of the root should have
        // been developed
        AABSP.GenerateRandomTree(tree, 1, 1, -1, 3);

        Assert.IsTrue(tree.Root != null);       
        Assert.IsTrue(tree.Root.Left != null);
        Assert.IsTrue(tree.Root.Right != null);
        Assert.IsTrue(tree.Root.Left.Left != null);
        Assert.IsTrue(tree.Root.Left.Right != null);
        Assert.IsTrue(tree.Root.Right.Left != null);
        Assert.IsTrue(tree.Root.Right.Right != null);

        Assert.IsTrue(tree.Root.Left.Left.Left == null);
        Assert.IsTrue(tree.Root.Left.Left.Right == null);
        Assert.IsTrue(tree.Root.Left.Right.Left == null);
        Assert.IsTrue(tree.Root.Left.Right.Right == null);

        Assert.IsTrue(tree.Root.Right.Left.Left == null);
        Assert.IsTrue(tree.Root.Right.Left.Right == null);
        Assert.IsTrue(tree.Root.Right.Right.Left == null);
        Assert.IsTrue(tree.Root.Right.Right.Right == null);

        Assert.IsTrue(tree.LeafNodes.Count == 4);
        Assert.IsTrue(tree.LeafNodes.Contains(tree.Root.Left.Left));
        Assert.IsTrue(tree.LeafNodes.Contains(tree.Root.Left.Right));

        Assert.IsTrue(tree.LeafNodes.Contains(tree.Root.Right.Left));
        Assert.IsTrue(tree.LeafNodes.Contains(tree.Root.Right.Right));
    }

    // test iterative deepening

    // test width constraint

    // test height constraint

}
