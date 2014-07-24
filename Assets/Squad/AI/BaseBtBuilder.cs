using UnityEngine;
using System.Collections;

public class BaseBtBuilder : MonoBehaviour, IBehaviorWithTree
{
	BehaviorTree _bt;

	void Awake()
	{
		CreateBehaviorTree();
	}

	public void CreateBehaviorTree()
	{
		_bt = new BehaviorTree();

		Selector mainAI = new Selector();
			/*Sequence tryToMoveSquad = new Sequence();
			tryToMoveSquad.AddChild(new CheckMemoryVar(SquadMemory.SelectedSquad));
			tryToMoveSquad.AddChild(new CheckIsMouseButtonDown(0));
			tryToMoveSquad.AddChild(new SaveTouchedPosition(SquadMemory.TouchedPosition));
		tryToMoveSquad.AddChild(new MoveSquadToTarget(SquadMemory.SelectedSquad, SquadMemory.TouchedPosition));
		mainAI.AddChild(tryToMoveSquad);*/
		
		Sequence tryToSelectSquad = new Sequence();
				tryToSelectSquad.AddChild(new CheckIsMouseButtonDown(0));
				tryToSelectSquad.AddChild(new CheckIsSquadTouched());
				tryToSelectSquad.AddChild(new SaveSquad(SquadMemory.SelectedSquad));
		mainAI.AddChild(tryToSelectSquad);

		Sequence tryToMoveByCommand = new Sequence();
			tryToMoveByCommand.AddChild(new CheckMemoryVar(SquadMemory.Command));
			tryToMoveByCommand.AddChild(new CheckMemoryType<MoveCommand>(SquadMemory.Command));
				Inverter invertCheckStatus = new Inverter();
				invertCheckStatus.SetChild(new CheckKnowledgeStatus(SquadMemory.Command, BaseKnowledge.StatusType.PROCESSED));
			tryToMoveByCommand.AddChild(invertCheckStatus);
			tryToMoveByCommand.AddChild(new SetKnowledgeStatus(SquadMemory.Command, BaseKnowledge.StatusType.PROCESSED));
			
		ActiveSequence tryToKeepMovingWithSquad = new ActiveSequence();
			Inverter invertCommandType = new Inverter();
			invertCommandType.SetChild(new CheckKnowledgeStatus(SquadMemory.Command, BaseKnowledge.StatusType.NEW));
			tryToKeepMovingWithSquad.AddChild(invertCommandType);
			Sequence moveWithSquad = new Sequence();
				moveWithSquad.AddChild(new SavePositionFromMoveCommand(SquadMemory.Command, SquadMemory.TouchedPosition));
				moveWithSquad.AddChild(new MoveSquadToTarget(SquadMemory.SelectedSquad, SquadMemory.TouchedPosition));
				Succeder removeCommandSuccess = new Succeder();
					Sequence removeCommand = new Sequence();
						removeCommand.AddChild(new CheckKnowledgeStatus(SquadMemory.Command, BaseKnowledge.StatusType.PROCESSED));
						removeCommand.AddChild(new CheckMemoryType<MoveCommand>(SquadMemory.Command));
						removeCommand.AddChild(new RemoveMemoryVar(SquadMemory.Command));
				removeCommandSuccess.SetChild(removeCommand);
				moveWithSquad.AddChild(removeCommandSuccess);
        	tryToKeepMovingWithSquad.AddChild(moveWithSquad);
		tryToMoveByCommand.AddChild(tryToKeepMovingWithSquad);
			
		mainAI.AddChild(tryToMoveByCommand);

		_bt.Init(mainAI);
	}

	public BehaviorTree GetBehaviorTree()
	{
		return _bt;
	}
}


