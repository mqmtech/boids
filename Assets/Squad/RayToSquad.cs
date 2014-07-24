using UnityEngine;

public class RayToSquad
{
	const float kHitMaxDistance = 99999f;

	public Squad FindSquadFromTouchedGameObject()
	{
		Squad squad = null;

		Ray ray = RayUtils.GetRayFromTouch();
		
		int layer = 1 << LayerMask.NameToLayer("Unit");
		RaycastHit hit = new RaycastHit();
		bool isHit = Physics.Raycast(ray, out hit, kHitMaxDistance, layer);
		if(isHit)
		{
			Debug.Log("Hit: " + hit.collider.gameObject.name);
			SquadUnit unit = hit.collider.gameObject.FindObjectOfTypeRecursiveDown<SquadUnit>();
			if(unit != null)
			{
				return unit.Squad;
			}
		}

		return squad;
	}
}
