using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waker;

[CreateAssetMenu(menuName = "Shots/One Shot")]
public class OneShot : Shot
{
    [SerializeField]
    private ElectricBall ballOriginal = null;

    public override IEnumerator Do(Vector2 position, Vector2 direction, float speed, int damage)
    {
        Pool.OfBehaviour(ballOriginal)
            .ActivateOne(position, Quaternion.identity)
            .Active(direction, speed, damage);

        yield break;
    }
}
