using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScoreManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]private TextMeshProUGUI resultScoreText;
    [SerializeField]private TextMeshProUGUI scoreRankText;
    [SerializeField]private CalculateResult calculateResult;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(calculateResult.scoreCheck == true){
            AssignmentResultScore();
            AssignmentScoreRank();
        }
        
    }

    void AssignmentResultScore(){
        resultScoreText.text = ResultData.resultScore.ToString();
    }

    void AssignmentScoreRank(){
        scoreRankText.text = ResultData.scoreRank;
    }
}
