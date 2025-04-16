using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;        //이동 속도 변수 설정
    public float jumpForce = 7f;        //점프의 힘 값을 준다.
    public float turnSpeed = 10f;       //회전 속도

    [Header("점프 개선 설정")]
    public float fallMultipiier = 2.5f;             //하강 중력 배율
    public float lowJumpMultiplier = 2.0f;          //짧은 점프 배율


    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;                //지면 관성 시간
    public float coyoteTImeCounter;                 //관성 타이머
    public bool realGround = true;                  //실제 지면 시간

    [Header("글라이더 설정")]
    public GameObject gliderObject;                 //글라이더 오브젝트
    public float gliderFallSpeed = 1.0f;            //글라이더 낙하 속도
    public float glioerMoveSpeed = 7.0f;            //글라이더 이동 속도
    public float gliderMaxTime = 5.0f;              //최대 사용시간
    public float gliderTimeLeft;                    //남은 사용시간
    public bool isGliding = false;                  //글라이딩 중인지 여부



    public bool isGrounded = true;      //땅에 있는지 체크 하는 변수 (true/false)

    public int coinCount = 0;           //코인 획득 변수 선언
    public int totalCoins = 10;         //총 코인 획들 필요 변수 선언

    public Rigidbody rb;                //플레이어 강체를 선언
    // Start is called before the first frame update
    void Start()
    {



        //글라이더 오브젝트 초기화
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);              //시작시 비활성화
        }

        gliderTimeLeft = gliderMaxTime;                 //글라이더 시간 초기화

        coyoteTImeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지 안정화
        UpdaterGroundedState();
        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        //이동 방향 벡터
        Vector3 movement = new Vector3(moveHorizontal, 0, moverVertical);     //이동 방향 감지

        //입력이 있을 때만 회전
        if (movement.magnitude > 0.1f) //입력이 있을 때만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);     //이동 항향을 바라보도록 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);


        }

        //G키로 글라이더 제어 (누르는 동안만 활성화)
        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)            //G키를 누르면서 땅에 있지 않고 글라이더 남은 시간이 있을때 (3가지 조건)
        {
            if (!isGrounded)             //글라이더 활성화 (누르고 있는 동안)
            {
                //글라이더 활성화 함수(아래 정의)
                EnableGlider();
            }

            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 되면 비활성화
            if (gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수 (아래 정의)
                DisalbeGlider();
            }
        }
        else if (isGrounded)
        {
            //G키를 때면 글라이더 비활성화
            DisalbeGlider();
        }

        if(isGliding) //움직임 처리
        {
            //글라이더 사용중 이동
            ApplyGliderMovement(moveHorizontal, moverVertical);
        }
        else //(기존 움직임 코드들을 else문안에 넣는다.
        {
            //속도를 직접 이동

            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moverVertical * moveSpeed);

            //착시 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultipiier - 1) * Time.deltaTime; //하강시 중력 강화
            }
            else if (rb.velocity.y > 0 && Input.GetButton("Jump")) //상승 중 점프 버튼을 때면 낮게 점프
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }


        }

        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
        if(isGrounded)
        {
            if(isGliding)
            {
                DisalbeGlider();
            }

            //지상에 있을 때 시간 회복
            gliderTimeLeft = gliderMaxTime;
        }




           

      

        
        

        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)              //&& 두 값을 만족할때 -> (스페이스 버튼일 눌렸을때 와 isGrounded가 ture일때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  //위쪽으로 설정한 힘만큼 강체에 준다.
            isGrounded = false;                                      //점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다.
            realGround = false;
            coyoteTImeCounter = 0;                                   //코요테 타임 즉시 리셋
        }

    }


    //글라이더 활성화 함수
    void EnableGlider()
    {
        isGliding = true;

        //글라이더 오브젝트 표시
        if(gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
        //하강 속도 초기화
        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }
    void DisalbeGlider()
    {
        isGliding = false;

        //글라이더 오브젝트 숨기기
        if(gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        //즉시 낙하하도록 중력 적용
        rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
    }
    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertical) //수평과 수직을 함수의 인수로 받는다.
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 더 빠르게 이동
        
        Vector3 gliderVelocity = new Vector3(
            horizontal * glioerMoveSpeed,       //X측
            -gliderFallSpeed,                   //Y축
            vertical * glioerMoveSpeed          //Z축
            );

        rb.velocity = gliderVelocity;
    }











    void OnCollisionEnter(Collision collision)            //충돌 처리 함수
    {
        if (collision.gameObject.tag == "Ground")                 //충돌 일어난 물체의 Tag가 Ground 경우
        {
            isGrounded = true;                                   //땅과 충돌하면 ture로 변경한다.
        }
    }

    private void OnCollisionStay(Collision collision)     //지면과의 충돌이 유지되는지 확인(추가)
    {
        if(collision.gameObject.CompareTag("Ground"))       //충돌이 유지되는 물체의 Tag가 Ground 경우
        {
            realGround = true;                          //충돌이 유지되기 떄문에 true
        }

     
    }
    private void OnCollisionExit(Collision collision)       //지면에 떨어졌는지 확인(추가)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            realGround = false;                             //지면에 떨어졌기 때문에 false
        }
    }
    private void OnTriggerEnter(Collider other)     //트리거 영역 안에 들어왔나를 검사하는 함수
    {
        //코인 수집
        if(other.tag == "Coin")                //코인 트리거와 충돌 확인
        {
            coinCount++;                            //coinCount = coinCount + 1 코인 변수 1을 올려준다.
            Destroy(other.gameObject);              //코인 오브젝트를 없앤다.
            Debug.Log($"코인 수집: {coinCount}/{totalCoins}");

        }

        //목적지 도착 시 종료 로그 출력
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 가능
        }
    }

    //지명 상태 업데이트 함수
    void UpdaterGroundedState()
    {
        if (realGround)                 //실제 지면에 있으면 코요테 타입 리셋
        {
            coyoteTImeCounter = coyoteTime;
            isGrounded = true;

        }
        else
        {
            //실제로는 지면에 없지만 코요테 타임 내에 있으면 여전히 지면으로 판단
            if (coyoteTImeCounter > 0)
            {
                coyoteTImeCounter -= Time.deltaTime;            //시간을 지속적으로 감소 시킨다
                isGrounded = true;
            }
            else
            {
                isGrounded = false;                             //타임이 끝나면 False
            }
        }
    }

}
