using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class MultipleSlice : MonoBehaviour
{
    public Material crossSectionMaterial;

    [SerializeField] KeyCode sliceKey = KeyCode.Mouse0;
    [SerializeField] Vector3 boxSize = new Vector3(5, 0.1f, 5);
    [SerializeField] LayerMask layerMask;
    [SerializeField] float cutForce = 100f;
    [SerializeField] float cutForceRadius = 30f;

    [SerializeField] RotateCuttingQuad rotateCuttingQuad;

    Animator referencePointAnimator;

    private void Start()
    {
        referencePointAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(sliceKey)) 
        {
            Slice();
            referencePointAnimator.SetTrigger("Slice");
        }
    }

    void Slice() 
    {
        Collider[] hits = Physics.OverlapBox(transform.position, boxSize, transform.rotation, layerMask);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, crossSectionMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossSectionMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossSectionMaterial);

                AddHullComponents(bottom);
                AddHullComponents(top);

                Destroy(hits[i].gameObject);
            }
        }
    }

    void AddHullComponents(GameObject obj) 
    {
        obj.layer = 6;

        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddExplosionForce(cutForce, obj.transform.position, cutForceRadius);

        obj.AddComponent<MeshCollider>().convex = true;
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(transform.position, transform.forward, crossSectionMaterial);
    }

}
