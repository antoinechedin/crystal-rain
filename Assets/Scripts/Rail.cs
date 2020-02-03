using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public Vector3 targetScale;
    float speedLerp = 0.8f;
    float fallSpeed = 0.1f;
    public bool goUnderground = false;
    
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
            if (transform.localScale.z > targetScale.z) transform.localScale = targetScale;
            if (transform.localScale.z < targetScale.z) transform.localScale += speedLerp * Vector3.forward;
        }
    }
}
