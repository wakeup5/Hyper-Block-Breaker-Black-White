using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shot : ScriptableObject
{
	public abstract IEnumerator Do(Vector2 position, Vector2 direction, float speed, int damage);
}
