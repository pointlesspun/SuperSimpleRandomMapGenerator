using UnityEngine;

/// <summary>
/// Extension methods for the Rect and RectInt structs.
/// </summary>
public static class RectangleExtensions
{

    /// <summary>
    /// Split the given source in two, dividing it randomly on the horizontal (x) axis.
    /// </summary>
    /// <param name="source">A rectangle with width > 0.</param>
    /// <param name="result">An non null array in which the result will be stored</param>
    /// <returns>The provided 'result' parameter./returns>
    public static RectInt[] RandomlySplitHorizontally(this in RectInt source, in RectInt[] result)
    {
        Contract.Requires(source.width > 1, "RectangleExtensions.RandomlySplitHorizontally: Cannot split a rectangle with width 1 or less.");
        Contract.Requires(result != null, "RectangleExtensions.RandomlySplitHorizontally: result cannot be null.");
        Contract.Requires(result.Length >= 2, "RectangleExtensions.RandomlySplitHorizontally: result must have length 2 (or greater).");

        var width = Random.Range(1, source.width - 1);

        result[0] = new RectInt(source.x, source.y, width, source.height);
        result[1] = new RectInt(source.x + width, source.y, source.width - width, source.height);

        return result;
    }


    /// <summary>
    /// Split the given source in two, dividing it randomly on the vertical (u) axis.
    /// </summary>
    /// <param name="source">A rectangle with height > 0.</param>
    /// <param name="result">An non null array in which the result will be stored</param>
    /// <returns>The provided 'result' parameter. Result[0] holds the bottom part, result[1] holds the top part. </returns>
    public static RectInt[] RandomlySplitVertically(this in RectInt source, in RectInt[] result)
    {
        Contract.Requires(source.height > 1, "RectangleExtensions.RandomlySplitVertically: Cannot split a rectangle with height 0 or less.");
        Contract.Requires(result != null, "RectangleExtensions.RandomlySplitVertically: result cannot be null.");
        Contract.Requires(result.Length >= 2, "RectangleExtensions.RandomlySplitVertically: result must have length 2 (or greater).");

        var height = Random.Range(1, source.height - 1);

        result[0] = new RectInt(source.x, source.y, source.width, height);
        result[1] = new RectInt(source.x, source.y + height, source.width, source.height - height);

        return result;
    }

    /// <summary>
    /// Tests if the two rects do not overlap or touch
    /// </summary>
    /// <param name="rectA"></param>
    /// <param name="rectB"></param>
    /// <returns></returns>
    public static bool AreDisconnected(this in RectInt rectA, in RectInt rectB)
    {
        var a = (rectA.min.x + rectA.width) - rectB.min.x;
        var b = (rectB.min.x + rectB.width) - rectA.min.x;

        var c = (rectA.min.y + rectA.height) - rectB.min.y;
        var d = (rectB.min.y + rectB.height) - rectA.min.y;

        var e = (a == 0 ? 1 : 0)
            + (b == 0 ? 1 : 0)
            + (c == 0 ? 1 : 0)
            + (d == 0 ? 1 : 0);

        return (a < 0 || b < 0)
               || (c < 0 || d < 0)
               || e >= 2;
    }

    /// <summary>
    /// Gets an intersection line between two rectangles. There are three possible outcomes:
    /// 
    /// * v1.x == v2.x or v1.y == v2.y, rectangles touch on a side 
    /// * v1.x > v2.x or v1.y > v2.y, rectangles are disjunct
    /// * none of the above applies and v1.x != v2.x or v1.y != v2.y, rectangles intersect 
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector2Int[] GetIntersection(this in RectInt a, in RectInt b)
    {
        var x1 = Mathf.Max(a.min.x, b.min.x);
        var x2 = Mathf.Min(a.max.x, b.max.x);

        var y1 = Mathf.Max(a.min.y, b.min.y);
        var y2 = Mathf.Min(a.max.y, b.max.y);

        return new Vector2Int[]
        {
                new Vector2Int(x1, y1),
                new Vector2Int(x2, y2),
        };
    }
}

