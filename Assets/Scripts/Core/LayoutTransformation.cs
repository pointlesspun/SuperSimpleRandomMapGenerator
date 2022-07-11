using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TileLayout", menuName = "LayoutTransformations/TileLayout", order = 1)]
public class LayoutTransformation : ScriptableObject, ITransformation
{
    public static readonly string ContextKey = "Layout";

    public GameObject _prefab;

    public float _padding = 0.1f;

    private GameObject _root;

    private TransformationState _state = TransformationState.None;

    public TransformationState State => _state;

    public void Initialize(Dictionary<string, object> context)
    {
        if (_prefab == null)
        {
            Debug.LogError("Cannot create a layout without a prefab defined.");
            _state = TransformationState.Complete;
        }
        else
        {
            if (_root != null)
            {
                GameObject.Destroy(_root);
            }

            _root = new GameObject();
            _root.name = "Layout Root";
            context[ContextKey] = _root;
            _state = TransformationState.Active;
        }
    }

    public void ShutDown(Dictionary<string, object> context)
    {
    }



    public TransformationState Iterate(Dictionary<string, object> context)
    {
        if (context.TryGetValue(AABSPTransformation.ContextKey, out var aabsp))
        {
            var graph = new AARectGraph<GameObject>(aabsp as AABSP, (node) =>
            {
                var tile = Instantiate(_prefab);
                tile.name = "tile " + node.ToString();
                
                tile.transform.position = new Vector3(node.Rectangle.center.x, node.Rectangle.center.y, 0);
                tile.transform.localScale = new Vector3(node.Rectangle.width - _padding, node.Rectangle.height - _padding, 1);

                tile.transform.SetParent(_root.transform, false);

                return tile;
            });

            context[ContextKey] = graph;

            _state = TransformationState.Complete;
        }

        return _state;
    }
}
