using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameLord : MonoBehaviour
{
    private static GameLord instance;
    public static GameLord Instance { get { return instance; } }

    [SerializeField]
    private GameObject playerPrefab;
    private PlayerLord player;
    public PlayerLord Player { get { return player; } }

    [SerializeField]
    private GameObject opponentPrefab;
    private OpponentLord opponentLord;
    public OpponentLord OpponentLord { get { return opponentLord; } }

    [SerializeField]
    private Transform playerSpawnPoint;
    public Transform PlayerSpawnPoint { get { return playerSpawnPoint; } }

    private Camera cam;
    public Camera Camera { get { return cam; } }

    private int opponentCount;
    private float textWait;

    public enum GameStates { Default, Playing, Lost, Won };
    public GameStates GameState;

    public bool Ddebug { get; set; }

    private float timer;
    private float timerDur = 60.0f / 20.0f;

    [SerializeField]
    private bool test = false;

    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        textWait = 1.0f;
        opponentCount = 30;
        Ddebug = true;

        timer = timerDur;
        GameState = GameStates.Default;

        InitPlayer ( );
        InitOpponentLord ( );
        
        InitCam ( );

        // Start Game.
        StartCoroutine ( GameLoop ( ) );
    }

    private void InitPlayer ( )
    {
        GameObject temp = Instantiate ( playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation );
        if ( temp.GetComponent<PlayerLord> ( ) != null )
        {
            player = temp.GetComponent<PlayerLord> ( );
            player.Init ( );
        }
        else
        {
            Debug.LogError ( "PlayerLord component not found." );
        }
    }

    private void InitCam ( )
    {
        cam = Camera.main;

        // Set pos relative to player. 
        cam.transform.position = player.transform.position - 15.0f * Vector3.forward + 5.0f * Vector3.up;
        // cam.transform.rotation *= Quaternion.LookRotation ( player.transform.position - cam.transform.position );
    }

    private void InitOpponentLord ( )
    {
        opponentLord = new GameObject ( ).AddComponent<OpponentLord> ( );
        opponentLord.gameObject.name = "OpponentLord";
        opponentLord.Init ( opponentCount, opponentPrefab );
        opponentLord.DisableOpponents ( );
    }

    private void OnBeat ( )
    {
        if ( Ddebug )
        {
            Debug.Log ( "OnBeat called." );
        }
        player.OnBeat ( );
        opponentLord.OnBeat ( );
    }


    // Update is called once per frame
    private void Update ( )
    {
        if ( GameState != GameStates.Playing )
        {
            return;
        }

        timer -= Time.deltaTime;
        if(timer < 0.0f )
        {
            timer = timerDur;

            OnBeat ( );
        }

        player.UpdatePlayerLord ( );
        opponentLord.UpdateOpponentLord ( );
    }

    private IEnumerator GameLoop ( )
    {
        timer = timerDur;
        GameState = GameStates.Default;

        yield return StartCoroutine ( GameStarting ( ) );

        yield return StartCoroutine ( GamePlaying ( ) );

        yield return StartCoroutine ( GameEnding ( ) );

        StopAllCoroutines ( ); // HACKL3Y: Why isn't this stopping the ReleaseOpponent call?

        if ( GameState == GameStates.Won )
        {
            // Restart the scene.
            SceneManager.LoadScene ( 0 );

        }
        else
        {
            // Restart the game. 
            StartCoroutine ( GameLoop ( ) );
            
        }
    }

    private IEnumerator GameStarting ( )
    {
        
        player.Reset ( );
        player.DisablePlayer ( );

        opponentLord.Reset ( );
        opponentLord.DisableOpponents ( );

        if ( test )
        {
            opponentLord.gameObject.SetActive ( false );
        }

        print ( "It's your birthday. Let's play." );
        GameState = GameStates.Playing;

        yield return textWait;
    }

    private IEnumerator GamePlaying ( )
    {
        player.EnablePlayer ( );
        opponentLord.EnableOpponents ( );

        while ( GameState != GameStates.Lost )
        {
            yield return null;
        }
    }

    private IEnumerator GameEnding ( )
    {

        player.DisablePlayer ( );
        opponentLord.DisableOpponents ( );

        // Display round score
        if ( GameState == GameStates.Won )
        {
            // PLAY ENDING ANIMATION.
            // Fade out.
            print ( "You're all grown up." );
        }
        else
        {
            print ( "And now we're going home." );
        }


        yield return textWait;
    }
}
