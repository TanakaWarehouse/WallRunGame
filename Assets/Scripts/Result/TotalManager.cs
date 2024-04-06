using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI totalScoreText;
    [SerializeField]private TextMeshProUGUI totalScoreRankText;
    [SerializeField]private CalculateResult calculateResult;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(calculateResult.scoreCheck == true){
            AssignmentTotalScore();
            AssignmentTotalRank();
        }
    }

    void AssignmentTotalScore(){
        totalScoreText.text = ResultData.totalScore.ToString();
    }

    void AssignmentTotalRank(){
        totalScoreRankText.text = ResultData.totalScoreRank;
    }
}
