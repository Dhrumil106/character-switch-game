using AstronautPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSwap : MonoBehaviour
{
    public Transform character;
    public List<Transform> possibleCharacters;
    public int whichCharacter;
    public CinemachineFreeLook cam;
    // Start is called before the first frame update
    void Start()
    {
        if (character == null && possibleCharacters.Count >= 1)
        {
            character = possibleCharacters[0];
        }
        Swap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (whichCharacter == 0)
            {
                whichCharacter = possibleCharacters.Count - 1;
            }
            else
            {
                whichCharacter -= 1;
            }
            Swap();
        }

    }
    public void Swap()
    {
        character = possibleCharacters[whichCharacter];
        character.GetComponent<ThirdPerson>().enabled = true;
        character.GetComponent<passiveGravity>().enabled = false;

        AudioListener newAudioListener = character.GetComponent<AudioListener>();
        if (newAudioListener != null)
        {
            newAudioListener.enabled = true;
        }

        PullingObjects newPuller = character.GetComponent<PullingObjects>();
        if (newPuller != null)
        {
            newPuller.enabled = true; // Enable the pulling functionality
        }

        for (int i = 0; i < possibleCharacters.Count; i++)
        {
            if (possibleCharacters[i] != character)
            {
                var charController = possibleCharacters[i].GetComponent<ThirdPerson>();
                var charAnimator = possibleCharacters[i].GetComponentInChildren<Animator>();
                

                if (possibleCharacters[i] != character)
                {
                    
                    if (charAnimator != null)
                    {
                        charAnimator.SetFloat("MoveSpeed", 0); // Set animation to idle
                    }

                    possibleCharacters[i].GetComponent<ThirdPerson>().enabled = false;
                    possibleCharacters[i].GetComponent<passiveGravity>().enabled = true;
                    

                    PullingObjects swappedOutPuller = possibleCharacters[i].GetComponent<PullingObjects>();
                    if (swappedOutPuller != null)
                    {
                        swappedOutPuller.StopPulling(); // Stop pulling immediately
                        swappedOutPuller.enabled = false; // Disable pulling
                    }
                    AudioListener oldAudioListener = possibleCharacters[i].GetComponent<AudioListener>();
                    if (oldAudioListener != null)
                    {
                        oldAudioListener.enabled = false;
                    }
                }
                
            }
            cam.LookAt = character;
            cam.Follow = character;
        }
    }
}

