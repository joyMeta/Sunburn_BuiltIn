using UnityEngine;

[CreateAssetMenu(menuName = "Attractor/LorentzAttractor")]
public class LorentzAttractor : AttractorBase {

    public float rho = 28;
    public float sigma = 10;
    public float beta = 8 / 3;

    public override Vector3 AttractorSolver(Vector3 position) {
        Vector3 newPosition = Vector3.zero;
        newPosition.x = sigma * (position.y - position.x);
        newPosition.y = position.x*(rho - position.z) - position.y;
        newPosition.z = position.x *position.y-beta*position.z;
        return newPosition/10;
    }
}
