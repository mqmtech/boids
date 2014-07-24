using UnityEngine;
using System.Collections;

public class SquadController : MonoBehaviour 
{
	enum State
	{
		IDLE,
		SELECTED,
	}

	[SerializeField]
	Squad _selectedSquad;

	[SerializeField]
	Squad _lastTouchedSquad;

	RayToSquad _rayToSquad;
	RayToposition _rayToPosition;

	State _state;

	SquadController()
	{
		_rayToSquad = new RayToSquad();
		_rayToPosition = new RayToposition();

		_state = State.IDLE;
	}

	void Update()
	{
		switch(_state)
		{
			case State.IDLE:
				IdleState();
				break;
			case State.SELECTED:
				SelectedState();
				break;
		}
	}

	void IdleState()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Squad temp = _rayToSquad.FindSquadFromTouchedGameObject();
			if(temp !=null)
			{
				_selectedSquad = temp;

				_state = State.SELECTED;
			}
		}
	}

	void SelectedState()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Squad temp = _rayToSquad.FindSquadFromTouchedGameObject();
			if(temp !=null)
			{
				if(temp == _selectedSquad)
				{
					_selectedSquad = null;
					_state = State.IDLE;
				}
				else
				{
					_selectedSquad = temp;
				}
			}
			else
			{
				Vector3 worldPos = Vector3.zero;
				bool isOk = _rayToPosition.GetPosFromTouchedGameObject(ref worldPos);
				if(isOk)
				{
					Debug.Log("[Selected] Moving units to: " + worldPos);
					_selectedSquad.MoveTo(worldPos);
				}
			}
		}
	}
}
