using UnityEngine;

public class RayToSquad
{
	const float kHitMaxDistance = 99999f;

	public BaseSquad FindSquadFromTouchedGameObject()
	{
		BaseSquad squad = null;

		Ray ray = RayUtils.GetRayFromTouch();
		
		int layer = 1 << LayerMask.NameToLayer("Unit");
		RaycastHit hit = new RaycastHit();
		bool isHit = Physics.Raycast(ray, out hit, kHitMaxDistance, layer);
		if(isHit)
		{
			Debug.Log("Hit: " + hit.collider.gameObject.name);
			BaseSquadUnit unit = hit.collider.gameObject.FindObjectOfTypeRecursiveDown<BaseSquadUnit>();
			if(unit != null)
			{
				return unit.Squad;
			}
		}

		return squad;
	}
}
