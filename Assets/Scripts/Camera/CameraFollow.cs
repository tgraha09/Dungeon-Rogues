using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using v3 = UnityEngine.Vector3;
public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public CameraFollow instance;
    public float smoothSpeed = 0.5f;
    public float smoothness = 1.0f;
    public v3 offset;
    public v3 camOffset;
    public float rotSpeed = 1.0f;
    public bool canRotate;
 
    v3 smoothPos;
    [SerializeField] private Camera cam;

    private v3 prevPos;
    //np
    public float distance = 4.0f;

    float mouseX;
    float mouseY;

    //public GameObject pivot;


    
    void Start()
    {
        instance = this;
        camOffset = transform.position - target.transform.position;
        transform.position = target.transform.position;
      
    }
    //Ran after update function
    void LateUpdate()
    {
        

        //transform.position = v3.Slerp(transform.position, newPos, smoothness);
        

    }
    // Start is called before the first frame update
   /* void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
    
}
