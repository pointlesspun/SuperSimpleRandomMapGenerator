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
    }
}
