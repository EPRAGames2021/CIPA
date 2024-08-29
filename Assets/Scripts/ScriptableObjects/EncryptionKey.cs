using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encryption Key", menuName = "Scriptable Objects/Encryption", order = 1)]
public class EncryptionKey : ScriptableObject
{
    [SerializeField] private string _key;

    public string Key => _key;
}
