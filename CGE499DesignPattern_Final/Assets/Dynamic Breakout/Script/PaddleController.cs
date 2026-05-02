using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public Vector2 CurrentVelocity => rb.linearVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        Vector2 direction = new Vector2(xMove, 0).normalized;
        rb.linearVelocity = direction * speed;
    }
}
