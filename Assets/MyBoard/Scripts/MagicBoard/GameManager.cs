using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace MagicBoard
{
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// Does the player loop to start when they hit the final tile, or do they stop there?
        /// </summary>
        public bool loopingGameBoard;
        public GameObject playerPrefab;
        public Player player;
        /// <summary>
        /// Sets up player game objects and components.
        /// </summary>
        /// <param name="players">How many players are in this game?</param>
        public void PreparePlayers(int players)
        {
            for (int x = 0; x < players; x++)
            {
                GameObject playerObject = Instantiate(playerPrefab) as GameObject;
                Player playerComponent = playerObject.GetComponent<Player>();
                Placeable placeableComponent = playerObject.GetComponent<Placeable>();
                playerObject.transform.SetParent(GameObject.Find("PlayerRoot").transform);
                playerComponent.placeable = placeableComponent;
                playerComponent.playerIndex = x;
                placeableComponent.PlaceAtTile(0);
                if (x == 0)
                {
                    player = playerComponent;
                }
            }
        }
        /// <summary>
        /// All players' instances which take part in game
        /// </summary>
        public Player[] players;
        /// <summary>
        /// Count of total turns so far
        /// </summary>
        public int turn = -1;
        public bool IsBotTurn
        {
            get
            {
                return player != null && player.IsBot;
            }
        }
        [SerializeField] private TMP_Text m_txt;

        private void Start()
        {
            GameUtilities.Tiles = GameObject.FindGameObjectsWithTag("Tile");
            GameUtilities.gameMode = GameMode.PLAYER_TURN;
            foreach (var pp in players) pp.MoveOn(0);

            ShowLog("Game starts! Roll Dice!");
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        /// <summary>
        /// Move current player
        /// </summary>
        /// <param name="steps">Count of steps for player to move.</param>
        public void MovePlayer(int steps)
        {
            if (player == null) return;
            player.MoveOn(steps);
            NextTurn();            
        }
        /// <summary>
        /// Move current player on chosen tile directly
        /// </summary>
        /// <param name="tile">Tile instance which the player will be placed on.</param>
        public void MovePlayer(GameBoardTile tile = null)
        {
            if (player == null) return;
            player.SelectRoute(tile);
            NextTurn();
        }
        /// <summary>
        /// Update turn to next player
        /// </summary>
        public void NextTurn()
        {
            if (GameUtilities.gameMode == GameMode.GAME_OVER) return;
            player.Highlight(false);
            turn += 1;
            var p = turn % players.Length;
            player = players[p];

            // check if bonus turn comes out. Every 3 turns, players get bonus turn
            var b = turn % (3 * players.Length);
            string turnDesc = "";
            if(b < players.Length && turn > players.Length)
            {
                GameUtilities.gameMode = GameMode.PLAYER_CHOOSING_ROUTE;
                player.ShowSelectableTiles();
                turnDesc = "Bonus turn";
            }
            else
            {
                GameUtilities.gameMode = GameMode.PLAYER_TURN;
                turnDesc = "turn";
            }
            if (IsBotTurn)
                Invoke(nameof(AutoRoll), 2f);
            player.Highlight(true);
            ShowLog(string.Format("{0}'s {1}", player.IsBot ? "Bot" : "Player", turnDesc));
        }
        /// <summary>
        /// Roll the dice automatically when bot player gets turn
        /// </summary>
        public void AutoRoll()
        {
            if (GameUtilities.gameMode == GameMode.PLAYER_TURN)
                RollDice.Instance.Roll(1f, (result) => { MovePlayer(result); });
            else if (GameUtilities.gameMode == GameMode.PLAYER_CHOOSING_ROUTE)
                MovePlayer();
        }

        public void ShowLog(string message)
        {
            m_txt.text = message;
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }
        /// <summary>
        /// Check if game is over
        /// </summary>
        public void CheckIfOver(TileTypes tileType)
        {
            if (tileType != TileTypes.FINAL && tileType != TileTypes.HOLE)
                return;
            GameUtilities.gameMode = GameMode.GAME_OVER;
            string message = "Game Over!";
            if (tileType == TileTypes.FINAL)
            {
                message += player.IsBot ? "Bot won!" : "Player won!";
            }
            else if(tileType == TileTypes.HOLE)
            {
                message += !player.IsBot ? "Bot won!" : "Player won!";
            }
            ShowLog(message);
        }
    }
}