using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectionHandler : MonoBehaviour
{
    [SerializeField] private float verticalMoveAmount = 30f;
    [SerializeField] private float moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float scaleAmount = 1.1f;

    private Vector3 startPos;
    private Vector3 startScale;

    private void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    //
    private IEnumerator MoveButton(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;

            if (startingAnimation)
            {
                endPosition = startPos + new Vector3(0f, verticalMoveAmount, 0f);
                endScale = startScale * scaleAmount;
            }

            else
            {
                endPosition = startPos;
                endScale = startScale;
            }
            
            //どれだけ位置やサイズを変更するか計算
            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / moveTime));
            
            //実際に位置と大きさをここで変える
            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }

    }

    
}
    
