using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseSquadUnit))]
public class SquadUnitMover : BaseSquadUnitMover 
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
	BaseSquadUnit _unit;
	Vector3 _targetPos;

	void Awake()
	{
		stopDistanceSQ = stopDistance * stopDistance;

		_unit = GetComponent<BaseSquadUnit>();
	}

	public virtual void MoveTo(Vector3 worldPos)
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
		float deltaStep = _speed * Time.deltaTime;
		Vector3 nextPos = Vector3.MoveTowards(transform.position, _targetPos, deltaStep);
		
		transform.position = nextPos;
	}
}
