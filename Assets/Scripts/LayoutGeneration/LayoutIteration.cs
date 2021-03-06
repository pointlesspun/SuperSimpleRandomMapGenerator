using UnityEngine;

/// <summary>
/// Behaviour controlling a LayoutGenerator in such a way the user can see
/// the steps applied to the map generation.
/// </summary>
[RequireComponent(typeof(LayoutGenerator))]
public class LayoutIteration : MonoBehaviour
{
    /// <summary>
    /// Time in between applying a division
    /// </summary>
    public float _iterationDelay = 0.7f;

    /// <summary>
    /// Clone of the configuration of the generator but with the max depth set to
    /// 2 so only a few divisions are applied (ie one iteration).
    /// </summary>
    private LayoutConfiguration _iterationConfig;

    /// <summary>
    /// Generator controlled by this behaviour
    /// </summary>
    private LayoutGenerator _generator;

    /// <summary>
    /// Number of iterations applied, when this exceeds the _generators.config.maxDepth
    /// this object will stop being active.
    /// </summary>
    private int _currentIteration = 0;

    /// <summary>
    /// Flag indicating if this object is iteratively generating a layout
    /// </summary>
    private bool _isActive;

    /// <summary>
    /// Time to keep track of the delays between iteration steps.
    /// </summary>
    private float _startTime = 0;

    public void Start()
    {
        _generator = GetComponent<LayoutGenerator>();

        OnGenerateLayout();
    }

    /// <summary>
    /// Callback from the ui, starts the iterations of generating a map.
    /// </summary>
    public void OnGenerateLayout()
    {
        _generator.TryApplyFixedRandomSeed();

        _iterationConfig = (LayoutConfiguration)_generator._configuration.Clone();
        _generator.ClearLayout();
        _iterationConfig._maxDepth = 0;
        _generator.GenerateLayout(_iterationConfig);
        _currentIteration = 0;
        _iterationConfig._maxDepth = 2;
        _isActive = true;
        _startTime = Time.time;

        _generator.TryRestoreRandomState();
    }

    public void Update()
    {
        if (_isActive)
        {
            // has a the time exceed the delay between iterations ?
            if (Time.time - _startTime > _iterationDelay)
            {
                _generator.TryApplyFixedRandomSeed();

                // update the layout for 2 steps
                _generator.UpdateLayout(_iterationConfig);
                _currentIteration += 2;
                _isActive = _currentIteration < _generator._configuration._maxDepth; 

                // apply the transformations.
                _generator.ApplyTransformations(_isActive ? TransformationStage.Iteration : TransformationStage.Complete);

                _startTime = Time.time;

                _generator.TryRestoreRandomState();
            }
        }
    }
}
