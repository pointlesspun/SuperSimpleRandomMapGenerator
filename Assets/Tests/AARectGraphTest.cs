using NUnit.Framework;
using UnityEngine;

public class AARectGraphTest
{
    /// <summary>
    /// Test create a AARectGraph from a AABSP with only the root as its node.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        // create a simple bsp with one node
        var bsp = AABSP.GenerateRandomTree(new RectInt(0, 0, 2, 2), 2, 2);

        Assert.IsTrue(bsp.NodeCount() == 1);

        var graph = new AARectGraph<string>(bsp, (aaNode) => aaNode.ToString());

        Assert.IsTrue(graph.Nodes.Count == 1);
        Assert.IsTrue(graph.Nodes[0].AABSPNode.Rectangle.Equals(bsp.Root.Rectangle));
        Assert.IsTrue(graph.Nodes[0].Data == bsp.Root.ToString());
    }

    [Test]
    public void TestWithFourNodes()
    {
        // create a simple bsp with one node
        var bsp = AABSP.GenerateRandomTree(new RectInt(0, 0, 2, 2), 1, 1);

        Assert.IsTrue(bsp.NodeCount() == 7);

        var graph = new AARectGraph<string>(bsp, (aaNode) => aaNode.ToString());

        Assert.IsTrue(graph.Nodes.Count == 4);
        graph.Nodes.ForEach(node =>
        {
            Assert.IsTrue(node.Neighbours.Count == 2);
            Assert.IsTrue(node.AABSPNode.Rectangle.width == 1);
            Assert.IsTrue(node.AABSPNode.Rectangle.height == 1);
        });
    }
}