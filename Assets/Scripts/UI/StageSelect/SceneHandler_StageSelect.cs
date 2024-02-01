using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler_StageSelect : MonoBehaviour
{
    [Header("Load Scene")]
    [SerializeField] private string stage1;

    [Header("Fader")] 
    [SerializeField] private RectTransform fader;
    [SerializeField] private RectTransform TransitionImg;

    private void Start()
    {
        TransitionImg.gameObject.SetActive(false);
        fader.gameObject.SetActive(true);
        
        //ALPHA値
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public void OpenStageScene()
    {
        TransitionImg.gameObject.SetActive(true);
        
        LeanTween.scale(TransitionImg, Vector3.zero, 0f);
        LeanTween.scale(TransitionImg, new Vector3(100,100,0), 1.0f).setOnComplete(() =>
        {
            SceneManager.LoadScene(stage1);
        });
    }
    
    
}
