using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour
{
    public Transform PlaneDebug;
    public GameObject Target;
    public Material CrossSectionMaterial;
    public float cutForce = 100;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) 
        {
            Slice(Target);
        }
    }

    public void Slice(GameObject target) 
    {
        SlicedHull hull = target.Slice(PlaneDebug.position, PlaneDebug.up);

        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, CrossSectionMaterial);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, CrossSectionMaterial);
            SetupSlicedComponent(lowerHull);

            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject) 
    {
        if (slicedObject.GetComponent<Rigidbody>() == null) 
            slicedObject.AddComponent<Rigidbody>();

        if (slicedObject.GetComponent<MeshCollider>() == null)
            slicedObject.AddComponent<MeshCollider>();

        Rigidbody rb = slicedObject.GetComponent<Rigidbody>();
        MeshCollider collider = slicedObject.GetComponent<MeshCollider>();

        collider.convex = true;

        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
