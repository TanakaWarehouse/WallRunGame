using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation_Stage : MonoBehaviour
{
    [SerializeField] private Vector3 upScale = new Vector3(1.1f, 1.1f, 1f);
    
    public void ScaleUpAnim()
    {
        LeanTween.scale(this.gameObject, upScale, 0.1f);
    }

    public void ScaleDownAnim()
    {
        LeanTween.scale(this.gameObject, Vector3.one, 0.1f);
    }

    public void SelectAnim()
    {
        LeanTween.scale(this.gameObject, Vector3.one, 0.1f);
        LeanTween.scale(this.gameObject, upScale, 0.1f).setDelay(0.1f);
    }

    public void SetOutLine()
    {
        
    }
}
