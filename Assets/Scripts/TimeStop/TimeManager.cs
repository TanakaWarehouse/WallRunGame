using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool TimeIsStopped;

    public void ContinueTime()
    {
        TimeIsStopped = false;
        
        //TimeBodyコンポーネントを持ったオブジェクトを探す
        TimeBody[] objects = FindObjectsOfType<TimeBody>();

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<TimeBody>().ContinueTime();
        }
    }

    public void StopTime()
    {
        TimeIsStopped = true;
    }
    
}
