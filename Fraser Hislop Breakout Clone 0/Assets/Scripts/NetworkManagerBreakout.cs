using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerBreakout : NetworkManager
{
    public Transform player1Spawn;
    public Transform player2Spawn;
    GameObject ball;
    Ball ballScript;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? player1Spawn : player2Spawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        // spawn ball for each player
        ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
        NetworkServer.Spawn(ball);

        // Assign Player its ball and ball its Player
        Paddle paddleScript = player.GetComponent<Paddle>();
        ballScript = ball.GetComponent<Ball>();

        paddleScript.ball = ballScript;
        ballScript.paddle = paddleScript;
        ballScript.startY = start.position.y + 1.5f;

        GameController.Instance.AddBall(ballScript);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // destroy ball
        if (ball != null)
        {
            GameController.Instance.RemoveBall(ballScript);

            NetworkServer.Destroy(ball);
        }

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }
}
