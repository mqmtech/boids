using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BehaviorTreeViewer : EditorWindow 
{
	const int kYOffsetSeparation = 2;
	const int kXOffsetSeparation = 40;

	const int kButtonWidth = 250;
	const int kButtonHeight = 20;

	const int kLabelWidth = 250;
	const int kLabelHeight = 20;

	BehaviorTree _bt;
	
	[MenuItem ("Window/BehaviorTreeViewer")]
	static void Init () 
	{
		BehaviorTreeViewer window = (BehaviorTreeViewer)EditorWindow.GetWindow (typeof (BehaviorTreeViewer));
	}
	
	void OnGUI () 
	{
		if(_bt == null)
		{
			BaseBtBuilder bWithTree = (BaseBtBuilder) GameObject.FindObjectOfType(typeof(BaseBtBuilder));
			if(bWithTree!=null)
			{
				_bt = bWithTree.GetBehaviorTree();
			}
		}

		if(_bt != null)
		{
			RenderBehaviorTree(_bt);
		}
	}

	void Update()
	{
		Repaint();
	}

	void RenderBehaviorTree(BehaviorTree bt)
	{
		Behavior behavior = bt.GetRootBehavior();
		DebugUtils.Assert(behavior!=null, "behavior!=null");

		int xOffset = 10;
		int yOffset = 10;
		RenderBehavior(behavior, xOffset, ref yOffset, Behavior.Status.SUCCESS);

		RenderMemoryVariables(xOffset, yOffset);
	}

	void RenderBehavior(Behavior behavior, int xOffset, ref int yOffset, Behavior.Status parentStatus)
	{
		int myXOffset = xOffset;
		int myYOffset = yOffset;

		string name = GetBehaviorName(behavior);

		Behavior.Status status = parentStatus;
		if(status != Behavior.Status.FAILURE)
		{
			status = behavior.BehaviorStatus;
		}
		RenderBox(name, status, xOffset, yOffset);

		yOffset += kButtonHeight + kYOffsetSeparation;
		xOffset += kXOffsetSeparation;

		List<Behavior> children = behavior.GetChildren();
		if(children == null)
		{
			return;
		}

		foreach (Behavior child in children) 
		{
			Handles.BeginGUI();
			Handles.DrawLine( 
			                 new Vector3(myXOffset, myYOffset + kButtonHeight / 2, 0f),
			                 new Vector3(myXOffset, yOffset + kButtonHeight / 2,   0f));

			Handles.DrawLine( 
			                 new Vector3(myXOffset, yOffset + kButtonHeight / 2, 0f),
			                 new Vector3(xOffset, yOffset + kButtonHeight / 2,   0f));
			Handles.EndGUI();

			RenderBehavior(child, xOffset, ref yOffset, status);
		}
	}

	void RenderMemoryVariables(int xOffset, int yOffset)
	{
		yOffset += 20;

		//GUI.backgroundColor = Color.gray;
		foreach (KeyValuePair<string, System.Object> keyPair in _bt.Memory) 
		{
			GUI.Label (new Rect ((float)xOffset, (float)yOffset, kLabelWidth, kButtonHeight), keyPair.Key);
			yOffset += 10;
		}
	}

	string GetBehaviorName(Behavior behavior)
	{
		string name = behavior.GetType().ToString();
		if(!string.IsNullOrEmpty(behavior.Name))
		{
			name += ": " + behavior.Name;
		}

		return name;
	}

	void RenderBox(string name, Behavior.Status status, int xOffset, int yOffset)
	{
		Color color = GetColorFromStatus(status);
		GUI.backgroundColor = color;
		GUI.contentColor = Color.white;

		GUI.Button (new Rect ((float)xOffset, yOffset, kButtonWidth, kButtonHeight), name);
	}

	Color GetColorFromStatus(Behavior.Status status)
	{
		switch(status)
		{
		case Behavior.Status.INVALID:
			return Color.gray;
			break;

		case Behavior.Status.FAILURE:
			return Color.red;
			break;

		case Behavior.Status.SUCCESS:
			return Color.green;
			break;

		case Behavior.Status.RUNNING:
			return Color.cyan;
			break;
		}

		return Color.magenta;
	}
}
