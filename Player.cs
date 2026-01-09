using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float acele = 5f;
    public float maxSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         //Debug.Log("velocidade " + rb.linearVelocity.x);
        float moveX = Input.GetAxis("Horizontal");
        rb.AddForce(new Vector2(moveX * acele, 0f));
        if (Mathf.Abs( rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x)*maxSpeed, rb.linearVelocity.y);
        }
    }
}
