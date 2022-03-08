using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShaderScreenSizeRefresher : MonoBehaviour
{
    [SerializeField] private KeyCode nextShaderTrigger = KeyCode.Tab;
    [SerializeField] private ComputeShader[] shaders;
    [SerializeField] private RawImage uiImage;

    public Vector2 trianglePointA, trianglePointB, trianglePointC;
    [SerializeField, Range(1, 15)] private int iterationsCount = 5;
    [SerializeField, Range(1, 10)] private int samplesCount = 1;

    private WindowResizeListener _windowResizeListener;
    private RenderTexture _targetTexture;
    private Vector2Int _dimensions;
    private int _shaderIndex;
    
    private int screenWidth, screenHeight;
    private int triA, triB, triC;
    private int time, iterations, samples;
    private int result;


    private void Awake()
    {
        _windowResizeListener = FindObjectOfType<WindowResizeListener>();

        screenWidth = Shader.PropertyToID(nameof(screenWidth));
        screenHeight = Shader.PropertyToID(nameof(screenHeight));
        result = Shader.PropertyToID(nameof(result));

        triA = Shader.PropertyToID(nameof(triA));
        triB = Shader.PropertyToID(nameof(triB));
        triC = Shader.PropertyToID(nameof(triC));
        time = Shader.PropertyToID(nameof(time));
        iterations = Shader.PropertyToID(nameof(iterations));
        samples = Shader.PropertyToID(nameof(samples));
        
        _windowResizeListener.OnWindowResized += OnWindowResized;
    }

    private void OnWindowResized(Vector2Int dimensions)
    {
        _dimensions = dimensions;
        
        if(_targetTexture != null && 
           _targetTexture.IsCreated())
           _targetTexture.Release();

        _targetTexture = new RenderTexture(
            dimensions.x, dimensions.y, 0)
        {
            enableRandomWrite = true,
            useMipMap = false,
            dimension = TextureDimension.Tex2D,
        };
        _targetTexture.Create();

        uiImage.texture = _targetTexture;
    }

    private void Update()
    {   
        if (Input.GetKeyDown(nextShaderTrigger))
        {
            _shaderIndex++;
            _shaderIndex %= shaders.Length;
            
            Debug.Log($"Current shader: {_shaderIndex}: {shaders[_shaderIndex].name}");
        }
        
        var shader = shaders[_shaderIndex];
        
        shader.SetInt(screenWidth, _dimensions.x);
        shader.SetInt(screenHeight, _dimensions.y);
        shader.SetTexture(0, result, _targetTexture);
        
        shader.SetVector(triA, trianglePointA);
        shader.SetVector(triB, trianglePointB);
        shader.SetVector(triC, trianglePointC);
        
        shader.SetFloat(time, Time.time);
        shader.SetInt(samples, samplesCount);
        shader.SetInt(iterations, iterationsCount);
        
        shader.Dispatch(
            0, 
            _dimensions.x / 8 + 1,
            _dimensions.y / 8 + 1,
            1);


        if (Input.GetKeyDown(KeyCode.UpArrow)) iterationsCount++;
        if (Input.GetKeyDown(KeyCode.DownArrow)) iterationsCount--;
        if (Input.GetKeyDown(KeyCode.RightArrow)) samplesCount++;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) samplesCount--;
    }

    private void OnGUI()
    {
        GUILayout.Label(
            $"{shaders[_shaderIndex].name}  " +
            $"Iterations: {iterationsCount}/px  " +
            $"Samples: {samplesCount}x{samplesCount} ({samplesCount*samplesCount}/px) " +
            $"Total: {samplesCount*samplesCount*iterationsCount}/px");
    }
}