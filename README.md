# Compute Helper
Shaderlab, Rendering and Compute Shader functions and classes

## Installation
Import from git or package.json into Unity Package manager

- `https://github.com/wTechnoo/ComputeHelper.git`

![image](https://user-images.githubusercontent.com/71846381/196069018-db5fcf8a-b5bf-45e1-b8a4-12c3a747c892.png)
![image](https://user-images.githubusercontent.com/71846381/196069037-961aa0b8-9427-49e9-b91e-9aa8dd84648e.png)

## Usage
### [ComputeRunner](https://github.com/wTechnoo/ComputeHelper/blob/main/Runtime/ComputeRunner.cs)
> This class implements boilerplate code of compute shaders

#### Basic usage:
```csharp
public class ExampleComputeRunner : MonoBehaviour
{
    ComputeRunner runner;
    ComputeShader computeShader;
    void Start()
    {
        runner = new ComputeRunner(computeShader, new Vector3Int(1,1,1));
    }

    void Update()
    {
        runner.Dispatch();
    }
}
```

Arguments:
1. `computeShader:` Compute shader reference.
2. `count:` Object count or texture resolution (ex: new Vector3Int(512, 512, 1)).
3. `moreThanOneKernel:` Boolean to control if there's more than one kernel.
4. `kernelCount:` How many kernels exist.
5. `kernels[]:` Kernel string array with kernel names.

### [BufferHolder](https://github.com/wTechnoo/ComputeHelper/blob/main/Runtime/BufferHolder.cs)
> This class handles generic data and ComputeBuffers

#### Basic usage:
```csharp
public class ExampleBufferHolder : MonoBehaviour
{
    public struct Example
    {
        Vector3 position;
        Matrix4x4 mat;
    }
    
    BufferHolder<Example> buffer;
    int count;
    
    void Start()
    {
        var data = new Example[count];
        for (int i = 0; i < count; i++)
        {
            data[i].position = Vector3.one * i;
            data[i].mat = Matrix4x4.zero;
        }
        
        buffer = new BufferHolder<Example>(data, "Buffer");
    }
}
```

Arguments:
1. `T[] data:` Array containing generic data.
2. `name:` Name inside ComputeShader.
3. `kernel:` Which kernel to set the buffer to (Default=0).

### [Instanced Renderer](https://github.com/wTechnoo/ComputeHelper/blob/main/Runtime/InstancedRenderer.cs)
> This class implements **Instanced Indirect** mesh rendering

#### Basic usage:
```csharp
public class ExampleInstancedRendering : MonoBehaviour
{
    public struct Example
    {
        Vector3 position;
        Matrix4x4 mat;
    }
    
    int count;
    Mesh mesh;
    Material material;
    BufferHolder<Example> buffer;
    InstancedRenderer renderer;
    
    void Start()
    {
        renderer = new InstancedRenderer(count, mesh, material, Vector3.zero);

        var data = new Example[count];
        for (int i = 0; i < count; i++)
        {
            data[i].position = Vector3.one*i;
            data[i].mat = Matrix4x4.TRS(data[i].position, Quaternion.identity, Vector3.one);
        }

        buffer = new BufferHolder<Example>(data, "Buffer");
        renderer.AddBuffer(buffer);
    }
    
    void Update()
    {
        renderer.Render();
    }
}
```

Arguments:
1. `instanceCount:` How many instances are going to be drawn.
2. `mesh:` Mesh to be drawn.
3. `material:` Shader that will draw the instanced meshes.
4. `boundCenter:` Where the center of the bound will be placed for rendering.
5. `boundSize:` Maximum bound size.

#### Shaderlab:
```HLSL
struct Example
{
    float3 position;
    float4x4 mat;
};

StructuredBuffer<Example> Buffer;

v2f vert(appdata v, uint instanceID : SV_InstanceID)
{
    v2f o;
    float4 position = mul(Buffer[instanceID].mat, v.vertex);
    o.vertex = UnityObjectToClipPos(position);
    o.uv = v.uv;
    return o;
}
```
Utilize `uint instanceID : SV_InstanceID` inside the arguments of the vert function to get the current instanced instance

### Instanced Rendering with Compute Runner
> Use both InstancedRendering together with ComputeRunner
#### Basic usage:
```csharp
public class ExampleInstancedRenderingCompute : MonoBehaviour
{
    public struct Example
    {
        Vector3 position;
        Matrix4x4 mat;
    }
    
    int count;
    Mesh mesh;
    Material material;
    
    ComputeShader computeShader;
    BufferHolder<Example> buffer;
    ComputeRunner runner;
    InstancedRenderer renderer;
    
    void Start()
    {
        runner = new ComputeRunner(computeShader, new Vector3Int(count, 1, 1));
        renderer = new InstancedRenderer(count, mesh, material, Vector3.zero);

        var data = new Example[count];
        for (int i = 0; i < count; i++)
        {
            data[i].position = Vector3.one*i;
            data[i].mat = Matrix4x4.TRS(data[i].position, Quaternion.identity, Vector3.one);
        }

        buffer = new BufferHolder<Example>(data, "Buffer");
        runner.AddBuffer(buffer);
        renderer.AddBuffer(buffer);
    }
    
    void Update()
    {
        runner.Dispatch();
        renderer.Render();
    }
}
```

#### Compute Shader:
```hlsl
#include "Packages/com.technoo.computehelper/Runtime/Computehelper.cginc"

struct Example
{
    float3 position;
    float4x4 mat;
};

RWStructuredBuffer<Example> Buffer;

[numthreads(8,1,1)]
void CSMain (uint3 id : SV_DispatchThreadID)
{
    Buffer[id.x].position = float3(id.x, 0, id.x);
    Buffer[id.x].mat = compose(GrassBuffer[id.x].position);
}
```

`Packages/com.technoo.computehelper/Runtime/Computehelper.cginc` contains the compose function that builds a 4x4 matrix (position, rotation quaternion, scale)

#### Shaderlab:
```HLSL
struct Example
{
    float3 position;
    float4x4 mat;
};

StructuredBuffer<Example> Buffer;

v2f vert(appdata v, uint instanceID : SV_InstanceID)
{
    v2f o;
    float4 position = mul(Buffer[instanceID].mat, v.vertex);
    o.vertex = UnityObjectToClipPos(position);
    o.uv = v.uv;
    return o;
}
```
