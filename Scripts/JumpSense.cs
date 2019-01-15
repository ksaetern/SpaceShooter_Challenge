#define DEBUG_SAFE_JUMPS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[RequireComponent(typeof(SphereCollider))]
public class JumpSense : MonoBehaviour {

    //Use Sphere trigger on gameobject to determine safe landing area for player


    public bool Danger = false;
    private float dangerSenseRadius = 1.5f;

    private void Awake()
    {
        //Let the Scriptable object populate information
        dangerSenseRadius = PlayerShip.GameSO.playerSafetyRadius;
        GetComponent<SphereCollider>().radius = dangerSenseRadius;
    }


    //if asteroid lands in the trigger area, its not safe to Jump
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            Danger = true;
            print("Asteroid in safe jump zone");
        }
    }

    public bool IsDanger()
    {
        return Danger;
    }

#if DEBUG_SAFE_JUMPS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, dangerSenseRadius);
    }
#endif
}
