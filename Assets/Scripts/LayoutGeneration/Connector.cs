using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data containing information for connecting two adjacent rectangles.
/// </summary>
public class Connector
{
    /// <summary>
    /// Rectangle A which has Rectangle B a neighbour
    /// </summary>
    public RectangleNode2DBehaviour _rectA;

    /// <summary>
    /// Rectangle B which has Rectangle A a neighbour
    /// </summary>
    public RectangleNode2DBehaviour _rectB;

    /// <summary>
    /// Line in object space making up the intersection between rectA and rectB .
    /// </summary>
    public Vector2Int[] _intersection;

    /// <summary>
    /// Point in object space between Connector.rectA.center and the Connector.intersection center
    /// </summary>
    public Vector3 projectionPointA;

    /// <summary>
    /// Point in in object space between Connector.rectB.center and the Connector.intersection center
    /// </summary>
    public Vector3 _projectionPointB;

    /// <summary>
    /// Build a list of connectors for each RectangleNode2DBehaviour in the layout
    /// </summary>
    /// <param name="layout"></param>
    /// <returns></returns>
    public static List<Connector> BuildLayoutConnectors(List<RectangleNode2DBehaviour> layout)
    {
        var connections = new List<Connector>();

        // for each RectangleNode2DBehaviour iterate over each other RectangleNode2DBehaviour
        for (var i = 0; i < layout.Count; i++)
        {
            var currentNode = layout[i];

            for (var j = i + 1; j < layout.Count; j++)
            {
                var potentialNeighbour = layout[j];

                // are the nodes neighbours ?
                if (currentNode._node.Neighbours.Contains(potentialNeighbour._node))
                {
                    connections.Add(BuildConnector(currentNode, potentialNeighbour));
                }
            }
        }

        return connections;
    }

    /// <summary>
    /// Build a connector between rectA and rectB
    /// </summary>
    /// <param name="rectA"></param>
    /// <param name="rectB"></param>
    /// <returns></returns>
    public static Connector BuildConnector(RectangleNode2DBehaviour rectA, RectangleNode2DBehaviour rectB)
    {
        var intersection = rectA._node.Rectangle.GetIntersection(rectB._node.Rectangle);

        return new Connector()
        {
            _rectA = rectA,
            _rectB = rectB,
            _intersection = intersection,
            projectionPointA = GetProjectionPoint(rectA._node.Rectangle.center, intersection),
            _projectionPointB = GetProjectionPoint(rectB._node.Rectangle.center, intersection)
        };
    }

    /// <summary>
    /// Gets a point from the center of the given axis aligned line projected on
    /// the line orthogonal to the line. 
    /// </summary>
    /// <param name="center"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static Vector3 GetProjectionPoint(in Vector2 center, Vector2Int[] line)
    {
        if (line[0].x == line[1].x)
        {
            float centerY = line[0].y;
            centerY += (line[1].y - line[0].y) * 0.5f;
            return new Vector3(center.x, centerY, 0);
        }
        else
        {
            float centerX = line[0].x;
            centerX += (line[1].x - line[0].x) * 0.5f;
            return new Vector3(centerX, center.y, 0);
        }
    }
}

