using UnityEngine;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour 
{
	[SerializeField]
	GameObject _goSquadPrefab;

	[SerializeField]
	List<BaseSquad> _squads;

	public BaseSquad CreateSquad(string name)
	{
		GameObject goSquad = GameObject.Instantiate(_goSquadPrefab) as GameObject;
		goSquad.name = name;

		BaseSquad squad = goSquad.GetComponent<BaseSquad>();
		DebugUtils.Assert(squad!=null, "squad!=null");

		_squads.Add(squad);

		return squad;
	}
}
