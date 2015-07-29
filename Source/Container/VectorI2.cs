using UnityEngine;
using System;
using System.Collections.Generic;

namespace Uzu
{
	public struct VectorI2 : IEquatable<VectorI2>
	{
		#region Constants
		public static readonly VectorI2 one = new VectorI2 (1, 1);
		public static readonly VectorI2 zero = new VectorI2 (0, 0);
		public static readonly VectorI2 left = new VectorI2 (-1, 0);
		public static readonly VectorI2 right = new VectorI2 (1, 0);
		public static readonly VectorI2 up = new VectorI2 (0, 1);
		public static readonly VectorI2 down = new VectorI2 (0, -1);
		#endregion
		
		public int x, y;
		
		public int this [int index] {
			get {
				if (index == 0) {
					return x;
				}
				if (index == 1) {
					return y;
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
			}
		}
		
		public VectorI2 (int xy)
		{
			this.x = xy;
			this.y = xy;
		}
		
		public VectorI2 (int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		public VectorI2 (float x, float y)
		{
			this.x = (int)x;
			this.y = (int)y;
		}
		
		public VectorI2 (VectorI2 vector)
		{
			this.x = vector.x;
			this.y = vector.y;
		}
		
		public VectorI2 (Vector2 vector)
		{
			this.x = (int)vector.x;
			this.y = (int)vector.y;
		}
		
		#region VectorI2 / Vector2 Methods
		
		public static VectorI2 GridCeil (Vector2 v, Vector2 roundBy)
		{
			return Round (new Vector2 (
				Mathf.Ceil (v.x / roundBy.x) * roundBy.x,
				Mathf.Ceil (v.y / roundBy.y) * roundBy.y
				));
		}
		
		public static VectorI2 GridFloor (Vector2 v, Vector2 roundBy)
		{
			return Round (new Vector2 (
				Mathf.Floor (v.x / roundBy.x) * roundBy.x,
				Mathf.Floor (v.y / roundBy.y) * roundBy.y
				));
		}
		
		public static VectorI2 GridRound (Vector2 v, Vector2 roundBy)
		{
			return Round (new Vector2 (
				Mathf.Round (v.x / roundBy.x) * roundBy.x,
				Mathf.Round (v.y / roundBy.y) * roundBy.y
				));
		}
		
		public static VectorI2 Ceil (Vector2 v)
		{
			return new VectorI2 (Mathf.CeilToInt (v.x), Mathf.CeilToInt (v.y));
		}
		
		public static VectorI2 Floor (Vector2 v)
		{
			return new VectorI2 (Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y));
		}
		
		public static VectorI2 Round (Vector2 v)
		{
			return new VectorI2 (Mathf.RoundToInt (v.x), Mathf.RoundToInt (v.y));
		}
		
		public static int ElementSum (VectorI2 v)
		{
			return v.x + v.y;
		}
		
		public static int ElementProduct (VectorI2 v)
		{
			return v.x * v.y;
		}
		
		public static int MinElement (VectorI2 v)
		{
			return Mathf.Min (v.x, v.y);
		}
		
		public static int MaxElement (VectorI2 v)
		{
			return Mathf.Max (v.x, v.y);
		}
		
		public static VectorI2 MinPerElement (VectorI2 v0, VectorI2 v1)
		{
			return new VectorI2 (Mathf.Min (v0.x, v1.x), Mathf.Min (v0.y, v1.y));
		}
		
		public static VectorI2 MaxPerElement (VectorI2 v0, VectorI2 v1)
		{
			return new VectorI2 (Mathf.Max (v0.x, v1.x), Mathf.Max (v0.y, v1.y));
		}
		
		public static VectorI2 Clamp (VectorI2 v, VectorI2 min, VectorI2 max)
		{
			return new VectorI2 (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y));
		}
		
		public int size {
			get { return Size (this); }
		}
		
		public static int Size (VectorI2 v)
		{
			return v.x * v.y;
		}
		
		public static bool AnyEqual (VectorI2 a, VectorI2 b)
		{
			return a.x == b.x || a.y == b.y;
		}
		
		public static bool AnyGreater (VectorI2 a, VectorI2 b)
		{
			return a.x > b.x || a.y > b.y;
		}
		
		public static bool AllGreater (VectorI2 a, VectorI2 b)
		{
			return a.x > b.x && a.y > b.y;
		}
		
		public static bool AnyLower (VectorI2 a, VectorI2 b)
		{
			return a.x < b.x || a.y < b.y;
		}
		
		public static bool AllLower (VectorI2 a, VectorI2 b)
		{
			return a.x < b.x && a.y < b.y;
		}
		
		public static bool AnyGreaterEqual (VectorI2 a, VectorI2 b)
		{
			return AnyEqual (a, b) || AnyGreater (a, b);
		}
		
		public static bool AllGreaterEqual (VectorI2 a, VectorI2 b)
		{
			return a.x >= b.x && a.y >= b.y;
		}
		
		public static bool AnyLowerEqual (VectorI2 a, VectorI2 b)
		{
			return AnyEqual (a, b) || AnyLower (a, b);
		}
		
		public static bool AllLowerEqual (VectorI2 a, VectorI2 b)
		{
			return a.x <= b.x && a.y <= b.y;
		}
		#endregion
		
		#region Advanced
		public static VectorI2 operator - (VectorI2 a)
		{
			return new VectorI2 (-a.x, -a.y);
		}
		
		public static VectorI2 operator - (VectorI2 a, VectorI2 b)
		{
			return new VectorI2 (a.x - b.x, a.y - b.y);
		}
		
		public static Vector2 operator - (Vector2 a, VectorI2 b)
		{
			return new Vector2 (a.x - b.x, a.y - b.y);
		}
		
		public static Vector2 operator - (VectorI2 a, Vector2 b)
		{
			return new Vector2 (a.x - b.x, a.y - b.y);
		}
		
		public static bool operator != (VectorI2 lhs, VectorI2 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}
		
		public static bool operator != (Vector2 lhs, VectorI2 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}
		
		public static bool operator != (VectorI2 lhs, Vector2 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}
		
		public static Vector2 operator * (float d, VectorI2 a)
		{
			return new Vector2 (d * a.x, d * a.y);
		}
		
		public static Vector2 operator * (VectorI2 a, float d)
		{
			return new Vector2 (a.x * d, a.y * d);
		}
		
		public static Vector2 operator / (VectorI2 a, float d)
		{
			return new Vector2 (a.x / d, a.y / d);
		}
		
		public static float operator / (float d, VectorI2 a)
		{
			d /= a.x;
			d /= a.y;
			return d;
		}
		
		public static VectorI2 operator * (VectorI2 a, VectorI2 b)
		{
			return new VectorI2 (a.x * b.x, a.y * b.y);
		}
		
		public static Vector2 operator * (Vector2 a, VectorI2 b)
		{
			return new Vector2 (a.x * b.x, a.y * b.y);
		}
		
		public static Vector2 operator * (VectorI2 a, Vector2 b)
		{
			return new Vector2 (a.x * b.x, a.y * b.y);
		}
		
		public static VectorI2 operator / (VectorI2 a, VectorI2 b)
		{
			return new VectorI2 (a.x / b.x, a.y / b.y);
		}
		
		public static Vector2 operator / (Vector2 a, VectorI2 b)
		{
			return new Vector2 (a.x / b.x, a.y / b.y);
		}
		
		public static Vector2 operator / (VectorI2 a, Vector2 b)
		{
			return new Vector2 (a.x / b.x, a.y / b.y);
		}
		
		public static VectorI2 operator + (VectorI2 a, VectorI2 b)
		{
			return new VectorI2 (a.x + b.x, a.y + b.y);
		}
		
		public static Vector2 operator + (Vector2 a, VectorI2 b)
		{
			return new Vector2 (a.x + b.x, a.y + b.y);
		}
		
		public static Vector2 operator + (VectorI2 a, Vector2 b)
		{
			return new Vector2 (a.x + b.x, a.y + b.y);
		}
		
		public static Vector2 operator + (VectorI2 a, float d)
		{
			return new Vector2 (a.x + d, a.y + d);
		}
		
		public static bool operator == (VectorI2 lhs, VectorI2 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		
		public static bool operator == (Vector2 lhs, VectorI2 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		
		public static bool operator == (VectorI2 lhs, Vector2 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		
		public static implicit operator VectorI2 (Vector2 v)
		{
			return new VectorI2 (v.x, v.y);
		}
		
		public static implicit operator Vector2 (VectorI2 v)
		{
			return new Vector2 (v.x, v.y);
		}

		public static implicit operator int[] (VectorI2 v)
		{
			return new int[] { v.x, v.y };
		}
		
		#region IEquatable interface.
		public bool Equals (VectorI2 v)
		{
			return this.x == v.x && this.y == v.y;
		}
		#endregion
		
		public override bool Equals (object obj)
		{
			if (obj.GetType () == typeof(VectorI2)) {
				return Equals ((VectorI2)obj);
			}
			
			return false;
		}
		
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
		
		public override string ToString ()
		{
			return "(" + x + ", " + y + ")";
		}
		
		public Vector2 ToVector2 ()
		{
			return new Vector2 (x, y);
		}
		#endregion
	}
}