using UnityEngine;

namespace ComputeHelper
{
	/// <summary>
	/// Buffer Holder class that handles data and ComputeBuffers
	/// </summary>
	public class BufferHolder<T>
	{
		public ComputeBuffer Buffer;
		public T[] Data;
		public string Name { get; }
		public int Kernel { get; }
		public int Count { get; }
	
		/// <summary>
		/// This constructor setups the ComputeBuffer
		/// </summary>
		/// <param name="T[] data">Array containing generic data.</param>
		/// <param name="name">Name inside ComputeShader.</param>
		/// <param name="kernel">Which kernel to set the buffer to.</param>
		public BufferHolder(T[] data, string name, int kernel = 0)
		{
			Count = data.Length;
			Name = name;
			Kernel = kernel;
			Data = data;

			Buffer = new ComputeBuffer(Count, typeof(T).SizeOf(), ComputeBufferType.Structured);
			Buffer.SetData(Data);
		}
	
		/// <summary>
		/// Gets current buffer data inside ComputeShader
		/// </summary>
		public void GetBufferData()
		{
			Buffer.GetData(Data);
		}
	
		/// <summary>
		/// Disposes buffer and data
		/// </summary>
		public void Pop()
		{
			Data = null;
			Buffer.Dispose();
		}
	}
}