
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
    public bool useFixedRandomSeed = false;

    /// <summary>
    /// See value used
    /// </summary>
    public int seed = 42;

    /// <summary>
    /// Randomstate before applying the fixed random seed
    /// </summary>
    private Random.State preApplyState;

    /// <summary>
    /// State before restore is called
    /// </summary>
    private Random.State postApplyState;

    /// <summary>
    /// Flag to check if a state is captured or the seed should be used
    /// </summary>
    private bool isPostApplyStateCaptured;

    public FixedRandomSeed()
    {
        isPostApplyStateCaptured = false;
    }

    /// <summary>
    /// Apply the fixed seed (or previous random state)
    /// </summary>
    public void Apply()
    {
        preApplyState = Random.state;

        if (isPostApplyStateCaptured)
        {
            Random.state = postApplyState;
        }
        else
        {
            Random.InitState(seed);
        }
    }

    /// <summary>
    /// Apply the fixed seed only if the flag to do so is set (convenience method)
    /// </summary>
    public void TryApply()
    {
        if (useFixedRandomSeed)
        {
            Apply();
        }
    }

    /// <summary>
    /// Restore the previous random seed
    /// </summary>
    public void Restore()
    {
        postApplyState = Random.state;
        Random.state = preApplyState;
        isPostApplyStateCaptured = true;
    }

    /// <summary>
    /// Try to restore the previous random seed if the flag to do so is set (convenience method)
    /// </summary>
    public void TryRestore()
    {
        if (useFixedRandomSeed)
        {
            Restore();
        }
    }
}
