using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    static readonly int positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    [Header("Rendering / Shaders")]
    [SerializeField]
    private ComputeShader functionLib = default;
    [SerializeField]
    private Material material = default;
    [SerializeField]
    private Mesh mesh = default;

    [Header("Settigns")]
    [SerializeField, Range(10, 1000)]
    private int resolution = 10;
    [SerializeField, Min(.1f)]
    private float functionDuration = 1f;
    [SerializeField]
    private bool interpolate = false;
    [SerializeField]
    private Function function = Function.Wave;

    private ComputeBuffer positionBuffer;

    private float step;
    private float duration;

    private Function nextFunction = Function.MultiWave;

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        positionBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    // This function is called when the behaviour becomes disabled or inactive
    private void OnDisable()
    {
        positionBuffer.Release();
        positionBuffer = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(interpolate)
        {
            duration += Time.deltaTime;
            if(duration >= functionDuration)
            {
                duration -= functionDuration;
                function = nextFunction;
                nextFunction = FunctionLibrary.GetNextFunction(function);
            }
        }

        UpdateFunctionOnGPU();
    }

    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        functionLib.SetInt(resolutionId, resolution);
        functionLib.SetFloat(stepId, step);
        functionLib.SetFloat(timeId, Time.time);

        functionLib.SetBuffer(0, positionsId, positionBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        functionLib.Dispatch(0, groups, groups, 1);
        //Vector3[] data = new Vector3[resolution * resolution];
        //positionBuffer.GetData(data);
        //Debug.Log(data[0].ToString());

        material.SetBuffer(positionsId, positionBuffer);
        material.SetFloat(stepId, step);
        Bounds bounds = new Bounds(Vector3.zero, new Vector3(2f, 2f + 2f / resolution, 2f));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionBuffer.count);
    }
}
