using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler_Stage : MonoBehaviour
{
    
    [Header("Fader")]
    [SerializeField] private RectTransform TransitionImg;

    private void Start()
    {
        TransitionImg.gameObject.SetActive(true);

        LeanTween.scale(TransitionImg, new Vector3(100,100,0), 0f);
        LeanTween.scale(TransitionImg, Vector3.zero, 1.0f).setOnComplete(() =>
        {
            TransitionImg.gameObject.SetActive(false);
        });
    }
    
    
    
}
