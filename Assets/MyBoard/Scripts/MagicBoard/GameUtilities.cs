using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public enum GameMode
    {
        PLAYER_TURN,                //the player has yet to act this round
        PLAYER_CHOOSING_ROUTE,      //the player must select a route
        GAME_OVER
    }
    public class GameUtilities : MonoBehaviour
    {
        public static GameMode gameMode = GameMode.PLAYER_TURN;
        private static GameObject[] m_tiles = null;
        public static GameObject[] Tiles
        {
            set
            {
                m_tiles = value;
            }
            get
            {
                if (m_tiles == null) m_tiles = GameObject.FindGameObjectsWithTag("Tile");
                return m_tiles;
            }
        }
        public static GameBoardTile GetTileById(int id)
        {
            for (int x = 0; x < Tiles.Length; x++)
            {
                if (Tiles[x].GetComponent<GameBoardTile>().tileNumber == id)
                {
                    return Tiles[x].GetComponent<GameBoardTile>();

                }
            }
            return null;
        }
    }
}