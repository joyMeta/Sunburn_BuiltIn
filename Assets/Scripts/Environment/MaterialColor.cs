using UnityEngine;

public class MaterialColor : MonoBehaviour {
    Material mat;
    [SerializeField]
    Color color;

    [SerializeField]
    string mapString;

    [SerializeField]
    int materialIndex = 0;

    public void Start() {
        mat = GetComponent<MeshRenderer>().materials[materialIndex];
        mat.SetColor(mapString, color * 2);
    }
}