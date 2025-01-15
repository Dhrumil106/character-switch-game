using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflector : MonoBehaviour
{
    private Vector3 position;
    private Vector3 direction;
    private LineRenderer lr;
    public bool isOpen;

    private GameObject tempReflector;
    private GameObject doors;

    // A HashSet to track reflectors that have already reflected a laser
    private HashSet<GameObject> hitReflectors;

    // A flag to prevent multiple hits on the same reflector
    private bool isReflecting;

    void Start()
    {
        isOpen = false;
        lr = gameObject.GetComponent<LineRenderer>();
        hitReflectors = new HashSet<GameObject>(); // Initialize the set to track hit reflectors
        isReflecting = false;
    }

    void Update()
    {
        if (isOpen)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, position);

            RaycastHit hit;
            if (Physics.Raycast(position, direction, out hit, Mathf.Infinity))
            {
                // If the laser hits a reflector, check if it has already been hit and is reflecting
                if (hit.collider.CompareTag("Reflector"))
                {
                    tempReflector = hit.collider.gameObject;

                    // If the reflector isn't already reflecting, allow it to reflect
                    if (!hitReflectors.Contains(tempReflector) && !isReflecting)
                    {
                        // Mark this reflector as reflecting to prevent future laser hits
                        hitReflectors.Add(tempReflector);

                        // Reflect the direction of the laser
                        Vector3 tempDirection = Vector3.Reflect(direction, hit.normal);
                        hit.collider.gameObject.GetComponent<LaserReflector>().OpenRay(hit.point, tempDirection);

                        // Set isReflecting to true while processing this reflection
                        isReflecting = true;
                    }
                }
                else
                {
                    // If the laser doesn't hit a reflector, finalize the laser path
                    if (tempReflector)
                    {
                        tempReflector.GetComponent<LaserReflector>().CloseRay();
                        tempReflector = null;
                    }
                    lr.SetPosition(1, hit.point);  // Set the final endpoint of the laser
                }

                lr.SetPosition(1, hit.point);  // Final endpoint of the ray
            }
        }
        else
        {
            if (tempReflector)
            {
                tempReflector.GetComponent<LaserReflector>().CloseRay();
                tempReflector = null;
            }
        }
    }

    public void OpenRay(Vector3 pos, Vector3 dir)
    {
        isOpen = true;
        position = pos;
        direction = dir;
        hitReflectors.Clear();  // Clear the set of hit reflectors when a new ray is opened
        isReflecting = false;   // Reset the reflecting flag when a new ray is opened
    }

    public void CloseRay()
    {
        isOpen = false;
        lr.positionCount = 0;
        hitReflectors.Clear();  // Clear the set when the ray is closed
        isReflecting = false;   // Reset the reflecting flag when the ray is closed
    }
}