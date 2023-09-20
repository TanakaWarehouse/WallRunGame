using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeManager : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private Camera cam;
    [SerializeField] private GravityGun gravityGun;
    [SerializeField] private LockObject lockObject;

    [Header("Manager")] 
    public bool addOutline = false;
    public bool uiColerChange = false;
    public GameObject changeMatObj;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        
        
        //ロック中もつかみたいならこっち
        //if (Physics.Raycast(ray, out hit, gravityGun.maxGrabDistance) && hit.collider.gameObject.layer != LayerMask.NameToLayer("grabbing") && (hit.collider.CompareTag("Grabbable") || hit.collider.CompareTag("Locking")))
        
        //アウトライン用
        if (Physics.Raycast(ray, out hit, gravityGun.maxGrabDistance) && hit.collider.gameObject.layer != LayerMask.NameToLayer("grabbing") && hit.collider.CompareTag("Grabbable"))
        {
            changeMatObj = hit.collider.gameObject;
            addOutline = true;
        }
        else
        {
            addOutline = false;
        }
        
       
        //クロスヘアの色を変える用(たぶん床に先に当たるとオブジェクト取れていないからそこをなんとかせよ)
        if (Physics.Raycast(ray, out hit, lockObject.maxLockDistance) && (hit.collider.gameObject.CompareTag("Grabbable") || hit.collider.gameObject.CompareTag("Lockable")) && hit.collider.gameObject.layer != LayerMask.NameToLayer("grabbing"))
        {
            uiColerChange = true;
        }
        else
        {
            uiColerChange = false;
        }
        
        
    }
}
