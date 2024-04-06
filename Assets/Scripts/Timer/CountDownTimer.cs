using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timer;

    [Header("Timer")]
    [SerializeField] public float countDownTime = 10.0f;
    [SerializeField] public bool countDownable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countDownable == true){
            countDownTime -= Time.deltaTime;
        }
        
        if(countDownTime <= 0){

            countDownTime = 0;
            
            
        }

        timer.text = countDownTime.ToString("F1");
    }
}
