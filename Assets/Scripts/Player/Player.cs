using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public void ChangeMove()
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
        NONE,
        RIGHT = 1,
        LEFT = -1
    };

    enum Direction
    {
        X, //x축으로 이동 중
        Z, //z축으로 이동 중
        READY, //아직 방향을 가지지 않은 상태
        WAIT //방향을 가지지 않는 상태
    }

    const int TILE = 7;
    private Direction seeX;

    //move
    public float speed; //move speed
    public float timeSpeed;
    private int move = 1;
    bool d;
    private float playTime = 0f; //속도 올리기용. 나중에 없애기. 
    private Vector3 GoalPosition;
    Tween tween;

    //turn
    private TurnState isturn = TurnState.NONE;
    private bool go; //it is going now?
    private float lastX;
    private float lastZ;

    //jump
    private bool isJump = false;
    private int f = 6; //jump Force

    //Controller
    private bool s = false; //Swipe mode true, Button mode false
    private int mistake = 100; //touch width

    //gameObject
    Rigidbody rg;
    Transform tf;
    float x;
    float z;

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

        tf = gameObject.GetComponent<Transform>();
        rg = gameObject.GetComponent<Rigidbody>();
        startPos = new Vector3(0, 0, 0);
        lastX = tf.position.x;
        lastZ = tf.position.z;

        timeSpeed = 2f;
        seeX = Direction.X;
        //tween = transform.DOMove(GoalPosition, timeSpeed).OnComplete(goToF);

        speed = 15;
        Debug.Log(tf.position);
        settingGoal();
    }

    // Update is called once per frame
    void Update()
    {
        //move
        //transform.DOMove(GoalPosition, timeSpeed).OnComplete(goToF);
        transform.DOMove(GoalPosition, timeSpeed).OnComplete(goToF);
        Debug.Log(GoalPosition);
        Debug.Log(go);

        //check animation
        anim.SetBool("isJumping", isJump);

        //check user control when it Swipe mode 
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

        //go == false -> check turn & compute next point 
        if (go == false)
        {
            //checkSeeX[if it place on Point, check its front]
            checkSeeX();
            Debug.Log(seeX);

            //until No check
            //be called Turn 
            if (isturn != TurnState.NONE)
            {
                if (isturn == TurnState.LEFT)
                {
                    turnL();
                }
                else //RIGHT
                {
                    turnR();
                }

                isturn = TurnState.NONE;

                //if turn, change seeX
                seeX = (Direction)((int)seeX * -1);
            }

            //setting correctGoal
            settingGoal();
            //GoalPosition = tf.position;

            //if (seeX == Direction.X)
            //{
            //    d = Vector3.forward.x < 0;
            //    if (move == -1)
            //    {
            //        d = !d;
            //    }
            //    GoalPosition.x = close7(x, d);
            //}
            //else if (seeX == Direction.Z)
            //{
            //    d = Vector3.forward.z < 0;
            //    if (move == -1)
            //    {
            //        d = !d;
            //    }
            //    GoalPosition.z = close7(z, d);
            //    Debug.Log(GoalPosition);
            //}
            //else
            //{
            //    Debug.Log("오류(seeX): 아직 방향이 지정되지 않음.");
            //}

            go = true;

            Debug.Log("다음 position 연산 결과 확인");
            Debug.Log(GoalPosition);
        }

        //speed up at an interval of 25s
        if (playTime > 25)
        {
            playTime = 0;
            speedUp();
        }

        playTime += Time.deltaTime;
        Debug.Log(playTime);
    }

    private void settingGoal()
    {
        //check now positon
        x = tf.position.x;
        z = tf.position.z;

        GoalPosition = tf.position;
        Debug.Log(GoalPosition);

        if (seeX == Direction.X)
        {
            d = Vector3.forward.x < 0;
            if (move == -1)
            {
                d = !d;
            }
            Debug.Log(x);
            Debug.Log(d);
            GoalPosition.x = close7(x, d);
        }
        else if (seeX == Direction.Z)
        {
            d = Vector3.forward.z < 0;
            if (move == -1)
            {
                d = !d;
            }
            GoalPosition.z = close7(z, d);
            Debug.Log(GoalPosition);
        }
        else
        {
            Debug.Log("오류(seeX): 아직 방향이 지정되지 않음.");
        }
    }

    private void goToF()
    {
        go = false;
    }

    private int close7(float p, bool d)
    {
        Debug.Log(p);
        int r = Mathf.CeilToInt(p);
        Debug.Log(r);
        int i;

        if (d == true)
        {
            i = 2;

            if (p >= 0)
            {
                Debug.Log(p);
                for (; r >= 0; i++)
                {
                    r -= 7;
                }
            }
            else
            {
                for (; r < -7; i--)
                {
                    r += 7;
                }
            }

            return i * 7;
        }
        else
        {
            i = -2;

            if (p >= 0)
            {
                for (; r >= 0 ; i++)
                {
                    r -= 7;
                }
            }
            else
            {
                for (; r < 0; i--)
                {
                    r += 7;
                }
            }

            return i * 7;
        }

    }

    private void speedUp() //1.2배속
    {
        timeSpeed /= 1.2f;
    }

    private Vector3 touch(Touch touch)
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

    private void swipe(Touch touch, Vector3 startPos) 
    {
        if (touch.position.x - startPos.x > mistake) 
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

    private void checkSeeX() //오차 1
    {
        int o = 2; //오차

        if (lastX + o < tf.position.x || lastX - o > tf.position.x)
        {
            seeX = Direction.X;
        }
        else if (lastZ + o < tf.position.z || lastZ - o > tf.position.z)
        {
            seeX = Direction.Z;
        }
        else
        {
            Debug.Log("오류: 이동하지 않음. seeX 값 유지.");
        }

        lastX = tf.position.x;
        lastZ = tf.position.z;
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

    private void turnR()
    {
        int rY = (int)tf.eulerAngles.y; //if error in angle, change int to float
        int goal = turnGoal(rY); //turn

        while (rY != goal)
        {
            rY += 5; //ry값 조정. -> speed 올라가면 같이 올라가도록
            rg.MoveRotation(Quaternion.Euler(0, rY, 0));
        }

        endTrun();
    }

    private void turnL() //ȸ�� �޼ҵ�.
    {
        int rY = (int)tf.eulerAngles.y;
        int goal = turnGoal(rY); //��ǥ ����

        while (rY != goal)
        {
            rY -= 5;
            rg.MoveRotation(Quaternion.Euler(0, rY, 0));
        }

        endTrun();
    }

    private void endTrun()
    {
        seeX = Direction.READY;
    }
}
