using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseSquad))]
public class BaseSquadMover : MonoBehaviour 
{
	protected BaseSquad Squad
	{
		get; set;
	}

	protected virtual void Awake()
	{
		Squad = GetComponent<BaseSquad>();
	}

	public virtual void MoveTo(Vector3 worldPos)
	{
		int numUnits = Squad.Units.Count;
		for (int i = 0; i < numUnits; ++i) 
		{
			BaseSquadUnit unit = Squad.Units[i];
			unit.MoveTo(worldPos);
		}
	}
}
