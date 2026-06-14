using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // ==========================================
    // 1. ตัวแปรตั้งค่าต่างๆ (Variables)
    // ==========================================
    [Header("ตั้งค่าเลือด")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI หลอดเลือด")]
    public Image hpBarFill;

    [Header("หน้าต่าง UI ตอนแพ้ (ลาก GameOverPanel มาใส่)")]
    public GameObject gameOverUI;

    private bool isDead = false;

    // ⭐ จุด Optimize 1: Caching (เตรียมคอมโพเนนต์ไว้ล่วงหน้า)
    // การเรียก GetComponent ตอนที่เกมกำลังเล่นอยู่จะกินทรัพยากรเครื่อง
    // เราจึงสร้างตัวแปรมารับค่าไว้ตั้งแต่เริ่มเกมเลย
    private SpriteRenderer[] allSprites;
    private PlayerController playerController;
    private Collider2D playerCollider;

    private Color color100, color80, color60, color40, color20;

    // ⭐ จุด Optimize 2: ลดขยะในหน่วยความจำ (Zero GC Allocation)
    // ปกติคำสั่ง 'new WaitForSeconds' จะสร้างขยะใน RAM ทุกครั้งที่เรียกใช้
    // เราจึงสร้างมันไว้ล่วงหน้าแค่ครั้งเดียว แล้วเรียกใช้ตัวแปรนี้แทน
    private WaitForSeconds deathWaitTime = new WaitForSeconds(0.3f);

    // ==========================================
    // 2. การเตรียมความพร้อมตอนเริ่มเกม (Start)
    // ==========================================
    void Start()
    {
        currentHealth = maxHealth;

        // ดึงคอมโพเนนต์ทั้งหมดมาเก็บไว้ในตัวแปรที่เตรียมไว้แต่แรก
        allSprites = GetComponentsInChildren<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponent<Collider2D>();

        ColorUtility.TryParseHtmlString("#e9ff69", out color100);
        ColorUtility.TryParseHtmlString("#fff56e", out color80);
        ColorUtility.TryParseHtmlString("#ffd869", out color60);
        ColorUtility.TryParseHtmlString("#ffaf4a", out color40);
        ColorUtility.TryParseHtmlString("#fb5017", out color20);

        UpdateHealthBar();
    }

    // ==========================================
    // 3. ระบบรับความเสียหายและอัปเดต UI (Take Damage)
    // ==========================================
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            // ⭐ จุด Optimize 3: ข้ามการเปลี่ยนสีถ้าตายแล้ว
            // สั่งให้อัปเดตสีเฉพาะตอนที่ยังรอดอยู่เท่านั้น เพราะถ้าตายเดี๋ยวตัวละครก็หายไปอยู่ดี
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = currentHealth / maxHealth;
        }

        HandlePlayerColor();
    }

    // ==========================================
    // 4. ระบบเปลี่ยนสีตามระดับเลือด (HandlePlayerColor)
    // ==========================================
    void HandlePlayerColor()
    {
        if (allSprites == null || allSprites.Length == 0) return;

        Color targetColor;

        // ⭐ ลดทอนโค้ดให้สั้นและอ่านง่ายขึ้น โดยไม่ต้องมีปีกกา (ละไว้ในฐานที่เข้าใจ)
        if (currentHealth <= 20f) targetColor = color20;
        else if (currentHealth <= 40f) targetColor = color40;
        else if (currentHealth <= 60f) targetColor = color60;
        else if (currentHealth <= 80f) targetColor = color80;
        else targetColor = color100;

        foreach (SpriteRenderer sprite in allSprites)
        {
            if (sprite != null) sprite.color = targetColor;
        }
    }

    // ==========================================
    // 5. ระบบการตายและคัตซีน (Death Sequence)
    // ==========================================
    void Die()
    {
        isDead = true;
        PauseMenu.isGameOver = true;

        // เคลียร์หลอดเลือดให้เป็น 0 ทันที เพื่อความสมจริงทาง UI
        if (hpBarFill != null) hpBarFill.fillAmount = 0;

        Debug.Log("Player เลือดหมดแล้ว!");
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        // ⭐ ใช้คอมโพเนนต์ที่เก็บไว้ (Caching) แทนการเรียก GetComponent ใหม่
        if (playerController != null) playerController.TriggerDeath();

        float shakeDuration = 1f;
        float elapsed = 0f;
        Vector3 originalPos = transform.position;

        while (elapsed < shakeDuration)
        {
            float x = originalPos.x + Random.Range(-0.2f, 0.2f);
            float y = originalPos.y + Random.Range(-0.2f, 0.2f);
            transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;

        foreach (SpriteRenderer sprite in allSprites)
        {
            if (sprite != null) sprite.enabled = false;
        }

        // ⭐ ใช้คอมโพเนนต์ Collider ที่เก็บไว้ (Caching) มาสั่งปิด
        if (playerCollider != null) playerCollider.enabled = false;

        // ⭐ จุด Optimize: ใช้เวลารอที่เตรียมไว้แล้วแต่ต้น (No GC)
        yield return deathWaitTime;

        if (gameOverUI != null) gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
}