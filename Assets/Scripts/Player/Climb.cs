using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsGround;

    [Header("Climbing")] 
    public float climbSpeed;
    public float climbBackSpeed;
    public float maxClimbTime;
    public float maxClimbBackTime;
    private float climbTimer;

    private bool climbing;

    [Header("ClimbJumping")] 
    public float climbJumpUpForce;
    public float climbJumpBackForce;
    
    public KeyCode jumpKey = KeyCode.Space;
    public int climbJump;
    private int climbJumpLeft;
    
    

    [Header("Detection")] 
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    public float maxBackWallLookAngle;
    private float wallLookAngle;
    private float backWallLookAngle;
    

    private RaycastHit frontWallHit;
    private RaycastHit backWallHit;
    private bool wallFront;
    private bool wallBack;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")] 
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wallCheck();
        StateMachine();
        
        if(climbing && !exitingWall) ClimbingMovement();
    }

    void StateMachine()
    {
        // State 1 - クライミング
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if(!climbing && climbTimer > 0 && !exitingWall)
                StartClimbing();
            
            //timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if(climbTimer <= 0) StopClimbing();
        }
        
        // State 2 - BackWallClimbing
        else if (wallBack && Input.GetKey(KeyCode.S) && wallLookAngle < maxBackWallLookAngle)
        {
            if(!climbing && climbTimer > 0 && !exitingWall)
                StartClimbing();
            
            //timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if(climbTimer <= 0) StopClimbing();
        }
        
        // State 3 - Exiting
        else if (exitingWall)
        {
            if(climbing) StopClimbing();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0) exitingWall = false;
        }

        // State 4 - none
        else
        {
            if(climbing) StopClimbing();
        }
        
        if(wallFront && Input.GetKeyDown(jumpKey) && climbJumpLeft > 0) ClimbJump(); 
    }
    
    private void wallCheck()
    {
        
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit,
            detectionLength, whatIsGround);
        
        wallBack = Physics.SphereCast(transform.position, sphereCastRadius, -orientation.forward, out backWallHit,
            detectionLength, whatIsGround);
            
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
        backWallLookAngle = Vector3.Angle(-orientation.forward, -backWallHit.normal);
        

        bool newWall = frontWallHit.transform != lastWall ||
                       Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || pm.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpLeft = climbJump;
        }
        
        if ((wallBack && newWall) || pm.grounded)
        {
            climbTimer = maxClimbBackTime;
            climbJumpLeft = climbJump;
        }
        
        if (pm.grounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    void StopClimbing()
    {
        climbing = false;
        pm.climbing = false;
    }

    void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        
        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpLeft--;
    }
    
}
