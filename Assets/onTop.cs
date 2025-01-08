using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTop : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var character = other.gameObject.GetComponent<moveableObject>();
            character.pushForce = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var character = other.gameObject.GetComponent<moveableObject>();
            character.pushForce = 5;
        }
    }
}