using UnityEngine;

[RequireComponent(typeof(SquadUnitMover))]
public class BoidSquadUnit : BaseSquadUnit
{
	public override BaseSquad Squad { get; protected set; }

	protected SquadUnitBoidMover SquadUnitMover
	{
		get; set;
	}

	public Vector3 Velocity
	{
		get
		{
			return SquadUnitMover.Velocty;
		}
	}
	
	void Awake()
	{
		SquadUnitMover = GetComponent<SquadUnitBoidMover>();
	}
	
	public override void Init(BaseSquad iSquad)
	{
		Squad = (BoidSquad) iSquad;
	}
	
	public override void MoveTo(Vector3 worldPos)
	{
		SquadUnitMover.MoveTo(worldPos);
	}
}
