using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph3 : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;
    public GraphFunctionName function;

    Transform[] points;
    GraphFunction[] functions =
    {
        SineFunction,
        Sine2DFunction, 
        MultiSineFunction,
        MultiSine2DFunction, 
        Ripple
    };

    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        points = new Transform[resolution * resolution];
        for (int i = 0, z = 0; z < resolution; z++) {
            position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++) {
                points[i] = Instantiate(pointPrefab);
                position.x = (x + 0.5f) * step - 1f;
                points[i].localPosition = position;
                points[i].localScale = scale;
                points[i].SetParent(transform, false);
            }
        }
    }

    private void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        for (int i = 0; i < points.Length; i++) {
            Vector3 position = points[i].localPosition;
            position.y = f(position.x, position.z, t);
            points[i].localPosition = position;
        }
    }

    const float pi = Mathf.PI;

    private static float SineFunction(float x, float z, float t)
    {
        return Mathf.Sin(pi * (x + t));
    }

    private static float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) * 0.5f;
        y *= 2f / 3f;
        return y;
    }

    private static float Sine2DFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }

    private static float MultiSine2DFunction(float x, float z, float t)
    {
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        return y;
    }

    private static float Ripple(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * d - t));
        y /= 1f + 10f * d;
        return y;
    }
}
