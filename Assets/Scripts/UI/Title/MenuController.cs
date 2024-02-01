using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    [Header("Load Scene")] 
    public string stageSelect;

    [Header("Fader")] 
    [SerializeField] private RectTransform fader;

    private void Start()
    {
        fader.gameObject.SetActive(true);

        //ALPHA値
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public void GoToStageSelect()
    {
        fader.gameObject.SetActive(true);
        
        //ALPHA値
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            SceneManager.LoadScene(stageSelect);
        });

    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
