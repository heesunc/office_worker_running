using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void ChangeMove() //넉백
    {
        move *= -1;
    }

    public void oderTurnR()
    {
        isturn = TurnState.RIGHT;
    }

    public void oderTurnL()
    {
        isturn = TurnState.LEFT;
    }

    public void oderFront()
    {
        move = 1;
    }


    public void Jump()
    {
        if (!isJump)
        {
            isJump = true;
            rg.AddForce(Vector3.up * f, ForceMode.Impulse);
        }
    }

    public bool seeX;
    private Animator anim;

    //private:
    enum TurnState
    {
        NONE,
        RIGHT = 1,
        LEFT = -1
    };

    const int TILE = 7;

    private int move = 1;
    private TurnState isturn = TurnState.NONE;
    private bool isJump = false;
    private int f = 5; //점프 힘 조절
    private float speed = 7; //달리기 속도
    private int mistake = 100; //스와이프 길이
    private bool s = false; //왼쪽 터치면 true, 오른쪽 터치면 false
    private bool go = true; //앞으로 가는 중 => 오차 줄이는 용도
    private float playTime = 0f;
    private float lastX;
    private float lastZ;

    Rigidbody rg;
    Transform tf;
    Vector3 startPos;

    void Start()
    {
        //seeX = false;
        tf = gameObject.GetComponent<Transform>();
        rg = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        startPos = new Vector3(0, 0, 0);
        lastX = tf.position.x;
        lastZ = tf.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isJumping", isJump);

        //Swipe인 경우 터치 입력 받기
        //아예 터치 받는 부분을 분리하고 다른 오브젝트에 붙여서 오브젝트 false하는 게 더 깔끔할 듯
        if (Option.getController() == Controller.SWIPE && Input.touchCount > 0) //ù ��° ��ġ �߻�
        {
            Touch touch0 = Input.GetTouch(0);

            if (touch0.phase == TouchPhase.Began) //오른쪽 터치면 점프, 왼쪽 터치면 스와이프 준비
            {
                startPos = touch(touch0);
            }
            else if (s && (touch0.phase == TouchPhase.Moved)) //왼쪽 터치일 때: 스와이프 어느쪽인지 확인
            {
                swipe(touch0, startPos);
            }

            if (Input.touchCount > 1) //두 번째 터치 발생 시 첫번째 터치와 같은 방식으로 처리.
            {
                Touch touch1 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began)
                {
                    startPos = touch(touch1);
                }
                else if (s && (touch1.phase == TouchPhase.Moved))
                {
                    swipe(touch1, startPos);
                }
            }
        }

        //회전 명령 확인
        if (isturn != TurnState.NONE)
        {
            Debug.Log("버튼은 문제 없다");
            float x = tf.position.x;
            float z = tf.position.z;

            if ((seeX && checkPlace(x) && Math.Round(x) % TILE == 0) //x축으로 이동 중일 경우 x축이 타일 사이일 때
                || (!seeX && checkPlace(z) && Math.Round(z) % TILE == 0)) //z축으로 이동 중일 경우 z축이 타일 사이일 때 회전.
            {
                Debug.Log("checkPlace도 되는데");
                go = false;

                if (isturn == TurnState.LEFT)
                {
                    StartCoroutine(turnL());
                }
                else //RIGHT
                {
                    StartCoroutine(turnR()); //테스트 해보기
                }
              
                isturn = TurnState.NONE;
                lastX = tf.position.x;
                lastZ = tf.position.z;
            }
        }

        checkSeeX();

        //앞으로 가즈아??
        if (go == true)
        {
            tf.Translate(Vector3.forward * speed * move * Time.deltaTime); //앞으로 가도록 함, 게임 속도 보정
        }

        //시간에 따른 가속도
        //플레이타임 만약에 필요하면 몇 번째 증속인지 세는 변수 도입.
        playTime += Time.deltaTime;
        if (playTime > 25)
        {
            playTime = 0;
            //speedUp();
        }
    }

    private void speedUp()
    {
        speed *= 1.2f;
    }

    private bool checkPlace(float x) //오차가 너무 크니까 조금만 더 가서 돌아라
    {
        float xf = x - (float)Math.Floor(x);
        Debug.Log(xf);

        if (0.75 <= xf || xf <= 0.25)
            return true; //xd + 1
        else
            return false; //xd
    }

    private Vector3 touch(Touch touch) //터치 발생 -> 왼쪽인지 오른쪽인지 확인.
    {
        if (touch.position.x > 960)
        {
            Jump();
        }
        else
        {
            s = true;
        }

        return touch.position;
    }

    private void swipe(Touch touch, Vector3 startPos) //왼쪽 터치 발생 -> 어느쪽 스와이프인지 확인.
    {
        if (touch.position.x - startPos.x > mistake) //직진보다는 회전 우선
        {
            oderTurnR();
            Debug.Log("오른쪽으로 스와이프");
        }
        else if (startPos.x - touch.position.x > mistake)
        {
            isturn = TurnState.LEFT;
            Debug.Log("왼쪽으로 스와이프");
        }
        else if (touch.position.y - startPos.y > mistake)
        {
            oderFront();
            Debug.Log("위쪽으로 스와이프");
        }
        else
            return;

        s = false;
    }

    private void OnCollisionEnter(Collision other) //점프 가능 상태 확인.
    {
        if (other.collider.CompareTag("Floor")) //대소문자 확인
        {
            isJump = false;
        }

    }
    private void checkSeeX() //오차 1
    {
        Debug.Log("checkSeeX 진입");

        Debug.Log(lastX);
        Debug.Log(lastZ);

        int o = 1; //오차

        if (lastX + o < tf.position.x || lastX - o > tf.position.x)
        {
            seeX = true;
        }
        else if (lastZ + o < tf.position.z || lastX - o > tf.position.z)
        {
            seeX = false;
        }
    }

    private int turnGoal(int r)
    {
        if (r % 90 == 0)
        {
            if (isturn == TurnState.RIGHT)
                return r + 90;
            else
                return r - 90;
        }

        return r;
    }

    IEnumerator turnR() //회전 메소드.
    {
        int rY = (int)tf.eulerAngles.y;
        int goal = turnGoal(rY); //목표 각도

        while (rY != goal)
        {
            rY += 3;
            rg.MoveRotation(Quaternion.Euler(0, rY, 0));
            yield return null;
        }
        go = true;
    }

    IEnumerator turnL() //회전 메소드.
    {
        int rY = (int)tf.eulerAngles.y;
        int goal = turnGoal(rY); //목표 각도
        while (rY != goal)
        {
            rY -= 3;
            rg.MoveRotation(Quaternion.Euler(0, rY, 0));
            yield return null;
        }
        go = true;
    }
}
