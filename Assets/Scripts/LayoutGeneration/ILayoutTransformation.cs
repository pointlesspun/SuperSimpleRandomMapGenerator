
/// <summary>
/// Interface used to define a transformation (modification/change/addition/substraction...) 
/// on a given layout
/// </summary>
public interface ILayoutTransformation
{
    /// <summary>
    /// Apply the transformation to the context and return the result (can be the same as the provided context)
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    LayoutContext Apply(LayoutContext context);
}

