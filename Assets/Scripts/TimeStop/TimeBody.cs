using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public float TimeBeforeAffected;
    private TimeManager timemanager;
    private Rigidbody rb;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    public bool IsStopped;

    [Header("Reference")] 
    [SerializeField] private GravityGun gravityGun; 
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        TimeBeforeAffectedTimer = TimeBeforeAffected;
    }

    // Update is called once per frame
    void Update()
    {
        //時間差で止まるように
        TimeBeforeAffectedTimer -= Time.deltaTime;
        if (TimeBeforeAffectedTimer <= 0f)
        {
            //Timerが0以下になったらとまる
            CanBeAffected = true;
        }
        
        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            //もしオブジェクト動いているのであれば
            if (rb.velocity.magnitude >= 0f)
            {
                recordedVelocity = rb.velocity.normalized;
                recordedMagnitude = rb.velocity.magnitude;

                //rigibodyの動きとめる
                rb.velocity = Vector3.zero;
                
                //力の影響をすべてなくす
                rb.isKinematic = true;

                IsStopped = true;
            }
        }
        
        
        
    }

    public void ContinueTime()
    {
        //isKinematicはtrueのとき運動量による影響を一切受けなくなる
        rb.isKinematic = false;
        IsStopped = false;
        
        //記録しておいた速度を時間戻したときに加える
        rb.velocity = recordedVelocity * recordedMagnitude;
    }
}
