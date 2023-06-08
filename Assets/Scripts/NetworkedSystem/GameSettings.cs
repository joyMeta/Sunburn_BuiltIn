using UnityEngine;

[CreateAssetMenu(menuName = "Server Settings/Room Settings")]
public class GameSettings : ScriptableObject {
    public byte MAXPLAYERS = 4;
    public byte MINPLAYERS = 2;
}