using UnityEngine;
using System.Collections;

public class BaseKnowledge : MonoBehaviour
{
	public enum StatusType
	{
		NEW,
		PROCESSED,
		DONE
	}

	public StatusType Status { get; set; }

	public BaseKnowledge()
	{
		Status = StatusType.NEW;
	}
}
