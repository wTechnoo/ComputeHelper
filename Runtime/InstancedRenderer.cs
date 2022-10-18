using UnityEngine;

namespace ComputeHelper
{
	/// <summary>
	/// Indirect Instanced Renderer class that reduces boilerplate code
	/// </summary>
	public class InstancedRenderer
	{
		int _instanceCount;
		
		ComputeBuffer _argsBuffer;
		Mesh _mesh;
		Material _material;
		
		Bounds _bounds;
		Vector3 _boundCenter;
		float _boundSize;
	
		/// <summary>
		/// This constructor setups the argsBuffer
		/// </summary>
		/// <param name="instanceCount">How many instances are going to be drawn.</param>
		/// <param name="mesh">Mesh to be drawn.</param>
		/// <param name="material">Shader that will draw the instanced meshes.</param>
		/// <param name="boundCenter">Where the center of the bound will be placed for rendering.</param>
		/// <param name="boundSize">Maximum bound size.</param>
		public InstancedRenderer(int instanceCount, Mesh mesh, Material material, Vector3 boundCenter, float boundSize = 100f)
		{
			_instanceCount = instanceCount;
			_mesh = mesh;
			_material = material;
			_boundCenter = boundCenter;
			_boundSize = boundSize;
			
			Initialize();
		}
	
		void Initialize()
		{
			_bounds = new Bounds(_boundCenter, Vector3.one * _boundSize);
			
			uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
			args[0] = _mesh.GetIndexCount(0);
			args[1] = (uint)_instanceCount;
			args[2] = _mesh.GetIndexStart(0);
			args[3] = _mesh.GetBaseVertex(0);
			args[4] = 0;
			
			_argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
			_argsBuffer.SetData(args);
		}
	
		/// <summary>
		/// Render meshes
		/// </summary>
		public void Render()
		{
			Graphics.DrawMeshInstancedIndirect(_mesh, 0, _material, _bounds, _argsBuffer);
		}

		/// <summary>
		/// Add buffer to shader
		/// </summary>
		public void AddBuffer<T>(BufferHolder<T> buffer)
		{
			_material.SetBuffer(buffer.Name, buffer.Buffer);
		}
	
		/// <summary>
		/// Dispose argBuffer
		/// </summary>
		public void Pop()
		{
			_argsBuffer.Dispose();
		}
	}
}