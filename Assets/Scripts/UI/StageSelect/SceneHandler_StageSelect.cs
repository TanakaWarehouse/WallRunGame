using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler_StageSelect : MonoBehaviour
{
    //読み込むシーン
    [Header("Load Scene")]
    [SerializeField] private string stage1;

    //シーン遷移の素材
    [Header("Fader")] 
    [SerializeField] private RectTransform fader;
    
    [Header("Decoration")]
    [SerializeField] private RectTransform topDecoration;
    [SerializeField] private RectTransform bottomDecoration;
    [SerializeField] private GameObject stageSelectText;

    private void Start()
    {
        fader.gameObject.SetActive(true);
    
        //ALPHA値(このシーンが始まるときは必ずフェードインする)
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    //ステージへ移行するときの動き
    public void OpenStageScene()
    {
        //TextAnim();
        LeanTween.moveY(topDecoration, 0f, 1f).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveY(bottomDecoration, 0f, 1f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() =>
        {
            SceneManager.LoadScene(stage1);
        });
    }

    public void TextAnim(){
        LeanTween.rotate(stageSelectText, new Vector3(-90, 0, 0), .3f);
    }

    
    
    
}
