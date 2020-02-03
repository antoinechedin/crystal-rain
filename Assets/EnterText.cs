using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterText : MonoBehaviour
{
    Text textEnter;
    float baseAlphaEnter;

    // Start is called before the first frame update
    void Start()
    {
        textEnter = GetComponent<Text>();
        baseAlphaEnter = textEnter.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        textEnter.color = new Color(textEnter.color.r, textEnter.color.g, textEnter.color.b, baseAlphaEnter * (Mathf.Cos(2 * Time.time) + 1) / 2);
    }
}
