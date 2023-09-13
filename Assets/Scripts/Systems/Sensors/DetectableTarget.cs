using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DetectableTargetManager.Instance.RegisterDetectableTarget(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        DetectableTargetManager.Instance.UnregisterDetectableTarget(this);
    }
}
