using UnityEngine;
using System.Collections;
using Squad.MathS ;
public class RotatePoint : MonoBehaviour {
	void Start () {
		// InvokeRepeating("CusUpdate",1, 1);
	}

	void Update () {
		var sd = MathS.RotateAroundAxis(3f,3f,3f,transform.position);
	}
	[RangeAttribute(0.1f, 10f)] public float radius ;
	public float amp ;

	[RangeAttribute(0f,360f)]public float angle ;
	[RangeAttribute(0f,10f)]public float distance ;
	public float Angle {
		get {
			return angle * Mathf.PI/180 ;
		}
	}
}
