using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // ==========================================
    // 1. ตัวแปรตั้งค่าต่างๆ (Variables)
    // ==========================================
    [Header("ตั้งค่าเลือด")]
    public float maxHealth = 100f;     // เลือดสูงสุดตอนเริ่มเกม
    private float currentHealth;       // เลือดปัจจุบัน (แอบไว้ไม่ให้แก้ในหน้าต่าง Unity)

    [Header("UI หลอดเลือด")]
    public Image hpBarFill;            // ตัวภาพหลอดเลือดที่จะให้หดสั้นลง

    [Header("หน้าต่าง UI ตอนแพ้ (ลาก GameOverPanel มาใส่)")]
    public GameObject gameOverUI;      // หน้าต่าง Game Over สีแดงที่จะเด้งขึ้นมาตอนจบ

    private bool isDead = false;       // สวิตช์เช็กว่า "ตอนนี้ตายหรือยัง?"

    // คลังเก็บชิ้นส่วนรูปภาพทั้งหมดของตัวละคร (เอาไว้สั่งเปลี่ยนสีหรือซ่อนตัว)
    private SpriteRenderer[] allSprites;

    // ตัวแปรสำหรับเก็บข้อมูลสีต่างๆ ที่เราแปลงมาจากรหัส Hex
    private Color color100, color80, color60, color40, color20;


    // ==========================================
    // 2. การเตรียมความพร้อมตอนเริ่มเกม (Start)
    // ==========================================
    void Start()
    {
        // เติมเลือดให้เต็มตอนเริ่มเกม
        currentHealth = maxHealth;

        // ค้นหาและจดจำชิ้นส่วนรูปภาพทุกชิ้น (ทั้งสามเหลี่ยมตัวแม่ และสี่เหลี่ยมตัวลูก)
        allSprites = GetComponentsInChildren<SpriteRenderer>();

        // แปลงโค้ดสี Hex ให้กลายเป็นข้อมูลสีที่ Unity เอาไประบายได้
        ColorUtility.TryParseHtmlString("#e9ff69", out color100); // 81 - 100 (เขียวอมเหลือง)
        ColorUtility.TryParseHtmlString("#fff56e", out color80);  // 61 - 80  (เหลือง)
        ColorUtility.TryParseHtmlString("#ffd869", out color60);  // 41 - 60  (เหลืองทอง)
        ColorUtility.TryParseHtmlString("#ffaf4a", out color40);  // 21 - 40  (ส้ม)
        ColorUtility.TryParseHtmlString("#fb5017", out color20);  // 0 - 20   (ส้มแดง)

        // อัปเดตหน้าจอ UI หลอดเลือดและสีตัวละครให้เป็นค่าเริ่มต้น
        UpdateHealthBar();
    }


    // ==========================================
    // 3. ระบบรับความเสียหายและอัปเดต UI (Take Damage)
    // ==========================================
    public void TakeDamage(float damageAmount)
    {
        // ถ้าตายไปแล้ว ก็ให้หยุดการทำงานตรงนี้ไปเลย ไม่ต้องลดเลือดซ้ำ
        if (isDead) return;

        // หักเลือดออกตามจำนวนดาเมจที่โดน
        currentHealth -= damageAmount;

        // ถ้าเลือดหมด (เหลือน้อยกว่าหรือเท่ากับ 0)
        if (currentHealth <= 0)
        {
            currentHealth = 0; // ล็อกไว้ไม่ให้เลือดติดลบ
            Die();             // เรียกใช้งานคำสั่งตาย
        }

        // อัปเดตหลอดเลือดและสีให้ตรงกับเลือดที่เหลืออยู่
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // ถ้ามีการใส่หลอดเลือด UI ไว้ ให้คำนวณเปอร์เซ็นต์ (เลือดปัจจุบัน / เลือดสูงสุด)
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = currentHealth / maxHealth;
        }

        // เรียกฟังก์ชันเปลี่ยนสีตัวละคร
        HandlePlayerColor();
    }


    // ==========================================
    // 4. ระบบเปลี่ยนสีตามระดับเลือด (HandlePlayerColor)
    // ==========================================
    void HandlePlayerColor()
    {
        // ถ้าหาตัวละครไม่เจอเลย ให้ข้ามไป
        if (allSprites == null || allSprites.Length == 0) return;

        Color targetColor = color100; // ตั้งเป้าหมายสีเริ่มต้นเป็นสีเขียวปลอดภัย

        // เช็กปริมาณเลือดจากน้อยไปมาก เพื่อเลือกสีที่ถูกต้อง
        if (currentHealth <= 20f)
        {
            targetColor = color20;   // วิกฤต! เลือด 0 - 20 
        }
        else if (currentHealth <= 40f)
        {
            targetColor = color40;   // อันตรายมาก เลือด 21 - 40 
        }
        else if (currentHealth <= 60f)
        {
            targetColor = color60;   // อันตรายปานกลาง เลือด 41 - 60 
        }
        else if (currentHealth <= 80f)
        {
            targetColor = color80;   // เริ่มโดนโจมตี เลือด 61 - 80 
        }
        else
        {
            targetColor = color100;  // ปลอดภัย เลือด 81 - 100 
        }

        // วนลูปเอางานสีที่ได้ ไประบายใส่ตัวละครทุกชิ้น
        foreach (SpriteRenderer sprite in allSprites)
        {
            if (sprite != null)
            {
                sprite.color = targetColor;
            }
        }
    }


    // ==========================================
    // 5. ระบบการตายและคัตซีน (Death Sequence)
    // ==========================================
    void Die()
    {
        isDead = true; // อัปเดตสถานะให้ระบบรู้ว่า "ตายแล้วนะ"
        Debug.Log("Player เลือดหมดแล้ว!");

        // สั่งเปิดระบบคัตซีนตาย (ระบบทำงานแบบหน่วงเวลา หรือ Coroutine)
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        // ขั้นที่ 1: สั่งให้ PlayerController ล็อกการเคลื่อนไหว ไม่ให้ผู้เล่นกดบินต่อได้
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null) controller.TriggerDeath();

        // ขั้นที่ 2: เอฟเฟกต์สั่น (Shake Effect) เป็นเวลา 1 วินาที
        float shakeDuration = 1f;
        float elapsed = 0f;
        Vector3 originalPos = transform.position; // จำตำแหน่งเดิมไว้ก่อนสั่น

        while (elapsed < shakeDuration)
        {
            // สุ่มขยับแกน X และ Y แบบรวดเร็ว ทำให้ดูเหมือนการสั่นสะเทือน
            float x = originalPos.x + Random.Range(-0.2f, 0.2f);
            float y = originalPos.y + Random.Range(-0.2f, 0.2f);
            transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime; // นับเวลาเดินหน้า
            yield return null;         // รอให้เฟรมนี้จบ แล้วค่อยกลับไปสั่นใหม่ในเฟรมหน้า
        }

        transform.position = originalPos; // ดึงตัวละครกลับมาตำแหน่งเดิมให้เรียบร้อย

        // ขั้นที่ 3: ระเบิดหายไป (ซ่อนรูปภาพและปิดกล่องชน แทนการใช้ SetActive(false) เพื่อไม่ให้สคริปต์นี้หยุดทำงาน)
        foreach (SpriteRenderer sprite in allSprites)
        {
            if (sprite != null) sprite.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;

        // ขั้นที่ 4: หยุดรอให้ฉากว่างเปล่าอีก 2 วินาที (เพื่อให้ผู้เล่นซึมซับการตาย)
        yield return new WaitForSeconds(0.3f);

        // ขั้นที่ 5: เด้งหน้าจอ Game Over ขึ้นมา
        if (gameOverUI != null) gameOverUI.SetActive(true);

        // ขั้นที่ 6: หยุดเวลาทั้งหมดในเกมอย่างสมบูรณ์
        Time.timeScale = 0f;
    }
}