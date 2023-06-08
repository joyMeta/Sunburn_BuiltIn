using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour {

  public List<AttractorBase> attractor=new();
    [SerializeField]
    int index;
    Vector3 lastPosition;

    private void Start() {
        lastPosition = transform.localPosition;
    }

    public void Update() {
        transform.localPosition += attractor[index].AttractorSolver(lastPosition)*Time.deltaTime;
        lastPosition = transform.localPosition;
    }
}