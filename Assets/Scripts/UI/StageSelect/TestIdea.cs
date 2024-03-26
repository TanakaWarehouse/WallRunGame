using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdea : MonoBehaviour
{
    [Header("Decoration")]
    [SerializeField] private RectTransform topDecoration;
    [SerializeField] private RectTransform bottomDecoration;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveY(topDecoration, 10f, 1.0f).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveY(bottomDecoration, 0f, 1.0f).setEase(LeanTweenType.easeOutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
