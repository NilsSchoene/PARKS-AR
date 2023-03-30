using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObjectInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    private Vector2 touchPosition;

    /// <summary>
    /// Checks on each tap, if the user tapped the AR Interactable. If so, the OnInteraction()-Function is started.
    /// </summary>
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    ARInteractable arObject = hitObject.transform.GetComponent<ARInteractable>();
                    if(arObject != null)
                    {
                        arObject.OnInteraction();
                    }
                }
            }
        }
    }
}
