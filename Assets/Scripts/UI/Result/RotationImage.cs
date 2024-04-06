using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.rotateAroundLocal(this.gameObject, Vector3.forward, -360, 10f).setLoopClamp();
    }

    
}
