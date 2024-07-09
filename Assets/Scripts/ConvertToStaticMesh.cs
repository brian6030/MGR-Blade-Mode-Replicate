using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConvertToStaticMesh : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    Collider cuttingQuad;
    bool isMouseDown = false;

    void Start()
    {
        cuttingQuad = GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            isMouseDown = true;
        else
            isMouseDown = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CuttingQuad") && isMouseDown) 
        {
            Debug.Log("Cut");
            CreateStaticMesh();
            other.gameObject.GetComponent<MultipleSlice>().Slice();
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
