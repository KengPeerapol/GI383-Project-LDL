using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("ใส่ Prefab ศัตรูทั้งหมดที่นี่")]
    public GameObject[] enemyPrefabs;

    [Header("หน่วงเวลาก่อนเริ่มเกิดตัวแรก (วินาที)")]
    public float initialDelay = 1.0f;

    [Header("ระยะเวลาเกิดแบบสุ่ม (วินาที)")]
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 3.0f;

    // ⭐ สร้างตัวแปรจับเวลาขึ้นมาเอง (ประหยัด RAM กว่าการใช้ Coroutine)
    private float spawnTimer;

    void Start()
    {
        // ⭐ จุด Optimize 1: เช็ก Array ขยะ แค่ครั้งเดียวตอนเริ่มเกม
        // ถ้าไม่มี Prefab ให้สั่งปิดสคริปต์ตัวเองทิ้งไปเลย จะได้ไม่เสียเวลาคำนวณใน Update
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("ยังไม่ได้ใส่ Enemy Prefab! ปิดการทำงาน Spawner");
            enabled = false;
            return;
        }

        // ตั้งเวลาเกิดตัวแรก
        spawnTimer = initialDelay;
    }

    void Update()
    {
        // ⭐ จุด Optimize 2: ใช้ระบบนับเวลาถอยหลัง (Timer) ลดปัญหา Memory รั่ว
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnSingleEnemy();

            // สุ่มเวลารอสำหรับตัวถัดไป แล้วรีเซ็ตนาฬิกาใหม่
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnSingleEnemy()
    {
        // (ไม่ต้องเช็ก Prefab == null ตรงนี้แล้ว เพราะเราดักไว้ตั้งแต่ Start แล้ว)

        // สุ่มเลือกศัตรู 1 ตัว
        int randomIndex = Random.Range(0, enemyPrefabs.Length);

        // เสกศัตรูตามตำแหน่งของ Spawner
        Instantiate(enemyPrefabs[randomIndex], transform.position, Quaternion.identity);
    }
}