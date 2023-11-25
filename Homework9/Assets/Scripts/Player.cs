using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float jumpForce;
    public bool isJumping = false;
    public GameManager gm;
    public Vector2 spawnPoint;
    public Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& !isJumping)
        {
            Debug.Log("jump");
            rb.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);
            isJumping = true;
        }
        Move();
        Death();
    }
    
    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"),0f);
        Vector2 moveAmount = moveInput.normalized * speed*Time.deltaTime;
        transform.position += (Vector3)moveAmount;
        cam.transform.position += (Vector3)moveAmount;
    }

    private void Death()
    {
        if (transform.position.y < -15)
        {
            transform.position = spawnPoint;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Wall")&&collision.contactCount>1)
        {
            isJumping = false;
        }
    }
}