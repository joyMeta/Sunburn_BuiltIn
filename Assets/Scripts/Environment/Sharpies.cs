using UnityEngine;

[System.Serializable]
public enum RotationMode {
    FullRotatation,
    PingPong
}

public class Sharpies : MonoBehaviour {
    [SerializeField] float _speed;
    [SerializeField] Vector3 _axis;
    public RotationMode mode;
    [SerializeField]
    Vector2 rangeVector;
    [SerializeField]
    bool random = false;


    private void Start() {
        _speed += Random.Range(-5f, 5f);
        if (random) {
            switch (mode) {
                case RotationMode.FullRotatation: {
                        transform.localRotation = Quaternion.Euler(-90f, -270f, Random.Range(0, 360));
                        break;
                    }
                case RotationMode.PingPong: {
                        transform.localRotation = Quaternion.Euler(Random.Range(30, -210), 0, 0);
                        break;
                    }
            }
        }
    }

    private void Update() {
        switch (mode) {
            case RotationMode.FullRotatation: {
                    transform.Rotate(Time.deltaTime * _speed * _axis);
                    break;
                }
            case RotationMode.PingPong: {
                    float xValue = (Mathf.PingPong(Time.time * _speed, 240) - 30)*-1f;
                    xValue=Mathf.Clamp(xValue,rangeVector.x,rangeVector.y);
                    transform.localRotation = Quaternion.Euler(xValue, 0, 0);
                    break;
                }
        }
    }
}