using UnityEngine;

public class RayToposition
{
	const float kHitMaxDistance = 99999f;

	public bool GetPosFromTouchedGameObject(ref Vector3 oWorldPos)
	{
		Squad squad = null;

		Ray ray = RayUtils.GetRayFromTouch();
		
		RaycastHit hit = new RaycastHit();
		bool isHit = Physics.Raycast(ray, out hit, kHitMaxDistance);
		if(isHit)
		{
			oWorldPos = hit.point;
		}

		return isHit;
	}
}
