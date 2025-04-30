using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmeraFllow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5 - 10);
    public float smoothSpeed = 0.125f;
    // Start is called before the first frame update


    private void LateUpdate()
    {
        Vector3 desiredPosition = transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;

        transform.LookAt(transform.position);
    }
}
