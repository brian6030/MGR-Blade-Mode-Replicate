using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToStaticMesh : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    bool isStaticMesh = true;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && isStaticMesh) 
        {
            CreateStaticMesh();
            isStaticMesh = false;
        }
    }

    public void CreateStaticMesh()
    {
        Mesh staticMesh = new Mesh();
        skinnedMesh.BakeMesh(staticMesh);
        GameObject newGo = new GameObject();
        //newGo.tag = "Target";
        newGo.layer = 6;
        newGo.transform.position = skinnedMesh.transform.position;
        newGo.transform.rotation = skinnedMesh.transform.rotation;
        newGo.AddComponent<MeshFilter>().sharedMesh = staticMesh;
        newGo.AddComponent<MeshRenderer>().sharedMaterials = skinnedMesh.sharedMaterials;
        newGo.AddComponent<Rigidbody>().useGravity = false;
        newGo.AddComponent<BoxCollider>();
        Destroy(this.gameObject);
    }
}
