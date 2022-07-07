using UnityEngine;

/// <summary>
/// Behaviour encapsulating a RectangleNode2D<GameObject>
/// </summary>
public class RectangleNode2DBehaviour : MonoBehaviour
{
    /// <summary>
    /// Node associated with this behaviour
    /// </summary>
    public RectangleNode2D<GameObject> node;

    /// <summary>
    /// Draws lines to all its neighbours.
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        // draw a line to all the neighbours
        foreach (var neighbour in node.Neighbours)
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
        var intersection = node.Rectangle.GetIntersection(neighbour.Rectangle);
        var scale = transform.parent != null ? transform.parent.transform.localScale : Vector3.one;
        var pi1 = new Vector3(intersection[0].x, intersection[0].y, 0);
        var pi2 = new Vector3(intersection[1].x, intersection[1].y, 0);
        var pp = GetProjectionPoint(node.Rectangle.center, intersection);

        pi1 = transform.rotation * Vector3.Scale(pi1, scale);
        pi2 = transform.rotation * Vector3.Scale(pi2, scale);
        pp = transform.rotation * Vector3.Scale(pp, scale);

        Gizmos.DrawLine(transform.position, pp);
        Gizmos.DrawLine(pp, pi1 + ((pi2 - pi1) * 0.5f));
    }

    /// <summary>
    /// Gets a point from the center of the given axis aligned line projected on
    /// the line orthogonal to the line. 
    /// </summary>
    /// <param name="center"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    private Vector3 GetProjectionPoint(in Vector2 center, Vector2Int[] line)
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

