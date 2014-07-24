using UnityEngine;
using System.Collections.Generic;

public class SquadUnitBuilder : MonoBehaviour
{
	[SerializeField]
	GameObject _unitPrefab;
	
	[SerializeField]
	int _unitsPerRow = 5;
	
	[SerializeField]
	Vector2 _unitPositionOffset = new Vector3(2f, 2f);

	public List<SquadUnit> CreateUnits(int numUnits, Squad squad)
	{
		GameObject goUnits = new GameObject("Units");
		goUnits.transform.parent = squad.transform;

		List<SquadUnit> units = new List<SquadUnit>();
		for (int i = 0; i < numUnits; ++i) 
		{
			Vector3 unitPosition = ToUnitWorldPosition(i, squad.transform.position);
			
			GameObject goUnit = GameObject.Instantiate(_unitPrefab, unitPosition, Quaternion.identity) as GameObject;
			goUnit.transform.parent = goUnits.transform;
			
			SquadUnit unit = goUnit.GetComponent<SquadUnit>();
			DebugUtils.Assert(unit!=null, "unit!=null");
			
			unit.Init(squad);
			
			units.Add(unit);
		}

		return units;
	}
	
	Vector3 ToUnitWorldPosition(int unitIdx, Vector3 squadPos)
	{
		int rowIdx = (int) unitIdx / _unitsPerRow;
		int columnIdx = unitIdx % _unitsPerRow;
		
		Vector3 pos = squadPos;
		pos.x += columnIdx * _unitPositionOffset.x;
		pos.y += 10f;
		pos.z += rowIdx * _unitPositionOffset.y;
		
		return pos;
	}
}
