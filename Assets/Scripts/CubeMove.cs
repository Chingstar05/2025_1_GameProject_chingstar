using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{public float MoveSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, -MoveSpeed * Time.deltaTime);//측 마이너스 방향으로 이동
        if (transform.position.z < -20)// 큐브가 z축으로 -20 여하로 갔는지 확인
        Destroy(gameObject);
    }
}
