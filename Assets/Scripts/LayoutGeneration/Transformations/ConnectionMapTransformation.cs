
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConnectionMap", menuName = "LayoutTransformations/Connection map", order = 1)]
public class ConnectionMapTransformation : ScriptableObject, ILayoutTransformation
{
    /// <summary>
    /// Layout context store id holding the connectors
    /// </summary>
    public static readonly string MapStoreName = "Connectors";

    /// <summary>
    /// Layout context store id holding the connectors line representations
    /// </summary>
    public static readonly string LinesStoreName = "ConnectorLines";
   
    /// <summary>
    /// 
    /// </summary>
    public bool _createLineObjects = false;

    /// <summary>
    /// Default width of the lines
    /// </summary>
    public float _lineWidth = 0.1f;

    /// <summary>
    /// Material used in the line renderer
    /// </summary>
    public Material _lineMaterial;

    /// <summary>
    /// Offset of the line's vertices 
    /// </summary>
    public Vector3 _offset = Vector3.back * 0.1f;

    // backing field for ApplyTransformation 
    public TransformationStage _stage = TransformationStage.Complete;

    /// <summary>
    /// When to apply the transformation.   
    /// </summary>
    public TransformationStage ApplyTransformation { get => _stage; set => _stage = value; }

    public LayoutContext Apply(LayoutContext context)
    {        
        // remove any existing line objects from the store
        if (context._store.TryGetValue(LinesStoreName, out object lineObjects))
        {
            foreach (var obj in ((List<GameObject>)lineObjects))
            {
                GameObject.Destroy(obj);
            }

            context._store.Remove(LinesStoreName);
        }

        var connections = Connector.BuildLayoutConnectors(context._layout);

        context._store[MapStoreName] = connections;

        // if desired, create lines to visualize the connectors
        if (_createLineObjects) 
        {
            context._store[LinesStoreName] = CreateLineObjects(context, connections);
        }

        return context;
    }

    /// <summary>
    /// For each connector create a gameobject with a line renderer
    /// </summary>
    /// <param name="context"></param>
    /// <param name="connections"></param>
    /// <returns></returns>
    private List<GameObject> CreateLineObjects(LayoutContext context, List<Connector> connections)
    {
        var lineObjects = new List<GameObject>();
        var parentObject = new GameObject("Connectors");

        lineObjects.Add(parentObject);

        if (context._layoutContainer != null)
        {
            parentObject.transform.SetParent(context._layoutContainer.transform, false);
            parentObject.transform.SetSiblingIndex(0);
        }

        foreach (var connector in connections)
        {
            var lineObject = new GameObject();
            var line = lineObject.AddComponent<LineRenderer>();
            var positions = CreatePositions(connector);

            line.transform.SetParent(parentObject.transform, false);
            line.useWorldSpace = false;

            line.material = _lineMaterial;
            line.startWidth = _lineWidth;
            line.endWidth = _lineWidth;

            line.positionCount = positions.Length;
            line.SetPositions(positions);

            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            
            line.name = "line " + connector._rectA.gameObject.name + " to " + connector._rectB.gameObject.name;
        }

        return lineObjects;
    }

    /// <summary>
    /// Create the vectors that make up the lines of the connection
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    private Vector3[] CreatePositions(Connector connection)
    {
        var result = new Vector3[4];

        result[0] = connection._rectA._node.Rectangle.center;
        result[1] = connection.projectionPointA;
        result[2] = connection._projectionPointB;
        result[3] = connection._rectB._node.Rectangle.center;

        result[0] += _offset;
        result[1] += _offset;
        result[2] += _offset;
        result[3] += _offset;

        return result;
    }
}

