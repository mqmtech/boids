using UnityEngine;

[RequireComponent(typeof(SquadUnitMover))]
public class SquadUnit : BaseSquadUnit
{
	public override BaseSquad Squad { get; protected set; }

	protected BaseSquadUnitMover SquadUnitMover
	{
		get; set;
	}

	void Awake()
	{
		SquadUnitMover = GetComponent<BaseSquadUnitMover>();
	}

	public override void Init(BaseSquad iSquad)
	{
		Squad = iSquad;
	}

	public override void MoveTo(Vector3 worldPos)
	{
		SquadUnitMover.MoveTo(worldPos);
	}
}
