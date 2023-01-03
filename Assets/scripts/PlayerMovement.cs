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
    private bool moving_right = false;
    private bool moving_left = false;
    private bool turnBack = false;
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
        bool isRunning = animator.GetBool("isRunning");
        bool isTurning = animator.GetBool("Turning180");

        if (Input.GetKey(KeyCode.D))
        {
            
            moving_right = true;
            moving_left = !moving_right;

            //if(transform.rotation.y )
            //transform.Rotate(0, 90, 0);
            //transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else //if (!Input.GetKey(KeyCode.D))
        {
            //moving_right = false;
        }

        if (Input.GetKey(KeyCode.A) && AnimationPlaying("isRunning"))
        {
            //moving_right = false;
            animator.SetBool("Turning180", true);
        }

        if (Input.GetKey(KeyCode.A) && !AnimationPlaying("Turning180")) //&& moving_right
        {
            
            moving_left = true;
            moving_right = !moving_left;
            //transform.rotation = Quaternion.Euler(0, 270, 0);

            transform.rotation = Quaternion.Euler(0, 270, 0);
            //transform.Rotate(0, 270, 0);
        }
        else
        {
            //moving_left = false;
        }

       // animator.SetBool("Turning180", turnBack);
        if (moving_right) //moving_left || 
        {
           animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);

        }
        
       

    }

    private bool AnimationPlaying(string name)
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            return true;
        }
        return false;
    }
}
