using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexTransform : MonoBehaviour
{
    public float framesPerTransition;
    public bool isScaledDown;
    public bool canTransition;

    private Mesh mesh;
    private Vector3[] currentVertices;

    private int completedFrames;
    private Vector3[] stepSizes;
    private float stepMultiplier;

    private Vector3 SCALED_DOWN_VERTEX_POINT = Vector3.zero;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        currentVertices = mesh.vertices;

        completedFrames = 0;
        stepMultiplier = isScaledDown ? 1.0f : -1.0f;

        CalculateStepSizes();
        if (isScaledDown) SetVerticesAtZeroPoint();
    }

    void CalculateStepSizes()
    {
        int numVertices = currentVertices.Length;
        stepSizes = new Vector3[numVertices];
        for (var j = 0; j < numVertices; j++)
        {
            Vector3 offset = currentVertices[j] - transform.InverseTransformPoint(SCALED_DOWN_VERTEX_POINT);
            stepSizes[j] = offset / framesPerTransition;
        }
    }

    void SetVerticesAtZeroPoint()
    {
        for (var i = 0; i < currentVertices.Length; i++)
        {
            currentVertices[i] = transform.InverseTransformPoint(SCALED_DOWN_VERTEX_POINT);
        }
        mesh.vertices = currentVertices;
        mesh.RecalculateBounds();
    }

    void Update()
    {
        if (!canTransition) return;
        UpdateVertices();
    }

    void UpdateVertices()
    {
        for (var i = 0; i < currentVertices.Length; i++)
        {
            currentVertices[i] += (stepSizes[i] * stepMultiplier);
        }
        completedFrames++;
        if (completedFrames == framesPerTransition) UpdateTransitionState();

        mesh.vertices = currentVertices;
        mesh.RecalculateBounds();
    }

    void UpdateTransitionState()
    {
        isScaledDown = !isScaledDown;
        completedFrames = 0;

        if (!isScaledDown && !InteractionManager.GetInstance().cyclesAreComplete)
        {
            stepMultiplier *= -1.0f;
        } else
        {
            canTransition = false;
            InteractionManager.GetInstance().ProcessCompletedTransformCycle();
        }
    }
}

