using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    // 0 Red
    // 1 Blue
    // 2 Green
    // 3 Orange

    public GameObject target;
    public float moveSpeed = 5f;
    public float thresholdToTravel = 1f;
    public string myColor;

    public GameObject miningPrefab;
    private GameObject miningSound;

    public float miningSpeed = 1;
    float miningTimer;
    public int oreCarrying = 0;
    public int maxOre = 6;

    WorkerState state;
    float distanceToTarget;
    List<Rock> nearRocks = new List<Rock>();

    private void TravelTo(Vector3 targetPosition)
    {
        Vector3 moveVector = (targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetPosition) < moveVector.magnitude)
        {
            transform.position = targetPosition;
            state = WorkerState.Waiting;
        }
        else
        {
            transform.position += moveVector;
            transform.LookAt(targetPosition);
        }
    }

    private void Mine(Rock rock)
    {
        miningTimer += Time.deltaTime;
        if (miningTimer > 1f / miningSpeed)
        {
            oreCarrying++;
            rock.currentOre--;

            if (rock.currentOre > 0)
            {
                if (oreCarrying >= maxOre)
                {
                    state = WorkerState.Waiting;
                    miningTimer = 0;
                }
                else miningTimer -= 1f / miningSpeed;
            }
            else
            {
                state = WorkerState.Waiting;
                miningTimer = 0;

                Destroy(miningSound, 0.5f);
                nearRocks.Remove(rock);
                Destroy(rock.gameObject);
            }
        }
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        switch (state)
        {
            case WorkerState.Waiting:
                if (GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Stop();
                }

                if (miningSound != null)
                {
                    Destroy(miningSound, 0.5f);
                }
                GetComponent<Animator>().SetBool("walking", false);
                GetComponent<Animator>().SetBool("idle", true);
                GetComponent<Animator>().SetBool("mining", false);
                if (distanceToTarget > thresholdToTravel)
                {
                    state = WorkerState.Traveling;
                    TravelTo(target.transform.position);
                }

                if (oreCarrying < maxOre && nearRocks.Count > 0)
                {
                    state = WorkerState.Mining;
                    transform.LookAt(new Vector3(nearRocks[0].transform.position.x, 0, nearRocks[0].transform.position.z));
                    Mine(nearRocks[0]);
                }
                break;

            case WorkerState.Traveling:
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Play();
                }

                if (miningSound != null)
                {
                    Destroy(miningSound, 0.5f);
                }
                GetComponent<Animator>().SetBool("walking", true);
                GetComponent<Animator>().SetBool("idle", false);
                GetComponent<Animator>().SetBool("mining", false);
                TravelTo(target.transform.position);
                break;

            case WorkerState.Mining:
                if (GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Stop();
                }

                if (miningSound == null)
                {
                    GameObject go = Instantiate(miningPrefab, transform);
                    miningSound = go;
                }
                GetComponent<Animator>().SetBool("walking", false);
                GetComponent<Animator>().SetBool("idle", false);
                GetComponent<Animator>().SetBool("mining", true);
                if (distanceToTarget > thresholdToTravel)
                {
                    miningTimer = 0;
                    state = WorkerState.Traveling;
                    TravelTo(target.transform.position);

                }
                if (nearRocks.Count > 0)
                {
                    Mine(nearRocks[0]);
                }
                else state = WorkerState.Waiting;

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(myColor)) nearRocks.Add(other.GetComponent<Rock>());
    }

    private void OnTriggerExit(Collider other)
    {
        nearRocks.Remove(other.GetComponent<Rock>());
    }
}

public enum WorkerState
{
    Waiting,
    Traveling,
    Mining,
}
