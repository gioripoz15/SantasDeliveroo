using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]//to debug in inspector
public struct PathPoint
{
	public enum PointType
    {
        POINT,
        GIFT,
        HOUSE
    }
	public Vector3 position;
    public PointType pointType;
    public GameObject targettedObject;
}
