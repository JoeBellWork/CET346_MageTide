using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnCanvas : MonoBehaviour
{
    // basic command that rotates object to face towards the camera varible assigned.
    public Transform cam;
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
