using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerCam : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private WallRunning wallRunning;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Setting Camera")]
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    private float xRotation;
    private float yRotation;
    
    [Header("WallRunCameraTilt")]
    [SerializeField] private float maxWallRunCameraTilt;
    [SerializeField] private float wallRunCameraTilt;
    [SerializeField] private float tiltSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        //ゲームウィンドウの中心にカーソルをロックする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //マウスのx,yインプット
        //GetAxisRaw()は矢印キーの左を押すと-1,右を押すと1,何も押さないと0を取得．
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        //Math.Clampはfloatの値を範囲で制限できる
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        //カメラとオブジェクトの向きを回転
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation,wallRunCameraTilt);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);
        
        //WallRun中
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && playerMovement.wallRunning && wallRunning.wallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * tiltSpeed;
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && playerMovement.wallRunning && wallRunning.wallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * tiltSpeed;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !playerMovement.wallRunning && !wallRunning.wallRight)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * tiltSpeed;
        if (wallRunCameraTilt < 0 && !playerMovement.wallRunning && !wallRunning.wallLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * tiltSpeed;

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
