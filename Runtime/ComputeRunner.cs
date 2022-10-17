using UnityEngine;

namespace ComputeHelper
{
    public class ComputeRunner
    {
        public ComputeShader ComputeShader;

        Vector3Int[] _groupSizes;
        int[] _kernels;
        int _kernelCount;

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

        public void AddBuffer<T>(BufferHolder<T> b)
        {
            ComputeShader.SetBuffer(b.Kernel, b.Name, b.Buffer);
        }

        public void AddTexture(int kernel, string name, RenderTexture tex)
        {
            ComputeShader.SetTexture(kernel, name, tex);
        }

        public void DispatchAll()
        {
            if (_kernelCount == 1)
            {
                Debug.LogError("Only 1 kernel exists");
                return;
            }

            for (int i = 0; i < _kernelCount; i++)
            {
                Vector3Int groupSize = _groupSizes[i];
                ComputeShader.Dispatch(_kernels[i], groupSize.x, groupSize.y, groupSize.z);
            }
        }

        public void Dispatch()
        {
            Vector3Int groupSize = _groupSizes[0];
            ComputeShader.Dispatch(0, groupSize.x, groupSize.y, groupSize.z);
        }

        public void Dispatch(int kernel)
        {
            Vector3Int groupSize = _groupSizes[0];
            ComputeShader.Dispatch(kernel, groupSize.x, groupSize.y, groupSize.z);
        }

        public void Dispatch(string kernelName)
        {
            Vector3Int groupSize = _groupSizes[0];
            int kernel = ComputeShader.FindKernel(kernelName);
            ComputeShader.Dispatch(kernel, groupSize.x, groupSize.y, groupSize.z);
        }
    }
}