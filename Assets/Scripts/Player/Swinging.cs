using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Input")] 
    public KeyCode swingKey = KeyCode.Mouse1;

    [Header("References")] 
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsSwing;
    public PlayerMovement pm;
    public AnimationCurve affectCurve;

    [Header("Swinging")] 
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;

    [Header("OdmGear")] 
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")] 
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
        
        if(Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();

        CheckForSwingPoint();
        
        if(joint != null) ObmGearMovement();
    }

    void StartSwing()
    {
        
        if (predictionHit.point == Vector3.zero) return;

        if (GetComponent<Swinging>() != null)
            GetComponent<Swinging>().StopSwing();
        
        pm.swinging = true;
        
            swingPoint = predictionHit.point;
            
            //Spring Joint は、2 つの Rigidbody をグループ化し、バネで連結されているかのように動かすことが可能．
            joint = player.gameObject.AddComponent<SpringJoint>();
            
            //autoConfigureConnectedAnchorは有効にするとconnectedAnchorがグローバル座標のanchorと一致するよう自動で計算される機能
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            
            //ここの数値でスウィングの感覚をいじれるよ
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        
    }

    void StopSwing()
    {
        pm.swinging = false;
        
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        //もしグラップルしてないときはロープを描画しない
        if (!joint) return;
        
        
        //Lerpはaとbの間でtによる線形補間を行う．
        //線形補間とは，２つのデータがあったとき、その間の値を一次関数で近似して求めること．
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    void ObmGearMovement()
    {
        //right
        if(Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        
        //left
        if(Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        
        //forward
        if(Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
        
        //short cable
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
    }

    void CheckForSwingPoint()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDistance,
            whatIsSwing);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDistance, whatIsSwing);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;
        
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        else realHitPoint = Vector3.zero;
        
        //realHitPointを見つける
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }

        //見つからなかったら
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
        
        
    }
}
