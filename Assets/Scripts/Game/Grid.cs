using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Checkers
{
    using ForcedMoves = Dictionary<Piece, List<Vector2Int>>;
    public class Grid : MonoBehaviour
    {
        public GameObject redPiecePrefab, whitePiecePrefab;
        public Vector3 boardOffset = new Vector3(-4.0f, 0.0f, -4.0f);
        public Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);
        public Piece[,] pieces = new Piece[8, 8];
        public bool isWhiteTurn;
        private Dictionary<Piece, List<Vector2Int>>forcedMove = new Dictionary<Piece, List<Vector2Int>>();

        //for Drag and drop
        private Vector2Int mouseOver; // Grid Coordinates the Mouse is over
        private Piece selectedPiece; // Piece that has been

        private ForcedMoves forcedMove = new ForcedMoves();

        Piece GetPiece(Vector2Int cell)
        {
            return pieces[cell.x, cell.y];
        }
        bool isOutOfBounds(Vector2Int cell)
        {
            return cell.x < 0 || cell.x >= 8 || cell.y < 0 || cell.y >= 8;
        }

        Piece SelectPiece(Vector2Int cell)
        {
            if (isOutOfBounds(cell))
            {
                return null;
            }
            Piece piece = GetPiece(cell);
            if (piece)
            {
                return piece;
            }
            return null;
        }
        void MouseOver()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                mouseOver.x = (int)(hit.point.x - boardOffset.x);
                mouseOver.y = (int)(hit.point.z - boardOffset.z);
            }
            else
            {
                mouseOver = new Vector2Int(-1, -1);
            }
        }
        void DragPiece(Piece selected)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                selected.transform.position = hit.point + Vector3.up;
            }
        }

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            GenerateBoard();
        }

        private void Update()
        {
            MouseOver();
            if (Input.GetMouseButtonDown(0))
            {
                selectedPiece = SelectPiece(mouseOver);
            }
            if (selectedPiece)
            {
                DragPiece(selectedPiece);
                if (Input.GetMouseButtonUp(0))
                {
                    TryMove(selectedPiece, mouseOver);
                    selectedPiece = null;
                }
            }
        }

        Vector3 GetWorldPosition(Vector2Int cell)
        {
            return new Vector3(cell.x, 0, cell.y) + boardOffset + pieceOffset;
        }
        void MovePiece(Piece piece, Vector2Int newCell)
        {
            Vector2Int oldCell = piece.cell;
            // Update array
            pieces[oldCell.x, oldCell.y] = null;
            pieces[newCell.x, newCell.y] = piece;
            // Update data on piece
            piece.oldCell = oldCell;
            piece.cell = newCell;
            //Translate the piece to another location
            piece.transform.localPosition = GetWorldPosition(newCell);
        }
        void GeneratePiece(GameObject prefab, Vector2Int desiredCell)
        {
            // Generate instance of prefab
            GameObject clone = Instantiate(prefab, transform);
            // Get the Piece component
            Piece piece = clone.GetComponent<Piece>();
            //set the Cell data for the first time
            piece.oldCell = desiredCell;
            piece.cell = desiredCell;
            // Reposition clone
            MovePiece(piece, desiredCell);
        }
        void GenerateBoard()
        {
            Vector2Int desiredCell = Vector2Int.zero;
            // Generate White Team
            for (int y = 0; y < 3; y++)
            {
                bool oddRow = y % 2 == 0;
                // loop through cloumns
                for (int x = 0; x < 8; x += 2)
                {
                    desiredCell.x = oddRow ? x : x + 1;
                    desiredCell.y = y;
                    // Generate piece
                    GeneratePiece(whitePiecePrefab, desiredCell);
                }
            }
            // Generate Red Team
            for (int y = 5; y < 8; y++)
            {
                bool oddRow = y % 2 == 0;
                // loop through cloumns
                for (int x = 0; x < 8; x += 2)
                {
                    desiredCell.x = oddRow ? x : x + 1;
                    desiredCell.y = y;
                    // Generate piece
                    GeneratePiece(redPiecePrefab, desiredCell);
                }
            }
        }
        bool VaildMove(Piece selected, Vector2Int desiredCell)
        {
            Vector2Int direction = selected.cell - desiredCell;

            #region Rule #01
            if (isOutOfBounds(desiredCell))
            {

                Debug.Log("<color=red> invalid - you cannot move out side of the map</color>");
                return false;
            }

            #endregion

            #region Rule #02
            if (selected.cell == desiredCell)
            {
                Debug.Log("<color=red> Invalid - putting pieces back don't count as valid move</color>");
                return false;
            }
            #endregion

            #region Rule #03
            if (GetPiece(desiredCell))
            {
                Debug.Log("<color=red>invalid - You can't go on top of other pieces</color>");
                return false;
            }
            #endregion

            #region Rule #04
            if()
            #endregion

            #region Rule #05
            if (direction.magnitude > 2)
            {

            }
            #endregion

            #region Rule #06
            #endregion

            #region Rule #07
            #endregion



            Debug.Log("<color=green>Success - Valid move detected!</color>");
            return true;

        }



        bool TryMove(Piece selected, Vector2Int desiredCell)
        {
            Vector2Int startCell = selected.cell;


            if (!VaildMove(selected, desiredCell))
            {
                MovePiece(selected, startCell);
                return false;
            }

            MovePiece(selected, desiredCell);
            return true;



        }
        void CheckForcedMove(Piece piece)
        {
            Vector2Int cell = piece.cell;
            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    Vector2Int offset = new Vector2Int(x, y);

                    Vector2Int desiredCell = cell + offset;

                    #region Check #01

                    if (!piece.isKing)
                    {
                        if (piece.isWhite)
                        {
                            if (desiredCell.y < cell.y)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (desiredCell.y > cell.y)
                        {
                            continue;
                        }
                    }
                    #endregion

                    #region Check #02

                    if (isOutOfBounds(desiredCell))
                    {
                        continue;
                    }

                    #endregion

                    Piece detectedPiece = GetPiece(desiredCell);
                    #region Check #03
                    if (detectedPiece == null)
                    {
                        continue;
                    }
                    #endregion

                    #region Check #04
                    if (detectedPiece.isWhite == piece.isWhite)
                    {
                        continue;
                    }



                    #endregion
                    Vector2Int jumpCell = cell + (offset * 2);
                    #region Check #05
                    if (isOutOfBounds(jumpCell))
                    {
                        continue;
                    }
                    #endregion


                    #region Check #06
                    detectedPiece = GetPiece(jumpCell);
                    if (detectedPiece)
                    {
                        continue;
                    }
                    #endregion



                    #region Store Forced Move
                    if(!forcedMove.ContainsKey(piece))
                    {
                        forcedMove.Add(piece, new List<Vector2Int>());
                    }
                    forcedMove[piece].Add(jumpCell);
                    #endregion
                }
            }


        }
        void DetectForcedMoves()
        {
            forcedMove = new ForcedMoves();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece pieceToCheck = pieces[x, y];
                    if(pieceToCheck)
                    {
                        CheckForcedMove(pieceToCheck);
                    }
                }
            }
        }

    }
}