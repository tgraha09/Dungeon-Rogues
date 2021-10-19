using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using v3 = UnityEngine.Vector3;
public class PlayerMovement : MonoBehaviour
{
    public GameObject PlayerPivot;
    
    Animator animator;
    
    int isWalkingHash;
    bool walkingLeft;
    int leftWalkingHash;
    bool walkingRight;
    int rightWalkingHash;
    bool running;
    int isRunningHash;
    bool runLeft;
    int sprintLeftHash;
    bool runRight;
    int sprintRightHash;

    v3 moveRot;
    v3 movePos;

    bool W, A, S, D, Lshift, middleMouse;
    Dictionary<string, bool> transitions;
    v3 def_camOffset;
    GameObject mainCam;
    CameraFollow  caminstance;
  
    public float velocity = 0.0f;
    public float acceleration;
    public float deceleration;
    int VelocityHash;

    float idleTurn = 0.0f;
    float idleBack = 0.0f;
    float idleturnAcc;
    int IdleTurnHash;

    float walkTurn = 0.0f;
    float walkturnspeed = 0.0f;
    float walkturnAcc;
    int WalkTurnHash;
    private bool InMyState;
    private string currentClip;
    private float clipDuration;
    private float dTime;
    private int angle;
    private v3 playerPosition;
    private v3 playerRotation;
    // Start is called before the first frame update
    void Start()
    {

        Init();

        
       /* isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        leftWalkingHash = Animator.StringToHash("isWalkingLeft");
        rightWalkingHash = Animator.StringToHash("isWalkingRight");
        sprintLeftHash = Animator.StringToHash("SprintLeft");
        sprintRightHash = Animator.StringToHash("SprintRight");*/

        VelocityHash = Animator.StringToHash("Velocity");
        IdleTurnHash = Animator.StringToHash("IdleTurn");
        WalkTurnHash = Animator.StringToHash("WalkTurn");
        
        walkingLeft = false;
        running = false;
        runLeft = false;
        runRight = false;

        //Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        W = Input.GetKey(KeyCode.W);
        A = Input.GetKey(KeyCode.A);
        D = Input.GetKey(KeyCode.D);
        S = Input.GetKey(KeyCode.S);
        Lshift = Input.GetKey(KeyCode.LeftShift);
        middleMouse = Input.GetMouseButton(2);

        ForwardMotion();
        WalkBack();

    }

    void LateUpdate()
    {
        IdleTurning();
        WalkTurnMotion();

        //Turning();
        //StartCoroutine("CheckAnimations");

    }

    

    private void IdleTurning()
    {
      //  float axis = Input.GetAxis("Horizontal");
        
        Rigidbody ridbody = GetComponent<Rigidbody>();
        bool right = (D && W == false && S == false);
        bool release_right = ((D == false && W == false) && S == false);
        bool left = (A && W == false);
        bool release_left = ((A == false && W == false) && S == false);
       // bool animationCheck = CheckAnimations();
        
        //right 
        if (right)
        {
           // Debug.Log("Animation Check: " + animationCheck);
            if(idleTurn < 90.0f)
            {
                idleTurn += 1f * Time.deltaTime * 90;
                playerRotation.y = idleTurn / 100;
            }
            
           // Debug.Log(idleTurn);

        }
        else if (release_right && left == false)
        {
           // Debug.Log("RELEASE");
            
            deceleration = 3f;
            if (idleTurn > 0f)
            {
              //  Debug.Log("Animation Check: " + animationCheck);
                idleTurn -= Time.deltaTime * deceleration * 90;
                playerRotation.y = idleTurn / 100;
            }
            else
            {
                playerRotation.y = 0;
                idleTurn = 0;
            }
        }


        if (left)
        {
            //Debug.Log("Animation Check: " + animationCheck);
            if (idleTurn > -90.0f)
            {
                idleTurn -= 1.25f * Time.deltaTime * 90;
                playerRotation.y = idleTurn / 100;
            }

            // Debug.Log(idleTurn);

        }
        else if (release_left && right == false)
        {
            //Debug.Log("RELEASE");

            deceleration = 3f;
            if (idleTurn < 0f)
            {
                //Debug.Log("Animation Check: " + animationCheck);
                idleTurn += Time.deltaTime * deceleration * 90;
                playerRotation.y = idleTurn / 100;
            }
            else
            {
               playerRotation.y = 0;
               idleTurn = 0;
            }
        }

        if (animator.hasRootMotion == false)
        {
            v3 currentRot = gameObject.transform.rotation.eulerAngles + playerRotation;
            gameObject.transform.rotation = Quaternion.Euler(currentRot);
        }
        
        //animator.SetFloat(WalkTurnHash, walkTurn);

        animator.SetFloat(IdleTurnHash, idleTurn);
        
    }

    private void WalkBack()
    {

        bool walking = (S && Lshift == false && W == false);
        bool release_walk = (S == false && Lshift == false && W == false);
        
        bool run = (S && Lshift == true && W == false);
        bool release_run = (S == true && Lshift == false && W == false);
        bool turning = walking;
        float angleRaw = Math.Abs(playerRotation.y);
        //Turning
        if (turning && velocity <= 0 && angleRaw < 1.3f)
        {
            acceleration = 1f;
            velocity -= Time.deltaTime * acceleration;
            playerRotation.y = (velocity * 90) / 100;
            Debug.Log("ANGLE: " + velocity);
            
            

        }
        if (walking && velocity <= 0 && velocity > -1f)
        {
           // acceleration = 2f;
           // velocity -= Time.deltaTime * acceleration;
            //playerRotation.y = (velocity * 90) / 100;
            //Debug.Log(velocity);



        }//running
        if (run && velocity >= -1f && velocity <= 0)
        {
            //acceleration = 3f;
            //velocity -= Time.deltaTime * acceleration;
            //playerRotation.y = (velocity * 90);

        }

        //key up/release
        if (release_walk)
        {
            deceleration = 3f;
            if (velocity < -0.1f)
            {
                velocity += Time.deltaTime * deceleration;
                playerRotation.y = (velocity * 90);
            }
            else
            {
                playerRotation.y = 0;
                velocity = 0;
            }
        }
        if (release_run)
        {
            //deceleration = 3f;
            if (velocity < -0.51f)
            {
               // velocity += Time.deltaTime * deceleration;
               // playerRotation.y = (velocity * 90);
            }
        }
       // Debug.Log("HAS Root MOTION: " + animator.hasRootMotion);
        //drives the player
        if (animator.hasRootMotion == false)
        {
            v3 currentRot = gameObject.transform.rotation.eulerAngles + playerRotation;
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(currentRot) , Time.deltaTime * acceleration);
        }
        //Debug.Log(velocity);
        //updates float for transitions 
        animator.SetFloat(VelocityHash, velocity);
    }
    private void ForwardMotion()
    {
        
        bool walking = (W && Lshift == false && S == false);
        bool release_walk = (W == false && Lshift == false && S == false);
        
        bool run = (W && Lshift == true && S == false);
        bool release_run = (W == true && Lshift == false && S == false);
        //Walking
        if (walking && velocity < 0.51f)
        {
            acceleration = 2f;
            velocity += Time.deltaTime * acceleration;
            playerPosition.z = velocity / 100;

        }//running
        if (run && velocity < 1.01f)
        {
            acceleration = 3f;
            velocity += Time.deltaTime * acceleration;
            playerPosition.z = velocity / 100;

        }

        //key up/release
        if (release_walk)
        {
            deceleration = 3f;
            if (velocity > 0.1f)
            {
                velocity -= Time.deltaTime * deceleration;
                playerPosition.z = velocity / 100;
            }
            else
            {
                playerPosition.z = 0;
                velocity = 0;
            }
        }
        if (release_run)
        {
            deceleration = 3f;
            if (velocity > 0.51f)
            {
                velocity -= Time.deltaTime * deceleration;
                playerPosition.z = velocity / 100;
            }
        }
       // Debug.Log("HAS Root MOTION: " + animator.hasRootMotion);
        //drives the player
        if(animator.hasRootMotion == false)
        {
            v3 currentPos = gameObject.transform.position + playerPosition;
            //PlayerPivot.transform.position = currentPos;
        }
         
        //updates float for transitions 
        animator.SetFloat(VelocityHash, velocity);
    }
    
    void WalkTurnMotion()
    {
        
        bool right = (D && W == true);
        bool release_right = ((D == false && W == true) || (D == false && W == false));
        bool left = (A && W == true);
        bool release_left = ((A == false && W == true) || (D == false && W == false));
       // bool animationCheck = CheckAnimations();

        //right 
        if (right)
        {
           // Debug.Log("Animation Check: " + animationCheck);
            if (walkTurn < 30.0f)
            {
                walkTurn += 1f * Time.deltaTime * 90;
                walkturnspeed += 1f * Time.deltaTime;
                playerRotation.y = walkTurn / 100;
                playerPosition.x = walkturnspeed / 100;
            }

            // Debug.Log(idleTurn);

        }
        else if (release_right && left == false)
        {
            //Debug.Log("RELEASE");

            
            if (walkTurn > 0f)
            {
                Debug.Log("BREAKING");
                walkTurn -= Time.deltaTime * 3.0f * 90;
                walkturnspeed -= 3.0f * Time.deltaTime;
                playerRotation.y = walkTurn / 100;
                playerPosition.x = walkturnspeed / 100;
            }
            else
            {
                playerRotation.y = 0;
                playerPosition.x = 0;
                walkTurn = 0;
            }
        }


        if (left)
        {
            //Debug.Log("Animation Check: " + animationCheck);
            if (walkTurn > -90.0f)
            {
                walkTurn -= 1f * Time.deltaTime * 90;
                playerRotation.y = walkTurn / 100;
            }

            // Debug.Log(idleTurn);

        }
        else if (release_left && right == false)
        {
           // Debug.Log("RELEASE");

           
            if (walkTurn < 0f)
            {
                //Debug.Log("Animation Check: " + animationCheck);
                walkTurn += Time.deltaTime * 3.0f * 90;
                playerRotation.y = walkTurn / 100;
            }
            else
            {
                playerRotation.y = 0;
                walkTurn = 0;
            }
        }


        if (animator.hasRootMotion == false)
        {
            v3 currentRot = gameObject.transform.rotation.eulerAngles + playerRotation;
           // gameObject.transform.rotation = Quaternion.Euler(currentRot);
            v3 currentPos = gameObject.transform.position + playerPosition;
           // PlayerPivot.transform.position = currentPos;
        }
        
        //animator.SetFloat(WalkTurnHash, walkTurn);

        animator.SetFloat(WalkTurnHash, walkTurn);
    }

    bool CheckAnimations()
    {
        var animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        var elapsedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //var bEnd = (elapsedTime == animDuration / 2f);
        var dur = animDuration / 2f;
        var bEnd = (elapsedTime == dur - 1);
        var bFinished = (elapsedTime > dur);
        var clipname = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //Debug.Log(clipname);
        AnimatorClipInfo[] animations = animator.GetCurrentAnimatorClipInfo(0);
        if (bFinished)
        {
            Debug.Log("FINISHED ANIMATION");
            return true;
        }

        // Debug.Log(bFinished);
        return false;


    }
    private void OnTriggerEnter(Collider other)
    {
       //if(other.CompareTag("leftturn")
    }
    private void OnTriggerExit(Collider other)
    {

    }

    void Init()
    {
        animator = GetComponent<Animator>();
        transitions = new Dictionary<string, bool>();
        playerPosition = new v3(0, 0, 0);
        playerRotation = new v3(0, 0, 0);
        moveRot = new v3(0, 0, 0);
        movePos = new v3(0, 0, 0);
    }
}
