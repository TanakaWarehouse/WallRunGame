using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] public float maxGrabDistance = 10f;
    [SerializeField] private float throwForce = 20f, lerpSpeed = 10f, rotationSpeed = 5f; 
    [SerializeField] private Transform objectHolder;

    public Rigidbody grabedRB;
    public GameObject grabObj;
    private LayerMask preLayer;
    private string preTag;
    

    private float scroll = 0;

    [Header("LockObjectReference")] 
    [SerializeField] private LockObject lockObject;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabedRB)
        {

            //移動
            grabedRB.MovePosition(Vector3.Lerp(grabedRB.position, objectHolder.transform.position, Time.deltaTime * lerpSpeed));
            
            
            //回転
            //scroll = 0のときは左，そうじゃないときは右
            scroll = Input.mouseScrollDelta.y == 0 ? scroll * 0.8f : Input.mouseScrollDelta.y * rotationSpeed;

            Quaternion rbRotation = Quaternion.AngleAxis(scroll,Vector3.up);
            
            grabedRB.MoveRotation(rbRotation * grabedRB.rotation);
            


            if (Input.GetMouseButtonDown(0))
            {
                grabedRB.isKinematic = false;
                
                //タグを元に戻す
                grabObj.tag = "Grabbable";

                //レイヤーを元に戻す
                grabObj.layer = preLayer;
                
                grabedRB.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
                grabedRB = null;
            }
        }

        Grab();
    }

    void Grab()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            //つかんでいる状態でRを押した時の処理
            if (grabedRB)
            {
                if (preTag != "Locking")
                {
                    grabedRB.isKinematic = false;
                }
                    
                
                //タグを元に戻す
                grabObj.tag = preTag;
                preTag = null;
                
                grabObj.layer = preLayer;

                grabedRB = null;
            }
            
            //つかんでいない状態からつかむとき
            else
            {
                RaycastHit hit;

                //画面左下の座標を(0, 0)、右上を(1, 1)としたときの座標に合わせて、その座標からRayを計算する関数.
                //(0.5f,0.5f)だと画面のど真ん中にRayをとばす．
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

                
                //もしロックしているやつもつかみたいならこっち
                //if (Physics.Raycast(ray, out hit, maxGrabDistance)　&& (hit.collider.CompareTag("Grabbable")　|| hit.collider.CompareTag("Locking")))
                
                if (Physics.Raycast(ray, out hit, maxGrabDistance)　&& hit.collider.CompareTag("Grabbable"))
                {
                    
                    //Rayがhitしたオブジェクトを取得
                    grabObj = hit.collider.gameObject;
                    
                    //Rayがhitしたオブジェクトのタグを取得
                    preTag = grabObj.tag;
                    
                    //grabbingタグを付与
                    grabObj.tag = "Grabbing";

                    //もともとのレイヤーを保存
                    preLayer = grabObj.layer;
                    
                    //レイヤー変更
                    grabObj.layer = LayerMask.NameToLayer("grabbing");
                    
                    grabedRB = hit.collider.gameObject.GetComponent<Rigidbody>();


                    if (grabedRB)
                    {
                        grabedRB.isKinematic = true;
                    }
                }
            }
        }
    }

   
}
