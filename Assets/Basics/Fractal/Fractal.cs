using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh[] meshes;
    public Material material;

    public float childScale;
    public float waitTime = 0.3f;

    public float spawnProbability;

    public int maxDepth;

    private int depth;

    public float maxRotationSpeed;

    private float rotationSpeed;

    public float maxTwist;

    private Material[,] materials;

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1,3];
        for (int i = 0; i <= maxDepth; i++) {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
            materials[i, 2] = new Material(material);
            materials[i, 2].color = Color.Lerp(Color.white, Color.red, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.green;
        materials[maxDepth, 2].color = Color.black;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (materials == null) {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = 
            meshes[Random.Range(0,meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = 
            materials[depth, Random.Range(0,3)];
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if (depth < maxDepth) {
            StartCoroutine(CreateChildren());
        }
    }

    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++) {
            if (Random.value < spawnProbability) {
                yield return new WaitForSeconds(Random.Range(0.1f, waitTime));
                new GameObject("Fractal Child").AddComponent<Fractal>().
                    Initialize(this, i);
            }
        }
    }

    private void Initialize(Fractal parent, int index)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[index] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[index];
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
