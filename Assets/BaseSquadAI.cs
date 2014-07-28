using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseSquad), typeof(BaseBtBuilder))]
public class BaseSquadAI : MonoBehaviour 
{
	BaseSquad _squad;
	BaseBtBuilder _builder;
	BehaviorTree _bt;

	void Awake()
	{
		_squad = GetComponent<BaseSquad>();
		_builder = GetComponent<BaseBtBuilder>();

		_builder.CreateBehaviorTree();
		_bt = _builder.GetBehaviorTree();

		DebugUtils.Assert(_bt!=null, "_bt!=null");
		DebugUtils.Assert(_bt.GetRootBehavior()!=null, "_bt.GetRootBehavior()!=null, GO: " + name + ", id: " + gameObject.GetInstanceID());
	}

	public void SetMemoryObject<T>(string memoryVarName, T obj)
	{
		_bt.SetMemoryObject(memoryVarName, obj);
	}

	void Update()
	{
		_bt.Tick();
	}
}
