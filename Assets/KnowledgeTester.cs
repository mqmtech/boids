using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Squad))]
public class KnowledgeTester : MonoBehaviour
{
	public enum KnowledgeType
	{
		MOVE_COMMAND,
		ATTACK_COMMAND
	}
	public KnowledgeType _knowledgeType;

	public bool _setKnowledgeIntoMemory;
	public string _knowledgeName = SquadMemory.Command;

	public Vector3 targetPos;

	BaseKnowledge _knowledge;

	Squad _squad;
	BaseSquadAI _squadAI;

	void Awake()
	{
		_squad = GetComponent<Squad>();
	}

	void Update()
	{
		if(_setKnowledgeIntoMemory)
		{
			_setKnowledgeIntoMemory = false;

			CreateKnowledge();
			_squad.SetMemoryObject(_knowledgeName, _knowledge);
		}
	}

	void CreateKnowledge()
	{
		switch(_knowledgeType)
		{
		case KnowledgeType.MOVE_COMMAND:
			_knowledge = new MoveCommand();
			//((MoveCommand) _knowledge).WorldPos = new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10));
			((MoveCommand) _knowledge).WorldPos = targetPos;
			break;
		case KnowledgeType.ATTACK_COMMAND:
			_knowledge = new AttackCommand();
			break;
		}
	}
}
