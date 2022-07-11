using System.Collections.Generic;
using UnityEngine;

public class AARectGraph<T>
{
    public class Node
    {
        public List<Node> Neighbours { get; private set; } = new List<Node>();

        public T Data { get; set; }

        public AABSP.Node AABSPNode { get; set; }

        public Node() { }

        public Node(AABSP.Node node)
        {
            AABSPNode = node;
        }

        public void AddAdjacentNeighbours(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (!AABSPNode.Rectangle.AreDisconnected(node.AABSPNode.Rectangle))
                {
                    Neighbours.Add(node);
                    node.Neighbours.Add(this);
                }
            }
        }

        /// <summary>
        /// Remove this node from all its neighbours and remove all this nodes' neighbours
        /// </summary>
        public void DisconnectFromNeighbours()
        {
            foreach (var node in Neighbours)
            {
                node.Neighbours.Remove(this);
            }

            Neighbours.Clear();
        }
    }

    public List<Node> Nodes { get; private set; } = new List<Node>();

    public AARectGraph(AABSP bsp, System.Func<AABSP.Node, T> factoryMethod)
    {
        var current = new Node(bsp.Root);

        GenerateGraph(current, bsp.Root);

        if (factoryMethod != null)
        {
            foreach (var node in Nodes)
            {
                node.Data = factoryMethod(node.AABSPNode);
            }
        }
    }

    private void GenerateGraph(Node current, AABSP.Node aabspNode)
    {
        if (aabspNode.Left != null)
        {
            var left = new Node(aabspNode.Left);
            var right = new Node(aabspNode.Right);

            left.Neighbours.Add(right);
            right.Neighbours.Add(left);

            left.AddAdjacentNeighbours(current.Neighbours);
            right.AddAdjacentNeighbours(current.Neighbours);

            current.DisconnectFromNeighbours();

            GenerateGraph(left, aabspNode.Left);
            GenerateGraph(right, aabspNode.Right);
        }
        else
        {
            Nodes.Add(current);
        }
    }
}
