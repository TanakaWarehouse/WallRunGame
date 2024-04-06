using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CountDownTimer countDownTimer;
    [SerializeField] private Camera playerCam;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private CalculateResult calculateResult;

    [Header("EnemyStatus")]
    [SerializeField] private float defaultScore = 100;
    BoxCollider boxCollider;

    private bool canGetScore = true;
    
    [Header("FloatingText")]
    [SerializeField] private GameObject FloatingTextPrefab;


    [Header("Animation")]
    private Animator dummyAnim;

    // Start is called before the first frame update
    void Start()
    {
        dummyAnim = gameObject.GetComponent<Animator>();
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    
    public int nowScore() {
        if(countDownTimer.countDownTime == 0){
            return 0;
        }
        else{
            return (int)(defaultScore / countDownTimer.countDownTime);
        }
    }
    

    void OnCollisionEnter(Collision collision)
    {
        //当たったオブジェクトがGrabbableタグなら
        if(collision.gameObject.CompareTag("Grabbable") && canGetScore == true){
            
            //デバッグ用
            //Debug.Log(nowScore());

            if(FloatingTextPrefab){


                //Bool型のパラメーターであるblRotをTrueにする
                dummyAnim.SetBool("startAnim", true);

                //スコアの表示
                ShowFloatingText();

                //スコアの加算
                scoreManager.AddScore(nowScore());

                //ターゲットヒット数加算
                calculateResult.Hit();

                //1回当たったらそれ以降はスコアなし
                canGetScore = false;

                //Colliderを無効化
                boxCollider.enabled = false;
            }     
        }
    }

    void ShowFloatingText(){
        Vector3 direction = playerCam.transform.position - this.transform.position;

        //GameObject floatText = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
        GameObject floatText = Instantiate(FloatingTextPrefab, transform.position, Quaternion.LookRotation(-1.0f * direction));
        floatText.GetComponent<TextMeshPro>().text = nowScore().ToString();

    }

    

}
