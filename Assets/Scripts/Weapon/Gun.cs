using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class Gun : MonoBehaviour
{

    [Header("Camera")]
    public Camera fpsCam;
    
    [Header("BulletShooting")]
    public GameObject bulletPrefab;
    public Transform FirePoint;
    public float bulletSpeed;
    public float fireRate;
    public float fireTimer;
    public ParticleSystem muzzleFlash;

    
    private Vector3 destination;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime; 
        
        if (Input.GetMouseButtonDown(0) && fireTimer <= 0f)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //マズルフラッシュ
        muzzleFlash.Play();

        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }
        
        
        Vector3 middleOfScreen = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 100f));
        FirePoint.LookAt(middleOfScreen);
        
        InstantiateBullet(FirePoint);

    }
    
    void InstantiateBullet(Transform firePoint)
    {
        
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position,Quaternion.identity) as GameObject;
        bulletObj.GetComponent<Rigidbody>().velocity = bulletObj.transform.forward * bulletSpeed;

        fireTimer = fireRate;
    }
}
