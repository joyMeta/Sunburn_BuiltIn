using UnityEngine;

public abstract class AttractorBase : ScriptableObject {
    public abstract Vector3 AttractorSolver(Vector3 position);
}