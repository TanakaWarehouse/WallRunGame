using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("References")]
    

    [Header("Option")]
    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private float firstTweenTime = 0.5f;
    [SerializeField] private float secondTweenTime = 0.5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 4, 0);
    [SerializeField] private Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
        LeanTween.scale(this.gameObject,Vector3.one * 0.8f, firstTweenTime).setEaseOutCubic().setOnComplete(() =>
        {
            LeanTween.scale(this.gameObject, Vector3.zero, secondTweenTime).setEaseOutCubic();
        });
        
        Destroy(this.gameObject, destroyTime);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
        Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
        Random.Range(-randomizeIntensity.z, randomizeIntensity.z));

    }

    void Update(){
        
    }

}
