using Photon.Realtime;
using UnityEditor;

[CustomPropertyDrawer(typeof(PlayerDictionary))]
public class CustomDictionaryDrawer : DictionaryDrawer<int, Player> { }
