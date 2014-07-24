using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Squad))]
public class SquadMover : MonoBehaviour 
{
	Squad _squad;

	void Awake()
	{
		_squad = GetComponent<Squad>();
	}

	public void MoveTo(Vector3 worldPos)
	{
		int numUnits = _squad.Units.Count;
		for (int i = 0; i < numUnits; ++i) 
		{
			SquadUnit unit = _squad.Units[i];
			unit.MoveTo(worldPos);
		}
	}
}
