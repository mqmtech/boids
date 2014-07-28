using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SquadUnitBuilder), typeof(BaseSquadMover), typeof(BaseSquadAI))]
public class BoidSquad : BaseSquad
{
	[SerializeField]
	float _separationDistance = 2.0f;
	float SeparationDistanceSQ
	{
		get
		{
			return _separationDistance * _separationDistance;
		}
	}

	[SerializeField]
	float _centerOfMassFactor = 0.01f;
	
	[SerializeField]
	float _separationFactor = 1.0f;
	
	[SerializeField]
	float _avgVelocityFactor = 0.128f;
	
	[SerializeField]
	float _goalFactor = 0.01f;

	[SerializeField]
	float _maxVelocity = 10f;
	float MaxVelocitySQ
	{
		get{ return _maxVelocity * _maxVelocity; }
	}

	Vector3 _goal;

	protected List<BaseSquadUnit> _units;
	public override List<BaseSquadUnit> Units
	{
		get
		{
			return _units;
		}

		protected set
		{
			_units = value;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();
		//_separationDistanceSQ = _separationDistance * _separationDistance;
	}

	public override void MoveTo(Vector3 worldPos)
	{
		_goal = worldPos;
		SquadMover.MoveTo(worldPos);
	}

	public override void CreateUnits (int numUnits)
	{
		_units = SquadUnitBuilder.CreateUnits(numUnits, this);
	}

	public Vector3 CalculateCenterOfMassForBoid(BoidSquadUnit boid)
	{
		Vector3 centerOfMass = Vector3.zero;
		
		int numBoidsCalculated = 0;
		for (int i = 0; i < Units.Count; ++i) 
		{
			BoidSquadUnit currentBoid = (BoidSquadUnit) Units[i];
			if(currentBoid == boid)
			{
				continue;
			}
			numBoidsCalculated++;
			centerOfMass += currentBoid.transform.position;
		}
		
		centerOfMass = centerOfMass / ((float) numBoidsCalculated);

		Vector3 dirToCenterOffMass = (centerOfMass - boid.transform.position) * _centerOfMassFactor;
		return dirToCenterOffMass;
	}

	public Vector3 CalculateSeparationForBoid(BoidSquadUnit boid)
	{
		Vector3 seprationDir = Vector3.zero;
		Vector3 boidPos = boid.transform.position;

		for (int i = 0; i < Units.Count; ++i) 
		{
			BoidSquadUnit currentBoid = (BoidSquadUnit) Units[i];
			if(currentBoid == boid)
			{
				continue;
			}
			Vector3 dirToBoid = boidPos - currentBoid.transform.position;
			if(dirToBoid.sqrMagnitude < SeparationDistanceSQ)
			{
				seprationDir += dirToBoid;
			}
		}

		seprationDir = seprationDir * _separationFactor;
		return seprationDir;
	}

	public Vector3 CalculateAvgVelocityToNearBoid(BoidSquadUnit boid)
	{
		Vector3 avgVelocity = Vector3.zero;

		int numBoidsCalculated = 0;
		for (int i = 0; i < Units.Count; ++i) 
		{
			BoidSquadUnit currentBoid = (BoidSquadUnit) Units[i];
			if(currentBoid == boid)
			{
				continue;
			}
			numBoidsCalculated++;
			avgVelocity += currentBoid.Velocity;
		}

		avgVelocity = avgVelocity / ((float) numBoidsCalculated);

		avgVelocity = avgVelocity * _avgVelocityFactor;
		return avgVelocity;
	}

	public Vector3 CalculateGoalVelocityForBoid(BoidSquadUnit boid)
	{
		Vector3 dirToGoal = (_goal - boid.transform.position) * _goalFactor;
		
		return dirToGoal;
	}

	public Vector3 LimitVelocity(Vector3 velocity)
	{
		float speed = velocity.sqrMagnitude;
		if(speed > MaxVelocitySQ)
		{
			return velocity.normalized * _maxVelocity;
		}

		return velocity;
	}
}
