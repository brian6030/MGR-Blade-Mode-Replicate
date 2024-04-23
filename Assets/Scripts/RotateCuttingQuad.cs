using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateCuttingQuad : MonoBehaviour
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

    Transform planeReference;
    [SerializeField] Transform planeReferenceCopy;
    [SerializeField] Animator animator;

    void Start()
    {
        planeReference = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimator();
        MeshRotate();
    }

    void SetAnimator()
    {
        SetTransform(planeReferenceCopy, planeReference);

        animator.SetFloat("PlaneX", planeReferenceCopy.localPosition.x);
        animator.SetFloat("PlaneY", planeReferenceCopy.localPosition.y - 1);
    }

    void MeshRotate() 
    {
        float rotationAmount;

        //rotationAmount = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
        rotationAmount = Input.GetAxis("Mouse ScrollWheel") * RotationSpeed * Time.deltaTime;

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

    void SetTransform(Transform transformCurrent, Transform transformTarget, bool isLocal = false) 
    {
        if (isLocal)
        {
            transformCurrent.localPosition = transformTarget.localPosition;
            transformCurrent.localRotation = transformTarget.localRotation;
        }
        else 
        {
            transformCurrent.position = transformTarget.position;
            transformCurrent.rotation = transformTarget.rotation;
        }
    }

}
