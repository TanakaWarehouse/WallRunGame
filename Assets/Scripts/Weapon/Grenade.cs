using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private bool hasExploded = false;
    public float radius = 5f;
    public float force = 700f;

    public bool targetHit;

    public GameObject explosionEffect;
    private Rigidbody granadeRb;
    
    // Start is called before the first frame update
    void Start()
    {
        granadeRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }

    void Explode()
    {
        //エフェクト
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        
        //foreachとは配列の要素の反復処理を行うもの。指定した配列の全ての要素に処理を加えられる。
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
           //爆発した際に力を加える

           if (rb != null)
           {
               rb.AddExplosionForce(force, transform.position, radius);
           }
        }

        //object消す
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;

        else
            targetHit = true;
        
        //くっつくように
        granadeRb.isKinematic = true;
        
        //くっついたあとオブジェクトを子オブジェクトに
        transform.SetParent(collision.transform);
    }
}
