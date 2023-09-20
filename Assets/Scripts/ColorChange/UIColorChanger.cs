using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorChanger : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private ColorChangeManager colorChangeManager;
    
    [Header("UI Color")] 
    private Color defaultColor;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = this.gameObject.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (colorChangeManager.uiColerChange == true)
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 1.0f);
        }

        else
        {
            this.gameObject.GetComponent<Image>().color = defaultColor;
        }
        
    }
}
