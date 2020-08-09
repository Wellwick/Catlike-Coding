using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph3 : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;

    Transform[] points;

    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        points = new Transform[resolution];
        for (int i = 0; i < points.Length; ++i) {
            points[i] = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            points[i].localPosition = position;
            points[i].localScale = scale;
            points[i].SetParent(transform, false);
        }
    }

    private void Update()
    {
        float t = Time.time;
        for (int i = 0; i < points.Length; i++) {
            Vector3 position = points[i].localPosition;
            position.y = MultiSineFunction(position.x, t);
            points[i].localPosition = position;
        }
    }

    private float SineFunction(float x, float t)
    {
        return Mathf.Sin(Mathf.PI * (x + t));
    }

    private float MultiSineFunction(float x, float t)
    {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (x + t)) / 2f;
        y *= 2f / 3f;
        return y;
    }
}
