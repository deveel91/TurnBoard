using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicBoard
{
    public class RollDice : Singleton<RollDice>
    {
        /// <summary>
        /// Duration period while rolling dice until gets result
        /// </summary>
        [SerializeField] private float m_duration = 1f;
        /// <summary>
        /// Vector list to set up dice to show certain result
        /// </summary>
        private Vector3[] directions = new Vector3[] { Vector3.zero, Vector3.back, Vector3.down, Vector3.right, Vector3.left, Vector3.up, Vector3.forward };

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.A))
            {
                Roll(m_duration);
            }
        }

        private void OnMouseUp()
        {
            if (GameUtilities.gameMode != GameMode.PLAYER_TURN) return;
            if (GameManager.Instance.IsBotTurn) return;
            Roll(m_duration, (result) => { GameManager.Instance.MovePlayer(result); });
        }

        public void Roll(float duration = 1f, Action<int> OnResponse = null)
        {
            float t = duration;
            StartCoroutine(RotateDice());
            IEnumerator RotateDice()
            {
                while (t > 0f)
                {
                    transform.rotation = Random.rotationUniform;
                    yield return new WaitForEndOfFrame();
                    t -= Time.deltaTime;
                }
                int r = Random.Range(1, 7);
                transform.up = directions[r];
                GameManager.Instance.ShowLog($"Rolled: {r}");
                if(OnResponse != null) OnResponse(r);
                //GameManager.Instance.MovePlayer(r);
            }
        }

    }
}