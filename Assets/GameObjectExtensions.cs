using UnityEngine;

public static class GameObjectExtensions
{
	public static T FindObjectOfTypeRecursiveDown<T>(this GameObject go) where T : Component
	{
		T comp = (T) go.GetComponent(typeof(T));
		if(comp != null)
		{
			return comp;
		}

		var transform = go.transform;
		var childCount = transform.GetChildCount();
		for (int i = 0; i < childCount; ++i)
		{
			Transform child = transform.GetChild(i);
			comp = child.gameObject.FindObjectOfTypeRecursiveDown<T>();

			if(comp != null)
			{
				return comp;
			}
		}

		return default(T);
	}
}
