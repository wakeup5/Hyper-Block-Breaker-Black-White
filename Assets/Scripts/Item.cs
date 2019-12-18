using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    IncreasePower,
    IncreaseShots
}

public class Item : MonoBehaviour
{
    [SerializeField]
    public ItemType type;

    [SerializeField]
    public int value;

    public static event System.Action<Item> OnHit;

    public void TakeDamage(int damage)
    {
        OnHit?.Invoke(this);
        Destroy(gameObject);
    }
}
