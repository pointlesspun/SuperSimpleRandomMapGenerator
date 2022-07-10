using UnityEngine;

/// <summary>
/// Hard coded link opening the project page. Per Unity's recommendation as outlined
/// here: https://docs.unity3d.com/ScriptReference/Application.OpenURL.html (or at least
/// how I understand this recommendation) this link is hard coded to minimize
/// exposure to security issues.
/// </summary>
public class ProjectHyperlink : MonoBehaviour
{
    public void OnClick()
    {
        Application.OpenURL("https://github.com/pointlesspun/SuperSimpleRandomMapGenerator");
    }
}
