using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AABSPTransformation", menuName = "LayoutTransformations/AABSP", order = 1)]
public class AABSPTransformation : ScriptableObject, ITransformation
{
    public static readonly string ContextKey = "AABSP";

    public RectInt _rectangle = new RectInt(-16, -16, 32, 32);

    public int _minRectWidth = 1;
    public int _minRectHeight = 1;

    public int _maxDepth = -1;

    public int _maxIterations = -1;

    public bool _allowSplitOnEitherAxis = true;

    public bool _cleanUpOnShutdown = false;

    private TransformationState _state = TransformationState.None;

    public TransformationState State => _state;

    private AABSP _tree;

    public void Initialize(Dictionary<string, object> context)
    {
        _tree = AABSP.GenerateRandomTree(_rectangle, _minRectWidth, _minRectHeight, _maxDepth, _maxIterations, _allowSplitOnEitherAxis);
        context[ContextKey] = _tree;
        _state = TransformationState.Active;
    }

    public void ShutDown(Dictionary<string, object> context)
    {
        if (_cleanUpOnShutdown)
        {
            context.Remove(ContextKey);
        }
    }

    public TransformationState Iterate(Dictionary<string, object> context)
    {
        if (_maxIterations > 0)
        {
            if (_tree.OpenNodes.Count > 0)
            {
                if (_maxDepth < 0 || _tree.OpenNodes.Any( node => node.Depth < _maxDepth))
                {
                    AABSP.GenerateRandomTree(_tree, _minRectWidth, _minRectHeight, _maxDepth, _maxIterations, _allowSplitOnEitherAxis);
                    _state = TransformationState.Active;
                    return _state;
                }
            }
        }

        _state = TransformationState.Complete;
        return _state;
    }
}
