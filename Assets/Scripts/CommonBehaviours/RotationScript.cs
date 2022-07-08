using UnityEngine;


/// <summary>
/// Rotates the gameobject around a constant angle 
/// </summary>
public class RotationScript : MonoBehaviour
{
    public Vector3 _eulerAnglesPerSecond = Vector3.left;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(_eulerAnglesPerSecond * Time.deltaTime);        
    }
}
