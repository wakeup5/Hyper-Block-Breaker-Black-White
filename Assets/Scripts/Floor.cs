using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
	private bool isRecord;
	private Vector2 pos;

	public void BeginHitRecord()
	{
		isRecord = true;
	}

	public void Hit(Vector2 pos)
	{
		if (isRecord)
		{
			this.pos = pos;
			isRecord = false;
		}
	}

	public Vector2 EndHitRecord()
	{
		return pos;
	}
}
