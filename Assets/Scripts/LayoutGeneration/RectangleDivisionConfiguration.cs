using UnityEngine;

/// <summary>
/// Configuration which drives rectangle division (/splitting)
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class RectangleDivisionConfiguration<T> : System.ICloneable
{
    /// <summary>
    /// Minimal number of divisions (depth is mentioned as there is a AABSP tree approach underneath).
    /// </summary>
    public int _minDepth = 1;

    /// <summary>
    /// Maximum number of divisions (depth is mentioned as there is a AABSP tree approach underneath).
    /// </summary>
    public int _maxDepth = 1;

    /// <summary>
    /// Minimal width of a rectangle, below this value the rectangle will no longer be divided.
    /// </summary>
    public int _minRectWidth = 2;

    /// <summary>
    /// Minimal height of a rectangle, below this value the rectangle will no longer be divided.
    /// </summary>
    public int _minRectHeight = 2;

    /// <summary>
    /// Value assigned to the Data property of a node when the node is created.
    /// </summary>
    public T _nodeDefaultValue;

    /// <summary>
    /// Checks if the node can be divided both vertically AND horizontally.
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public bool CanDivide(in RectInt rectangle) => CanDivideVertically(rectangle) && CanDivideHorizontally(rectangle);

    /// <summary>
    /// Checks if the node can be divided vertically.
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public bool CanDivideVertically(in RectInt rectangle) => rectangle.height / 2 >= _minRectHeight;

    /// <summary>
    /// Checks if the node can be divided horizontally.
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public bool CanDivideHorizontally(in RectInt rectangle) => rectangle.width / 2 >= _minRectWidth;

    public virtual object Clone()
    {
        return MemberwiseClone();
    }
}
