using UnityEngine;

public class Metal : MonoBehaviour
{
    public bool isGrapped=false;
    public float maxSpeed = 5f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("velocidade " + rb.linearVelocity.x);
        if (Mathf.Abs( rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x)*maxSpeed, rb.linearVelocity.y);
        }
    }
}
