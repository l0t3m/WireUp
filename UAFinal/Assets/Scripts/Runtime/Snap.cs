using System;
using UnityEngine;



public class Snap : MonoBehaviour
{
    public event Action<Snap, DragAndDrop> OnSnap;
    private bool isOccupied = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (CanSnapOntoMe(collision.gameObject.tag))
            OnSnap?.Invoke(this, collision.gameObject.GetComponent<DragAndDrop>());   
    }  

    public void DoSnap(GameObject obj)
    {
        obj.transform.position = transform.position;
        isOccupied = true;
    }

    public void DoUnsnap()
    {
        isOccupied = false;
    }

    private bool CanSnapOntoMe(string tag)
    {
        return tag == "BlockPiece" && !isOccupied;
    }
}
