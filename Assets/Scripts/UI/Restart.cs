using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Restart contains a method for reloading the current scene.
/// </summary>
public class Restart : MonoBehaviour
{
    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
