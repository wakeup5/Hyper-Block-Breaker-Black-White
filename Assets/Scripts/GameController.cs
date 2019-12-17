using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Shooter shooter;

    [SerializeField]
    private BlockManager blockManager;

    private Vector2 m;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 dir = (Vector2)Input.mousePosition - m;
            dir.Normalize();

            shooter.Shot(dir, 2);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            blockManager.MoveAndGenerate();
        }
    }
}
