using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[RequireComponent(typeof(LineRenderer))]
public class laza : MonoBehaviour
{
    public int reflections; // How many times the laser can reflect
    public float maxLength = 100f; // Maximum length of the laser ray
    public AudioClip laserSound; // Sound to play when the laser is active (moving/reflected)
    private Transform player; // Reference to the player
    public CharacterSwap CharacterSwap;
    public float maxSoundDistance = 10f; // Public adjustable distance for sound range
    private LineRenderer lineRenderer; // To visualize the laser line
    private Ray ray;
    private RaycastHit hit;
    private AudioSource audioSource; // AudioSource to play the laser sound
    private bool laserActive = false; // Whether the laser is active or not

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the same object
    }

    private void Update()
    {
        player = CharacterSwap.character;
        ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = maxLength;

        bool hitDoor = false;
        laserActive = false; // Reset the laser active state at the start of each update

        // Cast the ray for reflections
        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);

                // Check if the ray hit an object with the door script attached
                door doorScript = hit.collider.GetComponent<door>();
                if (doorScript != null)
                {
                    doorScript.SetLaserHitting(true); // Notify the door script the laser is hitting
                    hitDoor = true;
                }

                // Reflect the ray if it hits a reflective surface
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

                // If the ray hits anything other than a Reflector, stop
                if (hit.collider.tag != "Reflector")
                    break;

                laserActive = true; // Set the laser as active while it's reflecting
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
                break;
            }
        }

        // If the laser is no longer hitting the door, reset the state
        if (!hitDoor)
        {
            // Find all door instances and set their laser hitting state to false
            door[] doors = FindObjectsOfType<door>();
            foreach (door d in doors)
            {
                d.SetLaserHitting(false);
            }
        }

        // Calculate the distance from the player to the laser line
        float distanceToLine = GetDistanceToLaserLine(player.position);

        // If the laser is active, play the laser sound regardless of whether it's hitting a reflector or not
        if (laserActive || distanceToLine <= maxSoundDistance)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(laserSound); // Play the sound when the laser is active
            }

            // Adjust volume based on the distance from the laser line
            float volume = Mathf.Clamp01(1 - distanceToLine / maxSoundDistance); // Closer = louder
            audioSource.volume = volume;
        }
        else
        {
            // Stop the sound if the player is too far from the laser line or the laser is inactive
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // Function to calculate the shortest distance from the player to the laser line
    private float GetDistanceToLaserLine(Vector3 playerPosition)
    {
        float closestDistance = float.MaxValue;

        // Iterate through each segment of the laser line (check for each line segment in the laser path)
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 lineStart = lineRenderer.GetPosition(i);
            Vector3 lineEnd = lineRenderer.GetPosition(i + 1);

            // Calculate the shortest distance from the player to the line segment
            float distance = GetDistanceFromPointToLineSegment(playerPosition, lineStart, lineEnd);
            closestDistance = Mathf.Min(closestDistance, distance);
        }

        return closestDistance;
    }

    // Function to calculate the shortest distance from a point to a line segment
    private float GetDistanceFromPointToLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        // Vector from lineStart to lineEnd
        Vector3 lineVector = lineEnd - lineStart;
        // Vector from lineStart to the point
        Vector3 pointVector = point - lineStart;

        // Project the pointVector onto the lineVector to find the closest point on the line
        float lineLengthSquared = lineVector.sqrMagnitude;
        if (lineLengthSquared == 0f)
        {
            return Vector3.Distance(point, lineStart); // If the line length is zero, return distance to the start
        }

        float projection = Mathf.Clamp(Vector3.Dot(pointVector, lineVector) / lineLengthSquared, 0f, 1f);
        Vector3 closestPoint = lineStart + projection * lineVector;

        // Return the distance between the point and the closest point on the line
        return Vector3.Distance(point, closestPoint);
    }
}