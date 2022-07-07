using System.Collections.Generic;
using UnityEngine;

/**
 * Unity Behavior wrapper around the RectangleDivisionService, generating a game object for
 * each rectangle created by this service.
 */
public class LayoutGenerator : MonoBehaviour
{
    /// <summary>
    /// Prefab which will be created for each rectangle.
    /// </summary>
    public GameObject _prefab;

    /// <summary>
    /// Bounds of the starting rectangle.
    /// </summary>
    public RectInt _rectangle = new RectInt(Vector2Int.zero, Vector2Int.one);

    /// <summary>
    /// Configuration used to build the randomly generated layout. 
    /// </summary>
    public LayoutConfiguration _configuration;

    /// <summary>
    /// List of transformations to be applied after the layout has been constructed.
    /// </summary>
    public ScriptableObject[] _transformations;

    /// <summary>
    /// Generated layout
    /// </summary>
    private List<RectangleNode2DBehaviour> _layout = null;

    /// <summary>
    /// Returns the generated layout
    /// </summary>
    public List<RectangleNode2DBehaviour> Layout => _layout;

    /// <summary>
    /// Convenience event callback for ui elements which generates a layout, then applies
    /// all added transformations.
    /// </summary>
    public void OnGenerateLayout()
    {
        GenerateLayout();
        ApplyTransformations();
    }

    /// <summary>
    /// Generates a layout using the LayoutGenerator._configuration.
    /// </summary>
    /// <returns></returns>
    public List<RectangleNode2DBehaviour> GenerateLayout() => GenerateLayout(_configuration);

    /// <summary>
    /// Generates a layout using the given configuration and stores the result in LayoutGenerator.Layout. 
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public List<RectangleNode2DBehaviour> GenerateLayout(LayoutConfiguration config)
    {
        ClearLayout(_layout);
        _layout = CreateLayoutObjects(RectangleDivisionService.DivideRectangle(_rectangle, config), config);     
        return _layout;
    }

    /// <summary>
    /// Apply all transformations to LayoutGenerator.Layout and returns the result.
    /// </summary>
    public List<RectangleNode2DBehaviour> ApplyTransformations(TransformationStage stage = TransformationStage.Complete)
    {
        if (_transformations != null && _transformations.Length > 0)
        {
            var context = new LayoutContext()
            {
                bounds = _rectangle,
                layout = _layout,
                config = _configuration
            };

            foreach (var transform in _transformations)
            {
                if (transform != null && transform is ILayoutTransformation layoutTransform)
                {
                    if ((layoutTransform.ApplyTransformation & stage) == stage)
                    {
                        layoutTransform.Apply(context);
                    }
                }
                else
                {
                    Debug.LogWarning("ApplyTransformations, encountered a transform which is either null or not derived from ILayoutTransformation.");
                }
            }
        }

        return Layout;
    }

    /// <summary>
    /// Updates the LayoutGenerator.Layout using the given configuration.
    ///The update applies the rectangle division algorithm to each rectangle/tile in the LayoutGenerator.Layout.
    /// </summary>
    /// <param name="config"></param>
    public List<RectangleNode2DBehaviour> UpdateLayout(LayoutConfiguration config)
    {
        _layout = UpdateLayout(config, _layout);
        return _layout;
    }

    /// <summary>
    /// Updates the given using the given configuration and returns the result. The update applies 
    /// the rectangle division algorithm to each rectangle/tile in the provided layout.
    /// </summary>
    /// <param name="config"></param>
    public List<RectangleNode2DBehaviour> UpdateLayout(LayoutConfiguration config, List<RectangleNode2DBehaviour> layout)
    {
        var updatedLayout = new List<RectangleNode2DBehaviour>();

        foreach (var rectBehaviour in layout)
        {
            var rect = rectBehaviour.node.Rectangle;
            
            if (config.CanDivide(rect))
            {                
                var nodeLayout = CreateLayoutObjects(RectangleDivisionService.DivideRectangle(rectBehaviour.node, config), config);
                updatedLayout.AddRange(nodeLayout);

                rectBehaviour.node.DisconnectFromNeightbours();
                GameObject.Destroy(rectBehaviour.gameObject);             
            }
            else
            {
                updatedLayout.Add(rectBehaviour);
            }
        }

        return updatedLayout;
    }

    /// <summary>
    /// clears the LayoutGenerator.Layout.
    /// </summary>
    public void ClearLayout()
    {
        ClearLayout(_layout);
    }

    /// <summary>
    /// clears the given layout, disconnecting each rectangle and destroying any associated gameobject.
    /// </summary>
    public void ClearLayout(List<RectangleNode2DBehaviour> layout)
    {
        if (layout != null && layout.Count > 0)
        {
            foreach (var rectangle in layout)
            {
                rectangle.node.DisconnectFromNeightbours();
                GameObject.Destroy(rectangle.gameObject);
            }

            layout.Clear();
        }
    }
    

    /// <summary>
    /// Create GameObjects & Behaviours for each rectangleNode2D in the given layout
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="config"></param>
    /// <returns>A list of behaviours each associated with gameobject</returns>
    private List<RectangleNode2DBehaviour> CreateLayoutObjects(List<RectangleNode2D<GameObject>> layout, LayoutConfiguration config) 
    { 
        var offset = Vector3.zero;
        var result = new List<RectangleNode2DBehaviour>();

        foreach (var layoutNode in layout)
        {
            var rectangleBehaviour = CreateGameObjectAndRectangleBehaviour(layoutNode, offset, config);
            result.Add(rectangleBehaviour);
        }

        return result;
    }

    /// <summary>
    /// Create a gameobject and RectangleNode2DBehaviour for the given layoutNode.
    /// </summary>
    /// <param name="layoutNode"></param>
    /// <param name="offset"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    private RectangleNode2DBehaviour CreateGameObjectAndRectangleBehaviour(
        RectangleNode2D<GameObject> layoutNode, 
        in Vector3 offset, 
        LayoutConfiguration config)
    {
        var tile = Instantiate(_prefab);
        var rectangleBehaviour = tile.AddComponent<RectangleNode2DBehaviour>();
        var tileRect = layoutNode.Rectangle;

        tile.name = config.tileNamePrefix + tileRect.ToString();

        tile.transform.position = new Vector3(tileRect.center.x, tileRect.center.y, 0) + offset;
        tile.transform.localScale = new Vector3(tileRect.width - config.padding, tileRect.height - config.padding, 1);

        tile.transform.SetParent(transform, false);

        layoutNode.Data = rectangleBehaviour.gameObject;
        rectangleBehaviour.node = layoutNode;

        return rectangleBehaviour;
    }
}
