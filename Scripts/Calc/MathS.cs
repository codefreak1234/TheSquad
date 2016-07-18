using UnityEngine;
// using System.Collections;

namespace Squad.MathS
{
	public class MathS {
		public static Vector3 RotateAroundAxis(float radius , float rotateAngle , float rotateAxis , Vector3 center) { // the angles are in radians
			var rot_x = radius * Mathf.Cos(rotateAngle) ;
			var x = center.x + rot_x * Mathf.Cos(rotateAxis) ;
			var y = center.y + radius * Mathf.Sin(rotateAngle) ;
			var z = center.z + rot_x * Mathf.Sin(rotateAxis) ;
			return new Vector3(x,y,z);
		}
	}
}