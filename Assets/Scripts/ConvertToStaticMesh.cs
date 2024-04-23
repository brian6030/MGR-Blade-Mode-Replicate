using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToStaticMesh : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    Collider cuttingQuad;

    void Start()
    {
        cuttingQuad = GetComponent<Collider>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CuttingQuad")) 
        {
            CreateStaticMesh();
        }
    }

    public void CreateStaticMesh()
    {
        Mesh staticMesh = new Mesh();
        skinnedMesh.BakeMesh(staticMesh);
        GameObject newGameObj = new GameObject();
        newGameObj.layer = 6;
        newGameObj.transform.position = skinnedMesh.transform.position;
        newGameObj.transform.rotation = skinnedMesh.transform.rotation;
        newGameObj.AddComponent<MeshFilter>().sharedMesh = staticMesh;
        newGameObj.AddComponent<MeshRenderer>().sharedMaterials = skinnedMesh.sharedMaterials;
        newGameObj.AddComponent<Rigidbody>().useGravity = false;
        newGameObj.AddComponent<BoxCollider>();
        Destroy(this.gameObject);
    }
}
