
using UnityEngine;

/**
 * Transformation which applies a color to the Renderer component associated
 * with each RectangleNode2DBehaviour in the context's layout. 
 */
[CreateAssetMenu(fileName = "ColorTiles", menuName = "LayoutTransformations/Color tiles", order = 1)]
public class ColorTileTransformation : ScriptableObject, ILayoutTransformation
{
    /// <summary>
    /// Default color assigned if no other color applies
    /// </summary>
    public Color _fallbackColor = Color.gray;

    /// <summary>
    /// Range of colors which can be assigned (randomly)
    /// </summary>
    public Color[] _colors;

    /// <summary>
    /// Value used to flag tiles with no neighbours which could imply a bug
    /// </summary>
    public bool _markTilesWithNoNeigbors = false;

    /// <summary>
    /// Color assigned to tiles with no neighbours if the 'markTilesWithNoNeigbors' is set to true
    /// </summary>
    public Color _noNeighbourColor = Color.red;

    // backing field for ApplyTransformation 
    public TransformationStage _stage = TransformationStage.Iteration;

    /// <summary>
    /// When to apply the transformation.
    /// </summary>
    public TransformationStage ApplyTransformation { get => _stage; set => _stage = value; }

    public LayoutContext Apply(LayoutContext context)
    {
        foreach (var rectBehaviour in context._layout)
        {
            var renderer = rectBehaviour.gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {

                if (_markTilesWithNoNeigbors && rectBehaviour._node.Neighbours.Count == 0)
                {
                    renderer.material.color = _noNeighbourColor;
                }
                else if (_colors != null && _colors.Length > 0)
                {
                    renderer.material.color = _colors[Random.Range(0, _colors.Length)];
                }
                else
                {
                    renderer.material.color = _fallbackColor;
                }
            }
        }

        return context;
    }
}
