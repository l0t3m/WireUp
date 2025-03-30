using System;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Collections.AllocatorManager;

[RequireComponent(typeof(Collider))]
public class DragAndDrop : MonoBehaviour
{
    public event Action OnRotate;

    [HideInInspector] public Camera currentCamera;
    [HideInInspector] public Vector3 OriginalPosition;

    private bool isDragging = false;
    private bool isSnapped = false;
    private const float dragYLevel = 1.5f;

    [SerializeField] public BlockSection BlockSection;

    [SerializeField] bool canPickUp = true;
    [SerializeField] bool canRotate = true;
    private bool isRotated = false;
    [HideInInspector] public bool IsGenerated = false;
    [SerializeField] Material poweredMaterial;


    private void Start()
    {
        OriginalPosition = transform.position;
    }

    public void ChangeColor()
    {
        foreach (var child in GetComponentsInChildren<MeshRenderer>())
        {
            child.material = poweredMaterial;
        }
    }

    private void OnMouseDown()
    {
        if (!isSnapped && canPickUp)
            isDragging = true;

    }

    private void OnMouseDrag()
    {
        if (isDragging && canPickUp)
        {
            Vector3 mouseWorld = GetMouseWorldPosition();
            Vector3 targetPosition = mouseWorld;
            transform.position = new Vector3(targetPosition.x, dragYLevel, targetPosition.z);
        }
    }

    private void OnMouseUp()
    {

        isDragging = false;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (isSnapped) return;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 10))
        {
            if (hit.transform.gameObject.tag != "GridItem")
                ResetBlock();            
        }
        else
            ResetBlock();
    }  

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && isSnapped && canRotate)
        {
            transform.Rotate(new Vector3(0, isRotated ? -90 : 90, 0));
            isRotated = !isRotated;
            OnRotate?.Invoke();
            foreach (var link in GetComponentsInChildren<NavMeshLink>())
                link.UpdateLink();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || (collision.gameObject.tag == "BlockPiece") && !isSnapped)
            ResetPosition();

       
    }



    // Methods:
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        Plane dragPlane = new Plane(Vector3.up, new Vector3(0, dragYLevel, 0)); 

        if (dragPlane.Raycast(ray, out float enter))
            return ray.GetPoint(enter);

        return transform.position;
    }

    public void ResetPosition()
    {
        transform.position = OriginalPosition;
    }

    public void DoSnap()
    {
        isSnapped = true;   
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
    }

    private void ResetBlock()
    {
        isDragging = false;
        ResetPosition();
    }

    public void DisableActions()
    {
        canPickUp = false;
        canRotate = false;
    }
}
