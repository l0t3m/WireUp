using System;
using Unity.AI.Navigation;
using UnityEngine;



public class Snap : MonoBehaviour
{
    public event Action<Snap, DragAndDrop> OnSnap;
    private bool isOccupied = false;

    private void OnCollisionEnter(Collision collision)
    {
        DragAndDrop dragndrop = collision.gameObject.GetComponent<DragAndDrop>();
        if (CanSnapOntoMe(collision.gameObject.tag))
            OnSnap?.Invoke(this, dragndrop);
        else
            dragndrop.ResetPosition();
    }

    public void DoSnap(GameObject obj)
    {
        Vector3 newPos = transform.position;
        newPos.y += 0.5f;
        obj.transform.position = newPos;
        isOccupied = true;
    }

   

    private bool CanSnapOntoMe(string tag)
    {
        return (tag == "Obstacle" || tag == "BlockPiece") && !isOccupied;
    }
}
