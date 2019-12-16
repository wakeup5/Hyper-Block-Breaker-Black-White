using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waker;

[CreateAssetMenu(menuName = "Shots/Scatter Shot")]
public class ScatterShot : Shot
{
    [SerializeField]
    private Ball ballOriginal;

    [SerializeField]
    private int ballCount;

    [SerializeField]
    private bool isRandomAngle;

    [SerializeField]
    private float angle;

    public override IEnumerator Do(Vector2 position, Vector2 direction, float speed, int damage)
    {
        float t = (ballCount - 1) / 2f;

        for (int i = 0; i < ballCount; i++)
        {
            float a;
            if (isRandomAngle)
            {
                a = Random.Range(-angle / 2, angle / 2);
            }
            else
            {
                a = (i - t) * (angle / ballCount);
            }
            Vector2 dir = Quaternion.Euler(0f, 0f, a) * direction;

            Pool.OfBehaviour(ballOriginal)
                .ActivateOne(position, Quaternion.identity)
                .Active(dir, speed, damage);
        }

        yield break;
    }
}
