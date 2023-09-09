using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("Reference")] 
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    private bool readyToThrow;
    
    // Start is called before the first frame update
    void Start()
    {
        readyToThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    void Throw()
    {
        readyToThrow = false;
        
        //投げるオブジェクトのインスタンス化
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
        
        //投げるオブジェクトにRigidbodyつける
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        
        //投げる位置の計測
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            //normalizedは正規化．つまりベクトルの向きを変えずに大きさを1にする．
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        
        
        //力を加える
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    void ResetThrow()
    {
        readyToThrow = true;
    }
}
