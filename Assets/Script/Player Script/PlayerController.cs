using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("ตั้งค่าการบิน (คลิกค้าง)")]
    public float flyForce = 5f;

    [Header("ตั้งค่าการเอียงตัว")]
    public float baseRotationZ = -90f;
    public float tiltUpwardOffset = 20f;
    public float tiltDownwardOffset = -20f;
    public float rotationSpeed = 15f;

    private Rigidbody2D rb;
    private bool canControl = true;
    private bool isWinning = false;

    // ⭐ ตัวแปรใหม่: เช็กว่าตายหรือยัง
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // ถ้าตายแล้ว หรือกำลังอยู่ในฉากจบ ไม่ต้องหมุนตัว
        if (!isWinning && !isDead)
        {
            HandleRotationSmooth();
        }
    }

    void FixedUpdate()
    {
        // จะบินได้ต้อง ควบคุมได้, ไม่ชนะ, และ ไม่ตาย
        if (Input.GetMouseButton(0) && canControl && !isWinning && !isDead)
        {
            FlyUp();
        }
    }

    void FlyUp()
    {
        rb.velocity = new Vector2(0, flyForce);
    }

    public void ApplyStun(float duration)
    {
        if (isDead) return; // ถ้าตายแล้วไม่ต้องติดสตันซ้ำ
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        canControl = false;
        yield return new WaitForSeconds(duration);
        canControl = true;
    }

    void HandleRotationSmooth()
    {
        float targetAngle;

        if (rb.velocity.y > 0.1f) targetAngle = baseRotationZ + tiltUpwardOffset;
        else if (rb.velocity.y < -0.1f) targetAngle = baseRotationZ + tiltDownwardOffset;
        else return;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void TriggerWinDash()
    {
        isWinning = true;
        canControl = false;
        rb.gravityScale = 0f;
        transform.rotation = Quaternion.Euler(0, 0, baseRotationZ);
        rb.velocity = new Vector2(5f, 0f);
    }

    // ⭐ ฟังก์ชันใหม่: เรียกใช้ตอนเลือดหมด
    public void TriggerDeath()
    {
        isDead = true;
        canControl = false;

        // หยุดการเคลื่อนที่ทั้งหมด ลอยนิ่งๆ กลางอากาศเพื่อเตรียมสั่น
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }
}