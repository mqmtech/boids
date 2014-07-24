using UnityEngine;

public class CheckIsMouseButtonDown : Behavior
{
	int _button = 0;

	public CheckIsMouseButtonDown(int button)
	{
		_button = button;
	}
	
	public override Status Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			_status = Status.SUCCESS;
		}
		else
		{
			_status = Status.FAILURE;
		}

		return _status;
	}
}

public class CheckIsSquadTouched : Behavior
{
	RayToSquad _rayToSquad;
	
	public CheckIsSquadTouched()
	{
		_rayToSquad = new RayToSquad();
	}

	public override Status Update()
	{
		Squad temp = _rayToSquad.FindSquadFromTouchedGameObject();

		_status = temp != null ? Status.SUCCESS : Status.FAILURE;
		return _status;
	}
}

public class SaveSquad : Behavior
{
	RayToSquad _rayToSquad;
	string _varName;
	
	public SaveSquad(string varName)
	{
		_rayToSquad = new RayToSquad();
		_varName = varName;
	}
	
	public override Status Update()
	{
		Squad temp = _rayToSquad.FindSquadFromTouchedGameObject();
		if(temp !=null)
		{
			_bt.SetMemoryObject<Squad>(_varName, temp);

			return Status.SUCCESS;
		}
		
		return Status.FAILURE;
	}
}

public class SaveTouchedPosition : Behavior
{
	RayToposition _rayToPosition;
	string _varName;

	public SaveTouchedPosition(string varName)
	{
		_rayToPosition = new RayToposition();
		_varName = varName;
	}

	public override Status Update()
	{
		Vector3 worldPos = Vector3.zero;
		bool isOk = _rayToPosition.GetPosFromTouchedGameObject(ref worldPos);
		if(isOk)
		{
			_bt.SetMemoryObject<Vector3>(_varName, worldPos);
			_status = Status.SUCCESS;
		}
		else
		{
			_status = Status.FAILURE;
		}
		
		return _status;
	}
}

public class MoveSquadToTarget : Behavior
{
	string _squadVarName;
	string _targetVarName;
	bool _isOk;

	Vector3 _targetWorldPos;
	Squad _squad;

	Timer _timerToStop = new Timer();
	
	public MoveSquadToTarget(string squadVarName, string targetVarName)
	{
		_squadVarName = squadVarName;
		_targetVarName = targetVarName;
	}

	public override void OnInitialize ()
	{
		base.OnInitialize ();

		_isOk = _bt.GetMemoryObject<Squad>(_squadVarName, out _squad);
		
		Vector3 _targetWorldPos = Vector3.zero;
		_isOk &= _bt.GetMemoryObject<Vector3>(_targetVarName, out _targetWorldPos);
		if(_isOk)
		{
			_squad.MoveTo(_targetWorldPos);
		}

		_timerToStop.Wait(2f);
	}

	public override Status Update()
	{
		if(!_isOk)
		{
			return Status.FAILURE;
		}
		/*
		Vector3 dirToTarget = _targetWorldPos - _squad.Units[0].transform.position;
		if(dirToTarget.sqrMagnitude < 2f)
		{
			return Status.SUCCESS;
		}
		*/
		if(_timerToStop.IsEnd())
		{
			return Status.SUCCESS;
		}
		else
		{
			return Status.RUNNING;
		}
	}
}


public class SavePositionFromMoveCommand : Behavior
{
	string _varName;
	string _targetVarName;
	
	public SavePositionFromMoveCommand(string commandVarName, string targetVarName)
	{
		_varName = commandVarName;
		_targetVarName = targetVarName;
	}
	
	public override Status Update()
	{
		MoveCommand command;
		bool isOk = _bt.GetMemoryObject(_varName, out command);

		if(!isOk)
		{
			return Status.FAILURE;
		}

		_bt.SetMemoryObject(_targetVarName, command.WorldPos);

		return Status.SUCCESS;
	}
}
