using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ComputeHelper
{
	/// <summary>
	/// Useful extensions
	/// </summary>
	public static class HelperExtensions
	{
		/// <returns>Returns struct size</returns>
		public static int SizeOf(this Type t) => Marshal.SizeOf(t);
	
		public static Vector3Int ToV3(this int n) => new Vector3Int(n,n, 1);
		public static Vector2Int ToV2(this int n) => new Vector2Int(n,n);
	}
}