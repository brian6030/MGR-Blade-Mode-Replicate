using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Splitter_ZH : MonoBehaviour
{
    //切割预制体材质
    public Material _NewMaterial;

    //被切割预制体数组
    public  List <GameObject> _ListGamPreFab;

    
    void Update()
    {
        float _Mx = Input.GetAxis("Mouse X");

        transform.Rotate(0, 0, _Mx);

        //当点击鼠标左键时
        if (Input.GetMouseButtonDown(0))
        {
            //创建忽略切割对象
            Collider[] _Colliders = Physics.OverlapBox(transform.position, new Vector3(4, 0.00005f , 4), transform.rotation, ~LayerMask.GetMask("Solid"));

            foreach (var item in _Colliders)
            {
                Destroy(item);

                //切割
                //GameObject[] _objs GameObject[] _objs = item.gameObject.SliceInstantiate(transform.position, transform.up);

                //切割出现的物体
                SlicedHull _SlicedHull = item.gameObject.Slice(transform.position, transform.up);
                if (_SlicedHull != null)
                {
                    //切割下半部分部分  物体
                    GameObject _Lower = _SlicedHull.CreateLowerHull(item.gameObject, _NewMaterial);

                    //切割上半部分部分  物体
                    GameObject _Upper = _SlicedHull.CreateUpperHull(item.gameObject, _NewMaterial);

                    GameObject[] _objs = new GameObject[] { _Lower, _Upper };

                    for (int i = 0; i < _objs.Length; i++)
                    {
                        _objs[i].AddComponent<Rigidbody>();
                        _objs[i].AddComponent<MeshCollider>().convex = true;
                    }
                }
            }
        }


        //物体生成
        if (Input.GetMouseButtonDown(1))
        {
            GameObject _GamPrefab = _ListGamPreFab[Random.Range(0, _ListGamPreFab.Count - 1)];

            if (_GamPrefab.GetComponent<Rigidbody>())
            {
                GameObject _NewGamPrefab= Instantiate(_GamPrefab);
                _NewGamPrefab.GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
                
            }
            else
            {
                _GamPrefab.AddComponent<Rigidbody>();
                
                GameObject _NewGamPrefab = Instantiate(_GamPrefab);
                _NewGamPrefab.GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
            }
        }

    }
}
