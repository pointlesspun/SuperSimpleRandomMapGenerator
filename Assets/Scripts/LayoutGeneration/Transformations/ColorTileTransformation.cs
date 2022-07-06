
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
    public Color fallbackColor = Color.gray;

    /// <summary>
    /// Range of colors which can be assigned (randomly)
    /// </summary>
    public Color[] colors;

    /// <summary>
    /// Value used to flag tiles with no neighbours which could imply a bug
    /// </summary>
    public bool markTilesWithNoNeigbors = false;

    /// <summary>
    /// Color assigned to tiles with no neighbours if the 'markTilesWithNoNeigbors' is set to true
    /// </summary>
    public Color noNeighbourColor = Color.red;

    public LayoutContext Apply(LayoutContext context)
    {
        foreach (var rectBehaviour in context.layout)
        {
            var renderer = rectBehaviour.gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {

                if (markTilesWithNoNeigbors && rectBehaviour.node.Neighbours.Count == 0)
                {
                    renderer.material.color = noNeighbourColor;
                }
                else if (colors != null && colors.Length > 0)
                {
                    renderer.material.color = colors[Random.Range(0, colors.Length)];
                }
                else
                {
                    renderer.material.color = fallbackColor;
                }
            }
        }

        return context;
    }
}
