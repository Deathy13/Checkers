using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public class Piece : MonoBehaviour
    {

        //flag to store if white or king;
        public bool isWhite, isKing;
        // Stores X and Y cell location in grid;
        public Vector2Int cell, oldCell;
        // Reference to animator
        private Animator anim;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            //Get reference to Animator component
            anim = GetComponent<Animator>();
        }
        public void King()
        {
            // This piece is now king
            isKing = true;
            // trigger King animation
            anim.SetTrigger("King");
        }
    }
}