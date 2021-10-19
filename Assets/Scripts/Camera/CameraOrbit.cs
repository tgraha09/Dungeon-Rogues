using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using v3 = UnityEngine.Vector3;
public class CameraOrbit : MonoBehaviour
{

    protected Transform mainCam;
    protected Transform pivot;

    public GameObject target;

    protected Vector3 localrot;
    protected float cameraDistance = 1.5f;

    public float mouseSensitivity = 4f;
    public float scrollSensitivity = 2f;
    public float orbitDampen = 6f;
    public float ScrollDampen = 6f;
    
    public bool CameraDisabled = true;
    public float smoothness = 1.0f;
    public v3 camOffset;
    // Start is called before the first frame update
    void Start()
    {
        
        this.mainCam = this.transform;
        this.pivot = this.transform.parent;
        //camOffset = this.pivot.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        MouseOrbit();

        

    }

    void MouseOrbit()
    {
        v3 adjusted = target.transform.position;
        adjusted.y = target.transform.position.y + 0.5f;
        this.transform.parent.position = adjusted;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

            scrollAmount *= (this.cameraDistance * 0.3f);

            this.cameraDistance += scrollAmount * -1f;
            //no closer than 1.5 meters
            this.cameraDistance = Mathf.Clamp(this.cameraDistance, 1.5f, 100f);
            
        }
        if (this.mainCam.localPosition.z != this.cameraDistance * -1f)
        {
            this.mainCam.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.mainCam.localPosition.z, this.cameraDistance * -1f, Time.deltaTime * ScrollDampen));
        }
        if (Input.GetMouseButton(2))
        {
            CameraDisabled = !CameraDisabled;

            if (!CameraDisabled)
            {
                //rotate cam off mouse coords
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    localrot.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                    localrot.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                    //clamp
                    localrot.y = Mathf.Clamp(localrot.y, 0f, 90f);
                }

            }

            //camera orientations
            Quaternion qt = Quaternion.Euler(localrot.y, localrot.x, 0);

            this.pivot.rotation = Quaternion.Lerp(this.pivot.rotation, qt, Time.deltaTime * orbitDampen);


        }
    }
}
