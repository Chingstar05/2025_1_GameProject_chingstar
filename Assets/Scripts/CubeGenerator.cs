using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 큐브 프리팹
    public int tptalCubes = 10; // 총 생성할 큐브 개수
    public float cubeSpacing = 1.0f; // 큐브 간격
    // Start is called before the first frame update
    void Start()
    {
        GenCube();// 함수를 호출 한다
    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position; // 스크립트가 붙은 오브젝트의 위치(X,Y,Z)

        GameObject firestCube = Instantiate(cubePrefab, myPosition, Quaternion.identity);// 첫 번째 큐브 생성 (내위치에)

        for (int i = 1; i < tptalCubes; i++) // 나머지 큐브들 생성
        { 
            Vector3 position = new Vector3(myPosition.x, myPosition.y,myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, position, Quaternion.identity);//큐브생성
        }
    }

}
