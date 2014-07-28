using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoidSquadUnit))]
public class SquadUnitBoidMover : BaseSquadUnitMover
{
	enum State
	{
		IDLE,
		MOVING
	}

	[SerializeField]
	float _speed = 1f;

	[SerializeField]
	float stopDistance = 0.25f;
	float stopDistanceSQ;

	State _state;
	BoidSquadUnit _unit;
	Vector3 _targetPos;

	BoidSquad _squad;

	public Vector3 separationVel;

	public Vector3 Velocty
	{
		get;
		protected set;
	}

	void Awake()
	{
		stopDistanceSQ = stopDistance * stopDistance;

		_unit = GetComponent<BoidSquadUnit>();
	}

	void Start()
	{
		_squad = (BoidSquad) _unit.Squad;
		DebugUtils.Assert(_squad!=null, "_squad!=null");
	}

	public override void MoveTo(Vector3 worldPos)
	{
		_targetPos = worldPos;
		
		_state = State.MOVING;
	}

	void Update()
	{
		switch(_state)
		{
			case State.IDLE:
				IdleState();
				break;

			case State.MOVING:
				MovingState();
				break;
		}
	}

	void IdleState()
	{
		// Empty
	}

	void MovingState()
	{
		if(HasReachedEndOfPath())
		{
			_state = State.IDLE;
		}
		else
		{
			MakeStepToTarget();
		}
	}

	bool HasReachedEndOfPath()
	{
		Vector3 dirToEnd = _targetPos - transform.position;

		return dirToEnd.sqrMagnitude < stopDistanceSQ;
	}

	void MakeStepToTarget()
	{
		Vector3 centerOfMassVelocity = CalculateCenterOfMassVelocity();
		Vector3 separationVelocity = CalculateSeparation();
		separationVel = separationVelocity;
		Vector3 avgVelocity = CalculateAvgVelocity();
		Vector3 goalVelocity = CalculateGoalVelocity();

		// Calculate total velocity
		Velocty = Velocty + centerOfMassVelocity + separationVelocity + avgVelocity + goalVelocity;
		Velocty = LimitVelocity(Velocty);

		// Calculate new position from velocity
		float deltaStep = _speed * Time.deltaTime;
		Vector3 deltaPos = Velocty * deltaStep;

		// Append Position
		Vector3 nextPos = transform.position + deltaPos;
		transform.position = nextPos;
	}

	Vector3 LimitVelocity(Vector3 velocity)
	{
		return _squad.LimitVelocity(velocity);
	}

	Vector3 CalculateSeparation()
	{
		return _squad.CalculateSeparationForBoid(_unit);
	}

	Vector3 CalculateCenterOfMassVelocity()
	{
		return _squad.CalculateCenterOfMassForBoid(_unit);
	}

	Vector3 CalculateAvgVelocity()
	{
		return _squad.CalculateAvgVelocityToNearBoid(_unit);
	}

	Vector3 CalculateGoalVelocity()
	{
		return _squad.CalculateGoalVelocityForBoid(_unit);
	}
}
