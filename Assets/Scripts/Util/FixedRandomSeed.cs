
using UnityEngine;

/// <summary>
/// Class which sets and restore's Unity's random seed.
/// </summary>
[System.Serializable]
public class FixedRandomSeed
{
    //Settings to be used in the editor
    
    /// <summary>
    /// When set to true indicates this seed should be used
    /// </summary>
    public bool _useFixedRandomSeed = false;

    /// <summary>
    /// See value used
    /// </summary>
    public int _seed = 42;

    /// <summary>
    /// Randomstate before applying the fixed random seed
    /// </summary>
    private Random.State _preApplyState;

    /// <summary>
    /// State before restore is called
    /// </summary>
    private Random.State _postApplyState;

    /// <summary>
    /// Flag to check if a state is captured or the seed should be used
    /// </summary>
    private bool _isPostApplyStateCaptured;

    public FixedRandomSeed()
    {
        _isPostApplyStateCaptured = false;
    }

    /// <summary>
    /// Apply the fixed seed (or previous random state)
    /// </summary>
    public void Apply()
    {
        _preApplyState = Random.state;

        if (_isPostApplyStateCaptured)
        {
            Random.state = _postApplyState;
        }
        else
        {
            Random.InitState(_seed);
        }
    }

    /// <summary>
    /// Apply the fixed seed only if the flag to do so is set (convenience method)
    /// </summary>
    public void TryApply()
    {
        if (_useFixedRandomSeed)
        {
            Apply();
        }
    }

    /// <summary>
    /// Restore the previous random seed
    /// </summary>
    public void Restore()
    {
        _postApplyState = Random.state;
        Random.state = _preApplyState;
        _isPostApplyStateCaptured = true;
    }

    /// <summary>
    /// Try to restore the previous random seed if the flag to do so is set (convenience method)
    /// </summary>
    public void TryRestore()
    {
        if (_useFixedRandomSeed)
        {
            Restore();
        }
    }
}
