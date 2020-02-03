using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool started = false;
    public float startTime;

    public float intervalNewMiner = 5f;

    public RockSpawner rockSpawner;
    public MinecartSpawner minecartSpawner;
    public List<GameObject> dwarfInstances;
    public List<House> houseOre;

    public int dwarfAvailable = 1;

    public GameObject StartText;
    public GameObject EndText;

    public bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = dwarfAvailable; i < dwarfInstances.Count; i++)
        {
            dwarfInstances[i].SetActive(false);
        }
        dwarfInstances[0].GetComponent<Worker>().enabled = false;
        rockSpawner = FindObjectOfType<RockSpawner>();
        minecartSpawner = FindObjectOfType<MinecartSpawner>();
        rockSpawner.enabled = false;
        minecartSpawner.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if(!started && Input.GetKey(KeyCode.Return))
        {
            started = true;
            startTime = Time.time;
            StartText.SetActive(false);
            dwarfInstances[0].GetComponent<Worker>().enabled = true;

            rockSpawner.enabled = true;
            minecartSpawner.enabled = true;
        }

        if (started && (Time.time - startTime) > intervalNewMiner && rockSpawner.rockAvailable < 4)
        {
            startTime = Time.time;
            rockSpawner.rockAvailable++;
            dwarfInstances[rockSpawner.rockAvailable - 1].SetActive(true);
        }

        if(end)
        {
            int totalOre = 0;
            foreach(House h in houseOre)
            {
                totalOre += h.ore;
            }

            EndText.SetActive(true);
            EndText.GetComponent<FadeIn>().setTextOre(totalOre);
            rockSpawner.enabled = false;
            minecartSpawner.enabled = false;
            for (int i = 0; i < dwarfInstances.Count; i++)
            {
                dwarfInstances[i].GetComponent<Animator>().SetBool("idle", true);
                dwarfInstances[i].GetComponent<Animator>().SetBool("mining", false);
                dwarfInstances[i].GetComponent<Animator>().SetBool("walking", false);
                dwarfInstances[i].GetComponent<Worker>().enabled = false;
                dwarfInstances[i].GetComponent<AudioSource>().Stop();
            }

        }
            
    }
}
