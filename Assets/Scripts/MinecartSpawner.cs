using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartSpawner : MonoBehaviour
{
    public Minecart minecartPrefab;
    public Rail railPrefab;
    public Plank plankPrefab;

    float baseHeight = 50;
    float offsetHeight = 25;

    public Board board;
    public float spawnSpeed = 0.2f;
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f / spawnSpeed)
        {
            timer -= 1f / spawnSpeed;
            Vector3[] minecartPositions = board.RandomBorderPositions();
            Minecart minecart = Instantiate(minecartPrefab, minecartPositions[0] + new Vector3(0, 1.52f, 0), Quaternion.identity, transform);
            minecart.target = minecartPositions[1] + new Vector3(0, 1.52f, 0);
            minecart.transform.LookAt(minecartPositions[1] + new Vector3(0, 1.52f, 0));
            minecart.transform.position += 50f * Vector3.up;

            for (int i = 0; i <= (minecartPositions[1] - minecartPositions[0]).magnitude+2; i++)
            {
                Plank plank1 = Instantiate(plankPrefab, minecartPositions[0] -1f * (minecartPositions[1] - minecartPositions[0]).normalized + (minecartPositions[1] - minecartPositions[0]).normalized * i, Quaternion.identity);
                plank1.targetPos = minecartPositions[0] - 1f * (minecartPositions[1] - minecartPositions[0]).normalized + (minecartPositions[1] - minecartPositions[0]).normalized * i;
                plank1.transform.LookAt(minecartPositions[1]);
                plank1.transform.localScale = new Vector3(1.5f, 2f, 2.7f);
                plank1.transform.position += baseHeight * Vector3.up + i * offsetHeight * Vector3.up;
                plank1.transform.SetParent(transform);
                minecart.listPlank.Add(plank1);

                Plank plank2 = Instantiate(plankPrefab, minecartPositions[0] - 0.5f * (minecartPositions[1] - minecartPositions[0]).normalized + (minecartPositions[1] - minecartPositions[0]).normalized * i, Quaternion.identity);
                plank2.targetPos = minecartPositions[0] - 0.5f * (minecartPositions[1] - minecartPositions[0]).normalized + (minecartPositions[1] - minecartPositions[0]).normalized * i;
                plank2.transform.LookAt(minecartPositions[1]);
                plank2.transform.localScale = new Vector3(1.5f, 2f, 2.7f);
                plank2.transform.position += baseHeight * Vector3.up + (i+0.5f) * offsetHeight * Vector3.up;
                plank2.transform.SetParent(transform);
                minecart.listPlank.Add(plank2);
            }

            Rail rail1 = Instantiate(railPrefab, minecartPositions[0] - 0.46f * (Vector3.Cross((minecartPositions[1] - minecartPositions[0]).normalized, Vector3.up)) - 1.5f * (minecartPositions[1] - minecartPositions[0]).normalized, Quaternion.identity);
            rail1.transform.LookAt(minecartPositions[1] - 0.46f * (Vector3.Cross((minecartPositions[1] - minecartPositions[0]).normalized, Vector3.up)));
            rail1.transform.localScale = new Vector3(1, 1, 0.1f);
            rail1.targetScale = new Vector3(1, 1, 1.5f * (minecartPositions[1] - minecartPositions[0]).magnitude + 4.5f);
            rail1.transform.SetParent(transform);
            minecart.rail1 = rail1;

            Rail rail2 = Instantiate(railPrefab, minecartPositions[0] + 0.46f * (Vector3.Cross((minecartPositions[1] - minecartPositions[0]).normalized, Vector3.up)) - 1.5f * (minecartPositions[1] - minecartPositions[0]).normalized, Quaternion.identity);
            rail2.transform.LookAt(minecartPositions[1] + 0.46f * (Vector3.Cross((minecartPositions[1] - minecartPositions[0]).normalized, Vector3.up)));
            rail2.transform.localScale = new Vector3(1, 1, 0.1f);
            rail2.targetScale = new Vector3(1, 1, 1.5f * (minecartPositions[1] - minecartPositions[0]).magnitude + 4.5f);
            rail2.transform.SetParent(transform);
            minecart.rail2 = rail2;
        }
    }
}
