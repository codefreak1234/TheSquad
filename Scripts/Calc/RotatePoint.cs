using UnityEngine;
using System.Collections;
using Squad.MathS ;
public class RotatePoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// InvokeRepeating("CusUpdate",1, 1);
	}

	// Update is called once per frame
	void Update () {
		var sd = MathS.RotateAroundAxis(3f,3f,3f,transform.position);
		// Debug.DrawLine(transform.position , Rot(20) , Color.red);
		// Debug.DrawLine(transform.position , Rot(0) , Color.red);
		// Debug.DrawLine(transform.position , Rot(40) , Color.red);
		// Debug.DrawLine(transform.position , Rot(80) , Color.red);
		// Debug.DrawLine(transform.position , Rot(120) , Color.green);
		// Debug.DrawLine(transform.position , Rot(150) , Color.green);
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
