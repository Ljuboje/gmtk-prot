using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    [SerializeField] Transform targetTransform;
    [SerializeField] float rotationStrength = 1f;
    void Update(){
       CameraMovement();
    }

    void CameraMovement(){
        if(Input.GetMouseButton(1)){
            targetTransform.RotateAround(targetTransform.position, Vector3.up, -Input.GetAxis("Mouse X") * rotationStrength);
            //targetTransform.RotateAround(targetTransform.position, Vector3.up, -Input.GetAxis("Mouse Y") * rotationStrength);
        }
    }
}
