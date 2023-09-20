using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOutline : MonoBehaviour
{
    
    [Header("Reference")]
    [SerializeField] private ColorChangeManager colorChangeManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //アウトライン-つかめる距離にあるオブジェクトにつける
        if (colorChangeManager.addOutline == true && this.gameObject == colorChangeManager.changeMatObj)
        {
            this.gameObject.GetComponent<Outline>().enabled = true;
        }

        else
        {
            this.gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
