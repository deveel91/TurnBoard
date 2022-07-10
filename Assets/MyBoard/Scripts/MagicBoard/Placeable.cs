using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public class Placeable : MonoBehaviour
    {
        /// <summary>
        /// What tile is the placeable on? It should match its player.
        /// </summary>
        public int currentTileNumber = -1;
        GameObject[] TileGameObjects => GameUtilities.Tiles;
        /// <summary>
        /// For placing players on a tile. Do not use to place tiles.
        /// </summary>
        /// <param name="tileNumber">The tile number the Player will be on.</param>
        public void PlaceAtTile(int tileNumber)
        {
            for (int x = 0; x < TileGameObjects.Length; x++)
            {
                var tile = TileGameObjects[x].GetComponent<GameBoardTile>();
                if (tile.tileNumber == tileNumber)
                {

                    transform.position = TileGameObjects[x].transform.position;
                    currentTileNumber = tileNumber;
                    //Debug.Log(tile.tileType + ":" + tileNumber);
                    GameManager.Instance.CheckIfOver(tile.tileType);
                    break;
                }
            }
        }
        /// <summary>
        /// For placing players on a tile. Do not use to place tiles.
        /// </summary>
        /// <param name="tile">The tile instance the Player will be on.</param>
        public void PlaceAtTile(GameBoardTile tile)
        {
            PlaceAtTile(tile.tileNumber);
        }
        /// <summary>
        /// Returns the GameBoardTile the placeable is currently on.
        /// </summary>
        /// <returns></returns>
        public GameBoardTile ReturnCurrentTile()
        {
            return GameUtilities.GetTileById(currentTileNumber);
        }
    }
}