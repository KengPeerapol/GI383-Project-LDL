using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("ใส่ Prefab ศัตรูทั้งหมดที่นี่")]
    public GameObject[] enemyPrefabs;

    [Header("หน่วงเวลาก่อนเริ่มเกิดตัวแรก (วินาที)")]
    public float initialDelay = 1.0f;

    [Header("ระยะเวลาเกิดแบบสุ่ม (วินาที)")]
    public float minSpawnInterval = 1.0f; // เวลารอขั้นต่ำ
    public float maxSpawnInterval = 3.0f; // เวลารอสูงสุด

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        // 1. หน่วงเวลาก่อนเริ่มเสกตัวแรก (เช่น กดเล่น 1 วิค่อย spawn)
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            SpawnSingleEnemy(); // เรียกฟังก์ชันเกิดศัตรูทีละตัว

            // 2. สุ่มเวลารอสำหรับตัวถัดไป ให้อยู่ในช่วง min ถึง max
            float randomWaitTime = Random.Range(minSpawnInterval, maxSpawnInterval);

            yield return new WaitForSeconds(randomWaitTime); // รอเวลาตามที่สุ่มได้
        }
    }

    void SpawnSingleEnemy()
    {
        // ดัก Error เผื่อลืมใส่ Prefab ใน Inspector
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("ลืมใส่ Enemy Prefab ใน Inspector หรือเปล่าครับ!");
            return;
        }

        // 1. สุ่มเลือก Prefab ศัตรูมา 1 ตัวจากใน Array
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[randomIndex];

        // 2. เสกศัตรูตัวที่สุ่มได้ ออกมาที่ตำแหน่งของ Spawner เป๊ะๆ (ทีละตัว)
        Instantiate(selectedEnemy, transform.position, Quaternion.identity);
    }
}