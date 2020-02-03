using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    public float moveSpeed = 5;
    public float fallSpeed = 0.4f;
    public Vector3 target;

    public GameObject explosionPrefab;
    public GameObject impact;
    public GameObject rollingPrefab;

    private GameObject rollingSound;

    public List<Plank> listPlank;
    public Rail rail1;
    public Rail rail2;

    private bool explode = false;

    private void Update()
    {
        if (transform.position.y < 1.52f)
        {
            GameObject go = Instantiate(rollingPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            rollingSound = go;
            go.transform.SetParent(this.transform);

            GameObject go2 = Instantiate(impact, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(go2, 2f);
            go2.transform.SetParent(this.transform);
            transform.position = new Vector3(transform.position.x, 1.52f, transform.position.z);
        }
        if (transform.position.y > 1.52f) transform.position -= fallSpeed * Vector3.up;

        if (transform.position == target && !explode)
        {
            explode = true;

            foreach (Plank p in listPlank)
            {
                p.goUnderground = true;
                Destroy(p.gameObject, 2f);
            }
            rail1.goUnderground = true;
            rail2.goUnderground = true;
            Destroy(rail1.gameObject, 2f);
            Destroy(rail2.gameObject, 2f);

            StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.5f, 0.3f));
            GameObject go = Instantiate(explosionPrefab, new Vector3(transform.position.x, 0, transform.position.z) , Quaternion.identity);
            Destroy(go, 1f);

            Destroy(rollingSound);

            Destroy(gameObject, 0.55f);
        }
        else if (transform.position.y == 1.52f) TravelTo(target);
    }

    private void TravelTo(Vector3 targetPosition)
    {
        Vector3 moveVector = (targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetPosition) < moveVector.magnitude)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position += moveVector;
            transform.LookAt(targetPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().end = true;
            explode = true;

            foreach (Plank p in listPlank)
            {
                p.goUnderground = true;
                Destroy(p.gameObject, 2f);
            }
            rail1.goUnderground = true;
            rail2.goUnderground = true;
            Destroy(rail1.gameObject, 2f);
            Destroy(rail2.gameObject, 2f);

            StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.5f, 0.3f));
            GameObject go = Instantiate(explosionPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(go, 1f);

            Destroy(rollingSound);

            Destroy(gameObject, 0.55f);
        }
    }
}
