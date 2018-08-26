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

    [SerializeField]
    private Transform opponentSpawnPoint;

    private Camera cam;
    public Camera Camera { get { return cam; } }

    private int opponentCount;
    private float textWait;
    private int roundNum;


    public bool WinState { get; set; }

    public bool Ddebug { get; set; }
    private int round;

    private int hits2WinRound;
    public int Hits2WinRound { get { return hits2WinRound; } }

    private float playTime2WinGame;
    public float PlayTime2WinGame { get { return playTime2WinGame; } }

    // Use this for initialization
    private void Start ( )
    {
        instance = this;

        textWait = 1.0f;
        opponentCount = 30;

        WinState = false;
        Ddebug = true;
        round = 0;
        hits2WinRound = 3;
        playTime2WinGame = 20.0f;

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
        cam.transform.position = player.transform.position - 10.0f * Vector3.forward;
        cam.transform.rotation *= Quaternion.LookRotation ( player.transform.position - cam.transform.position );
    }

    private void InitOpponentLord ( )
    {
        opponentLord = new GameObject ( ).AddComponent<OpponentLord> ( );
        opponentLord.gameObject.name = "OpponentLord";
        opponentLord.Init ( opponentCount, opponentPrefab, opponentSpawnPoint );
        opponentLord.DisableOpponents ( );
    }

    // Update is called once per frame
    private void Update ( )
    {

    }

    private IEnumerator GameLoop ( )
    {
        yield return StartCoroutine ( RoundStarting ( ) );

        yield return StartCoroutine ( RoundPlaying ( ) );

        if ( Ddebug )
        {
            Debug.Log ( "got here" );
        }

        yield return StartCoroutine ( RoundEnding ( ) );

        if ( WinState )
        {
            // Ending animation 
            StopAllCoroutines ( ); // HACKL3Y: Why isn't this stopping the ReleaseOpponent call?
            SceneManager.LoadScene ( 0 );
            
        }
        else
        {
            // Accumulate points / growth? 
            StartCoroutine ( GameLoop ( ) );
        }
    }

    private IEnumerator RoundStarting ( )
    {
        player.Reset ( );
        player.DisablePlayer ( );

        opponentLord.Reset ( );
        opponentLord.DisableOpponents ( );

        // set cam pos
        roundNum++;

        round++;
        // display round num text
        print ( "Round " + round );

        yield return textWait;
    }

    private IEnumerator RoundPlaying ( )
    {
        player.EnablePlayer ( );
        opponentLord.EnableOpponents ( );

        // Make text empty

        while ( !player.LostRound )
        {
            yield return null;
        }

    }

    private IEnumerator RoundEnding ( )
    {
        player.DisablePlayer ( );

        opponentLord.DisableOpponents ( );

        // Display round score
        print ( "Player health " + Player.Hits + ", you won = " + WinState );

        yield return textWait;
    }
}
