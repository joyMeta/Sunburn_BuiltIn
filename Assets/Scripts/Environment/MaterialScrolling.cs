using UnityEngine;

public class MaterialScrolling : MonoBehaviour
{
    Material mat;
    public Vector2 scrollFactor;
    Vector2 offset;

    [SerializeField]
    string mapString;

    [SerializeField]
    int materialIndex = 0;

    public void Awake() {
        mat=GetComponent<MeshRenderer>().materials[materialIndex];
    }

    public void LateUpdate() {
        offset += scrollFactor * Time.deltaTime;
        mat.SetTextureOffset(mapString, offset);
    }
}
