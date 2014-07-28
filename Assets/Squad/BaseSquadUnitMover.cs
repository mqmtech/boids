using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseSquadUnit))]
public class BaseSquadUnitMover : MonoBehaviour 
{
	public virtual void MoveTo(Vector3 worldPos)
	{
	}
}
