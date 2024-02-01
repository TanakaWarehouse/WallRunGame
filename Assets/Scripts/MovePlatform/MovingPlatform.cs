using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private PlayerMovement playerMovement;

    [Header("MovePosition")] 
    [SerializeField] private GameObject ways;
    [SerializeField] private Transform[] wayPoints;
    private int pointIndex;
    private int pointCount;
    private int direction = -1;
    
    private Vector3 targetPos;

    [Header("MoveSpeed")]
    [SerializeField] private float speed;
    [SerializeField] private float waitDuration;

    private Rigidbody platformRB;
    private Collider platformCol;
    private Vector3 moveDirection;

    private void Awake()
    {
        platformRB = GetComponent<Rigidbody>();
        platformCol = GetComponent<BoxCollider>();

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pointIndex = 1;
        pointCount = wayPoints.Length;
        targetPos = wayPoints[1].transform.position;
        
        
        DirectionCalculate();
        
    }
    

    //platformの移動
    private void FixedUpdate()
    {
        //Debug.Log(platformRB.velocity);
        platformRB.velocity = moveDirection * speed;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            NextPoint();
            
        }

        if (platformRB.isKinematic)
        {
            platformCol.isTrigger = false;
        }

        else 
        {
            platformCol.isTrigger = true;
        }
        
    }
    
    
    void NextPoint()
    {
        transform.position = targetPos;
        moveDirection = Vector3.zero;

        //最終ポイント到着時
        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }

        //最初のポイント到着時
        else if (pointIndex == 0)
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;

        StartCoroutine(WaitNextPoint());
    }

    //移動する向きの計算
    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        
        DirectionCalculate();
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = true;
            
            playerMovement.platformRB = platformRB;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = false;
        }
    }
    */
}
