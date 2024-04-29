// using System.Collections;
// using UnityEngine;

// public class MonsterMovement : MonoBehaviour
// {
//     public Transform monster1;
//     public Transform monster2;

//     public float moveSpeed = 1f;
//     public float returnSpeed = 1f;

//     private Vector3 monster1StartPosition;
//     private Vector3 monster2StartPosition;

//     private bool isGameActive = true;

//     private void Start()
//     {
//         monster1StartPosition = new Vector3(monster1.position.x, Random.Range(3f, 9f), monster1.position.z);
//         monster2StartPosition = new Vector3(Random.Range(3f, 9f), monster2.position.y, monster2.position.z);

//         StartCoroutine(MoveVerticalMonster(monster1, 9f, 3f, monster1StartPosition, moveSpeed));
//         StartCoroutine(MoveHorizontalMonster(monster2, 7.45f, 4.75f, monster2StartPosition, moveSpeed));
//     }

//     private IEnumerator MoveVerticalMonster(Transform monster, float upperBound, float lowerBound, Vector3 originalPosition, float speed)
//     {
//         while (isGameActive)
//         {
//             float newY = Mathf.PingPong(Time.time * speed, upperBound - lowerBound) + lowerBound;
//             monster.position = new Vector3(originalPosition.x, newY, originalPosition.z);

//             yield return null;
//         }
//     }

//     private IEnumerator MoveHorizontalMonster(Transform monster, float rightBound, float leftBound, Vector3 originalPosition, float speed)
//     {
//         while (isGameActive)
//         {
//             float newX = Mathf.PingPong(Time.time * speed, rightBound - leftBound) + leftBound;
//             monster.position = new Vector3(newX, originalPosition.y, originalPosition.z);

//             yield return null;
//         }
//     }

//     public void StopMonsterMovements()
//     {
//         isGameActive = false;
//     }

    
// }

using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform monster1;
    public Transform monster2;

    public float moveSpeed = 2f;
    public float returnSpeed = 2f;

    private Vector3 monster1StartPosition;
    private Vector3 monster2StartPosition;

    private bool isGameActive = true;

    private void Start()
    {
        monster1StartPosition = monster1.position;
        monster2StartPosition = monster2.position;

        StartCoroutine(MoveVerticalMonster(monster1, 4f, monster1StartPosition, moveSpeed));
        StartCoroutine(MoveHorizontalMonster(monster2, 3f, monster2StartPosition, moveSpeed));
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

    public void StopMonsterMovements()
    {
        isGameActive = false;
    }
}

