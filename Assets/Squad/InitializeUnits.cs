using UnityEngine;
using System.Collections;

public class InitializeUnits : MonoBehaviour 
{
	[SerializeField]
	string _squadName;

	[SerializeField]
	int _numUnits;

	public bool _createSquad;

	SquadManager _squadManager;

	void Awake()
	{
		_squadManager = GameObject.FindObjectOfType<SquadManager>();
		DebugUtils.Assert(_squadManager!=null, "_squadManager!=null");
	}

	void Start()
	{
		StartCoroutine("CreateUnits");
	}

	IEnumerator CreateUnits()
	{
		yield return new WaitForSeconds(1f);
		BaseSquad squad = _squadManager.CreateSquad(_squadName);
		squad.CreateUnits(_numUnits);
		squad.transform.parent = transform;
	}
}
