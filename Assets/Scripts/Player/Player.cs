using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public void ChangeMove()
    {
        move *= -1;
    }

    public void oderTurnR() // Add condition statement if (!isPause && !isOver) in Pause, GameManager script when clicking the button *****
    {
        if (isturn != TurnState.DOING)
            isturn = TurnState.RIGHT;
    }

    public void oderTurnL()
    {
        if (isturn != TurnState.DOING)
            isturn = TurnState.LEFT;
    }

    public void oderFront()
    {
        move = 1;
    }

    public int getMove()
    {
        return move;
    }

    public void Jump()
    {
        if (!isJump)
        {
            isJump = true;
            rg.AddForce(Vector3.up * f, ForceMode.Impulse);
            playerSound.SoundPlay("Jump");

        }
    }

    //private:
    enum TurnState
    {
        REEADY,
        NONE,
        DOING,
        RIGHT,
        LEFT
    };

    //enum Direction
    //{
    //    X,
    //    Z,
    //    READY,
    //    WAIT
    //}

    const int TILE = 7;

    //control
    float timeAmongButton = 0.02f;

    //move
    public float speed; //move speed
    private int move = 1;
    private bool go = false; //it is going now?

    private float playTime = 0f; //속도 올리기용. 나중에 없애기. 

    //turn
    private TurnState isturn = TurnState.NONE;
    private bool rotateComplete = false;

    //jump
    private bool isJump = false;
    private int f = 6; //jump Force

    //Controller
    private bool s = false; //Swipe mode true, Button mode false
    private int mistake = 100; //touch width

    //gameObject
    Rigidbody rg;
    Transform tf;

    public Animator anim;
    Vector3 startPos; //touch position in device

    //PlayerSound
    GameObject soundSource;
    PlayerSound playerSound;


    void Start()
    {
        soundSource = GameObject.Find("PlayerSoundSource");
        if(soundSource != null)
            playerSound = soundSource.GetComponent<PlayerSound>();

        tf = GetComponent<Transform>();
        rg = GetComponent<Rigidbody>();

        startPos = new Vector3(0, 0, 0);

        speed = 0.001f;

        //checkCount = 2;

        //Test
        //tf.DOMove(nextPosition(), speed * 2000);
        //tf.DORotate(angleRL(), speed * 500);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isJumping", isJump);

        //check user controller when it Swipe mode 
        if (Option.getController() == Controller.SWIPE && Input.touchCount > 0) //first touch
        {
            Touch touch0 = Input.GetTouch(0);

            if (touch0.phase == TouchPhase.Began) //if it just touch
            {
                startPos = touch(touch0);
            }
            else if (s && (touch0.phase == TouchPhase.Moved)) //if it swipe
            {
                swipe(touch0, startPos);
            }

            if (Input.touchCount > 1) //second touch
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

        //move
        if (rotateComplete == true)
        {
            go = true; //회전 막고
            rotateComplete = false; //검사 새로 해야함.
            step(); //앞으로 가요.
        }

        if (go == false) //앞으로 걷는 거 끝남.
        {
            rotateCheck(); //다시 회전 검사.
        }

        

        //speed up at an interval of 25s
        if (playTime > 25)
        {
            //motion = true;
            Debug.Log(playTime);
            //playTime = 0;
            //checkCount = 1;
            //speedUp();
        }

        playTime += Time.deltaTime;
        //Debug.Log(playTime);
    }

    private void step()
    {
        Debug.Log("앞으로 가기");

        Tween tween = tf.DOMove(nextPosition(), speed * 500)
            .SetEase(Ease.Linear); //원하는 위치로

        StartCoroutine(stepComplete(tween));
        //go = false;

    //    target.DOMoveX(5f, 1f)
    //.SetEase(Ease.InOutSine)
    //.OnComplete(StartNextTween);
    }

    private Vector3 nextPosition() //XXXXXXXXXXXX
    {
        Vector3 point = tf.position;

        point += tf.forward * move * TILE; //연산 메소드가 필요할 수도

        return point;
    }

    IEnumerator stepComplete(Tween move) //isturn 초기화 겸 이동 완료 검사.
    {
        float time = 0f;

        while (time < timeAmongButton) //연타 시 오차 범위
        {
            time += Time.deltaTime;
            yield return null;
        }
        rotateOrderDelete();

        while(move.IsComplete() == false)
        {
            yield return null;
        }

        go = false;
    }

    private void rotateCheck()
    {
        Debug.Log("회전 검사");
        //Tween tween;

        if (isturn == TurnState.NONE)
            rotateEnd();
        else if (isturn == TurnState.LEFT || isturn == TurnState.RIGHT)
        {
            tf.DORotate(angleRL(), speed * 500) //200000
                //2번 .SetSpeedBased(true)
                .SetEase(Ease.InOutSine)
                .OnComplete(rotateEnd);
        }
        else if (isturn == TurnState.DOING)
        {
            ;
        }
        else if (isturn == TurnState.REEADY)
        {
            tf.DORotate(angleRL(), speed * 500)
                .OnComplete(rotateOrderDelete);
        }
    }

    private void rotateEnd()
    {
        rotateComplete = true;
        isturn = TurnState.NONE;
    }

    private Vector3 angleRL()
    {
        Vector3 angle = Vector3.zero;

        if (isturn == TurnState.RIGHT)
            angle = tf.rotation.eulerAngles + new Vector3(0f, 90f, 0f);
        else if (isturn == TurnState.LEFT)
            angle = tf.rotation.eulerAngles + new Vector3(0f, -90f, 0f);
        else
            Debug.Log("isturn: " + isturn);

        isturn = TurnState.DOING;
        return angle;
    }

    //private void correctLine() //no used
    //{
    //    //correctLine
    //    if (seeX == Direction.X)
    //    {
    //        transform.position = Vector3.SmoothDamp(tf.position, new Vector3(x, tf.position.y, correctGoalZ), ref correctVelocity, correctTime);
    //    }
    //    else if (seeX == Direction.Z)
    //    {
    //        transform.position = Vector3.SmoothDamp(tf.position, new Vector3(correctGoalX, tf.position.y, z), ref correctVelocity, correctTime);
    //    }
    //}

    //private void speedUp()
    //{
    //    speed *= 1.2f;
    //}

    //private bool checkPlace(float x) //no used.
    //{
    //    float xf = x - (float)Math.Floor(x);

    //    if (0.75 <= xf || xf <= 0.25)
    //        return true; //xd + 1
    //    else
    //        return false; //xd
    //}

    private void rotateOrderDelete()
    {
        isturn = TurnState.NONE;
    }

    private Vector3 touch(Touch touch) //��ġ �߻� -> �������� ���������� Ȯ��.
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

    private void swipe(Touch touch, Vector3 startPos) //���� ��ġ �߻� -> ����� ������������ Ȯ��.
    {
        if (touch.position.x - startPos.x > mistake) //�������ٴ� ȸ�� �켱
        {
            oderTurnR();
        }
        else if (startPos.x - touch.position.x > mistake)
        {
            isturn = TurnState.LEFT;
        }
        else if (touch.position.y - startPos.y > mistake)
        {
            oderFront();
        }
        else
            return;

        s = false;
    }

    private void OnCollisionEnter(Collision other) //���� ���� ���� Ȯ��.
    {
        if (other.collider.CompareTag("Floor")) //��ҹ��� Ȯ��
        {
            isJump = false;
        }
    }

    //private void checkSeeX() //오차 1
    //{
    //    int o = 1; //오차

    //    if (lastX + o < tf.position.x || lastX - o > tf.position.x)
    //    {
    //        seeX = Direction.X;
    //    }
    //    else
    //    {
    //        seeX = Direction.Z;
    //    }

    //    //lastX = tf.position.x;
    //    //lastZ = tf.position.z;
    //}

    //private int turnGoal(int r)
    //{
    //    if (r % 90 == 0)
    //    {
    //        if (isturn == TurnState.RIGHT)
    //            return r + 90;
    //        else
    //            return r - 90;
    //    }

    //    return r;
    //}

    //IEnumerator turnR()
    //{
    //    int rY = (int)tf.eulerAngles.y; //if error in angle, change int to float
    //    int goal = turnGoal(rY); //turn

    //    while (rY != goal)
    //    {
    //        rY += 5; //ry값 조정. -> speed 올라가면 같이 올라가도록
    //        rg.MoveRotation(Quaternion.Euler(0, rY, 0));
    //        yield return null;
    //    }

    //    endTrun();
    //}

    //IEnumerator turnL() //ȸ�� �޼ҵ�.
    //{
    //    int rY = (int)tf.eulerAngles.y;
    //    int goal = turnGoal(rY); //��ǥ ����

    //    while (rY != goal)
    //    {
    //        rY -= 5;
    //        rg.MoveRotation(Quaternion.Euler(0, rY, 0));
    //        yield return null;
    //    }

    //    endTrun();
    //}

    //private void endTrun()
    //{
    //    go = true;
    //    lastX = tf.position.x;
    //    lastZ = tf.position.z;
    //    seeX = Direction.READY;
    //}
}
