using UnityEngine;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour 
{
	[SerializeField]
	GameObject _goSquadPrefab;

	[SerializeField]
	List<Squad> _squads;

	public Squad CreateSquad(string name)
	{
		GameObject goSquad = GameObject.Instantiate(_goSquadPrefab) as GameObject;
		goSquad.name = name;

		Squad squad = goSquad.GetComponent<Squad>();
		DebugUtils.Assert(squad!=null, "squad!=null");

		_squads.Add(squad);

		return squad;
	}
}
