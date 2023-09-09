using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private float maxLockDistance;
    private Rigidbody lockRB;
    public GameObject lockObj;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;
    
    [Header(("UnLock"))]
    private GameObject releaseObj;
    private Vector3 releaseVelocity;
    private float releaseMagnitude;
    private List<GameObject> releaseObjStorage = new List<GameObject>();

    [Header("Keybinds")] 
    public KeyCode lockObjKey = KeyCode.Q;
    public KeyCode unlockObjKey = KeyCode.E;

    [Header("Queue")]
    Queue<GameObject> lockObjs = new Queue<GameObject>();
    Queue<Vector3> recordVelocity = new Queue<Vector3>();
    Queue<float> recordMagnitude = new Queue<float>();


    private void Update()
    {
        //デバッグ用
        //Debug.Log(lockObjs.Count);
        //Debug.Log(releaseObjStorage.Count);
        
        if (Input.GetKeyDown(lockObjKey) && !gravityGun.grabedRB)
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

            if (Physics.Raycast(ray, out hit, maxLockDistance))
            {
                lockRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                
                //hitしたオブジェクトを取得
                lockObj = hit.collider.gameObject;
                

                if (lockRB && lockRB.velocity.magnitude >= 0f && !lockObj.CompareTag("Locking"))
                {
                    recordedVelocity = lockRB.velocity.normalized;
                    recordedMagnitude = lockRB.velocity.magnitude;
                    
                    StorageObjSpeed(lockObj, recordedVelocity.normalized, recordedMagnitude);

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
            
            
            //Queueから出たオブジェクトのRigidbody
            lockRB = releaseObj.GetComponent<Rigidbody>();

            if (releaseObj.layer != LayerMask.NameToLayer("grabbing"))
            {
                lockRB.isKinematic = false;

                if (!releaseObj.CompareTag("Grabbable") && !lockObjs.Contains(releaseObj))
                {
                    lockRB.velocity = releaseVelocity * releaseMagnitude;
                    
                    //デバッグ用
                    //Debug.Log(i+1 +"回実行");
                    //Debug.Log(lockRB.velocity);
                }
                    
            }
            
        }

        for (int n = 0; n < releaseObjStorage.Count; n++)
        {
            //Queueから出たオブジェクトのタグ変更
            releaseObjStorage[n].tag = ("Grabbable");
        }
        
        lockObjs.Clear();
        recordVelocity.Clear();
        recordMagnitude.Clear();
    }

    
    void StorageObjSpeed(GameObject obj, Vector3 velocity, float magnitude)
    {
        lockObjs.Enqueue(obj);
        recordVelocity.Enqueue(velocity);
        recordMagnitude.Enqueue(magnitude);
    }
}
