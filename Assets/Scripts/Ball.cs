using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    public Vector2 direction;
    public float speed;
    public int damage;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }

    public void Active(Vector2 direction, float speed, int damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;

        rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
