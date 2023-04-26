using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbody2d;
    private Vector2 direction = Vector2.left;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody2d.velocity = direction * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            bool dead = collision.contacts.All(c => c.point.y > transform.position.y);

            if (dead || player.StarPower)
            {
                Destroy(gameObject);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            bool changeDirection = !collision.contacts.All(c => c.point.y < transform.position.y);
            if (changeDirection)
            {
                direction = -direction;
                Vector2 velocity = rigidbody2d.velocity;
                velocity.x = direction.x * speed;
                rigidbody2d.velocity = velocity;
            }
        }
    }
}
