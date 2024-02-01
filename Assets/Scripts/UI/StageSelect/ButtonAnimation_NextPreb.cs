using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation_NextPreb : MonoBehaviour
{
    private Button btn;
    private Vector3 upScale = new Vector3(1.2f, 1.2f, 1);


    void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ClickAnim);
        
    }

    

    void ClickAnim()
    {
        LeanTween.scale(this.gameObject, upScale, 0.1f);
        LeanTween.scale(this.gameObject, Vector3.one, 0.1f).setDelay(0.1f);
    }

    
}
