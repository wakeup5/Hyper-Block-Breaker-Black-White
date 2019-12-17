using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waker;

[CreateAssetMenu(menuName = "Shots/Multiple Shot")]
public class MultipleShot : Shot
{
    [SerializeField]
    private ElectricBall ballOriginal;

    [SerializeField]
    private int shotCount;

    [SerializeField]
    private float shotDelay;

    public override IEnumerator Do(Vector2 position, Vector2 direction, float speed, int damage)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(shotDelay);

        for (int i = 0; i < shotCount; i++)
        {
            Pool.OfBehaviour(ballOriginal)
                .ActivateOne(position, Quaternion.identity)
                .Active(direction, speed, damage);

            if (i == shotCount - 1)
            {
                // 마지막일 경우 딜레이 미적용.
                yield break;
            }

            yield return waitForSeconds;
        }
    }
}
