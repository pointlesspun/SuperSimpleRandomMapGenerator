using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Axis aligned Binary Space Partitioning (tree). 
/// </summary>
public class AABSP 
{
    /// <summary>
    /// Axis along which a division is made
    /// </summary>
    public enum Axis
    {
        /// <summary>
        /// No axis defined - may indicate a constraint cannot be met (eg rect is too small to divide)
        /// </summary>
        None,

        /// <summary>
        /// Divide over the Horizontal axis such that the rect is divided in a top and bottom part
        /// </summary>
        Horizontal,

        /// <summary>
        /// Divide over the Vertical axis such that the rect is divided in a left and right part
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Node of the AABSP
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Parent of this node, if null this node is the root.
        /// </summary>
        public Node Parent { get; set; }

        public Node Left { get; set; }
        
        public Node Right { get; set; }

        public RectInt Rectangle { get; set; }

        /// <summary>
        /// Over which angle has this node been divided
        /// </summary>
        public Axis SplitAxis { get; set; }

        /// <summary>
        /// Current depth of the node
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Generic application specific data associated with this node
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// Determine an axis to split the node over. If the node is split over the xaxis, it will
        /// try to divide the node over yaxis and vice versa. 
        /// </summary>
        /// <param name="minRectWidth">Minimal width of a node. If the node is below this value either
        /// the node will try to split over the other axis if allowed or Axis.None will be returned.</param>
        /// <param name="minRectHeight">Minimal height of a node. If the node is below this value either
        /// the node will try to split over the other axis if allowed or Axis.None will be returned.</param>
        /// <param name="allowSplitOnEitherAxis">Allow splitting over the other axis if the width or height 
        /// constraint cannot be met.</param>
        /// <returns></returns>
        public Axis DetermineSplitAxis(int minRectWidth, int minRectHeight, bool allowSplitOnEitherAxis )
        {
            
            if (SplitAxis == Axis.Horizontal)
            {
                // prefer to split along the other axis if possible
                if (Rectangle.width/2 >= minRectWidth)
                {
                    return Axis.Vertical;
                }
                // try to see if another horizontal split is possible

                if (allowSplitOnEitherAxis && Rectangle.height/2 >= minRectHeight)
                {
                    return Axis.Horizontal;
                }
            }
            else if (SplitAxis == Axis.Vertical)
            {
                // prefer to split along the other axis if possible
                if (Rectangle.height/2 >= minRectHeight)
                {
                    return Axis.Horizontal;
                }

                // try to see if another horizontal split is possible
                if (allowSplitOnEitherAxis && Rectangle.width/2 >= minRectWidth)
                {
                    return Axis.Vertical;
                }
            }

            return Axis.None;
        }

        public override string ToString()
        {
            return "rect = " + Rectangle + ", depth = " + Depth + ", axis = " + SplitAxis + ", l/r = " + (Left != null) + ", " + (Right != null);
        }
    }

    /// <summary>
    /// Generate a randomly divided AABSP tree 
    /// </summary>
    /// <param name="rect">Starting rect</param>
    /// <param name="minRectWidth">Constaint which requires the width of rectangles to be above this value.</param>
    /// <param name="minRectHeight">Constaint which requires the height of rectangles to be above this value.</param>
    /// <param name="maxDepth">Constraint which limits the depth of the nodes.</param>
    /// <param name="maxIterations">Max number of iterations (splits)</param>
    /// <param name="allowSplitOnEitherAxis">Allow splitting over the other axis if the width or height 
    /// constraint cannot be met.</param>
    /// <returns>A non null randomly split tree</returns>
    public static AABSP GenerateRandomTree(in RectInt rect, int minRectWidth = 1, int minRectHeight = 1, int maxDepth = -1, int maxIterations = -1, bool allowSplitOnEitherAxis = true)
    {
        var result = new AABSP(rect, Random.value > 0.5 ? Axis.Horizontal : Axis.Vertical);
        return GenerateRandomTree(result, minRectWidth, minRectHeight, maxDepth, maxIterations, allowSplitOnEitherAxis);
    }

    /// <summary>
    /// Generate a randomly divided AABSP tree 
    /// </summary>
    /// <param name="tree">Starting tree</param>
    /// <param name="minRectWidth">Constaint which requires the width of rectangles to be above this value.</param>
    /// <param name="minRectHeight">Constaint which requires the height of rectangles to be above this value.</param>
    /// <param name="maxDepth">Constraint which limits the depth of the nodes.</param>
    /// <param name="maxIterations">Max number of iterations (splits)</param>
    /// <param name="allowSplitOnEitherAxis">Allow splitting over the other axis if the width or height 
    /// constraint cannot be met.</param>
    /// <returns>A non null randomly split tree</returns>
    public static AABSP GenerateRandomTree(AABSP tree, int minRectWidth = 1, int minRectHeight = 1, int maxDepth = -1, int maxIterations = -1, bool allowSplitOnEitherAxis = true)
    {
        var iteration = 0;
        var nodeIndex = 0;

        while (tree.OpenNodes.Count > 0 
                && nodeIndex < tree.OpenNodes.Count
                && (maxIterations < 0 || iteration < maxIterations))
        {
            var node = tree.OpenNodes[nodeIndex];

            // can we develop this node? If the max depth has been reached for this node
            // we skip this node
            if (node.Depth < maxDepth || maxDepth < 0)
            {
                var splitAxis = node.DetermineSplitAxis(minRectWidth, minRectHeight, allowSplitOnEitherAxis);

                // if the axis is none the node cannot be divided any further, so do not split
                // it and remove it from the open list
                if (splitAxis != Axis.None)
                {
                    var minSize = splitAxis == Axis.Horizontal ? minRectHeight : minRectWidth;
                    var maxSize = splitAxis == Axis.Horizontal ? node.Rectangle.height - minRectHeight : node.Rectangle.width - minRectWidth;
                    var size = Random.Range(minSize, maxSize);

                    tree.Split(node, splitAxis, size);
                    tree.OpenNodes.Add(node.Left);
                    tree.OpenNodes.Add(node.Right);
                }

                tree.OpenNodes.RemoveAt(0);
                iteration++;
            } 
            else
            {
                nodeIndex++;
            }           
        }

        return tree;
    }

    /// <summary>
    /// Root node of the tree
    /// </summary>
    public Node Root { get; set; }

    /// <summary>
    /// Nnodes with no children which can still be developed.
    /// </summary>
    public List<Node> OpenNodes { get; private set; } = new List<Node>();

       
    public AABSP(in RectInt rectangle, Axis initialAxis = Axis.Horizontal)
    {
        Root = CreateNode(null, rectangle, initialAxis);

        OpenNodes.Add(Root);
    }

    public Node Split(Node node, Axis axis, int size)
    {
        Contract.Requires(node != null, "AABSP.Split: Cannot split a null rect.");
        Contract.Requires(size > 0, "AABSP.Split: Size must be greater than 0.");

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

    /// <summary>
    /// Returns the max depth of the tree by recursively going down its children
    /// </summary>
    /// <returns></returns>
    public int MaxDepth()
    {
        return MaxDepth(1, Root);
    }

    public int NodeCount()
    {
        return NodeCount(Root);
    }

    private int NodeCount(Node node)
    {
        if (node != null)
        {
            return NodeCount(node.Left) + NodeCount(node.Right) + 1;
        }

        return 0;
    }


    private int MaxDepth(int currentDepth, Node node)
    {
        if (node.Left != null && node.Right != null)
        {
            return System.Math.Max(MaxDepth(currentDepth + 1, node.Left), MaxDepth(currentDepth + 1, node.Right));
        }
        else if (node.Left != null)
        {
            return MaxDepth(currentDepth + 1, node.Left);
        }
        else if (node.Right != null)
        {
            return MaxDepth(currentDepth + 1, node.Right);
        }

        return currentDepth;
    }

    private Node SplitHorizontal(Node node, int size)
    {
        Contract.Requires(node != null, "AABSP.SplitHorizontal: Cannot split a null rect.");
        Contract.Requires(size > 0, "AABSP.SplitHorizontal: Size must be greater than 0.");

        var rect = node.Rectangle;
        node.Left = CreateNode(node, new RectInt(rect.x, rect.y, rect.width, size), Axis.Horizontal);
        node.Right = CreateNode(node, new RectInt(rect.x, rect.y + size, rect.width, rect.height - size), Axis.Horizontal);
        return node;
    }

    private Node SplitVertical(Node node, int size)
    {
        Contract.Requires(node != null, "AABSP.SplitVertical: Cannot split a null rect.");
        Contract.Requires(size > 0, "AABSP.SplitVertical: Size must be greater than 0.");

        var rect = node.Rectangle;
        node.Left = CreateNode(node, new RectInt(rect.x, rect.y, size, rect.height), Axis.Vertical);
        node.Right = CreateNode(node, new RectInt(rect.x + size, rect.y, rect.width - size, rect.height), Axis.Vertical);
        return node;
    }

    private Node CreateNode(Node parent, in RectInt rect, Axis splitAxis)
    {
        Contract.Requires(rect.width > 0 && rect.height > 0, "AABSP.CreateNode: Rectangle must have width & height > 0.");

        return new Node()
        {
            Parent = parent,
            Depth = parent == null ? 0 : parent.Depth + 1,
            Rectangle = rect,
            SplitAxis = splitAxis
        };
    }
}
