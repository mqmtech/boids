using UnityEngine;
using System.Collections.Generic;

public class BehaviorTree
{
	Behavior _root;
	Behavior.Status _status;

	Dictionary<string, System.Object> _memory = new Dictionary<string, object>();
	public Dictionary<string, System.Object> Memory
	{
		get{ return _memory; }
	}

	public bool GetMemoryObject<T>(string name, out T oVar)
	{
		oVar = default(T);
		System.Object obj;
		bool isOk = _memory.TryGetValue(name, out obj);
		isOk &= obj is T;

		if(isOk)
		{
			oVar = (T) obj;
		}

		return isOk;
	}

	public void SetMemoryObject<T>(string name, T oVar)
	{
		if (ContainsMemoryObject(name)) 
		{
			RemoveMemoryObject(name);
		}

		_memory.Add(name, oVar);
	}

	public bool RemoveMemoryObject(string name)
	{
		if (ContainsMemoryObject(name)) 
		{
			_memory.Remove(name);

			return true;
		}

		return false;
	}

	public bool ContainsMemoryObject(string name)
	{
		return _memory.ContainsKey(name);
	}

	public void Tick()
	{
		if(_status != Behavior.Status.RUNNING)
		{
			_root.OnInitialize();
		}

		_status = _root.Tick();
	}

	public void Init(Behavior iBehavior)
	{
		_status = Behavior.Status.INVALID;
		_root = iBehavior;

		_root.OnInit(this);
	}

	public Behavior GetRootBehavior()
	{
		return _root;
	}
}

public abstract class Behavior
{
	public enum Status
	{
		INVALID,
		FAILURE,
		SUCCESS,
		RUNNING
	}

	protected Status _status;
	public Status BehaviorStatus
	{ 
		get{ return _status; }
	}

	protected BehaviorTree _bt;
	public string Name{get; set;}

	public virtual void OnInit(BehaviorTree bt)
	{
		_bt = bt;
		OnReset();
	}

	public virtual void OnReset()
	{
		_status = Status.INVALID;
	}

	public Status Tick()
	{
		if(_status == Status.INVALID)
		{
			OnInitialize();
		}

		_status = Update();

		if(_status != Status.RUNNING)
		{
			OnTerminate();
		}

		return _status;
	}

	public virtual void OnInitialize(){}
	public abstract Status Update();
	public virtual void OnTerminate(){}

	public virtual List<Behavior> GetChildren()
	{
		return null;
	}
}

public abstract class Composite : Behavior
{
	protected List<Behavior> _children = new List<Behavior>();
	protected int _childrenIdx;

	public override void OnInit(BehaviorTree bt)
	{
		base.OnInit(bt);

		foreach (Behavior behavior in _children) 
		{
			behavior.OnInit(bt);
		}
	}

	public virtual void AddChild(Behavior behavior)
	{
		_children.Add(behavior);
	}

	public override void OnInitialize()
	{
		foreach (Behavior behavior in _children) 
		{
			behavior.OnReset();
		}

		_childrenIdx = 0;
	}

	public override List<Behavior> GetChildren()
	{
		return _children;
	}
}

public class Sequence : Composite
{
	public override Status Update()
	{
		for (int i = _childrenIdx; i < _children.Count; ++i) 
		{
			Status status = _children[i].Tick();

			if(		status == Status.RUNNING 
			   || 	status == Status.FAILURE)
			{
				_childrenIdx = i;

				_status = status;
				return _status;
			}
		}

		_status = Status.SUCCESS;
		return _status;
	}
}

public class ActiveSequence : Composite
{
	public override Status Update()
	{
		for (int i = 0; i < _children.Count; ++i) 
		{
			if(i < _childrenIdx)
			{
				_children[i].OnReset();
			}

			Status status = _children[i].Tick();
			
			if(		status == Status.RUNNING 
			   || 	status == Status.FAILURE)
			{
				_childrenIdx = i;
				
				_status = status;
				return _status;
			}
		}
		
		_status = Status.SUCCESS;
		return _status;
	}
}

public class Selector : Composite
{
	public override Status Update()
	{
		for (int i = _childrenIdx; i < _children.Count; ++i) 
		{
			Status status = _children[i].Tick();
			
			if(status != Status.FAILURE)
			{
				_childrenIdx = i;

				_status = status;
				return _status;
			}
		}

		_status = Status.FAILURE;
		return _status;
	}
}

public class ActiveSelector : Composite
{
	public override Status Update()
	{
		for (int i = 0; i < _children.Count; ++i) 
		{
			if(i < _childrenIdx)
			{
				_children[i].OnReset();
			}

			Status status = _children[i].Tick();
			
			if(status != Status.FAILURE)
			{
				_childrenIdx = i;

				_status = status;
				return _status;
			}
		}

		_status = Status.FAILURE;
		return _status;
	}
}

public abstract class Decorator : Behavior
{
	protected Behavior _child;

	List<Behavior> _children = null;
	protected List<Behavior> Children
	{
		get
		{  
			if(_children == null)
			{
				_children = new List<Behavior>();
				_children.Add(_child);
			}

			return _children;
		}
	}

	public override void OnInit(BehaviorTree bt)
	{
		base.OnInit(bt);
		_child.OnInit(bt);
	}

	public void SetChild(Behavior behavior)
	{
		_child = behavior;
	}

	public override List<Behavior> GetChildren()
	{
		return Children;
	}
}

public class UntilFail : Decorator
{
	public override Status Update()
	{
		_status = Status.RUNNING;

		Status childStatus = _child.Tick();
		if(childStatus == Status.FAILURE)
		{
			_status = Status.SUCCESS;
		}

		return _status;
	}
}

public class Inverter : Decorator
{
	public override Status Update()
	{
		_status = _child.Tick();

		if(_status == Status.SUCCESS)
		{
			_status = Status.FAILURE;
		}

		if(_status == Status.FAILURE)
		{
			_status = Status.SUCCESS;
		}

		return _status;
	}
}

public class Succeder : Decorator
{
	public override Status Update()
	{
		_status = _child.Tick();
		
		if(_status != Status.RUNNING)
		{
			_status = Status.SUCCESS;
		}
		
		return _status;
	}
}

public class Failer : Decorator
{
	public override Status Update()
	{
		_status = _child.Tick();
		
		if(_status != Status.RUNNING)
		{
			_status = Status.FAILURE;
		}
		
		return _status;
	}
}

public class RunForever : Behavior
{
	public override Status Update()
	{
		_status = Status.RUNNING;

		return _status;
	}
}

public class TimeWaiter : Behavior
{
	float _waitTime;
	float _endTime;

	public TimeWaiter(float time)
	{
		_waitTime = time;
		_endTime = 0f;
	}

	public override void OnInitialize ()
	{
		base.OnInitialize ();

		_endTime = Time.timeSinceLevelLoad + _waitTime;
	}

	public override Status Update()
	{
		if(_endTime < Time.timeSinceLevelLoad)
		{
			_status = Status.SUCCESS;
		}
		else
		{
			_status = Status.RUNNING;
		}

		_status = _status;
		return _status;
	}
}

public class Success : Behavior
{
	public override Status Update()
	{
		_status = Status.SUCCESS;
		return _status;
	}
}

public class Failure : Behavior
{
	public override Status Update()
	{
		_status = Status.FAILURE;
		return _status;
	}
}

public class CheckMemoryVar : Behavior
{
	string _varName;

	public CheckMemoryVar(string memoryVarName)
	{
		_varName = memoryVarName;
	}

	public override Status Update()
	{
		DebugUtils.Assert(_bt!=null, "_bt!=null");
		bool containsVar = _bt.ContainsMemoryObject(_varName);
		return containsVar ? Status.SUCCESS : Status.FAILURE;
	}
}

public class CheckMemoryType<T> : Behavior
{
	string _varName;

	public CheckMemoryType(string memoryVarName)
	{
		_varName = memoryVarName;
	}
	
	public override Status Update()
	{
		DebugUtils.Assert(_bt!=null, "_bt!=null");

		T memoryObj;
		bool isOk = _bt.GetMemoryObject<T>(_varName, out memoryObj);
		if(!isOk)
		{
			return Status.FAILURE;
		}

		return memoryObj is T ? Status.SUCCESS : Status.FAILURE;
	}
}

public class CheckKnowledgeStatus : Behavior
{
	string _varName;
	BaseKnowledge.StatusType _status;

	public CheckKnowledgeStatus(string memoryVarName, BaseKnowledge.StatusType knowledgeStatus)
	{
		_varName = memoryVarName;
		_status = knowledgeStatus;
	}
	
	public override Status Update()
	{
		DebugUtils.Assert(_bt!=null, "_bt!=null");
		
		BaseKnowledge memoryObj;
		bool isOk = _bt.GetMemoryObject<BaseKnowledge>(_varName, out memoryObj);
		if(!isOk)
		{
			return Status.FAILURE;
		}

		return memoryObj.Status == _status ? Status.SUCCESS : Status.FAILURE;
	}
}

public class SetKnowledgeStatus : Behavior
{
	string _varName;
	BaseKnowledge.StatusType _status;
	
	public SetKnowledgeStatus(string memoryVarName, BaseKnowledge.StatusType knowledgeStatus)
	{
		_varName = memoryVarName;
		_status = knowledgeStatus;
	}
	
	public override Status Update()
	{
		DebugUtils.Assert(_bt!=null, "_bt!=null");
		
		BaseKnowledge memoryObj;
		bool isOk = _bt.GetMemoryObject<BaseKnowledge>(_varName, out memoryObj);
		if(!isOk)
		{
			return Status.FAILURE;
		}

		memoryObj.Status = _status;
		
		return Status.SUCCESS;
	}
}

public class RemoveMemoryVar : Behavior
{
	string _varName;
	
	public RemoveMemoryVar(string memoryVarName)
	{
		_varName = memoryVarName;
	}
	
	public override Status Update()
	{
		bool containsVar = _bt.RemoveMemoryObject(_varName);
		return Status.SUCCESS;
	}
}

public class CheckAreEqualMemoryVars : Behavior
{
	string _varNameA;
	string _varNameB;
	
	public CheckAreEqualMemoryVars(string varNameA, string varNameB)
	{
		_varNameA = varNameA;
		_varNameB = varNameB;
	}
	
	public override Status Update()
	{
		System.Object objA = null;
		_bt.GetMemoryObject<System.Object>(_varNameA, out objA);

		System.Object objB = null;
		_bt.GetMemoryObject<System.Object>(_varNameB, out objB);

		return objA == objB ? Status.SUCCESS : Status.FAILURE;
	}
}

public class CopyMemoryVar : Behavior
{
	string _source;
	string _target;

	public CopyMemoryVar(string source, string target)
	{
		_source = source;
		_target = target;
	}
	
	public override Status Update()
	{
		System.Object objA = null;
		_bt.GetMemoryObject<System.Object>(_source, out objA);
		
		_bt.SetMemoryObject<System.Object>(_target, objA);
		
		return Status.SUCCESS;
	}
}
