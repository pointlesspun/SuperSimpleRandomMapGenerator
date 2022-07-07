
/// <summary>
/// When should this transformation be applied
/// </summary>
[System.Flags]
public enum TransformationStage
{
    /// <summary>
    /// When using an iterative generator, apply a transformation after each iteration
    /// </summary>
    Iteration = 1,

    /// <summary>
    /// Only apply the transformation after the 
    /// </summary>
    Complete = 2
}

/// <summary>
/// Interface used to define a transformation (modification/change/addition/substraction...) 
/// on a given layout
/// </summary>
public interface ILayoutTransformation
{

    /// <summary>
    /// When to apply the transformation.
    /// </summary>
    TransformationStage ApplyTransformation { get; set; }

    /// <summary>
    /// Apply the transformation to the context and return the result (can be the same as the provided context)
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    LayoutContext Apply(LayoutContext context);
}

