using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    private Animator walk;
    private bool isRight = true;
    private bool isMove = false;
    public GameObject cam;
    public float speed = 0.5f;

    private float temp = 0f;

    public Animator Walk
    {
        get { return walk = walk ? walk : GetComponent<Animator>(); }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            isMove = true;
            Walk.SetTrigger("Run");
        }

        Debug.Log(horizontal);

        if (horizontal == 0 && isMove)
        {
            Walk.SetTrigger("Stay");
            isMove = false;
            Debug.Log("stay" + temp);
        }

        if (!isRight && horizontal > 0)
        {
            Flip();
        }
        else if (isRight && horizontal < 0)
        {
            Flip();
        }

        Vector2 moveInput = new Vector2(horizontal, 0f);
        Vector2 moveAmount = moveInput.normalized * (speed * Time.deltaTime);
        cam.transform.position += (Vector3)moveAmount;
    }

    private void Flip()
    {
        isRight = !isRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}