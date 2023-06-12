using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnStyle {
    RoundRobin,
    Random
}

public class NPCHandler : MonoBehaviour
{
    private List<Transform> spawnPoints=new List<Transform>();
    public List<GameObject> npcTransforms= new List<GameObject>();
    [Range(1, 22)]
    [SerializeField]
    int npcsToSpawn;
    [SerializeField]
    SpawnStyle spawnStyle;

    public List<Animator> animators = new List<Animator>();

    private void Start() {
        foreach (Transform child in transform) {
            spawnPoints.Add(child);
        }
        switch (spawnStyle) {
            case SpawnStyle.RoundRobin: {
                    for (int i = 0; i < npcsToSpawn; i++) {
                        GameObject go = Instantiate(npcTransforms[Random.Range(0, npcTransforms.Count - 1)], spawnPoints[i].position, Quaternion.identity);
                        animators.Add(go.GetComponent<Animator>());
                    }
                    break;
                }
                case SpawnStyle.Random: {
                    for (int i = 0; i < npcsToSpawn; i++) {
                        GameObject go = Instantiate(npcTransforms[Random.Range(0, npcTransforms.Count - 1)], spawnPoints[Random.Range(0, spawnPoints.Count - 1)].position, Quaternion.identity);
                        animators.Add(go.GetComponent<Animator>());
                    }
                    break;
                }
        }
    }
}
