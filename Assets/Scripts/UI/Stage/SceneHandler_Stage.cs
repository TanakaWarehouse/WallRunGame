using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler_Stage : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private ControlableManager controlableManager;
    [SerializeField] private CountDownTimer countDownTimer;
    
    //シーン遷移の素材
    [Header("Decoration")]
    [SerializeField] private RectTransform topDecoration;
    [SerializeField] private RectTransform bottomDecoration;
    [SerializeField] private GameObject stageSelectText;



    
    

    private void Start()
    {
        //LeanTween.rotate(stageSelectText, new Vector3(0, 0, 3.5f), .3f);
        LeanTween.moveY(topDecoration, 800f, 1.0f).setEase(LeanTweenType.easeOutQuart).setDelay(1.5f);
        LeanTween.moveY(bottomDecoration, -800f, 1.0f).setEase(LeanTweenType.easeOutQuart).setDelay(1.5f).setOnComplete(() =>
        {
            controlableManager.controlable = true;
            countDownTimer.countDownable = true;
            topDecoration.gameObject.SetActive(false);
            bottomDecoration.gameObject.SetActive(false);
        });


    }



    
    
}
