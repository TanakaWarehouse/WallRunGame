using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed;
    private float desireMoveSpeed;
    private float lastDesireMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float climbSpeed;
    public float swingSpeed;


    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    
    public float groundDrag;
    

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;
    private float jumpCount;

    [Header("Crouching")] 
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;
    
    [Header("Sliding")] 
    public float maxSliderTime;
    public float slideForce;
    private float sliderTimer;
    
    
    [Header("TimeStop")]
    private TimeManager timemanager;


    [Header("Keybinds")] 
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    //public KeyCode slideKey = KeyCode.C;
    //public KeyCode timeStopKey = KeyCode.Q;
    //public KeyCode continueTimeKey = KeyCode.E;
    
    [Header("Ground Check")] 
    public float playerHeight; 
    public LayerMask whatIsGround;
    private RaycastHit groundHit;
    public bool grounded;
    public bool isOnPlatform;
    

    [Header("Slope Handling")] 
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")] 
    public Climb climbScript;
    
    public Transform orientation;

    //horizontal:水平方向の移動量　vertical:垂直方向の移動量
    private float horizontalInput;
    private float verticalInput;

    public Vector3 moveDirection;

    public Rigidbody rb;
    public Rigidbody platformRB;

    [Header("State")]
    public MovementState state;
    
    //enumは列挙型
    public enum MovementState
    {
        walking,
        sprinting,
        wallRunning,
        climbing,
        crouching,
        sliding,
        swinging,
        air
    }

    public bool wallRunning;
    public bool climbing;
    public bool swinging;
    

    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        readyToJump = true;
        jumpCount = 0;

        startYScale = transform.localScale.y;
        
        //時止め関連
        //timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    { 
        
        
        //デバッグ用
        //Debug.Log(rb.velocity);
        //Debug.Log(jumpCount);
        //Debug.Log(readyToJump);
        //Debug.Log(sliderTimer);
        

        //AキーとDキーに対応
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        //WキーとSキーに対応
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //地面があるかのチェック
        grounded = Physics.Raycast(transform.position, Vector3.down, out groundHit, playerHeight * 0.5f + 0.2f , whatIsGround);
        
        MyInput();
        SpeedControl();
        StateHandler();
        
        //抵抗力変化
        //Rigidbodyのdragは抵抗力，つまりオブジェクトの減速に利用可能
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        
    }

    //FixedUpdateは一定時間ごとに呼ばれる関数
    private void FixedUpdate()
    {
        MovePlayer();
        
        //スライディングに関するもの
        if(Input.GetKeyDown(crouchKey) && (horizontalInput != 0 || verticalInput != 0))
            SlidingMovement();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //ジャンプ時
        if (Input.GetKeyDown(jumpKey) && readyToJump  && jumpCount < 1)
        {
            //Debug.Log("Space");
            jumpCount += 1;
            Jump();
            
            if(jumpCount > 1) 
                readyToJump = false;
            
            
            //Invokeは設定した時間に関数を呼び出すことが可能．
            //Invoke(nameof(ResetJump),jumpCooldown);
        }
        
        //しゃがむとき
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        
        //しゃがみ終了時
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
        
        /*
        //時止めスキル
        if (Input.GetKeyDown(timeStopKey))
        {
            timemanager.StopTime();
            //Grayscale.enabled = true;
        }

        if (Input.GetKeyDown(continueTimeKey) && timemanager.TimeIsStopped)
        {
           timemanager.ContinueTime();
            //Grayscale.enabled = true;
        }
        */
    }

    private void StateHandler()
    {
        
        // クライミングモード
        if (climbing)
        {
            state = MovementState.climbing;
            desireMoveSpeed = climbSpeed;
        }
        
        //ウォールランモード
        else if (wallRunning)
        {
            state = MovementState.wallRunning;
            desireMoveSpeed = wallRunSpeed;
            jumpCount = 0;
        }
        
        //スウィングモード
        else if (swinging)
        {
            state = MovementState.swinging;
            desireMoveSpeed = swingSpeed;
            
        }
        
        //スライディングモード
        else if (Input.GetKeyDown(crouchKey))
        {
            sliderTimer = maxSliderTime;
        }
        
        else if (Input.GetKey(crouchKey) && rb.velocity.magnitude > 6)
        {
            state = MovementState.sliding;
            
            if (!grounded)
            {
                sliderTimer = maxSliderTime;
            }
            
            sliderTimer -= Time.deltaTime;

            


            if (OnSlope() && rb.velocity.y < 0.1f)
                desireMoveSpeed = slideSpeed;

            else
                desireMoveSpeed = sprintSpeed;

            if (sliderTimer < 0f)
            {
                state = MovementState.crouching;
                desireMoveSpeed = crouchSpeed;
            }
            
        }
        
        
        //しゃがみモード
        else if (Input.GetKey(crouchKey) && rb.velocity.magnitude <= 6)
        {
            state = MovementState.crouching;
            desireMoveSpeed = crouchSpeed;
        }
        

        //走りモード
        else if (grounded && Input.GetKey(sprintKey) && state != MovementState.crouching && state != MovementState.sliding)
        {
            state = MovementState.sprinting;
            desireMoveSpeed = sprintSpeed;
        }
        //歩きモード
        else if (grounded)
        {
            state = MovementState.walking;
            desireMoveSpeed = walkSpeed;
        }
        //Jumpモード
        else if(!grounded)
        {
            state = MovementState.air;
        }

        if (Math.Abs(desireMoveSpeed - lastDesireMoveSpeed) > 5f && moveSpeed != 0 )
        {
            //コルーチンを一度止める
            StopAllCoroutines();

            StartCoroutine(SmoothlyLerpMoveSpeed());
            
        }
        else
        {
            moveSpeed = desireMoveSpeed;
        }

        lastDesireMoveSpeed = desireMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //斜面から平面に出たときゆっくりと速度を減少
        float time = 0;
        float difference = Mathf.Abs(desireMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desireMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else 
                time += Time.deltaTime * speedIncreaseMultiplier;
            
            yield return null;
        }

        moveSpeed = desireMoveSpeed;
    }

    

    void MovePlayer()
    {
        if (climbScript.exitingWall) return;
        if (swinging) return;
        
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //斜面上にいるとき
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                
        }
        
        //地面にいるとき
        //AddForceで力を加える．ForceMode.Forceはベクトルの方向に力を継続的に加える
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            jumpCount = 0;
        }
            
        
        
        //空中にいるとき
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
            
        
        //turn gravity off while on slope
        if (!wallRunning) rb.useGravity = !OnSlope();
        
        //斜面上では重力なくす(滑り落ちないように)
        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        //斜面上での速度制限
        if (OnSlope() && !exitingSlope)
        {
            //rb.velocityでVector3型の速度を取得,magnitudeでfloatに変換
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        
        //動く床上の時(こことPlatformControlerいじればいけそう)
        /*
        else if(isOnPlatform)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            
            rb.AddForce(platformRB.velocity * Time.fixedDeltaTime, ForceMode.VelocityChange);
            
            
            
            if (flatVel.magnitude > moveSpeed + + platformRB.GetPointVelocity(Vector3.zero).magnitude  && !platformRB.isKinematic)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x , rb.velocity.y, limitedVel.z);
            }

        }
        */
        
        //速度制限(地面・空中時)
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                
                
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
       
    }

    void Jump()
    {
        exitingSlope = true;
        
        //y方向の力をリセット
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        //ForceMode.Impulseは瞬間的に力を与える
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
        jumpCount = 0;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            //RaycastHit.normal:衝突面が向いている方向がVector3で返ってくる．
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    
    
    void StartSlide()
    {
        //プレイヤーサイズを縮める(しゃがむので)
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        sliderTimer = maxSliderTime;
    }
    
    void SlidingMovement()
    {
        //forward(正面)にW,Sキーを対応，right(右を正とした横)にD,Aキーを対応
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //地面でのスライディング
        if (!OnSlope() || rb.velocity.normalized.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            sliderTimer -= Time.deltaTime;
        }

        //斜面でのスライディング
        else
        {
            rb.AddForce(GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }
        
        
        //if(sliderTimer <= 0)
            //StopSlide();
    }
    
    void StopSlide()
    {
        //プレイヤーサイズを元に戻す
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }

}
