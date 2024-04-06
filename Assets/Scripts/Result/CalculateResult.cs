using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateResult : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private CountDownTimer countDownTimer;

    [Header("Total Score")]
    private int totalScore = 0;
    private string totalScoreRank;
    
    [Header("Score")]
    private int resultScore = 0;
    private string scoreRank;

    [Header("Target")]
    private int hitTarget = 0;
    [SerializeField]private int numOfTargets;
    private string targetRank;
    private int bonus;

    [Header("TotalRank")]
    [SerializeField]private int totalRankS;
    [SerializeField]private int totalRankA;
    [SerializeField]private int totalRankB;
    [SerializeField]private int totalRankC;
    [SerializeField]private int totalRankD;

    [Header("ScoreRank")]
    [SerializeField]private int scoreRankS;
    [SerializeField]private int scoreRankA;
    [SerializeField]private int scoreRankB;
    [SerializeField]private int scoreRankC;
    [SerializeField]private int scoreRankD;

    [Header("TargetRank")]
    [SerializeField]private int targetRankS;
    [SerializeField]private int targetRankA;
    [SerializeField]private int targetRankB;
    [SerializeField]private int targetRankC;
    [SerializeField]private int targetRankD;

    //スコアの計算を1回だけ行う用
    public bool scoreCheck = false;




    void Update()
    {
        if(countDownTimer.countDownTime == 0 && scoreCheck == false){
            
            //スコアマネージャからスコア持ってくる
            resultScore = scoreManager.SetScore();

            //スコアの計算
            CalcurateScoreRank();
            CalculateTargetRank();

            //totalスコアは最後に計算
            CalcurateTotalScore();
            CalculateTotalScoreRank();
            



            //リザルトデータ保管クラスにデータを渡す
            sendCalcuratedData();

            
            
            //スコアチェック完了したよ用
            scoreCheck = true;
        }
    }

    //TotalScore関連
    void CalcurateTotalScore(){
        totalScore = resultScore + bonus;
    }

    void CalculateTotalScoreRank(){
        if(totalScore >= totalRankS){
            totalScoreRank = "S";
        }
        else if(totalScore >= totalRankA && totalScore < totalRankS){
            totalScoreRank = "A";
        }
        else if(totalScore >= totalRankB && totalScore < totalRankA){
            totalScoreRank = "B";
        }
        else if(totalScore >= totalRankC && totalScore < totalRankB){
            totalScoreRank = "C";
        }
        else{
            totalScoreRank = "D";
        }
    }


    //Score関連
    void CalcurateScoreRank(){
        if(resultScore >= scoreRankS){
            scoreRank = "S";
        }

        else if(resultScore >= scoreRankA && resultScore < scoreRankS){
            scoreRank = "A";
        }

        else if(resultScore >= scoreRankB && resultScore < scoreRankA){
            scoreRank = "B";
        }

        else if(resultScore >= scoreRankC && resultScore < scoreRankB){
            scoreRank = "C";
        }

        else{
            scoreRank = "D";
        }
        
    }


    //Target関連
    public void Hit(){
        hitTarget += 1;
    }

    void CalculateTargetRank(){
        if(hitTarget >= targetRankS){
            targetRank = "S";
            bonus = 4000;
        }

        else if(hitTarget >= targetRankA && hitTarget < targetRankS){
            targetRank = "A";
            bonus = 3000;
        }

        else if(hitTarget >= targetRankB && hitTarget < targetRankA){
            targetRank = "B";
            bonus = 2000;
        }

        else if(hitTarget >= targetRankC && hitTarget < targetRankB){
            targetRank = "C";
            bonus = 1000;
        }
        else{
            targetRank = "D";
            bonus = 0;
        }
    }


    //ResultDataクラスに必要情報を送る関数
    void sendCalcuratedData(){
        
        //トータル関連
        ResultData.totalScore = totalScore;
        ResultData.totalScoreRank = totalScoreRank;

        //スコア関連
        ResultData.resultScore = resultScore;
        ResultData.scoreRank = scoreRank;

        //ターゲット関連
        ResultData.hitTarget = hitTarget;
        ResultData.numOfTargets = numOfTargets;
        ResultData.targetRank = targetRank;
        ResultData.bonus = bonus;
    }





    


}
