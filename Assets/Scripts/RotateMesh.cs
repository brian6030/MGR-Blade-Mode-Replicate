using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RotateMesh;

public class RotateMesh : MonoBehaviour
{
    public float RotationSpeed = 1.0f;

    public RotationAxis rotationAxis = RotationAxis.Y; // Default rotation axis
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public bool InverseDirection = false;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("Mouse X"));
        MeshRotate();
    }

    void MeshRotate() 
    {
        float rotationAmount;

        rotationAmount = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;

        if (InverseDirection) 
        {
            rotationAmount *= -1;
        }

        // Determine the axis of rotation based on the selected enum
        Vector3 axis = Vector3.zero;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                axis = Vector3.right;
                break;
            case RotationAxis.Y:
                axis = Vector3.up;
                break;
            case RotationAxis.Z:
                axis = Vector3.forward;
                break;
        }

        // Rotate the GameObject's transform around the selected axis
        transform.Rotate(axis, rotationAmount);
    }
}
