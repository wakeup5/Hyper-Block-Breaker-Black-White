using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waker;

public enum ShotType
{
    Black,
    White
}

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private float shotDelay;

    [SerializeField]
    private float ballSpeed;

    [SerializeField]
    private ElectricBall white;

    [SerializeField]
    private ElectricBall black;

    private bool isShooting;

    private IEnumerator ShotRoutine(Vector2 direction, int damage, int count, ShotType type = ShotType.Black)
    {
        isShooting = true;

        Vector2 position = transform.position;

        for (int i = 0; i < count; i++)
        {
            Pool.OfBehaviour(type == ShotType.Black ? black : white)
                .ActivateOne(position, Quaternion.identity)
                .Active(direction, ballSpeed, damage);

            yield return new WaitForSeconds(shotDelay);
        }

        isShooting = false;
    }

    public IEnumerator Shot(Vector2 direction, int damage, int count, ShotType type = ShotType.Black)
    {
        if (isShooting)
        {
            Debug.Log("Aleady Shooting.");
            yield break;
        }

        yield return ShotRoutine(direction, damage, count, type);
    }
}
