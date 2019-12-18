using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Item powerItem;

    [SerializeField]
    private Item shotItem;

    [SerializeField]
    private Transform pos;

    [SerializeField]
    private int column = 10;

    [SerializeField]
    private float moveTime;

    [SerializeField]
    private AnimationCurve moveAnimation;

    private Vector3[] positions;

    private void Awake()
    {
        positions = new Vector3[column];

        for (int i = 0; i < column; i++)
        {
            positions[i] = pos.position + new Vector3(i - (column / 2f) + 0.5f, 0f, 0f);
        }
    }

    private IEnumerator MoveAnimation()
    {
        float currentTime = 0f;
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down;

        while (currentTime < moveTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            float t = currentTime / moveTime;
            transform.position = Vector3.Lerp(start, end, moveAnimation.Evaluate(t));
        }

        transform.position = start;
    }

    public IEnumerator MoveAndGenerate(int hp, bool createPower, bool createShots)
    {
        yield return MoveAnimation();

        // Move blocks
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.Translate(Vector3.down);
        }

        List<int?> nums = new List<int?>();
        
        for (int i = 0; i < column; i++)
        {
            int r = Random.Range(0, 10);

            if (r < blocks.Length)
            {
                nums.Add(r);
            }
            else
            {
                nums.Add(null);
            }
        }

        if (createPower && powerItem != null)
        {
            nums[0] = -1;
        }

        if (createShots && shotItem != null)
        {
            nums[1] = -2;
        }

        for (int i = 1; i < nums.Count; i++)
        {
            int n = Random.Range(1, nums.Count);
            int? t = nums[0];
            nums[0] = nums[n];
            nums[n] = t;
        }

        // Generate new blocks
        for (int i = 0; i < nums.Count; i++)
        {
            if (!nums[i].HasValue)
            {
                continue;
            }

            if (nums[i] == -1)
            {
                Instantiate(powerItem, positions[i], Quaternion.identity, transform);
            }
            else if (nums[i] == -2)
            {
                Instantiate(shotItem, positions[i], Quaternion.identity, transform);
            }
            else
            {
                Block random = blocks[nums[i].Value];
                Block b = Instantiate(random, positions[i], Quaternion.identity, transform);
                b.Active(hp);
            }
        }

        yield break;
    }

    public float MinPosition()
    {
        float min = float.MaxValue;
        for (int i = 0; i < transform.childCount; i++)
        {
            float y = transform.GetChild(i).position.y;
            if (min > y)
            {
                min = y;
            }
        }

        return min;
    }
}
