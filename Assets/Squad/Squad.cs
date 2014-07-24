using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SquadUnitBuilder), typeof(SquadMover), typeof(BaseSquadAI))]
public class Squad : MonoBehaviour
{
	[SerializeField]
	List<SquadUnit> _units;
	public List<SquadUnit> Units
	{
		get{ return _units; }
	}

	SquadUnitBuilder _squadUnitBuilder;
	SquadMover _squadMover;
	BaseSquadAI _squadAI;

	void Awake()
	{
		_squadUnitBuilder = GetComponent<SquadUnitBuilder>();
		_squadMover = GetComponent<SquadMover>();
		_squadAI = GetComponent<BaseSquadAI>();
	}

	public void CreateUnits(int numUnits)
	{
		_units = _squadUnitBuilder.CreateUnits(numUnits, this);
	}

	public void MoveTo(Vector3 worldPos)
	{
		_squadMover.MoveTo(worldPos);
	}

	public void SetMemoryObject<T>(string memoryVarName, T obj)
	{
		_squadAI.SetMemoryObject(memoryVarName, obj);
	}
}
