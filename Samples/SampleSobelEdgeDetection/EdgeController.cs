using ComputeHelper;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
    [SerializeField] ComputeShader shader;
    [SerializeField] Material material;
    [SerializeField] Texture2D tex;
    
    [SerializeField] [Range(0, 6)] int selectedStep = 0;
    [SerializeField] [Range(0, 1f)] float threshold;
    [SerializeField] bool additive;
    [SerializeField] [ColorUsage(false, true)] Color color;

    ComputeRunner runner;
    
    const int num = 7;
    RenderTexture[] textures;
    void Awake()
    {
        runner = new ComputeRunner(shader, new Vector3Int(tex.width, tex.height, 1), new []{ "Greyscale", "HorzSobel", "VerSobel", "FullSobel", "Threshold", "Final" });

        textures = new RenderTexture[num];
        for (int i = 0; i < num; i++)
        {
            textures[i] = new RenderTexture(tex.width, tex.height, 0);
            textures[i].enableRandomWrite = true;
            textures[i].Create();
        }
        
        Graphics.Blit(tex, textures[0]);
        
        runner.AddTexture(0, "originalTex", textures[0]);
        runner.AddTexture(0, "greyscaleTex", textures[1]);
        
        runner.AddTexture(1, "greyscaleTex", textures[1]);
        runner.AddTexture(1, "horzSobelTex", textures[2]);
        
        runner.AddTexture(2, "greyscaleTex", textures[1]);
        runner.AddTexture(2, "verSobelTex", textures[3]);
        
        runner.AddTexture(3, "horzSobelTex", textures[2]);
        runner.AddTexture(3, "verSobelTex", textures[3]);
        runner.AddTexture(3, "sobelTex", textures[4]);
        
        runner.AddTexture(4, "sobelTex", textures[4]);
        runner.AddTexture(4, "thresholdTex", textures[5]);

        runner.AddTexture(5, "originalTex", textures[0]);
        runner.AddTexture(5, "thresholdTex", textures[5]);
        runner.AddTexture(5, "finalTex", textures[6]);
    }
    
    void Update()
    {
        material.mainTexture = textures[selectedStep];
        runner.SetVar("threshold", threshold);
        runner.SetVar("additive", additive);
        runner.SetVar("color", color);
        
        runner.DispatchAll();
    }
}
