using UnityEngine;

public static class RayUtils
{
	public static Ray GetRayFromTouch()
	{
		Camera camera = Camera.main;
		
		float w = Screen.width;
		float h = Screen.height;
		float ar = w / h;
		
		Vector3 screenPos = Input.mousePosition;
		screenPos.x = (2f * screenPos.x / w) - 1f;
		screenPos.y = (2f * screenPos.y / h) - 1f;
		screenPos.z = 0f;
		
		float vHalfFov = camera.fieldOfView * 0.5f;
		float vHalfFovRad = vHalfFov * Mathf.PI / 180f;
		float yScale = Mathf.Tan(vHalfFovRad);
		float xScale = yScale * ar;
		
		Vector3 rd = camera.transform.forward + (camera.transform.right * screenPos.x * xScale) + (camera.transform.up * screenPos.y * yScale);
		rd.Normalize();
		
		Ray ray = new Ray(camera.transform.position, rd);
		//Ray ray = return camera.ScreenPointToRay(Input.mousePosition);
		
		return ray;
	}
}
