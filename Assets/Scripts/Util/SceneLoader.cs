using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple behaviour which loads a scene
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Load the scene with the given index. This index is defined
    /// by the scenes added to the build process.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void OnBeginLoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
