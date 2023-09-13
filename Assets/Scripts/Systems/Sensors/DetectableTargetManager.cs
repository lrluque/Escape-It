using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableTargetManager : MonoBehaviour
{

    public static DetectableTargetManager Instance { get; private set; } = null;
    public List<DetectableTarget> AllTargets { get; private set; } = new List<DetectableTarget>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one DetectableTargetManager in the scene.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterDetectableTarget(DetectableTarget detectableTarget)
    {
        AllTargets.Add(detectableTarget);
    }

    public void UnregisterDetectableTarget(DetectableTarget detectableTarget)
    {
        AllTargets.Remove(detectableTarget);
    }
}
