using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    [SerializeField] string playerTag = "Player";
    [SerializeField] string pullableTag = "Pullable"; // Add this for the pullable tag
    [SerializeField] Transform platform;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the "Player" or "Pullable" tag
        if (other.gameObject.CompareTag(playerTag) || other.gameObject.CompareTag(pullableTag))
        {
            other.gameObject.transform.parent = platform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object has the "Player" or "Pullable" tag
        if (other.gameObject.CompareTag(playerTag) || other.gameObject.CompareTag(pullableTag))
        {
            other.gameObject.transform.parent = null;
        }
    }
}
