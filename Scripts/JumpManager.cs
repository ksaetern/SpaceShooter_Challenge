#define DEBUG_SAFE_JUMPS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if DEBUG_SAFE_JUMPS
using UnityEditor;
#endif

public class JumpManager : MonoBehaviour {

    static private JumpManager _JM;
    static public JumpManager JM
    {
        get
        {
            return _JM;
        }
        private set
        {
            if (_JM != null)
            {
                Debug.LogWarning("Second attemt to set ScoreManager _JM.");
            }
            _JM = value;
        }
    }

    [Tooltip("Player Gameobject")]
    public GameObject playerShip;
    [Tooltip("Gameobject with sphere collider to detect incoming asteroids")]
    public JumpSense jumpSense;
    [Tooltip("Jumps Remaing GUI text")]
    [SerializeField] Text jumpText;
    private int jumpLives = 3;
    private float jumpDelay = 0.1f;
    private bool jumping = false;

#if DEBUG_SAFE_JUMPS
    private Vector3 prevJumpSpot;
#endif

    private void Awake()
    {
        JM = this;
        
    }

    private void Start()
    {
        JM.jumpLives = PlayerShip.GameSO.startPlayerLife;
        JM.jumpDelay = PlayerShip.GameSO.jumpDelay;
    }

    static public void LoseLife()
    {
        //Wait until safe jump is establish before letting the player get damaged again.
        if (JM.jumping)
            return;

        JM.jumping = true;
        JM.playerShip.SetActive(false);
        JM.jumpLives -= 1;
        print("Lives = " + JM.jumpLives);
        JM.jumpText.text = JM.jumpLives.ToString() + " Jumps";

        //If lives remain, jump to safe location or else GameOverScreen
        if (JM.jumpLives != 0)
            JM.RunJump();
        else
            JM.RunGameOver();
    }


    private void RunJump()
    {
        StartCoroutine(JumpGates());
    }

    IEnumerator JumpGates()
    {
        //Keep moving the jumpsense gameobject until a safe location is established

       //move the jumpsense to the player death spot for better debug lines.
       jumpSense.transform.position = JM.playerShip.transform.position;
       int jumpTime = 0;
       do
       {
   
        #if DEBUG_SAFE_JUMPS
            print("Jump! " + (jumpTime += 1).ToString());
            prevJumpSpot = jumpSense.transform.position;
        #endif

            jumpSense.Danger = false;
            jumpSense.transform.position = ScreenBounds.RANDOM_ON_SCREEN_LOC;
            jumpSense.gameObject.SetActive(true);

        #if DEBUG_SAFE_JUMPS
            //track where the different jump attempts
            print("Waiting for danger");
            Debug.DrawLine(prevJumpSpot, jumpSense.transform.position, Color.cyan, 1f);
        #endif

            yield return new WaitForSeconds(JM.jumpDelay);
       } while (jumpSense.IsDanger());
       
        //safe location found, turn off jumpsense object and turn on the player object
        JM.playerShip.transform.position = jumpSense.transform.position;
        jumpSense.gameObject.SetActive(false);
        JM.playerShip.SetActive(true);
        JM.jumping = false;
    }

    private void RunGameOver()
    {
        ScoreManager.FinalScore();
        AsteraX.GameOver();

    }

}
