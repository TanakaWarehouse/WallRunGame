using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    private float xRotation;

    private float yRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        //ゲームウィンドウの中心にカーソルをロックする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //マウスのx,yインプット
        //GetAxisRaw()は矢印キーの左を押すと-1,右を押すと1,何も押さないと0を取得．
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        //Math.Clampはfloatの値を範囲で制限できる
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        //カメラとオブジェクトの向きを回転
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation,0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);

    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
