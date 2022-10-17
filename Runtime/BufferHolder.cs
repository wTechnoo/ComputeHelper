using UnityEngine;

namespace ComputeHelper
{
	public class BufferHolder<T>
	{
		public ComputeBuffer Buffer;
		public T[] Data;
		public string Name { get; }
		public int Kernel { get; }
		public int Count { get; }
	
		public BufferHolder(T[] data, string name, int kernel = 0)
		{
			Count = data.Length;
			Name = name;
			Kernel = kernel;
			Data = data;

			Buffer = new ComputeBuffer(Count, typeof(T).SizeOf(), ComputeBufferType.Structured);
			Buffer.SetData(Data);
		}
	
		public void GetBufferData()
		{
			Buffer.GetData(Data);
		}
	
		public void Pop()
		{
			Data = null;
			Buffer.Dispose();
		}
	}
}