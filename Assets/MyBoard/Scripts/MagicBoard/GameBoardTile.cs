using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public enum TileTypes
    {
        GROUND = 0,
        BONUS = 1,
        FINAL = 2,
        HOLE = 3
    }
    public class GameBoardTile : MonoBehaviour
    {
        public TileTypes tileType = TileTypes.GROUND;
        /// <summary>
        /// The tile's number on the game board. Also serves as th etile's id.
        /// </summary>
        public int tileNumber = -1;
        /// <summary>
        /// All materials the tile may use. Should be at least as large as the TileTypes enum.
        /// The first material should be ground
        /// </summary>
        [SerializeField] private Material[] materials;
        /// <summary>
        /// The status of tile if it can be selected to move player directly        
        /// </summary>
        public bool IsSelectable
        {
            set
            {
                GetComponent<BoxCollider>().enabled = value;
            }
            get
            {
                return GetComponent<BoxCollider>().enabled;
            }
        }
        // Use this for initialization
        void Start()
        {
            if (tileNumber == -1) tileNumber = transform.GetSiblingIndex();
            IsSelectable = false;
            GetComponent<MeshRenderer>().material = materials[(int)tileType];
        }

        public void OnMouseDown()
        {
            //Selects this tile if the player has to choose a route.
            if (GameUtilities.gameMode == GameMode.PLAYER_CHOOSING_ROUTE)
            {
                GameManager.Instance.MovePlayer(this);
            }
        }
    }
}