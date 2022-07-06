using System;
using UnityEngine;

/// <summary>
/// Utility to define pre conditions
/// </summary>
public static class Contract
{
    /// <summary>
    /// The given test needs to be true otherwise the application may get in an error state.
    /// When test is false will stop the unity editor.
    /// </summary>
    /// <param name="test"></param>
    /// <param name="message"></param>
    public static void Requires(bool test, string message)
    {
        if (!test)
        {
            Debug.LogError("Failed requirement: " + message);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            throw new InvalidProgramException("Failed requirement: " + message);
#endif                
        }
    }

}

