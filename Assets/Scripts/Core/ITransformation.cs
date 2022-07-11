using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransformationState
{
    None,
    Active,
    Complete
}

public interface ITransformation 
{
    TransformationState State { get; }

    void Initialize(Dictionary<string, object> context);

    TransformationState Iterate(Dictionary<string, object> context);

    void ShutDown(Dictionary<string, object> context);
}
