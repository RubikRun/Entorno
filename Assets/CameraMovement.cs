using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    private float smoothTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the camera follow Player horizontally
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 2, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
