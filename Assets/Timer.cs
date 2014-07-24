using UnityEngine;
using System.Collections;

public class Timer 
{
	private float _endTime;

	public void Wait(float timeToWait)
	{
		_endTime = Time.timeSinceLevelLoad + timeToWait;
	}

	public bool IsEnd()
	{
		return Time.timeSinceLevelLoad >= _endTime;
	}
}
