using UnityEngine;

/// <summary>
/// Behaviour encapsulating a RectangleNode2D<GameObject>
/// </summary>
public class RectangleNode2DBehaviour : MonoBehaviour
{
    /// <summary>
    /// Node associated with this behaviour
    /// </summary>
    public RectangleNode2D<GameObject> _node;

    /// <summary>
    /// Draws lines to all its neighbours.
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        // draw a line to all the neighbours
        foreach (var neighbour in _node.Neighbours)
        {
            if (neighbour != null)
            {
                DrawLineToNeighbourIntersection(neighbour);
            }
        }
    }

    /// <summary>
    /// Draw a line from the center of the rect to the center of the intersection with
    /// the given neighbour.
    /// </summary>
    /// <param name="neighbour"></param>
    private void DrawLineToNeighbourIntersection(RectangleNode2D<GameObject> neighbour)
    {
        var intersection = _node.Rectangle.GetIntersection(neighbour.Rectangle);
        var scale = transform.parent != null ? transform.parent.transform.localScale : Vector3.one;
        var pi1 = new Vector3(intersection[0].x, intersection[0].y, 0);
        var pi2 = new Vector3(intersection[1].x, intersection[1].y, 0);
        var pp = Connector.GetProjectionPoint(_node.Rectangle.center, intersection);

        pi1 = transform.rotation * Vector3.Scale(pi1, scale);
        pi2 = transform.rotation * Vector3.Scale(pi2, scale);
        pp = transform.rotation * Vector3.Scale(pp, scale);

        Gizmos.DrawLine(transform.position, pp);
        Gizmos.DrawLine(pp, pi1 + ((pi2 - pi1) * 0.5f));
    }  
}

