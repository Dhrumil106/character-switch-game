using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTop : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] PullingObjects pulling;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var character = other.gameObject.GetComponent<moveableObject>();
            character.pushForce = 0;
            PullingObjects swappedOutPuller = other.gameObject.GetComponent<PullingObjects>();
            if (swappedOutPuller != null)
            {
                swappedOutPuller.StopPulling(); // Stop pulling immediately
                swappedOutPuller.enabled = false; // Disable pulling
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var character = other.gameObject.GetComponent<moveableObject>();
            character.pushForce = 5;
            PullingObjects swappedOutPuller = other.gameObject.GetComponent<PullingObjects>();
            if (swappedOutPuller != null)
            { 
                swappedOutPuller.enabled = true; // Disable pulling
            }
        }
    }
}