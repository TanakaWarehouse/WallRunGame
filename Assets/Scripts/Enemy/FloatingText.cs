using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [Header("Reference")]
    private TextMeshProUGUI textMesh;

    [Header("TextColor")]
    private Color color;
    private Color fadeoutColor;
    

    [Header("Option")]
    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private float scaleUpTime = 1f;
    [SerializeField] private float textScale = .8f;
    [SerializeField] private float delay;
    [SerializeField] private float scaleDownTime = 1f;
    [SerializeField] private Vector3 offset = new Vector3(0, 4, 0);
    [SerializeField] private Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);

    

    
    // Start is called before the first frame update
    void Start()
    {

        LeanTween.scale(this.gameObject, Vector3.one * textScale, scaleUpTime).setEaseOutQuint();
        LeanTween.scale(this.gameObject, Vector3.zero, scaleDownTime).setDelay(delay);

        Destroy(this.gameObject, destroyTime);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
        Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
        Random.Range(-randomizeIntensity.z, randomizeIntensity.z));

    }

    void update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }

    

    
}
