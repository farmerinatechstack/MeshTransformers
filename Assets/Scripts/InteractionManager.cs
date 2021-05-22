using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Text;
    public int numCycles;
    public bool cyclesAreComplete;
    public bool runEndAnimation;
    public VertexScaling mainMeshTransformer;
    public VertexScaling[] otherMeshTransformers;
    public static InteractionManager GetInstance()
    {
        return instance;
    }
    
    private static void SetInstance(InteractionManager value)
    {
        instance = value;
    }
    private static InteractionManager instance;
    private VertexScaling currentMesh;
    private int completedCycles;

    // Start is called before the first frame update
    void Awake()
    {
        if (GetInstance() != null && GetInstance() != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SetInstance(this);
        }

        completedCycles = 0;
        cyclesAreComplete = (completedCycles == numCycles);
        runEndAnimation = false;
        mainMeshTransformer.isScaledDown = true;
        mainMeshTransformer.canTransition = false;

        foreach (VertexScaling vt in otherMeshTransformers)
        {
            vt.isScaledDown = true;
            vt.canTransition = false;
        }

        RunSingleMeshTransformCycle();
    }

    private void Update()
    {
        if (runEndAnimation)
        {
            if (Camera.transform.position.x < 1.1)
            {
                Camera.transform.Translate(Vector3.right * Time.deltaTime * 0.75f);
            } else
            {
                Text.SetActive(true);
            }
        }
    }
    void RunSingleMeshTransformCycle()
    {
        if (completedCycles == numCycles) return;
        currentMesh = otherMeshTransformers[Random.Range(0, otherMeshTransformers.Length)];
        currentMesh.canTransition = true;
    }

    public void ProcessCompletedTransformCycle()
    {
        completedCycles++;
        cyclesAreComplete = (completedCycles == numCycles);
        if (currentMesh == mainMeshTransformer)
        {
            runEndAnimation = true;
        } else
        {
            if (completedCycles == numCycles)
            {
                currentMesh = mainMeshTransformer;
                currentMesh.canTransition = true;
            }
            else
            {
                RunSingleMeshTransformCycle();
            }
        }
    }

    public void RunEndAnimation()
    {

    }
}
