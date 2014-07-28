using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SquadUnitBuilder), typeof(BaseSquadMover), typeof(BaseSquadAI))]
public abstract class BaseSquad : MonoBehaviour
{
	public abstract List<BaseSquadUnit> Units
	{
		get; protected set;
	}

	protected SquadUnitBuilder SquadUnitBuilder
	{
		get; set;
	}
	protected BaseSquadMover SquadMover
	{
		get; set;
	}
	protected BaseSquadAI SquadAI
	{
		get; set;
	}

	protected virtual void Awake()
	{
		SquadUnitBuilder = GetComponent<SquadUnitBuilder>();
		SquadMover = GetComponent<BaseSquadMover>();
		SquadAI = GetComponent<BaseSquadAI>();
	}

	public abstract void CreateUnits(int numUnits);

	public virtual void MoveTo(Vector3 worldPos)
	{
		SquadMover.MoveTo(worldPos);
	}

	public virtual void SetMemoryObject<T>(string memoryVarName, T obj)
	{
		SquadAI.SetMemoryObject(memoryVarName, obj);
	}
}
