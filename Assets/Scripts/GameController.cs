using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 0.0f;
    public GameObject MonsterGo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;
        if (GameTimer <= 0)
        {
            GameTimer = 3.0f;

            GameObject Temp = Instantiate(MonsterGo);
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);

        }
        if (Input.GetMouseButtonDown(0))// 마우스 버튼을 누르면
        {
            RaycastHit hit; //Ray 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// 카메라에서 레이를 쏴서 검출한다.

            if (Physics.Raycast(ray, out hit)) // Hit 된 오브젝트를 검출한다.
            {
                if (hit.collider != null) //Hit 된 오브젝트가 있을경우
                {
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(50);
                }
            }
        }
    }
}