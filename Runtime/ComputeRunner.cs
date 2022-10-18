using UnityEngine;

namespace ComputeHelper
{
    /// <summary>
    /// Compute Shader class that reduces boilerplate code
    /// </summary>
    public class ComputeRunner
    {
        public ComputeShader ComputeShader;

        Vector3Int[] _groupSizes;
        int[] _kernels;
        int _kernelCount;

        /// <summary>
        /// This constructor setups threads and kernels (Can contain more than one kernel)
        /// </summary>
        /// <param name="computeShader">Compute Shader reference.</param>
        /// <param name="count">Object count or texture resolution (ex: Vector3Int(512, 512, 1)).</param>
        /// <param name="moreThanOneKernel">Boolean to control more than one kernel.</param>
        /// <param name="kernelCount">How many kernels there are on the compute shader.</param>
        /// <param name="kernels">kernels string array with kernel names.</param>
        public ComputeRunner(ComputeShader computeShader, Vector3Int count, bool moreThanOneKernel = false,
            int kernelCount = 1, string[] kernels = null)
        {
            ComputeShader = computeShader;
            _kernelCount = kernelCount;

            _groupSizes = new Vector3Int[_kernelCount];
            _kernels = new int[_kernelCount];
            if (moreThanOneKernel && kernels != null)
            {
                for (int i = 0; i < _kernelCount; i++)
                {
                    int kernel = ComputeShader.FindKernel(kernels[i]);
                    _kernels[i] = kernel;

                    uint threadSizeX, threadSizeY, threadSizeZ;
                    ComputeShader.GetKernelThreadGroupSizes(kernel, out threadSizeX, out threadSizeY, out threadSizeZ);

                    int x, y, z;
                    x = threadSizeX == 1 ? 1 : Mathf.CeilToInt((float) count.x / (float) threadSizeX);
                    y = threadSizeY == 1 ? 1 : Mathf.CeilToInt((float) count.y / (float) threadSizeY);
                    z = threadSizeZ == 1 ? 1 : Mathf.CeilToInt((float) count.z / (float) threadSizeZ);
                    
                    _groupSizes[i] = new Vector3Int(x, y, z);
                }
            }
            else
            {
                _kernels[0] = 0;

                uint threadSizeX, threadSizeY, threadSizeZ;
                ComputeShader.GetKernelThreadGroupSizes(0, out threadSizeX, out threadSizeY, out threadSizeZ);

                int x, y, z;
                x = threadSizeX == 1 ? 1 : Mathf.CeilToInt((float) count.x / (float) threadSizeX);
                y = threadSizeY == 1 ? 1 : Mathf.CeilToInt((float) count.y / (float) threadSizeY);
                z = threadSizeZ == 1 ? 1 : Mathf.CeilToInt((float) count.z / (float) threadSizeZ);
                _groupSizes[0] = new Vector3Int(x, y, z);
            }
        }

        /// <summary>
        /// Add buffer to compute shader
        /// </summary>
        public void AddBuffer<T>(BufferHolder<T> b)
        {
            ComputeShader.SetBuffer(b.Kernel, b.Name, b.Buffer);
        }

        /// <summary>
        /// Add RenderTexture to compute shader
        /// </summary>
        public void AddTexture(int kernel, string name, RenderTexture tex)
        {
            ComputeShader.SetTexture(kernel, name, tex);
        }
        
        /// <summary>
        /// Add Texture2D to compute shader
        /// </summary>
        public void AddTexture(int kernel, string name, Texture2D tex)
        {
            ComputeShader.SetTexture(kernel, name, tex);
        }

        /// <summary>
        /// Dispatches all kernels with the group size of each kernel
        /// </summary>
        public void DispatchAll()
        {
            for (int i = 0; i < _kernelCount; i++)
            {
                Vector3Int groupSize = _groupSizes[i];
                ComputeShader.Dispatch(_kernels[i], groupSize.x, groupSize.y, groupSize.z);
            }
        }

        /// <summary>
        /// Dispatches compute shader
        /// </summary>
        public void Dispatch()
        {
            Vector3Int groupSize = _groupSizes[0];
            ComputeShader.Dispatch(0, groupSize.x, groupSize.y, groupSize.z);
        }
        
        /// <summary>
        /// Dispatches specifying thread numbers
        /// </summary>
        public void Dispatch(int threadX, int threadY, int threadZ)
        {
            Vector3Int groupSize = _groupSizes[0];
            ComputeShader.Dispatch(0, threadX, threadY, threadZ);
        }

        /// <summary>
        /// Dispatches individual kernel
        /// </summary>
        public void Dispatch(int kernel)
        {
            Vector3Int groupSize = _groupSizes[0];
            ComputeShader.Dispatch(kernel, groupSize.x, groupSize.y, groupSize.z);
        }

        /// <summary>
        /// Dispatches individual kernel by name
        /// </summary>
        public void Dispatch(string kernelName)
        {
            Vector3Int groupSize = _groupSizes[0];
            int kernel = ComputeShader.FindKernel(kernelName);
            ComputeShader.Dispatch(kernel, groupSize.x, groupSize.y, groupSize.z);
        }
    }
}