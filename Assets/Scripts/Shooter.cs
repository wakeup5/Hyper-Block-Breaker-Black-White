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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
    }

    private IEnumerator ShotRoutine()
    {
        isShooting = true;

        Vector2 position = transform.position;
        Vector2 direction = transform.up;

        foreach (var shot in shots)
        {
            yield return shot.Do(position, direction, ballSpeed, 1);
            yield return new WaitForSeconds(shotDelay);
        }

        isShooting = false;
    }

    public void Shot()
    {
        if (isShooting)
        {
            Debug.Log("Aleady Shooting.");
            return;
        }

        StartCoroutine(ShotRoutine());
    }
}
