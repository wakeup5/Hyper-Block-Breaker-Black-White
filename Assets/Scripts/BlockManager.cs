using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform pos;

    [SerializeField]
    private int column = 10;

    private Vector3[] positions;

    private void Awake()
    {
        positions = new Vector3[column];

        for (int i = 0; i < column; i++)
        {
            positions[i] = pos.position + new Vector3(i - (column / 2f) + 0.5f, 0f, 0f);
        }

        MoveAndGenerate();
    }

    public void MoveAndGenerate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.Translate(Vector3.down);
        }

        for (int i = 0; i < column; i++)
        {
            int r = Random.Range(0, 5);
            if (r < 2)
            {
                Block random = blocks[r];
                Instantiate(random, positions[i], Quaternion.identity, transform);
            }
        }
    }
}
