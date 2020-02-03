using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public List<Rock> rockPrefab;
    public Board board;
    public float spawnDuration = 5f;
    public int maxNumOfRocks = 10;
    public int rockAvailable = 1;

    public GameObject smokePrefab;

    float timer;
    [HideInInspector] public List<Rock> rocks;


    private IEnumerator SpawnRock(float duration, Vector3 position)
    {
        yield return new WaitForSeconds(duration);

        Rock rock = Instantiate(rockPrefab[(int)Random.Range(0, rockAvailable)], position - Vector3.up * 5, Quaternion.Euler(0, Random.Range(0f, 360), 0f), transform).GetComponent<Rock>();
        rock.rockSpawner = this;
        rocks.Add(rock);
    }

    private void Awake()
    {
        rocks = new List<Rock>();
        timer = 0f;

        if (board == null)
        {
            board = GetComponent<Board>();
        }
    }

    private void Update()
    {
        if (rocks.Count < maxNumOfRocks)
        {
            timer += Time.deltaTime;
            if (timer > spawnDuration)
            {
                timer -= spawnDuration;
                
                Vector3 pos = board.RandomPosition();

                GameObject go = Instantiate(smokePrefab, pos - Vector3.up * 4, Quaternion.identity);
                go.transform.SetParent(this.transform);
                Destroy(go, 1f);

                //StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.5f, 0.1f));
                StartCoroutine(SpawnRock(0.2f, pos));

                if (rocks.Count >= maxNumOfRocks) timer = 0f;
            }
        }
    }
}
