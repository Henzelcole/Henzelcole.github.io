using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float inputDirection; //create a float in reference to the input i created and renamed Horizontal (X value of our moveVector)
    private float verticalVelocity; //Y value of our moveVector

    private float speed = 10.0f;
    private float gravity = 50.0f;
    private float jumpForce = 20.0f;
    private bool secondJumpAvail = false;

    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;

    Animator anim;
    int jumpHash = Animator.StringToHash("Jump");


    void Start()
    {
        anim = GetComponent<Animator>(); // access the animator controller
        controller = GetComponent<CharacterController>();
       
    }


    void Update() {
       

        moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal") * speed; //calling for the "inputDirection" i decleared above
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", move);

        if (IsControllerGrounded()) //if your on the ground
        {
            verticalVelocity = 0;
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (Input.GetKeyDown(KeyCode.Space)) // if you press the space bar
            {
                anim.SetTrigger(jumpHash); //jump animation
                verticalVelocity = jumpForce; //Make player jump
                secondJumpAvail = true;
            }

            moveVector.x = inputDirection;// only if player is grounded move left or right
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space)) // if you press the space bar
            {
                if (secondJumpAvail)
                {
                    verticalVelocity = jumpForce; //Double jump
                    secondJumpAvail = false;
                }
            }
            verticalVelocity -= gravity * Time.deltaTime;
            moveVector.x = lastMotion.x;
        }


        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
        lastMotion = moveVector;
    }

    private bool IsControllerGrounded() //created my own grounded function using raycast istead of physics to jump more smoothly
    {
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = controller.bounds.center;
        rightRayStart = controller.bounds.center;

        leftRayStart.x -= controller.bounds.extents.x;
        rightRayStart.x += controller.bounds.extents.x;

        Debug.DrawRay(leftRayStart, Vector3.down,Color.red);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.green);

        if (Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f))
            return true;

        if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f))
            return true;

        return false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (controller.collisionFlags == CollisionFlags.Sides)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 2.0f);
                moveVector = hit.normal * speed;
                verticalVelocity = jumpForce;
                secondJumpAvail = true;// wall jump
            }
        }

        //Collectables
        switch(hit.gameObject.tag)
        {
            case"Coin":
                Destroy(hit.gameObject);
                break;
                case "JumpPad":
                verticalVelocity = jumpForce * 2; //Activates JumpPad note I ignored raycast in the inspectors layer
                break;
            case "Teleport":
                transform.position = hit.transform.GetChild(0).position; // takes the players position and move it to the WayPoint aka teleporters Child object
                break;
            case "Winbox":
                LevelManager.Instance.Win();
                break;
            default:
                break;
        }
    }

}
