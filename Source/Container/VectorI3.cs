using UnityEngine;
using System;
using System.Collections.Generic;

namespace Uzu
{
	public struct VectorI3 : IEquatable<VectorI3>
	{
		#region Constants
		public static readonly VectorI3 one = new VectorI3 (1, 1, 1);
		public static readonly VectorI3 zero = new VectorI3 (0, 0, 0);
		public static readonly VectorI3 left = new VectorI3 (-1, 0, 0);
		public static readonly VectorI3 right = new VectorI3 (1, 0, 0);
		public static readonly VectorI3 up = new VectorI3 (0, 1, 0);
		public static readonly VectorI3 top = new VectorI3 (0, 1, 0);
		public static readonly VectorI3 down = new VectorI3 (0, -1, 0);
		public static readonly VectorI3 bottom = new VectorI3 (0, -1, 0);
		public static readonly VectorI3 forward = new VectorI3 (0, 0, 1);
		public static readonly VectorI3 back = new VectorI3 (0, 0, -1);
		#endregion
		
		public int x, y, z;
	
		public int this [int index] {
			get {
				if (index == 0) {
					return x;
				}
				if (index == 1) {
					return y;
				}
				if (index == 2) {
					return z;
				}
	
				return 0;
			}
			set {
				if (index == 0) {
					x = value;
				}
				if (index == 1) {
					y = value;
				}
				if (index == 2) {
					z = value;
				}
			}
		}
		
		public VectorI3 (int xyz)
		{
			this.x = xyz;
			this.y = xyz;
			this.z = xyz;
		}
	
		public VectorI3 (int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	
		public VectorI3 (float x, float y, float z)
		{
			this.x = (int)x;
			this.y = (int)y;
			this.z = (int)z;
		}
	
		public VectorI3 (VectorI3 vector)
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}
	
		public VectorI3 (Vector3 vector)
		{
			this.x = (int)vector.x;
			this.y = (int)vector.y;
			this.z = (int)vector.z;
		}
	
		#region VectorI3 / Vector3 Methods
	
		public static VectorI3 GridCeil (Vector3 v, Vector3 roundBy)
		{
			return Round (new Vector3 (
				Mathf.Ceil (v.x / roundBy.x) * roundBy.x,
				Mathf.Ceil (v.y / roundBy.y) * roundBy.y,
				Mathf.Ceil (v.z / roundBy.z) * roundBy.z
				));
		}
	
		public static VectorI3 GridFloor (Vector3 v, Vector3 roundBy)
		{
			return Round (new Vector3 (
				Mathf.Floor (v.x / roundBy.x) * roundBy.x,
				Mathf.Floor (v.y / roundBy.y) * roundBy.y,
				Mathf.Floor (v.z / roundBy.z) * roundBy.z
				));
		}
	
		public static VectorI3 GridRound (Vector3 v, Vector3 roundBy)
		{
			return Round (new Vector3 (
				Mathf.Round (v.x / roundBy.x) * roundBy.x,
				Mathf.Round (v.y / roundBy.y) * roundBy.y,
				Mathf.Round (v.z / roundBy.z) * roundBy.z
				));
		}
	
		public static VectorI3 Ceil (Vector3 v)
		{
			return new VectorI3 (Mathf.CeilToInt (v.x), Mathf.CeilToInt (v.y), Mathf.CeilToInt (v.z));
		}
	
		public static VectorI3 Floor (Vector3 v)
		{
			return new VectorI3 (Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y), Mathf.FloorToInt (v.z));
		}
	
		public static VectorI3 Round (Vector3 v)
		{
			return new VectorI3 (Mathf.RoundToInt (v.x), Mathf.RoundToInt (v.y), Mathf.RoundToInt (v.z));
		}
		
		public static int ElementSum (VectorI3 v)
		{
			return v.x + v.y + v.z;
		}
		
		public static int ElementProduct (VectorI3 v)
		{
			return v.x * v.y * v.z;
		}
		
		public static int MinElement (VectorI3 v)
		{
			return Mathf.Min (v.z, Mathf.Min (v.x, v.y));
		}
		
		public static int MaxElement (VectorI3 v)
		{
			return Mathf.Max (v.z, Mathf.Max (v.x, v.y));
		}
		
		public static VectorI3 MinPerElement (VectorI3 v0, VectorI3 v1)
		{
			return new VectorI3 (Mathf.Min (v0.x, v1.x), Mathf.Min (v0.y, v1.y), Mathf.Min (v0.z, v1.z));
		}
		
		public static VectorI3 MaxPerElement (VectorI3 v0, VectorI3 v1)
		{
			return new VectorI3 (Mathf.Max (v0.x, v1.x), Mathf.Max (v0.y, v1.y), Mathf.Max (v0.z, v1.z));
		}
		
		public static VectorI3 Clamp (VectorI3 v, VectorI3 min, VectorI3 max)
		{
			return new VectorI3 (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y), Mathf.Clamp (v.z, min.z, max.z));
		}
	
		public int size {
			get { return Size (this); }
		}
	
		public static int Size (VectorI3 v)
		{
			return v.x * v.y * v.z;
		}
		
		public static bool AnyEqual (VectorI3 a, VectorI3 b)
		{
			return a.x == b.x || a.y == b.y || a.z == b.z;
		}
	
		public static bool AnyGreater (VectorI3 a, VectorI3 b)
		{
			return a.x > b.x || a.y > b.y || a.z > b.z;
		}
	
		public static bool AllGreater (VectorI3 a, VectorI3 b)
		{
			return a.x > b.x && a.y > b.y && a.z > b.z;
		}
	
		public static bool AnyLower (VectorI3 a, VectorI3 b)
		{
			return a.x < b.x || a.y < b.y || a.z < b.z;
		}
	
		public static bool AllLower (VectorI3 a, VectorI3 b)
		{
			return a.x < b.x && a.y < b.y && a.z < b.z;
		}
	
		public static bool AnyGreaterEqual (VectorI3 a, VectorI3 b)
		{
			return AnyEqual (a, b) || AnyGreater (a, b);
		}
	
		public static bool AllGreaterEqual (VectorI3 a, VectorI3 b)
		{
			return a.x >= b.x && a.y >= b.y && a.z >= b.z;
		}
	
		public static bool AnyLowerEqual (VectorI3 a, VectorI3 b)
		{
			return AnyEqual (a, b) || AnyLower (a, b);
		}
	
		public static bool AllLowerEqual (VectorI3 a, VectorI3 b)
		{
			return a.x <= b.x && a.y <= b.y && a.z <= b.z;
		}
		#endregion
	
		#region Advanced
		public static VectorI3 operator - (VectorI3 a)
		{
			return new VectorI3 (-a.x, -a.y, -a.z);
		}
	
		public static VectorI3 operator - (VectorI3 a, VectorI3 b)
		{
			return new VectorI3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}
	
		public static Vector3 operator - (Vector3 a, VectorI3 b)
		{
			return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}
	
		public static Vector3 operator - (VectorI3 a, Vector3 b)
		{
			return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}
	
		public static bool operator != (VectorI3 lhs, VectorI3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}
	
		public static bool operator != (Vector3 lhs, VectorI3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}
	
		public static bool operator != (VectorI3 lhs, Vector3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}
	
		public static Vector3 operator * (float d, VectorI3 a)
		{
			return new Vector3 (d * a.x, d * a.y, d * a.z);
		}
	
		public static Vector3 operator * (VectorI3 a, float d)
		{
			return new Vector3 (a.x * d, a.y * d, a.z * d);
		}
	
		public static Vector3 operator / (VectorI3 a, float d)
		{
			return new Vector3 (a.x / d, a.y / d, a.z / d);
		}
	
		public static float operator / (float d, VectorI3 a)
		{
			d /= a.x;
			d /= a.y;
			d /= a.z;
			return d;
		}
	
		public static VectorI3 operator * (VectorI3 a, VectorI3 b)
		{
			return new VectorI3 (a.x * b.x, a.y * b.y, a.z * b.z);
		}
	
		public static Vector3 operator * (Vector3 a, VectorI3 b)
		{
			return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
		}
	
		public static Vector3 operator * (VectorI3 a, Vector3 b)
		{
			return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
		}
	
		public static VectorI3 operator / (VectorI3 a, VectorI3 b)
		{
			return new VectorI3 (a.x / b.x, a.y / b.y, a.z / b.z);
		}
	
		public static Vector3 operator / (Vector3 a, VectorI3 b)
		{
			return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
		}
	
		public static Vector3 operator / (VectorI3 a, Vector3 b)
		{
			return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
		}
	
		public static VectorI3 operator + (VectorI3 a, VectorI3 b)
		{
			return new VectorI3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}
	
		public static Vector3 operator + (Vector3 a, VectorI3 b)
		{
			return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}
	
		public static Vector3 operator + (VectorI3 a, Vector3 b)
		{
			return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}
	
		public static Vector3 operator + (VectorI3 a, float d)
		{
			return new Vector3 (a.x + d, a.y + d, a.z + d);
		}
	
		public static bool operator == (VectorI3 lhs, VectorI3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}
	
		public static bool operator == (Vector3 lhs, VectorI3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}
	
		public static bool operator == (VectorI3 lhs, Vector3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}
	
		public static implicit operator VectorI3 (Vector2 v)
		{
			return new VectorI3 (v.x, v.y, 0);
		}
	
		public static implicit operator VectorI3 (Vector3 v)
		{
			return new VectorI3 (v);
		}
	
		public static implicit operator VectorI3 (Vector4 v)
		{
			return new VectorI3 (v.x, v.y, v.z);
		}
	
		public static implicit operator Vector2 (VectorI3 v)
		{
			return new Vector3 (v.x, v.y, v.z);
		}
	
		public static implicit operator Vector3 (VectorI3 v)
		{
			return new Vector3 (v.x, v.y, v.z);
		}
	
		public static implicit operator Vector4 (VectorI3 v)
		{
			return new Vector3 (v.x, v.y, v.z);
		}
	
		public static implicit operator int[] (VectorI3 v)
		{
			return new int[] { v.x, v.y, v.z };
		}
		
		#region IEquatable interface.
		public bool Equals (VectorI3 v)
		{
			return this.x == v.x && this.y == v.y && this.z == v.z;
		}
		#endregion
		
		public override bool Equals (object obj)
		{
			if (obj.GetType () == typeof(VectorI3)) {
				return Equals ((VectorI3)obj);
			}
	
			return false;
		}
	
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	
		public override string ToString ()
		{
			return "(" + x + ", " + y + ", " + z + ")";
		}
		
		public Vector3 ToVector3 ()
		{
			return new Vector3 (x, y, z);
		}
		#endregion
	}
}