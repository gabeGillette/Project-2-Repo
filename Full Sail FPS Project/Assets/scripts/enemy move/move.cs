using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3.0f;

    void Update()
    {
        // Move forward continuously
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}