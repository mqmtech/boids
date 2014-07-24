using UnityEngine;

[RequireComponent(typeof(SquadUnitMover))]
public class SquadUnit : MonoBehaviour 
{
	public Squad Squad { get; protected set; }

	private SquadUnitMover _squadUnitMover;

	void Awake()
	{
		_squadUnitMover = GetComponent<SquadUnitMover>();
	}

	public void Init(Squad iSquad)
	{
		Squad = iSquad;
	}

	public void MoveTo(Vector3 worldPos)
	{
		_squadUnitMover.MoveTo(worldPos);
	}
}
