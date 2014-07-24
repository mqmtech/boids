public static class DebugUtils
{
	public static void Assert(bool assertion)
	{
		Assert(assertion, "");
	}

	static public void Assert(bool assertion, string msg)
	{
		if(!assertion)
		{
			UnityEngine.Debug.LogError(msg);
		}
	}
}
