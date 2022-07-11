using NUnit.Framework;
using System.Linq;
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
        Assert.IsTrue(tree.OpenNodes[0] == tree.Root);
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
        Assert.IsTrue(tree.OpenNodes.Count == 2);
        Assert.IsTrue(tree.OpenNodes.Contains(tree.Root.Left));
        Assert.IsTrue(tree.OpenNodes.Contains(tree.Root.Right));
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

        Assert.IsTrue(tree.OpenNodes.Count == 4);
        Assert.IsTrue(tree.OpenNodes.Contains(tree.Root.Left.Left));
        Assert.IsTrue(tree.OpenNodes.Contains(tree.Root.Left.Right));

        Assert.IsTrue(tree.OpenNodes.Contains(tree.Root.Right.Left));
        Assert.IsTrue(tree.OpenNodes.Contains(tree.Root.Right.Right));
    }

    /// <summary>
    /// See if we can incrementally generate a randomly split tree
    /// </summary>
    [Test]
    public void TestWithIterativeDeeping()
    {
        var rect = new RectInt(0, 0, 999999, 999999);
        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);

        // fix the seed so no random issues can occur
        Random.InitState(1234);

        // Do 8 full iterations
        for (var i = 0; i < 8; i++)
        {
            AABSP.GenerateRandomTree(tree, 1, 1, i+1, (int) System.Math.Pow(2,i));

            // each node should produce two children
            Assert.IsTrue(tree.OpenNodes.Count == (int)System.Math.Pow(2, i+1));
            Assert.IsTrue(tree.OpenNodes.All(node => node.Parent != null));
            Assert.IsTrue(tree.OpenNodes.All(node => node.Rectangle.x >= 0 && node.Rectangle.y >= 0));
            Assert.IsTrue(tree.OpenNodes.All(node => node.Rectangle.width > 1 && node.Rectangle.height > 1));
            Assert.IsTrue(tree.OpenNodes.All(node => node.Depth == (i + 1)));
            Assert.IsTrue(tree.OpenNodes.All(node => node.Left == null && node.Right == null));
        }
   }

    /// <summary>
    /// Test if the algorithm using either axis if the rect can still be split over that axis.
    /// In this case test with the width set to 1.
    /// </summary>
    [Test]
    public void TestWidthConstraint()
    {
        var rect = new RectInt(0, 0, 1, 999999);

        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);
        bool testWidthHeight (AABSP.Node node)
        {
            return node.Rectangle.width == 1
                && node.Rectangle.height >= 1
                && (node.Left == null || (testWidthHeight(node.Left)))
                && (node.Right == null || (testWidthHeight(node.Right)));
        }

        // fix the seed so no random issues can occur
        Random.InitState(1234);

        // should be able to divide over the x-axis 
        AABSP.GenerateRandomTree(tree, 1, 1, 3, 8);

        Assert.IsTrue(testWidthHeight(tree.Root));
    }

    /// <summary>
    /// Test if the algorithm using either axis if the rect can still be split over that axis
    /// In this case test with the height set to 1.
    /// </summary>
    [Test]
    public void TestHeightConstraint()
    {
        var rect = new RectInt(0, 0, 999999, 1);

        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);
        bool testWidthHeight(AABSP.Node node)
        {
            return node.Rectangle.width >= 1
                && node.Rectangle.height == 1
                && (node.Left == null || (testWidthHeight(node.Left)))
                && (node.Right == null || (testWidthHeight(node.Right)));
        };

        // fix the seed so no random issues can occur
        Random.InitState(1234);

        // should be able to divide over the x-axis 
        AABSP.GenerateRandomTree(tree, 1, 1, 4, 16);

        var depth = tree.MaxDepth();

        Assert.IsTrue(depth == 5);
        Assert.IsTrue(testWidthHeight(tree.Root));
        
    }

    /// <summary>
    /// Test if the algorithm will stop if it runs out of space even though
    /// there is space on the other axis
    /// </summary>
    [Test]
    public void TestWidthConstraintDoNotAllowUsageOfOtherAxis()
    {
        var rect = new RectInt(0, 0, 1, 999999);

        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);

        // fix the seed so no random issues can occur
        Random.InitState(1234);

        AABSP.GenerateRandomTree(tree, 1, 1, -1, -1, false);

        Assert.IsTrue(tree.MaxDepth() <= 2);
    }

    /// <summary>
    /// Test if the algorithm will stop if it runs out of space even though
    /// there is space on the other axis
    /// </summary>
    [Test]
    public void TestHeightConstraintDoNotAllowUsageOfOtherAxis()
    {
        var rect = new RectInt(0, 0, 999999, 1);

        var axis = AABSP.Axis.Horizontal;
        var tree = new AABSP(rect, axis);

        // fix the seed so no random issues can occur
        Random.InitState(1234);

        AABSP.GenerateRandomTree(tree, 1, 1, -1, -1, false);

        Assert.IsTrue(tree.MaxDepth() <= 2);
    }
}
