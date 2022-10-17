using UnityEngine;

namespace ComputeHelper
{
	public class InstancedRenderer
	{
		int _instanceCount;
		
		ComputeBuffer _argsBuffer;
		Mesh _mesh;
		Material _material;
		
		Bounds _bounds;
		Vector3 _boundCenter;
		float _boundSize;
	
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
	
		public void Render()
		{
			Graphics.DrawMeshInstancedIndirect(_mesh, 0, _material, _bounds, _argsBuffer);
		}

		public void AddBuffer<T>(BufferHolder<T> buffer)
		{
			_material.SetBuffer(buffer.Name, buffer.Buffer);
		}
	
		public void Pop()
		{
			_argsBuffer.Dispose();
		}
	}
}