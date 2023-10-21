using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    private float startPosition;
    private float length;
    public float speed;
    public GameObject cam;

    private void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = cam.transform.position.x * (1 - speed);
        float distance = cam.transform.position.x * speed;
        transform.position = new Vector3(startPosition + distance, transform.position.y,
            transform.position.z);

        if (temp > startPosition + length)
        {
            startPosition += length;
        }
        else if(temp<startPosition-length)
        {
            startPosition -= length;
        }
    }
}