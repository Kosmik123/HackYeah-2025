using System;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get WinEnd Component
        // if found, end game
        var winEnd = other.GetComponent<WinEnd>();
        if (winEnd != null)
        {
            winEnd.StartExit();
        }
    }
}
