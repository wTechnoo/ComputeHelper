using UnityEngine;

namespace ComputeHelper
{
	public static class ComputeRunnerExtensions
	{
		public static void SetVar(this ComputeRunner a, string n, float var) => a.ComputeShader.SetFloat(n, var);
		
		public static void SetVar(this ComputeRunner a, string n, int var) => a.ComputeShader.SetInt(n, var);
	
		public static void SetVar(this ComputeRunner a, string n, Vector2 var) => a.ComputeShader.SetVector(n, var);
	
		public static void SetVar(this ComputeRunner a, string n, Vector3 var) => a.ComputeShader.SetVector(n, var);
	
		public static void SetVar(this ComputeRunner a, string n, Vector4 var) => a.ComputeShader.SetVector(n, var);
	
		public static void SetVar(this ComputeRunner a, string n, Color var) => a.ComputeShader.SetVector(n, var);
		
		public static void SetVar(this ComputeRunner a, string n, bool var) => a.ComputeShader.SetBool(n, var);
		
		public static void SetVar(this ComputeRunner a, int kernel, string n, Texture2D var) => a.ComputeShader.SetTexture(kernel, n, var);
	
		public static void SetVar(this ComputeRunner a, string n, Matrix4x4 var) => a.ComputeShader.SetMatrix(n, var);
		
	}
}