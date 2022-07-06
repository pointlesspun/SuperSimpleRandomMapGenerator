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
        // draw a line to all the neighbours
        foreach (var neighbour in node.Neighbours)
        {
            if (neighbour != null)
            {
                Gizmos.DrawLine(gameObject.transform.position, neighbour.Data.transform.position);
            }
        }
    }
}

