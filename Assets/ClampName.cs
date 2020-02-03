using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampName : MonoBehaviour
{
    private Text nameLabel;
    public Transform transformToFollow;

    // Start is called before the first frame update
    void Start()
    {
        nameLabel = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(transformToFollow.position);
        nameLabel.transform.position = namePos;
    }
}
