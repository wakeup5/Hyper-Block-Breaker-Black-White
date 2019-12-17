using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private List<Shot> shots;

    [SerializeField]
    private float shotDelay;

    [SerializeField]
    private float ballSpeed;

    private bool isShooting;

    public event System.Action OnShotEnd;

    private IEnumerator ShotRoutine(Vector2 direction, int damage)
    {
        isShooting = true;

        Vector2 position = transform.position;

        foreach (var shot in shots)
        {
            yield return shot.Do(position, direction, ballSpeed, 1);
            yield return new WaitForSeconds(shotDelay);
        }

        isShooting = false;
        OnShotEnd?.Invoke();
    }

    public void Shot(Vector2 direction, int damage)
    {
        if (isShooting)
        {
            Debug.Log("Aleady Shooting.");
            return;
        }

        StartCoroutine(ShotRoutine(direction, damage));
    }
}
