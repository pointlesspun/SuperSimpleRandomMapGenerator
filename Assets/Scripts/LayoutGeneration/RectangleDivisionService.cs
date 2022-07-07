using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A service which creates a datastructure out of a given rectangle by randomly and recursively dividing this
/// rectangle into smaller parts.
/// </summary>
public static class RectangleDivisionService
{
    // preallocated buffer which holds two rects
    private static RectInt[] _rectangleBuffer = new RectInt[2];

    /// <summary>
    /// Divides the given rectangle according to the given configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rectangle"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static List<RectangleNode2D<T>> DivideRectangle<T>(in RectInt rectangle, RectangleDivisionConfiguration<T> config)
    {
        // recursively split until a stop condition has been reached
        return Split(new List<RectangleNode2D<T>>(), CreateNode(null, rectangle, config.nodeDefaultValue), 0, config);
    }

    public static List<RectangleNode2D<T>> DivideRectangle<T>(RectangleNode2D<T> rectangle, RectangleDivisionConfiguration<T> config)
    {
        // recursively split until a stop condition has been reached
        return Split(new List<RectangleNode2D<T>>(), rectangle, 0, config);
    }

    /// <summary>
    /// Create a node from the given parent (to hook it up to the parent's neighbours) and the rectangle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parentNode"></param>
    /// <param name="rectangle"></param>
    /// <param name="nodeDefaultValue"></param>
    /// <returns></returns>
    private static RectangleNode2D<T> CreateNode<T>(RectangleNode2D<T> parentNode, in RectInt rectangle, T nodeDefaultValue)
    {
        return parentNode == null
            ? new RectangleNode2D<T>(rectangle, nodeDefaultValue)
            // if there is a parentNode, add all neighbours of the parent adjacent to this node
            : new RectangleNode2D<T>(rectangle, nodeDefaultValue).AddAdjacentNeighbours(parentNode.Neighbours);
    }

    /// <summary>
    /// Split the parent node into two children. The split will either be horizontal or vertical depending
    /// on the depth (see ShouldSplitVertically). The split is constraint by the values of the config. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nodeCollection">Collection which will hold all the leaf nodes (nodes which won't or 
    /// can't split anymore).</param>
    /// <param name="node">Node which needs to be split.</param>
    /// <param name="depth">Depth / number of splits which have been done prior.</param>
    /// <param name="config">Holds the constraints of the split operation.</param>
    /// <returns>The provided nodeCollection.</returns>
    private static List<RectangleNode2D<T>> Split<T>(
        List<RectangleNode2D<T>> nodeCollection,
        RectangleNode2D<T> node,
        int depth,
        RectangleDivisionConfiguration<T> config
    )
    {
        if (depth < config.maxDepth)
        {
            // split horizontal or vertical ?
            if (ShouldSplitVertically(depth))
            {
                // make sure we can still divide this rectangle
                if (config.CanDivideVertically(node.Rectangle))
                {
                    node.Rectangle.RandomlySplitVertically(_rectangleBuffer);
                    return DivideNode(nodeCollection, node, _rectangleBuffer, depth, config);
                }
            }
            else
            {
                // make sure we can still divide this rectangle
                if (config.CanDivideHorizontally(node.Rectangle))
                {
                    node.Rectangle.RandomlySplitHorizontally(_rectangleBuffer);
                    return DivideNode(nodeCollection, node, _rectangleBuffer, depth, config);
                }
            }
        }

        // no more divisions possible which means the current node is a leaf node
        // add it to the result collection
        nodeCollection.Add(node);

        return nodeCollection;
    }

    /// <summary>
    /// Abitrary decision to go horizontal or vertical as long as one follows the other.
    /// </summary>
    /// <param name="currentDepth"></param>
    /// <returns></returns>
    private static bool ShouldSplitVertically(int currentDepth) => currentDepth % 2 == 0;

    /**
     * Divide the parent's rectangle on the vertical axis, a top and bottom child.
     */
    private static List<RectangleNode2D<T>> DivideNode<T>(
        List<RectangleNode2D<T>> nodeCollection,
        RectangleNode2D<T> node,
        RectInt[] rectangles,
        int depth,
        RectangleDivisionConfiguration<T> config
    )
    {
        var childNode1 = CreateNode(node, rectangles[0], config.nodeDefaultValue);
        var childNode2 = CreateNode(node, rectangles[1], config.nodeDefaultValue);

        // add the two children as neighbours
        childNode1.Neighbours.Add(childNode2);
        childNode2.Neighbours.Add(childNode1);

        // the parent node is not a leaf node so we'll discard it, it will 
        // not be part of the final result
        node.DisconnectFromNeightbours();

        // recursively split the children
        Split(nodeCollection, childNode1, depth + 1, config);
        Split(nodeCollection, childNode2, depth + 1, config);

        return nodeCollection;
    }
}
