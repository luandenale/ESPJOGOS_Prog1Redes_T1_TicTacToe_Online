using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPiecesManager : MonoBehaviour
{
    private List<GameObject> _oPieces = new List<GameObject>();
    private List<GameObject> _xPieces = new List<GameObject>();
    
    private void Start()
    {
        foreach(Transform __piece in transform)
        {
            if (__piece.name.StartsWith("o"))
                _oPieces.Add(__piece.gameObject);
            else if (__piece.name.StartsWith("x"))
                _xPieces.Add(__piece.gameObject);
        }
    }

    public void FallPieces(string p_pieceToFall)
    {
        if(p_pieceToFall == "x")
        {
            foreach(GameObject __xPiece in _xPieces)
            {
                __xPiece.transform.localPosition = new Vector3(Random.Range(-7f, 6f), Random.Range(10f, 30f), Random.Range(-4f, -1f));
                __xPiece.transform.localEulerAngles = new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f));
                __xPiece.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            foreach (GameObject __oPiece in _oPieces)
            {
                __oPiece.transform.localPosition = new Vector3(Random.Range(-7f, 6f), Random.Range(10f, 30f), Random.Range(-4f, -1f));
                __oPiece.transform.localEulerAngles = new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f));
                __oPiece.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    public void ResetPieces()
    {
        foreach(GameObject __xPiece in _xPieces)
        {
            __xPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            __xPiece.GetComponent<Rigidbody>().useGravity = false;
            __xPiece.transform.localPosition = new Vector3(0, 10, -2);
        }

        foreach (GameObject __oPiece in _oPieces)
        {
            __oPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            __oPiece.GetComponent<Rigidbody>().useGravity = false;
            __oPiece.transform.localPosition = new Vector3(0, 10, -2);
        }
    }
}
