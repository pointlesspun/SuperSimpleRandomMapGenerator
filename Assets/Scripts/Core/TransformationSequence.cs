using System.Collections.Generic;
using UnityEngine;

public class TransformationSequence : ScriptableObject, ITransformation
{
    public ITransformation[] _transformations;

    private TransformationState _state = TransformationState.None;

    public TransformationState State => _state;

    public void Initialize(Dictionary<string, object> context)
    {
        if (_transformations != null && _transformations.Length > 0)
        {
            for (var i = 0; i < _transformations.Length; i++)
            {
                _transformations[i].Initialize(context);
            }
        }

        _state = TransformationState.Active;
    }

    public TransformationState Iterate(Dictionary<string, object> context)
    {
        if (_state == TransformationState.Active)
        {
            var activeTransformations = 0;

            if (_transformations != null && _transformations.Length > 0)
            {
                for (var i = 0; i < _transformations.Length; i++)
                {
                    if (_transformations[i].Iterate(context) == TransformationState.Active)
                    {
                        activeTransformations++;
                    }
                }
            }

            _state = activeTransformations > 0 ? TransformationState.Active : TransformationState.Complete;
        }

        return _state;
    }

    public void ShutDown(Dictionary<string, object> context)
    {
        if (_transformations != null && _transformations.Length > 0)
        {
            for (var i = 0; i < _transformations.Length; i++)
            {
                _transformations[i].ShutDown(context);
            }
        }
    }
}
