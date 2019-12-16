using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waker;

[CreateAssetMenu(menuName = "Shots/One Shot")]
public class OneShot : Shot
{
    [SerializeField]
    private Ball ballOriginal = null;

    public override void Do(Vector2 position, Vector2 direction, float speed, int damage)
    {
        Ball ball = Pool.OfBehaviour(ballOriginal, 10)
            .ActivateOne(position, Quaternion.identity);

        ball.transform.position = position;
        ball.Active(direction, speed, damage);
    }
}
