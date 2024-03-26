using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnim : MonoBehaviour
{
    private Animator dummyAnim;
    // Start is called before the first frame update
    void Start()
    {
        dummyAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         //もし、スペースキーが押されたらなら
        if (Input.GetKey(KeyCode.Space))
        {
            //Bool型のパラメーターであるblRotをTrueにする
            dummyAnim.SetBool("startAnim", true);
        }
    }
}
