using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;
    public float JumpForce = 7f;
    public float turnSpeed = 10; //회전 속도 
    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;// 하강 중력 배율
    public float lowJumpMultiplier = 2.0f;// 짧은 점프 배율
    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrounded = true;
    [Header("글라이더  설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;
    public int coinCount = 0;
    public int totalCoins = 5;
    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);

        }
        gliderTimeLeft = gliderMaxTime;

        
        
        
        
        
        coyoteTimeCounter = 0;




    }

    // Update is called once per frame
    void Update()
    {


        //지면 감지 안정화
        UpdateGroundedState();


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //이동 방향 백터 
        Vector3 movement = new Vector3(moveHorizontal, 0 , moveVertical); // 이동 방향 감지 


        // 입력이 있을 때만 회전
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        }
        //G키로 글라이더 제어 (누르는 동안만 활성화)
        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if (!isGliding)
            {
                EnableGlider();
            }
            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;
            //글라이더 시간이 다 되면 비활성화

            if (gliderTimeLeft <= 0)
            {
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            DisableGlider();



           
        }
        if (isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {

            //속도로 직접 이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);
            //착시 점프 논리 구현
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;  //하강 시 중력 강화
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))  //상승 중 점프 버튼을 떼면 낮게 점프
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

            }
            //점프 입력
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                isGrounded = false;
                realGrounded = false;
                coyoteTimeCounter = 0;
            }
        
            if (!isGrounded)
            {
               if (isGliding)
                {
                    DisableGlider();
                }

                gliderTimeLeft = gliderMaxTime;

            }
            









        }


    }

    void EnableGlider()
    {
        isGliding = true;

        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
    
        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);












    }
    void DisableGlider()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, -gliderFallSpeed, vertical * gliderMoveSpeed);
        rb.velocity = gliderVelocity;
        
    }






    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = true;
        }
    }

    void OnCollisionSyay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = true;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집: { coinCount} / {totalCoins }");
        
        }

        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {


            Debug.Log("게임 클리어");

        }
    }
    void UpdateGroundedState()
    {
        if (realGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;

            }
            else

            {
                isGrounded = false;
            }






        }   
        
        

    }


























}






