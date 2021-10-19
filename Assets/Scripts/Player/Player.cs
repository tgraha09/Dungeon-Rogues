using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
public class Player : MonoBehaviour
{

    private Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {

        mesh = gameObject.GetComponent<Mesh>();
        //mesh = GetComponent(MeshFilter).mesh;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
