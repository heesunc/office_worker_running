using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
        X,
        Z,
        READY,
        WAIT
    }

    const int TILE = 7;
    private Direction seeX = Direction.READY;

    //move
    public float speed = 7; //move speed
    private int move = 1;
    private float playTime = 0f; //속도 올리기용. 나중에 없애기. 
    private Vector3 correctVelocity = Vector3.zero; //it move to right line using this velocity
    private float correctTime = 1f; //it move to right line while this time
    private float correctGoalX;
    private float correctGoalZ;

    //turn
    private TurnState isturn = TurnState.NONE;
    private bool go = true; //it is going now?
    private int checkCount;
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
    public CameraController camera;

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
        correctGoalX = lastX = tf.position.x;
        correctGoalZ = lastZ = tf.position.z;

        checkCount = 2;
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

        //check front at an inteval of 0.3s
        if (playTime > 0.3 * checkCount && seeX != Direction.WAIT)
        {
            checkSeeX();
            checkCount++;
            Debug.Log(checkCount);
        }

        x = tf.position.x;
        z = tf.position.z;

        //ready turn
        if (isturn != TurnState.NONE)
        {
            if (((seeX == Direction.X) && Math.Round(x) % TILE == 0) //when it going x line, check x position
                || ((seeX == Direction.Z) && Math.Round(z) % TILE == 0)) //when it gotin z line, check z position
            //if (Math.Round(x) % TILE == 0 && Math.Round(z) % TILE == 0)
            {
                correctGoalX = (float)Math.Round(x);
                correctGoalZ = (float)Math.Round(z);

                seeX = Direction.WAIT;
                checkCount += 2;
                go = false;

                if (isturn == TurnState.LEFT)
                {
                    StartCoroutine(turnL());
                }
                else //RIGHT
                {
                    StartCoroutine(turnR());
                }
                isturn = TurnState.NONE;
            }
        }

        //default move
        if (go == true)
        {
            Debug.Log(Vector3.forward * speed * move * Time.deltaTime);
            tf.Translate(Vector3.forward * speed * move * Time.deltaTime);
        }

        //speed up at an interval of 25s
        if (playTime > 25)
        {
            playTime = 0;
            checkCount = 1;
            speedUp();
        }

        playTime += Time.deltaTime;
    }

    private void correctLine() //no used
    {
        //correctLine
        if (seeX == Direction.X)
        {
            transform.position = Vector3.SmoothDamp(tf.position, new Vector3(x, tf.position.y, correctGoalZ), ref correctVelocity, correctTime);
        }
        else if (seeX == Direction.Z)
        {
            transform.position = Vector3.SmoothDamp(tf.position, new Vector3(correctGoalX, tf.position.y, z), ref correctVelocity, correctTime);
        }
    }

    private void speedUp()
    {
        speed *= 1.2f;
        camera.smoothSpeedUp();
    }

    private bool checkPlace(float x) //no used.
    {
        float xf = x - (float)Math.Floor(x);

        if (0.75 <= xf || xf <= 0.25)
            return true; //xd + 1
        else
            return false; //xd
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

    private void checkSeeX() //오차 1
    {
        Debug.Log(seeX);

        int o = 1; //오차

        if (lastX + o < tf.position.x || lastX - o > tf.position.x)
        {
            seeX = Direction.X;
        }
        else
        {
            seeX = Direction.Z;
        }

        //lastX = tf.position.x;
        //lastZ = tf.position.z;
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

    IEnumerator turnR()
    {
        int rY = (int)tf.eulerAngles.y; //if error in angle, change int to float
        int goal = turnGoal(rY); //turn

        while (rY != goal)
        {
            rY += 5; //ry값 조정. -> speed 올라가면 같이 올라가도록
            rg.MoveRotation(Quaternion.Euler(0, rY, 0));
            yield return null;
        }

        endTrun();
    }

    IEnumerator turnL() //ȸ�� �޼ҵ�.
    {
        int rY = (int)tf.eulerAngles.y;
        int goal = turnGoal(rY); //��ǥ ����

        while (rY != goal)
        {
            rY -= 5;
            rg.MoveRotation(Quaternion.Euler(0, rY, 0));
            yield return null;
        }

        endTrun();
    }

    private void endTrun()
    {
        go = true;
        lastX = tf.position.x;
        lastZ = tf.position.z;
        seeX = Direction.READY;
    }
}
