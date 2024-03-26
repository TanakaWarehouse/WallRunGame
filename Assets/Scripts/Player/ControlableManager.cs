using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlableManager : MonoBehaviour
{
    [Header("boolean")]
    public bool controlable;

    [Header("References")]
    [SerializeField] private GameObject player;


    public void Start()
    {
        controlable = false;
    }

    
    public void Update(){
        
    }

    //これで今コントロールできるかどうかを取得可能
    public bool SetControrableState(){
        return controlable;
    }
    
}
