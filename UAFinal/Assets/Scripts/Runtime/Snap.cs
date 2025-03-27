using System;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public event Action<Snap, DragAndDrop> OnSnap;
    private bool isOccupied = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BlockPiece" && !isOccupied)
            OnSnap?.Invoke(this, collision.gameObject.GetComponent<DragAndDrop>());   
    }

    public void DoSnap(DragAndDrop block)
    {
        block.transform.position = transform.position;
        isOccupied = true;
    }

    public void DoUnsnap()
    {       
        isOccupied = false;
    }
}
