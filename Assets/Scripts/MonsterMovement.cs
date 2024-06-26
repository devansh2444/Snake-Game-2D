using System.Collections;
using UnityEngine;
using System;

public class MonsterMovement : MonoBehaviour
{
    public Transform monster1;
    public Transform monster2;
    public Transform monster3;
    public GameObject monsterObject3;

    public float moveSpeed = 2f;
    public float returnSpeed = 2f;
    public float circleRadius = 5f;

    private Vector3 monster1StartPosition;
    private Vector3 monster2StartPosition;
    private Vector3 monster3StartPosition;

    private bool isGameActive = true;
    private String difficulty;
    
    private void Start()
    {
        difficulty = PlayerPrefs.GetString("Difficulty");

        if (difficulty == "Medium")
        {
            monster1StartPosition = GetRandomPosition();
            // monster2StartPosition = GetRandomPosition();
        }
        else if(difficulty == "Hard"){
            monsterObject3.SetActive(true);
            monster1StartPosition = monster1.position;
            monster3StartPosition = monster3.position;
            moveSpeed = 3f;
        }
        else
        {
            monster1StartPosition = monster1.position;
        }

        monster2StartPosition = monster2.position;

        monster1.position = monster1StartPosition;
        monster2.position = monster2StartPosition;
        monster3.position = monster3StartPosition;

        StartCoroutine(MoveVerticalMonster(monster1, 4f, monster1StartPosition, moveSpeed));

        if (difficulty == "Medium")
        {
            StartCoroutine(MoveInCircle(monster2, circleRadius, monster2StartPosition, moveSpeed));
        }
        else if( difficulty == "Hard"){
            StartCoroutine(MoveInCircle(monster2, circleRadius, monster2StartPosition, moveSpeed));
            StartCoroutine(MoveVerticalMonster(monster1, 4f, monster1StartPosition, moveSpeed));
            StartCoroutine(MoveHorizontalMonster(monster3, 5f, monster3StartPosition, moveSpeed));
        }
        else
        {
            StartCoroutine(MoveHorizontalMonster(monster2, 3f, monster2StartPosition, moveSpeed));
        }
        
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = UnityEngine.Random.Range(-15f, 12f);
        float randomY = UnityEngine.Random.Range(-6f, 6f);
        return new Vector3(randomX, randomY);
    }

    private IEnumerator MoveVerticalMonster(Transform monster, float range, Vector3 originalPosition, float speed)
    {
        float startY = originalPosition.y;
        while (isGameActive)
        {
            float newY = startY + Mathf.Sin(Time.time * speed) * range;
            monster.position = new Vector3(originalPosition.x, newY, originalPosition.z);

            yield return null;
        }
    }

    private IEnumerator MoveHorizontalMonster(Transform monster, float range, Vector3 originalPosition, float speed)
    {
        float startX = originalPosition.x;
        while (isGameActive)
        {
            float newX = startX + Mathf.Sin(Time.time * speed) * range;
            monster.position = new Vector3(newX, originalPosition.y, originalPosition.z);

            yield return null;
        }
    }

    private IEnumerator MoveInCircle(Transform monster, float radius, Vector3 originalPosition, float speed)
    {
        float angle = 0f;
        while (isGameActive)
        {
            angle += Time.deltaTime * speed;
            float newX = originalPosition.x + Mathf.Cos(angle) * radius;
            float newy = originalPosition.y + Mathf.Sin(angle) * radius;
            monster.position = new Vector3(newX, newy);

            yield return null;
        }
    }

    public void StopMonsterMovements()
    {
        isGameActive = false;
    }
}

