using System;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public event Action<Snap, DragAndDrop> OnSnap;
    private bool isOccupied = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BlockPiece" && !isOccupied)
        {
            OnSnap?.Invoke(this, collision.gameObject.GetComponent<DragAndDrop>());
        }
        else if (collision.gameObject.tag == "BlockPiece")
        {
            collision.gameObject.GetComponent<DragAndDrop>().ResetPosition();
        }
    }

    public void DoSnap(DragAndDrop block)
    {
        block.transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);
        block.isSnapped = true;
        isOccupied = true;
    }

    public void DoUnsnap(DragAndDrop block)
    {
        block.ResetPosition();
        block.isSnapped = false;
        isOccupied = false;
    }
}
