using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using v3 = UnityEngine.Vector3;
public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private bool isRunning;
    private bool S, D = false;
    private bool moving = false;
    [SerializeField]
    GameObject Player;
    void Start()
    {
        animator = Player.GetComponent<Animator>();
        //isRunning = animator.GetBool("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            moving = true;
            //if(transform.rotation.y )
            //transform.Rotate(0, 90, 0);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            moving = true;

            transform.rotation = Quaternion.Euler(0, 270, 0);
            //transform.Rotate(0, 270, 0);
        }
        
        
        animator.SetBool("isRunning", moving);

    }
}
