using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int ore;
    public GameObject playerTarget;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == playerTarget && other.GetComponent<Worker>().oreCarrying > 0)
        {
            Worker player = other.GetComponent<Worker>();
            ore += player.oreCarrying;
            player.oreCarrying = 0;
            GetComponent<AudioSource>().Play();
        }
    }
}
