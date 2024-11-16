using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Displaying the player's current speed and velocity onto a TextMeshProUGUI. 
/// </summary>
public class VelocityCheck : MonoBehaviour
{
    TextMeshProUGUI text;
    Rigidbody playerRB;

    /// <summary>
    /// Gets the references to the text component where the speed and velocity are displayed, 
    /// as well as the player's Rigidbody, where the velocity information is obtained.
    /// </summary>
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        playerRB = GameObject.Find("Player").GetComponentInChildren<Rigidbody>();
    }

    /// <summary>
    /// Updates the text with the player's speed and velocity values.
    /// </summary>
    private void Update()
    {
        text.text = "Speed: " + (int) playerRB.velocity.magnitude + "\n" + "Velocity: " + Vector3Int.FloorToInt(playerRB.velocity);
    }
}
