using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[RequireComponent(typeof(LineRenderer))]
public class laza : MonoBehaviour
{
    public int reflections; // How many times the laser can reflect
    public float maxLength = 100f; // Maximum length of the laser ray
    private LineRenderer lineRenderer; // To visualize the laser line
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = maxLength;

        bool hitDoor = false;

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
    }
}
