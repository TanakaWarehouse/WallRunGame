using System;
using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class PlatformControler : MonoBehaviour
 {
     [Header("Reference")] 
     [SerializeField] private PlayerMovement playerMovement;
     
     [Header("MovePosition")]
     [SerializeField] private GameObject[] movePoint;
     
     [Header("MoveSpeed")]
     [SerializeField] private float speed;
 
     private Rigidbody rb;
     private List<Rigidbody> rbs = new List<Rigidbody>();
     
     private int nowPoint = 0;
     private bool returnPoint = false;
     
     private Vector3 oldPos = Vector3.zero;
     private Vector3 myVelocity = Vector3.zero;
     
     private Vector3 rbVel;
 
     private void Start()
     {
         rb = GetComponent<Rigidbody>();
         if (movePoint != null && movePoint.Length > 0 && rb != null)
         {
             rb.position = movePoint[0].transform.position;
         }
     }
     

     private void FixedUpdate()
     {
         //デバッグ用
         Debug.Log(rbVel);

         if(movePoint != null && movePoint.Length > 1 && rb != null && this.gameObject.tag != "Locking")
         {
             //通常進行
             if (!returnPoint)
             {
                 int nextPoint = nowPoint + 1;
 
                 //目標ポイントとの誤差がわずかになるまで移動
                 if (Vector3.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                 {
                     //現在地から次のポイントへのベクトルを作成し，normalize * speed で速度変換
                     rbVel =  (movePoint[nextPoint].transform.position - transform.position).normalized * speed; 
                     
                     //次のポイントへ移動
                     rb.velocity = rbVel;
                     
                 }
                 //次のポイントを１つ進める
                 else
                 {
                     rb.velocity = rbVel;
                     ++nowPoint;
 
                     //現在地が配列の最後だった場合
                     if (nowPoint + 1 >= movePoint.Length)
                     {
                         returnPoint = true;
                     }
                 }
             }
             
             //折返し進行
             else
             {
                 int nextPoint = nowPoint - 1;
 
                 //目標ポイントとの誤差がわずかになるまで移動
                 if (Vector3.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                 {
                     //現在地から次のポイントへのベクトルを作成し，normalize * speed で速度変換
                     rbVel =  (movePoint[nextPoint].transform.position - transform.position).normalized * speed; 
                     
                     //次のポイントへ移動
                     rb.velocity = rbVel;
                 }
                 //次のポイントを１つ戻す
                 else
                 {
                     rb.velocity = rbVel;
                     --nowPoint;
 
                     //現在地が配列の最初だった場合
                     if (nowPoint <= 0)
                     {
                         returnPoint = false;
                     }
                 }
             }
             
             AddVelocity();
         }
         
         
         

     }

     
     private void OnCollisionEnter(Collision collision)
     {
         rbs.Add(collision.gameObject.GetComponent<Rigidbody>());
     }

     void OnCollisionExit(Collision collision)
     {
         rbs.Remove(collision.gameObject.GetComponent<Rigidbody>());
     }
     

     
     void AddVelocity()
     {
         if (rb.velocity.sqrMagnitude <= 0.01f)
         {
             return;
         }
         
         for (int i = 0; i < rbs.Count; i++)
         {
             if (rbs[i].CompareTag("Player"))
             {
                 //ここが問題点
                 rbs[i].velocity = rbVel;
                 
             }
             
             else
                 rbs[i].velocity = rbVel;
         }
     }
     
     
 }
