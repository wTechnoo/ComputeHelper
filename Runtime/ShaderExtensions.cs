using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ComputeHelper
{
	public static class ShaderExtensions
	{
		public static int SizeOf(this Type t) => Marshal.SizeOf(t);
	
		public static Vector3Int ToV3(this int n) => new Vector3Int(n,n, 1);
		public static Vector2Int ToV2(this int n) => new Vector2Int(n,n);
	}
}