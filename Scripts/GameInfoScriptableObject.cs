using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameSO", fileName = "GameSO.asset")]
[System.Serializable]
public class GameInfoScriptableObject : ScriptableObject {

    static public GameInfoScriptableObject S; // This Scriptable Object is an unprotected Singleton

    public GameInfoScriptableObject()
    {
        S = this;
    }

    [Tooltip("Players starting Life")]
    public int startPlayerLife = 3;
    [Tooltip("How large of an area to check around new jump location to check for asteroids")]
    public float playerSafetyRadius = 2f;
    [Tooltip("How long after safety check before jumping to new location")]
    public float jumpDelay = 0.1f;
    [Tooltip("Time before restarting the Game")]
    public float timeToReset = 4f;

}
