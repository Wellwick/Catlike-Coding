using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10,100)]
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
        for (int i = 0; i < points.Length; i++) {
            Vector3 position = points[i].localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            points[i].localPosition = position;
        }
    }
}
