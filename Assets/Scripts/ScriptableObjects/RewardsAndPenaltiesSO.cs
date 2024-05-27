using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rewards", menuName = "Scriptable Objects/Reward", order = 1)]

public class RewardsAndPenaltiesSO : ScriptableObject
{
    [SerializeField] private bool _isPenalty;
    [SerializeField] private int _score;

    public bool IsPenalty => _isPenalty;
    public int Score => _score;
}
