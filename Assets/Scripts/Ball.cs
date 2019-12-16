using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    private float maxLifeTime = 5f;

    private new Rigidbody2D rigidbody;

    public Vector2 direction;
    public float speed;
    public int damage;

    private Coroutine disableRoutine;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        disableRoutine = StartCoroutine(DisableOnLifeTime(maxLifeTime));
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;

        StopCoroutine(disableRoutine);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }

    private IEnumerator DisableOnLifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    public void Active(Vector2 direction, float speed, int damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;

        rigidbody.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }
}
