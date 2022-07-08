
using UnityEngine;

/// <summary>
/// Randomly cull (remove) tiles from the context's layouts randomly according to some criteria
/// </summary>
[CreateAssetMenu(fileName = "CullLayout", menuName = "LayoutTransformations/Cull tiles", order = 1)]
public class CullTileTransformation : ScriptableObject, ILayoutTransformation
{
    /// <summary>
    /// Maximum number of tiles which are allowed to be culled, set to -1 to have no limit.
    /// </summary>
    public int _maxCulledRectangles = -1;

    /// <summary>
    /// Chance value between 0 and 1, where 0 = no culling 1 = cull everything.
    /// </summary>
    public float _cullChance = 0.1f;

    // backing field for ApplyTransformation 
    public TransformationStage _stage = TransformationStage.Complete;

    /// <summary>
    /// When to apply the transformation.
    /// </summary>
    public TransformationStage ApplyTransformation { get => _stage; set => _stage = value; }


    /// <summary>
    ///  Randomly cull tiles from the given context' layout according to the parameters defined in this transformation.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public LayoutContext Apply(LayoutContext context)
    {
        var cullCount = _maxCulledRectangles;
        var iterations = context._layout.Count;

        for (var i = 0; i < iterations; i++)
        {
            if (Random.value < _cullChance)
            {
                var index = Random.Range(0, context._layout.Count);
                var rectBehaviour = context._layout[index];

                rectBehaviour._node.DisconnectFromNeightbours();
                GameObject.Destroy(rectBehaviour.gameObject);

                context._layout.RemoveAt(index);

                cullCount--;

                if (cullCount == 0)
                {
                    break;
                }
            }
        }

        return context;
    }
}

