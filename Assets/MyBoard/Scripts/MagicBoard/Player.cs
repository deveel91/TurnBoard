using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Which player is this?
        /// </summary>
        public int playerIndex = -1;
        /// <summary>
        /// Which tile is this player on?
        /// </summary>
        public int currentTileNumber
        {
            get
            {
                return placeable != null ? placeable.currentTileNumber : -1;
            }
        }
        /// <summary>
        /// Moveable object which is belong to this player
        /// </summary>
        public Placeable placeable;
        public bool IsBot = false;
        /// <summary>
        /// Tile list which the player can be placed directly
        /// </summary>
        private GameBoardTile[] selectableTiles;

        public void Highlight(bool isShow = false)
        {
            transform.localScale = Vector3.one * (isShow ? 0.8f : 0.7f);
        }
        /// <summary>
        /// Move placeable object over count of steps
        /// </summary>
        /// <param name="steps">Count of steps for player to move.</param>
        public void MoveOn(int steps)
        {
            placeable.PlaceAtTile(currentTileNumber + steps);
        }
        /// <summary>
        /// Move player to best matched tile
        /// </summary>
        public void SelectRoute()
        {
            List<GameBoardTile> bestTiles = new List<GameBoardTile>();
            foreach(var t in selectableTiles)
            {
                if(t.tileType == TileTypes.FINAL)
                {
                    SelectRoute(t);
                    return;
                }
                if (t.tileType == TileTypes.HOLE)
                    continue;
                bestTiles.Add(t);
            }
            GameBoardTile title;
            if(bestTiles.Count > 0)
                title = bestTiles[Random.Range(0, bestTiles.Count)];
            else
                title = selectableTiles[Random.Range(0, selectableTiles.Length)];
            SelectRoute(title);
        }
        /// <summary>
        /// Move player to selected tile
        /// </summary>
        /// <param name="tile">Tile instance which the player will be placed on.</param>
        public void SelectRoute(GameBoardTile tile)
        {
            placeable.PlaceAtTile(tile);
            foreach (var t in selectableTiles)
            {
                if (t == null) continue;
                t.IsSelectable = false;
                t.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
            }
            selectableTiles = null;
        }
        /// <summary>
        /// Show availabe tiles which player can select to move
        /// </summary>
        public void ShowSelectableTiles()
        {
            selectableTiles = new GameBoardTile[6];
            for (int i = 0; i < 6; i += 1)
            {
                var tile = GameUtilities.GetTileById(currentTileNumber + i + 1);
                if (tile == null) continue;
                if (!IsBot) tile.IsSelectable = true;
                tile.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                selectableTiles[i] = tile;
            }
        }
    }
}