using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    [Header("Page")]
    [SerializeField] private int maxPage;
    
    private int currentPage;
    private Vector3 targetPos;
    
    [SerializeField] private Vector3 pageStep;
    
    //RectTransformは一般的なTransformコンポーネントの代わりにすべての UI 要素で使用される新しいトランスフォームコンポーネント
    [SerializeField] private RectTransform levelPagesRect; 

    [Header("MovePage")]
    [SerializeField] private float tweenTime;
    [SerializeField] private LeanTweenType tweenType;

    [Header("Guide")] 
    [SerializeField] private Image[] barImage;
    [SerializeField] private Sprite barClosed, barOpen;

    [SerializeField] private Button previousBtn, nextBtn;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        UpdateBar();
        UpdateArrowButton();
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        UpdateBar();
        UpdateArrowButton();
    }

    void UpdateBar()
    {
        //foreach : コレクションのすべての要素を１つ１つ取得するときに使用する。
        foreach (var item in barImage)
        {
            
            item.sprite = barClosed;
            
        }
        barImage[currentPage - 1].sprite = barOpen;

    }

    void UpdateArrowButton()
    {
        nextBtn.interactable = true;
        previousBtn.interactable = true;

        if (currentPage == 1) previousBtn.interactable = false;
        else if (currentPage == maxPage) nextBtn.interactable = false;
    }
}
