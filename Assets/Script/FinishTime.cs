using System.Collections;
using UnityEngine;
using TMPro;

public class FinishTime : MonoBehaviour
{
    [Header("ตั้งค่าเวลาเอาชีวิตรอด (วินาที)")]
    public float countdownTime = 60f;

    [Header("อ้างอิง UI ตัวเลข")]
    public TextMeshProUGUI timerText;

    [Header("หน้าต่าง UI ตอนชนะ (ลาก WinMenuPanel มาใส่)")]
    public GameObject winMenuUI;

    [Header("UI ที่ต้องการซ่อนตอนจบเกม (ลาก HP_Background มาใส่)")]
    public GameObject hpBarUI;

    [Header("ลากตัว Player (Triangle) มาใส่ช่องนี้")]
    public PlayerController player;

    public static bool IsGameWon = false;

    private bool isGameEnded = false;

    // ⭐ เพิ่มตัวแปรใหม่ เพื่อเช็กว่าเราได้ทำการปิด Spawner ไปหรือยัง
    private bool areSpawnersStopped = false;

    void Start()
    {
        IsGameWon = false;
        areSpawnersStopped = false; // รีเซ็ตค่าตอนเริ่มเกม
    }

    void Update()
    {
        if (isGameEnded) return;

        countdownTime -= Time.deltaTime;

        // ⭐ เช็กว่าถ้าเวลาเหลือ 1 วินาที (หรือน้อยกว่า) และยังไม่ได้ปิด Spawner ให้ทำการปิด
        if (countdownTime <= 1f && !areSpawnersStopped)
        {
            StopEnemySpawners();
        }

        if (countdownTime <= 0)
        {
            countdownTime = 0;
            WinGame();
        }

        if (timerText != null)
        {
            timerText.text = "Time: " + countdownTime.ToString("F0") + " s";
        }
    }

    // ⭐ สร้างฟังก์ชันใหม่สำหรับปิด Spawner โดยเฉพาะ
    void StopEnemySpawners()
    {
        areSpawnersStopped = true; // ล็อกไว้จะได้ไม่ทำงานซ้ำหลายรอบ

        EnemySpawner[] allSpawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in allSpawners)
        {
            spawner.gameObject.SetActive(false);
        }

        Debug.Log("เวลาเหลือ 1 วิ! หยุดการเกิดของศัตรู");
    }

    void WinGame()
    {
        isGameEnded = true;
        IsGameWon = true;

        if (timerText != null)
        {
            timerText.text = "Finish!";
        }

        // ❌ เอาโค้ดปิด Spawner ตรงนี้ออกไปแล้ว เพราะเราย้ายไปทำล่วงหน้าแล้ว ❌

        // สั่งให้ Player พุ่งออกไป
        if (player != null)
        {
            player.TriggerWinDash();
        }

        // สั่งเริ่มนับเวลารอ 3 วินาที เพื่อเปิดหน้า Game Win
        StartCoroutine(ShowWinScreenRoutine());
    }

    private IEnumerator ShowWinScreenRoutine()
    {
        yield return new WaitForSeconds(3f);

        if (hpBarUI != null)
        {
            hpBarUI.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}