using System.Collections.Generic;
using UnityEngine;

public class AABSP 
{
    public enum Axis
    {
        None,
        Horizontal,
        Vertical
    }

    public class Node
    {
        public Node Parent { get; set; }

        public Node Left { get; set; }
        
        public Node Right { get; set; }

        public RectInt Rectangle { get; set; }

        public Axis SplitAxis { get; set; }

        public int Depth { get; set; }

        public Axis DetermineSplitAxis(int maxDepth, int minRectWidth, int minRectHeight)
        {
            if (Depth < maxDepth)
            {
                if (SplitAxis == Axis.Horizontal)
                {
                    // prefer to split along the other axis if possible
                    if (Rectangle.width >= minRectWidth)
                    {
                        return Axis.Vertical;
                    }
                    // try to see if another horizontal split is possible

                    if (Rectangle.height >= minRectHeight)
                    {
                        return Axis.Horizontal;
                    }
                }
                else if (SplitAxis == Axis.Vertical)
                {
                    // prefer to split along the other axis if possible
                    if (Rectangle.height >= minRectHeight)
                    {
                        return Axis.Horizontal;
                    }

                    // try to see if another horizontal split is possible
                    if (Rectangle.width >= minRectWidth)
                    {
                        return Axis.Vertical;
                    }

                }
            }

            return Axis.None;
        }
    }

    public static AABSP GenerateRandomTree(in RectInt rect, int maxDepth, int minRectWidth, int minRectHeight, int maxIterations)
    {
        var result = new AABSP(rect, Random.value > 0.5 ? Axis.Horizontal : Axis.Vertical);
        return GenerateRandomTree(result, maxDepth, minRectWidth, minRectHeight, maxIterations);
    }

    public static AABSP GenerateRandomTree(AABSP tree, int maxDepth, int minRectWidth, int minRectHeight, int maxIterations)
    {
        var iteration = 0;

        while (tree.LeafNodes.Count > 0 && iteration < maxIterations)
        {
            var lastIndex = tree.LeafNodes.Count - 1;
            var node = tree.LeafNodes[lastIndex];
            var splitAxis = node.DetermineSplitAxis(maxDepth, minRectWidth, minRectHeight);

            if (splitAxis != Axis.None)
            {
                var size = Random.Range(0, splitAxis == Axis.Horizontal ? node.Rectangle.width : node.Rectangle.height);
                
                tree.Split(node, splitAxis, size);
                tree.LeafNodes.Add(node.Left);
                tree.LeafNodes.Add(node.Right);
            }

            tree.LeafNodes.Remove(node);
            iteration++;
        }

        return tree;
    }

    

    public Node Root { get; set; }

    public List<Node> LeafNodes { get; private set; } = new List<Node>();


    public AABSP(in RectInt rectangle, Axis initialAxis = Axis.Horizontal)
    {
        Root = CreateNode(null, rectangle, initialAxis);

        LeafNodes.Add(Root);
    }

    public Node Split(Node node, Axis axis, int size)
    {
        if (axis == Axis.Horizontal)
        {
            SplitHorizontal(node, size);
        }
        else if (axis == Axis.Vertical)
        {
            SplitVertical(node, size);
        }

        return node;
    }

    private Node SplitHorizontal(Node node, int size)
    {
        var rect = node.Rectangle;
        node.Left = CreateNode(node, new RectInt(rect.x, rect.y, rect.width, size), Axis.Horizontal);
        node.Right = CreateNode(node, new RectInt(rect.x, rect.y + size, rect.width, rect.height - size), Axis.Horizontal);
        return node;
    }

    private Node SplitVertical(Node node, int size)
    {
        var rect = node.Rectangle;
        node.Left = CreateNode(node, new RectInt(rect.x, rect.y, size, rect.height), Axis.Vertical);
        node.Right = CreateNode(node, new RectInt(rect.x + size, rect.y, rect.width - size, rect.height), Axis.Vertical);
        return node;
    }

    private Node CreateNode(Node parent, in RectInt rect, Axis splitAxis)
    {
        return new Node()
        {
            Parent = parent,
            Depth = parent == null ? 0 : parent.Depth + 1,
            Rectangle = rect,
            SplitAxis = splitAxis
        };
    }
}
