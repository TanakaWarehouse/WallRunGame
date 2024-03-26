using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    [Header("SlowMotionSettings")]
    [SerializeField] private float slowMotion = 0.1f;
    [SerializeField] private float normalTime = 1.0f;
    private bool doSlowMotion = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButton(1)){
            doSlowMotion = true;
        }
        else{
            doSlowMotion = false;
        }

        SlowMotionController();
    }
        


    public void SlowMotionController(){

        if(!doSlowMotion)
        {
            Time.timeScale = normalTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            doSlowMotion = false;
        }

        else if(doSlowMotion){
            Time.timeScale = slowMotion;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            doSlowMotion = true;
        }
    }
}
