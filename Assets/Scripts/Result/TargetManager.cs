using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TargetManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private TextMeshProUGUI targetRank;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private CalculateResult calculateResult;

    [Header("Target")]
    public int hitTarget = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(calculateResult.scoreCheck == true){
            AssignmentHitTarget();
            AssignmentTargetRank();
            AssignmentBonus();
        }
    }

    void AssignmentHitTarget(){
        targetText.text = ResultData.hitTarget.ToString() + "/" + ResultData.numOfTargets.ToString();
    }

    void AssignmentTargetRank(){
        targetRank.text = ResultData.targetRank;
    }

    void AssignmentBonus(){
        bonusText.text = "+" + ResultData.bonus.ToString();
    }

}
