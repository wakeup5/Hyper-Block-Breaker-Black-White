using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static HashSet<Block> Actives = new HashSet<Block>();
    public static event System.Action<Block> OnBlockDestroy;

    [SerializeField]
    private TextMesh textMesh;

    [SerializeField]
    private int maxHP;

    private int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        textMesh.text = maxHP.ToString();
    }

    private void OnEnable()
    {
        Actives.Add(this);
    }

    private void OnDisable()
    {
        Actives.Remove(this);
    }

    public void Active(int maxHP)
    {
        this.maxHP = maxHP;
        currentHP = maxHP;
        textMesh.text = maxHP.ToString();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        textMesh.text = currentHP.ToString();

        if (currentHP <= 0)
        {
            OnBlockDestroy?.Invoke(this);
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}
