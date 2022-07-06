using UnityEngine;

/// <summary>
/// Configuration which adds additional properties
/// </summary>
[System.Serializable]
public class LayoutConfiguration : RectangleDivisionConfiguration<GameObject>
{
    /// <summary>
    /// Padding applied to each generated tile
    /// </summary>
    public float padding = 0.1f;

    /// <summary>
    /// Prefix applied to the generated object's name
    /// </summary>
    public string tileNamePrefix = "Tile ";
}
