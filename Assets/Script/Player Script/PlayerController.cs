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
    private bool isDead = false;

    // ⭐ ตัวแปรใหม่สำหรับรับค่าคลิกค้างจาก Update ไปใช้ใน FixedUpdate
    private bool isFlapping = false;

    // ⭐ จุด Optimize 1: สร้างตัวแปรเช็กสถานะรวม ช่วยลดความยาวของ if
    // IsActive = สถานะปกติ (ยังไม่ตาย และ ยังไม่จบเกม)
    private bool IsActive => !isWinning && !isDead;
    // CanFly = บังคับได้ (สถานะปกติ และ ไม่ติดสตัน)
    private bool CanFly => IsActive && canControl;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // ⭐ จุด Optimize 2: ย้ายการเช็ก Input มาไว้ใน Update เสมอ ป้องกันการกดไม่ติด
        isFlapping = Input.GetMouseButton(0);

        // ถ้าอยู่ในสถานะปกติ (ไม่ตาย/ไม่จบฉาก) ให้หมุนตัวได้
        if (IsActive)
        {
            HandleRotationSmooth();
        }
    }

    void FixedUpdate()
    {
        // ⭐ จุด Optimize 3: การออกแรงฟิสิกส์ยังคงต้องอยู่ใน FixedUpdate เหมือนเดิม
        if (isFlapping && CanFly)
        {
            FlyUp();
        }
    }

    void FlyUp()
    {
        // เปลี่ยนมาใช้ linearVelocity ให้เป็นมาตรฐาน Unity 6
        rb.linearVelocity = new Vector2(0, flyForce);
    }

    public void ApplyStun(float duration)
    {
        if (isDead) return;
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

        // อัปเดตมาใช้ linearVelocity เหมือนกัน
        if (rb.linearVelocity.y > 0.1f) targetAngle = baseRotationZ + tiltUpwardOffset;
        else if (rb.linearVelocity.y < -0.1f) targetAngle = baseRotationZ + tiltDownwardOffset;
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

        // ใช้ linearVelocity สำหรับพุ่งไปข้างหน้า
        rb.linearVelocity = new Vector2(5f, 0f);
    }

    public void TriggerDeath()
    {
        isDead = true;
        canControl = false;

        // ใช้ linearVelocity สำหรับหยุดนิ่ง
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }
}