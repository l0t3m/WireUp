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
        Vector3 newPos = transform.position;
        newPos.y += 0.5f;
        obj.transform.position = newPos;
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
