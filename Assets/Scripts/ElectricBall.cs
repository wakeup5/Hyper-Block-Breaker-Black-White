using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    public static HashSet<ElectricBall> Actives = new HashSet<ElectricBall>();
    public static event System.Action<int> OnActiveCountChanged;

    [SerializeField]
    private float maxLifeTime = 0.1f;

    [SerializeField]
    private int maxHitCount = 10;

    [SerializeField]
    private GameObject hitEffect;

    public Vector2 direction;
    public float speed;
    public int damage;

    private int collisionLayer;
    private Coroutine disableRoutine;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        collisionLayer = Physics2D.GetLayerCollisionMask(gameObject.layer);

        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        //disableRoutine = StartCoroutine(DisableOnLifeTime(maxLifeTime));

        Actives.Add(this);
        OnActiveCountChanged?.Invoke(Actives.Count);
    }

    private void OnDisable()
    {
        //StopCoroutine(disableRoutine);

        Actives.Remove(this);
        OnActiveCountChanged?.Invoke(Actives.Count);
    }
    
    private IEnumerator DisableOnLifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    private IEnumerator ActiveRoutine(Vector2 direction, int damage)
    {
        List<Vector3> positions = new List<Vector3>();

        RaycastHit2D hit;
        Vector3 dir = direction;
        Vector3 pos = transform.position;

        positions.Add(pos);

        int hitCount = 0;
        while (hitCount < maxHitCount)
        {
            hit = Physics2D.Raycast(pos, dir, float.MaxValue, collisionLayer);

            pos = hit.point + hit.normal * 0.005f;
            dir = Vector2.Reflect(dir, hit.normal);

            Instantiate(hitEffect, pos, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.zero, hit.normal)));

            Block block = hit.collider?.GetComponent<Block>();

            if (block)
            {
                block.TakeDamage(damage);
            }

            Item item = hit.collider?.GetComponent<Item>();

            if (item)
            {
                item.TakeDamage(damage);
            }

            positions.Add(pos);

            hitCount++;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, positions[positions.Count - 2]);
            lineRenderer.SetPosition(1, positions[positions.Count - 1]);

            yield return new WaitForSeconds(0.075f);

            Floor floor = hit.collider?.GetComponent<Floor>();
            if (floor != null)
            {
                floor.Hit(pos);
                break;
            }
        }
        // while (!hit.collider?.GetComponent<Floor>() && hitCount < maxHitCount);

        //lineRenderer.positionCount = positions.Count;
        //lineRenderer.SetPositions(positions.ToArray());
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, positions[positions.Count - 2]);
        lineRenderer.SetPosition(1, positions[positions.Count - 1]);

        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }

    public void Active(Vector2 direction, float speed, int damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;

        lineRenderer.positionCount = 0;
        StartCoroutine(ActiveRoutine(direction, damage));
    }
}
