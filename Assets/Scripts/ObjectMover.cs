using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private readonly float zDestroy = -10;

    void Update()
    {
        if (GameManager.IsGameActive)
        {
            Move();
        }
    }

    void Move()
    {
        transform.Translate(GameManager.CharacterSpeed * Time.deltaTime * Vector3.back);

        if (transform.position.z < zDestroy)
        {
            if (gameObject.CompareTag("Obstacle") || gameObject.CompareTag("Star") || gameObject.CompareTag("Power Icon"))
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Star") || other.gameObject.CompareTag("Power Icon"))
        {
            Destroy(gameObject);
        }
    }
}