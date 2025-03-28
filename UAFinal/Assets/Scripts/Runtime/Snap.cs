using System;
using UnityEngine;

public enum GridType
{
    Empty,
    Occupied,
    Start,
    End
}

public class Snap : MonoBehaviour
{
    public event Action<Snap, DragAndDrop> OnSnap;
    private GridType gridType;
    private bool isOccupied = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (CanSnapOntoMe(collision.gameObject.tag))
            OnSnap?.Invoke(this, collision.gameObject.GetComponent<DragAndDrop>());   
    }  

    public void DoSnap(GameObject obj)
    {
        obj.transform.position = transform.position;
        gridType = GridType.Occupied;
        isOccupied = true;
    }

    public void DoUnsnap()
    {
        gridType = GridType.Empty;
        isOccupied = false;
    }

    private bool CanSnapOntoMe(string tag)
    {
        return tag == "BlockPiece" && !isOccupied && gridType != GridType.End && gridType != GridType.Start;
    }
}
