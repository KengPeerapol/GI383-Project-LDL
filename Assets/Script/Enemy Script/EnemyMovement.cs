using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("ความเร็วของศัตรู")]
    public float speed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float randomAngle = Random.Range(-30f, 30f);
        Vector2 moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.left;

        // เปลี่ยนจาก velocity เป็น linearVelocity
        rb.linearVelocity = moveDirection * speed;
    }

    void Update()
    {
        // เปลี่ยนจาก velocity เป็น linearVelocity
        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}