using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCoord : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh txtMesh;
    public Pawn pawn;
    void Start()
    {
        txtMesh = GetComponentInChildren<TextMesh>();
        pawn = GetComponentInChildren<Pawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if(txtMesh != null && pawn != null)
            txtMesh.text = pawn.currentCell.ToString();
    }
}
