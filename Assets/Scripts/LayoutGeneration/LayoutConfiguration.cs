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
    public float _padding = 0.1f;

    /// <summary>
    /// Prefix applied to the generated object's name
    /// </summary>
    public string _tileNamePrefix = "Tile ";

    /// <summary>
    /// Indication whether or not we're using a predefined seed to have predictable 
    /// map generation.
    /// </summary>
    public FixedRandomSeed _randomSeed;

}
