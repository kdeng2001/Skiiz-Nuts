using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VelocityCheck : MonoBehaviour
{
    TextMeshProUGUI text;
    Rigidbody playerRB;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        text.text = "Speed: " + (int) playerRB.velocity.magnitude + "\n" + "Velocity: " + Vector3Int.FloorToInt(playerRB.velocity);
    }
}
