using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager: MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Score")]
    private int gameScore = 0;
    
    

    [Header("AnimationOption")]
    [SerializeField] private float offset;
    [SerializeField] private float tweenTime; 

    
    public int Score 
    {  
        get { return gameScore; }

        set 
        { 
            gameScore = value;

            //値が変更された際の処理;
            LeanTween.cancel(this.gameObject);
            LeanTween.moveY(this.gameObject, this.gameObject.transform.position.y + offset, tweenTime).setEaseOutExpo();
            LeanTween.moveY(this.gameObject, this.gameObject.transform.position.y, tweenTime).setEaseOutExpo().setDelay(tweenTime);

            //テキストに値代入
            scoreText.text = Score.ToString();

        }

        
    }

    //スコア加算用
    public void AddScore(int a){
        Score += a;
    }

    //他からスコア取得したいとき用
    public int SetScore(){
        return gameScore;
    }

    



}
