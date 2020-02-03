using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugUI : MonoBehaviour
{
    public List<Worker> player;
    public List<House> house;
    public List<Text> playerText;
    public List<Text> houseText;
    public List<string> colorString;

    private void Update()
    {
        for(int i = 0; i < player.Count; i++)
        {
            if (player[i] != null)
                if (player[i].gameObject.activeSelf)
                    playerText[i].text = player[i].oreCarrying + "/" + player[i].maxOre;
                else
                    playerText[i].text = "";
            if (house[i] != null)
                if (player[i].gameObject.activeSelf)
                    houseText[i].text = "Ore : "+house[i].ore;
                else
                    houseText[i].text = "";

        }  
    }
}
