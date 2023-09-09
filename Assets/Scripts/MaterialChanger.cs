using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [Header("Material")] 
    private Material defaultMaterial;
    //[SerializeField] private Material highlightMaterial;
    [SerializeField] private Material lockingMaterial;

    

    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = this.gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
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
