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
        if (isturn != TurnState.DOING && !isJump && !isBomper) //점프 중 회전 금지 추가 && 넉백과 충돌 중일 때 회전 금지 추가
            isturn = TurnState.RIGHT;
    }

    public void oderTurnL()
    {
        if (isturn != TurnState.DOING && !isJump && !isBomper)
            isturn = TurnState.LEFT;
    }

    public void oderFront()
    {
        if(!isBomper) //넉백과 충돌 중에 앞으로 명령 금지 추가
        {
            move = 1;
        }

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
            int direction = getMove();
            playerSound.SoundPlay("Jump");
            jumpStartPos = tf.position;
            StartCoroutine(JumpProcess(direction));
        }
    }

    IEnumerator JumpProcess(int direction)
    {
        passedDistance = 0.0f;
        while(passedDistance <= maxJumpDistance && passedDistance >= -1*maxJumpDistance) //지난 거리의 절대값이 최대 점프 거리보다 작아야 함.
        {
            yield return new WaitForFixedUpdate();
            currentPos = tf.position;
            Vector3 temp = (currentPos - jumpStartPos);
            passedDistance = Vector3.Dot(temp, tf.forward); //Vector3 -> float 
            deltaJump = passedDistance / maxJumpDistance; //지난 거리 비율
            if (direction == -1) //뒤로 이동 중일 경우 x축 대칭
                deltaJump *= -1;
            currentH = (1 - Mathf.Pow((2 * deltaJump - 1), 2.0f)) * maxH; //2차 방정식 포물선
            currentH = Mathf.Max(currentH, 0.0f); //방정식 Low Bound를 0으로 설정
            if (currentH == 0) //넉백 맞고 다시 땅으로 착지했을 때
                break;
            tf.DOMoveY(currentH, 0.01f); //Dotween을 이용해 Y 좌표를 계산한 값으로 이동
        }
        tf.DOMoveY(currentH, 0.01f); //Dotween을 이용해 Y 좌표를 계산한 값으로 이동
        isJump = false;
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

    public bool doing;

    //control
    float timeAmongButton;

    //move
    public float ns; //move ns
    private int move = 1;
    private bool go = false; //it is going now?
    //private bool isMoving = false; //it is steping now?

    private float playTime = 0f; //속도 올리기용. 나중에 없애기. 
    public int timeCount;
    private float speed; 

    //turn
    private TurnState isturn = TurnState.NONE;
    private bool rotateComplete = false;
    private float speedTurn;

    //jump
    private bool isJump = false;
    private float currentH; //current Height
    public float maxH;
    public float maxJumpDistance = 14.0f;
    private Vector3 currentPos;
    private Vector3 jumpStartPos;
    private float passedDistance;
    private float deltaJump;
    //private int f = 150; //jump Force

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

    //Bomper
    public bool isBomper;

    void Start()
    {
        soundSource = GameObject.Find("PlayerSoundSource");
        if(soundSource != null)
            playerSound = soundSource.GetComponent<PlayerSound>();

        tf = GetComponent<Transform>();
        rg = GetComponent<Rigidbody>();

        startPos = new Vector3(0, 0, 0);

        ns = 0.001f;
        speedSet();
        doing = true;

        //checkCount = 2;

        //Test
        //tf.DOMove(nextPosition(), ns * 2000);
        //tf.DORotate(angleRL(), ns * 500);

        //for Test
        rotateComplete = true;
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

        //Player Main Move
        if (doing == true)
        {
            //move
            if (rotateComplete == true) 
            {
                //go = true; //회전 검사 x
                rotateComplete = false; //step 중복 x
                step(); //앞으로 가요.
            }

            //if (isMoving == true)
            //{

            //}

            //if (go == false) //앞으로 걷는 거 끝남.
            //{
            //    Debug.Log("go는 false가 됨.");
            //    rotateCheck(); //다시 회전 검사.
            //}
        }

        //ns up at an interval of 25s
        if (playTime > 30)
        {
            //motion = true;
            Debug.Log(playTime);
            playTime = 0;
            timeCount += 1;
            speedSet();
        }

        playTime += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other) //넉백이랑 충돌 중일 경우 isBomper = true
    {
        if (other.CompareTag("Bomper"))
        {
            isBomper = true;
            go = true; //회전 막고
            rotateComplete = false; //검사 새로 해야함.
            rotateOrderDelete();
        }           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bomper"))
            isBomper = false;
    }

    public void speedSet()
    {
        switch (timeCount)
        {
            case 0:
                speed = 400;
                speedTurn = 500;
                timeAmongButton = 0.02f;
                break;
            case 1:
                speed = 330;
                speedTurn = 420;
                timeAmongButton = 0.015f;
                break;
            case 2:
                speed = 270;
                speedTurn = 350;
                timeAmongButton = 0.01f;
                break;
            case -1:
                doing = false;
                break;
            default:
                speed = 200;
                speedTurn = 270;
                timeAmongButton= 0f;
                break;
        }
    }

    Tween tweenZ;
    Tween tweenX;

    private void step() //3. 구조 문제
    {
        //Position 이동이 Y 값에 영향을 미치지 않게 하도록 X와 Z로 나눠서 이동
        if(tf.forward == Vector3.forward || tf.forward == Vector3.back)
        {
            //2. Tween을 만들어서 StartCoroutine을 반복하는 것
            tweenZ = tf.DOMoveZ(nextPosition().z, ns * speed) //초기 400 최대200
                .SetEase(Ease.Linear)
                .OnComplete(stepComplete); //원하는 위치로
            //stepComplete(tweenZ);
            //StartCoroutine(stepComplete(tweenZ));
        }
        else if(tf.forward == Vector3.right || tf.forward == Vector3.left)
        {
            tweenX = tf.DOMoveX(nextPosition().x, ns * speed) //초기 400 최대200
                .SetEase(Ease.Linear)
                .OnComplete(stepComplete); //원하는 위치로
            //StartCoroutine(stepComplete(tweenX));
        }

        //go = false;

        //    target.DOMoveX(5f, 1f)
        //.SetEase(Ease.InOutSine)
        //.OnComplete(StartNextTween);
    }

    private Vector3 nextPosition() //XXXXXXXXXXXX
    {
        Vector3 point = tf.position;

        point += tf.forward * move * TILE; //연산 메소드 필요
        return point;
    }

    //IEnumerator stepComplete(Tween move) //isturn 초기화 겸 이동 완료 검사.
    //{
    //    float time = 0f;
    //    //1. while 날려보기
    //    while (time < timeAmongButton) //연타 시 오차 범위
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //    rotateOrderDelete();

    //    while(move.IsComplete() == false)
    //    {
    //        yield return null;
    //    }

    //    go = false;
    //}
    void stepComplete()
    {
        Debug.Log("step OnComplete");
        Debug.Log(tf.position);
        rotateComplete = true;
        //go = false;
        //isMoving = true;
    }

    private void rotateCheck()
    {
        Debug.Log("rotateCheck isturn: " + isturn);
        if (isturn == TurnState.NONE)
            rotateEnd();
        else if (isturn == TurnState.LEFT || isturn == TurnState.RIGHT)
        {
            tf.DORotate(angleRL(), ns * 500) //200000
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
            tf.DORotate(angleRL(), ns * 500)
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
    //    ns *= 1.2f;
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

    //private void OnCollisionEnter(Collision other) //���� ���� ���� Ȯ��.
    //{
    //    if (other.collider.CompareTag("Floor")) //��ҹ��� Ȯ��
    //    {
    //        isJump = false;
    //    }
    //}

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
    //        rY += 5; //ry값 조정. -> ns 올라가면 같이 올라가도록
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
