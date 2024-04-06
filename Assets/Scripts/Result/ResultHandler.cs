using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CalculateResult calculateResult;
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private GameObject hudCanvas;
    


    // Update is called once per frame
    void Update()
    {
        if(calculateResult.scoreCheck == true){
            hudCanvas.SetActive(false); 
            resultCanvas.SetActive(true);
        }
    }
}
