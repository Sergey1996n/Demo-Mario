//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedCoefficient;
    [SerializeField] private float jumpForce;
    [SerializeField] private Text scoreText;

    private Vector2 velocity;
    private Rigidbody2D rigidbody2d;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int score;

    public bool StarPower { get; private set; }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //Vector3 position = transform.position;

        //position.x += Input.GetAxis("Horizontal") * speed;

        //transform.position = position;

        float inputAxis = Input.GetAxis("Horizontal");

        velocity = rigidbody2d.velocity;

        Vector2 leftEdge = Camera.main.ScreenToWorldPoint(Vector2.zero);

        if (leftEdge.x + 0.5f < transform.position.x)
        {
            velocity.x = inputAxis * speed;
        }
        else
        {
            velocity.x = Mathf.Max(inputAxis * speed, 0);
        }
        rigidbody2d.velocity = velocity;

        if (isGrounded)
        {
            if (inputAxis != 0)
            {
                animator.SetInteger("State", 1);
            }
            else
            {
                animator.SetInteger("State", 0);
            }
        }

        if (inputAxis < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputAxis > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
    }

    private void Jump()
    {
        animator.SetInteger("State", 2);
        rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded)
        {
            isGrounded = collision.contacts.All(c => c.point.y < transform.position.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = !collision.contacts.All(c => c.point.y > transform.position.y);
    }
    public void AddCoin(int count)
    {
        score += count;
        scoreText.text = score.ToString();
    }

    public void StarPowerActive(float duration = 5f)
    {
        StartCoroutine(StarPowerAnimation(duration));
    }

    private IEnumerator StarPowerAnimation(float duration)
    {
        StarPower = true;
        float elapsed = 0f;
        speed *= speedCoefficient;

        while (elapsed < duration)
        {
            if (Time.frameCount % 4 == 0)
            {
                spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
            elapsed += Time.deltaTime;
        }

        speed /= speedCoefficient;
        spriteRenderer.color = Color.white;
        StarPower = false;

    }
}
