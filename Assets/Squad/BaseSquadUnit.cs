using UnityEngine;

[RequireComponent(typeof(SquadUnitMover))]
public abstract class BaseSquadUnit : MonoBehaviour
{
	public abstract BaseSquad Squad {get; protected set;}
	public abstract void Init(BaseSquad iSquad);
	public abstract void MoveTo(Vector3 worldPos);
}
