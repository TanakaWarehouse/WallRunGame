using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //もし球を進行方向に向けて少し傾けたいなら(矢とかにオススメ)
        /*
        if (rb.velocity.magnitude >= 0.2f)
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            */
    }

    private void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject,0.05f);
    }

}
