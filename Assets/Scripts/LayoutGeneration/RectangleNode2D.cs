using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Datastructure holding a rectangle with neighbouring rectangles.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RectangleNode2D<T>
{
    /// <summary>
    /// Rectangle covered by this node
    /// </summary>
    public RectInt Rectangle { get; set; } = new RectInt(Vector2Int.zero, Vector2Int.one);

    /// <summary>
    /// Neighbours to this node
    /// </summary>
    public HashSet<RectangleNode2D<T>> Neighbours { get; private set; } = new HashSet<RectangleNode2D<T>>();

    /// <summary>
    /// Data associated with the node (eg GameObject, Color or otherwise)
    /// </summary>
    public T Data { get; set; }

    public RectangleNode2D()
    {
    }

    public RectangleNode2D(in RectInt rectangle, T data)
    {
        Rectangle = rectangle;
        Data = data;
    }

    /// <summary>
    /// Add all nodes which rectangles are touching this rectangle
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public RectangleNode2D<T> AddAdjacentNeighbours(HashSet<RectangleNode2D<T>> nodes)
    {
        foreach( var node in nodes)
        {
            if (!Rectangle.AreDisconnected(node.Rectangle))
            {
                Neighbours.Add(node);
                node.Neighbours.Add(this);
            }
        }
        return this;
    }

    /// <summary>
    /// Remove this node from all its neighbours and remove all this nodes' neighbours
    /// </summary>
    public void DisconnectFromNeightbours()
    {
        foreach (var node in Neighbours)
        {
            node.Neighbours.Remove(this);
        }

        Neighbours.Clear();
    }
}

