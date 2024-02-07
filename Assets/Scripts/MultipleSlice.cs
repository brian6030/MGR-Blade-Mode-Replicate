using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class MultipleSlice : MonoBehaviour
{
    public Material crossSectionMaterial;

    [SerializeField] KeyCode sliceKey = KeyCode.Mouse0;
    [SerializeField] Vector3 boxSize = new Vector3(5, 0.00005f, 5);
    [SerializeField] Color gizmoColor = Color.green;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(sliceKey)) 
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, boxSize, transform.rotation, ~LayerMask.GetMask("solid"));

            foreach (Collider collider in colliders) 
            {
                Destroy(collider.gameObject);
                SlicedHull hull = collider.gameObject.Slice(transform.position, transform.forward);

                if (hull != null) 
                {
                    GameObject lower = hull.CreateLowerHull(collider.gameObject, crossSectionMaterial);
                    GameObject upper = hull.CreateUpperHull(collider.gameObject, crossSectionMaterial);
                    GameObject[] objs = new GameObject[] { lower, upper };

                    foreach (GameObject o in objs) 
                    {
                        if(o.GetComponent<Rigidbody>() != null)
                            o.AddComponent<Rigidbody>();
                        if(o.GetComponent<MeshCollider>() != null)
                            o.AddComponent<MeshCollider>().convex = true;
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
