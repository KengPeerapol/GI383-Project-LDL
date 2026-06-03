using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("ใส่ Prefab ศัตรูทั้งหมดที่นี่")]
    public GameObject[] enemyPrefabs;

    [Header("ระยะเวลาเกิดต่อ 1 ตัว (วินาที)")]
    public float spawnInterval = 2.5f;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnSingleEnemy(); // เรียกฟังก์ชันเกิดศัตรูทีละตัว
            yield return new WaitForSeconds(spawnInterval); // รอเวลาตามที่กำหนด
        }
    }

    void SpawnSingleEnemy()
    {
        // ดัก Error เผื่อลืมใส่ Prefab ใน Inspector
        if (enemyPrefabs.Length == 0)
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