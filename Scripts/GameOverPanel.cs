using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : ActiveOnlyDuringSomeGameStates {

    static private GameOverPanel _GO;
    static public GameOverPanel GO
    {
        get
        {
            return _GO;
        }
        private set
        {
            if (_GO != null)
            {
                Debug.LogWarning("Second attemt to set ScoreManager _JM.");
            }
            _GO = value;
        }
    }



    private float timedReset = 4f;
    private Animator anim;


    override public void Awake () {
        GO = this;
        timedReset = PlayerShip.GameSO.timeToReset;
        anim = GetComponent<Animator>();

        gameObject.SetActive(false);

        base.Awake();
    }

    private void OnEnable()
    {
        print("game over active!!!!!!!~~~~~~~~~~~~~~");
    }

    protected override void DetermineActive()
    {
        base.DetermineActive();
        if (AsteraX.GAME_STATE == AsteraX.eGameState.gameOver)
        {
            // This should only happen when the game is over
            GO.RestartNow();
        }
    }


    static public void GameOver()
    {
        print("restart now");
        GO.RestartNow();
    }

    public void RestartNow()
    {
        GO.StartCoroutine(Redo());
    }

    IEnumerator Redo()
    {
        //trigger Gameover panel Animations
        anim.SetTrigger("GameOver");
        yield return new WaitForSeconds(timedReset);
        print("Loading Scene");
        SceneManager.LoadScene(0);
        yield return null;
    }
}
