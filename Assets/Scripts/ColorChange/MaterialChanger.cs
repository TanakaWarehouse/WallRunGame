using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [Header("Material")] 
    private Material defaultMaterial;
    //[SerializeField] private Material highlightMaterial;
    [SerializeField] private Material lockingMaterial;

    [Header("Reference")] 
    [SerializeField] private Camera cam;
    //[SerializeField] private GravityGun gravityGun;
    //[SerializeField] private LockObject lockObject;
    [SerializeField] private ColorChangeManager colorChangeManager;



    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = this.gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        //オブジェクト本体に関する色
        if (this.gameObject.tag == "Locking")
        {
            this.gameObject.GetComponent<Renderer>().material = lockingMaterial;
        }
        
        else
        {
            this.gameObject.GetComponent<Renderer>().material = defaultMaterial;
        }
        
        
        
    }
    
}
