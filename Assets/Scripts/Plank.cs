using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    float fallSpeed = 0.1f;
    public bool goUnderground = false;

    float speedLerp = 4f;
    public Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goUnderground)
        {
            transform.position -= Vector3.up * fallSpeed * Time.deltaTime;
        }
        else
        {
            if (transform.position.y < targetPos.y) transform.position = targetPos;
            if (transform.position.y > targetPos.y) transform.position -= speedLerp * Vector3.up;
        }
    }
}
