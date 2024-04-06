using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class LockObject : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private Camera cam;
    [SerializeField] private GravityGun gravityGun;

    [Header("Material")] 
    private Material defaultMat;
    //[SerializeField] private Material highlightMat;
    //[SerializeField] private Material lockingMat;
    


    [Header("Lock")]
    [SerializeField] public float maxLockDistance;
    private Rigidbody lockRB;
    public GameObject lockObj;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;
    private string recordedTag;


    [Header(("UnLock"))]
    private GameObject releaseObj;
    private Vector3 releaseVelocity;
    private float releaseMagnitude;
    private string releaseTag;
    private List<GameObject> releaseObjStorage = new List<GameObject>();

    [Header("Keybinds")] 
    public KeyCode lockObjKey = KeyCode.Q;
    public KeyCode unlockObjKey = KeyCode.E;

    [Header("Queue")]
    Queue<GameObject> lockObjs = new Queue<GameObject>();
    Queue<Vector3> recordVelocity = new Queue<Vector3>();
    Queue<float> recordMagnitude = new Queue<float>();
    private Queue<string> recordTags = new Queue<string>();


    private void Update()
    {
        //デバッグ用
        //Debug.Log(lockObjs.Count);
        //Debug.Log(releaseObjStorage.Count);
        
        if (Input.GetKeyDown(lockObjKey))
        {
            Lock();
        }

        if (Input.GetKeyDown(unlockObjKey) && lockObjs.Count != 0)
        {
            for (int i = 0; 0 < lockObjs.Count; i++)
            {
                UnLock();
            }
        }
    }

    void Lock()
    {
        RaycastHit hit;
            
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            int LayerMask = 1 << 12;
            LayerMask = ~LayerMask;

            if (Physics.Raycast(ray,  out hit, maxLockDistance, LayerMask))
            {

                lockRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                
                //hitしたオブジェクトを取得
                lockObj = hit.collider.gameObject;

                

                if (lockRB && lockRB.velocity.magnitude >= 0f && !lockObj.CompareTag("Locking"))
                {
                    recordedVelocity = lockRB.velocity.normalized;
                    recordedMagnitude = lockRB.velocity.magnitude;
                    recordedTag = lockObj.tag;
                    
                    StorageObjSpeed(lockObj, recordedVelocity.normalized, recordedMagnitude, recordedTag);

                    //速度を止める
                    lockRB.velocity = Vector3.zero;
                    
                    //力の影響をすべてなくす
                    lockRB.isKinematic = true;
                    
                    //tag変更
                    lockObj.tag = "Locking";

                }
            }
            
        
    }

    void UnLock()
    {
        for (int i = 0;  0 < lockObjs.Count; i++)
        {
            //Queueから出たオブジェクト
            releaseObj = lockObjs.Dequeue();
            releaseObjStorage.Add(releaseObj);
            
            releaseVelocity = recordVelocity.Dequeue();
            releaseMagnitude = recordMagnitude.Dequeue();
            releaseTag = recordTags.Dequeue();
            
            
            
            //Queueから出たオブジェクトのRigidbody
            lockRB = releaseObj.GetComponent<Rigidbody>();
            
            //Queueから出たオブジェクトにtag付ける
            releaseObj.tag = releaseTag;

            if (releaseObj.layer != LayerMask.NameToLayer("grabbing"))
            {

                if (!lockObjs.Contains(releaseObj) && !releaseObj.CompareTag("Grabbing"))
                {
                    lockRB.velocity = releaseVelocity * releaseMagnitude;

                    //if(releaseTag != "Lockable") 　これをつけると元々KinematicだったものはKinematicになる 
                    lockRB.isKinematic = false;
                    
                    
                    //デバッグ用
                    //Debug.Log(i+1 +"回実行");
                    //Debug.Log(lockRB.velocity);
                }
                    
            }
            
        }

        /*
        for (int n = 0; n < releaseObjStorage.Count; n++)
        {
            //Queueから出たオブジェクトのタグ変更
            releaseObjStorage[n].tag = ("Grabbable");
        }
        */
        
        lockObjs.Clear();
        recordVelocity.Clear();
        recordMagnitude.Clear();
        recordTags.Clear();
    }

    
    void StorageObjSpeed(GameObject obj, Vector3 velocity, float magnitude, string preTag)
    {
        lockObjs.Enqueue(obj);
        recordVelocity.Enqueue(velocity);
        recordMagnitude.Enqueue(magnitude);
        recordTags.Enqueue(preTag);
    }
}
