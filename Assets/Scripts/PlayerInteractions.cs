using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask lostObjectsLayer;
    public GameEvent onLostObjectFound;
    public float maxRayDist = 15f;

    Camera cam;
    LostObjectController lastHighligted;

    private void Start()
    {
        cam = Camera.main; 
    }

    private void OnInteract()
    {
        if (lastHighligted)
        {
            onLostObjectFound.Raise();
            lastHighligted.ObjectFound();
            lastHighligted = null;
        }
    }

    private void Update()
    {
        LostObjectDetection();
    }

    private void LostObjectDetection ()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit rayHit;

        Debug.DrawRay(ray.origin, ray.direction * maxRayDist);

        if (Physics.Raycast(ray, out rayHit, maxRayDist, lostObjectsLayer))
        {
            var collController = rayHit.collider.GetComponent<LostObjectController>();

            if (lastHighligted == collController)
            {
                return;
            }
            else
            {
                lastHighligted = collController;
                lastHighligted.Highlight();
            }
        }
        else if (lastHighligted)
        {
            lastHighligted.DisableHighlight();
            lastHighligted = null;
        }
    }
}
