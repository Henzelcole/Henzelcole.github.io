using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    public Transform lookAt;

    private Vector3 offset = new Vector3(0, 2, -7.0f); // the offset moves the camera away from the player on the Z axis

    private void Start()
    {
        
    }

    private void LateUpdate() //LateUpdate makes sure that this code is called after any other update
    {
        transform.position = lookAt.transform.position + offset; // access the transform of what ever object the script is attacted to
    }
}
