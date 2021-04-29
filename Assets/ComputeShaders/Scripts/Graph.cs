using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    private Transform pointPrefab = default;
    [SerializeField, Range(10, 100)]
    private int resolution = 10;
    [SerializeField, Min(.1f)]
    private float functionDuration = 1f;
    [SerializeField]
    private bool interpolate = false;
    [SerializeField]
    private Function function = Function.Wave;

    private Transform[] points;
    private float step;
    private float duration;

    private Function nextFunction = Function.MultiWave;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        points = new Transform[resolution * resolution];
        step = 2f / resolution;
        Vector3 scale = Vector3.one * step;

        for(int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab, transform);
            point.localScale = scale;
            points[i] = point;
        }
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

        UpdateFunction();
    }

    void UpdateFunction()
    {
        for(int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if(x == resolution)
            {
                x = 0;
                z++;
            }

            Transform point = points[i];
            float u = (x + .5f) * step - 1f;
            float v = (z + .5f) * step - 1f;

            if(interpolate)
            {
                Vector3 current = FunctionLibrary.GetOffsetByFunction(u, v, Time.time, function);
                Vector3 next = FunctionLibrary.GetOffsetByFunction(u, v, Time.time, nextFunction);

                point.localPosition = Vector3.Lerp(current, next, Mathf.Clamp01(duration / functionDuration));
            }
            else
            {
                point.localPosition = FunctionLibrary.GetOffsetByFunction(u, v, Time.time, function);
            }
        }
    }
}
