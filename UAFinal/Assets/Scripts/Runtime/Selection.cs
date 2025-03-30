using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Selection : MonoBehaviour
{
    public Material highlightMaterial;
    public Material selectionMaterial;

    private Material originalMaterial;
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    private void Update()
    {
        if (highlight != null)
        {
            ChangeChildrenMaterial(highlight, originalMaterial);
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit) )
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("BlockPiece") && highlight != selection)
            {
                if (highlight.GetComponentInChildren<MeshRenderer>().material != highlightMaterial)
                {
                    originalMaterial = highlight.GetComponentInChildren<MeshRenderer>().material;
                    ChangeChildrenMaterial(highlight, highlightMaterial);
                }
            }
            else
            {
                 highlight = null;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (selection != null)
            {
                ChangeChildrenMaterial(selection, originalMaterial);
                selection = null;
            }
            if (EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
            {
                selection = raycastHit.transform;
                if (selection.CompareTag("BlockPiece"))
                {
                    ChangeChildrenMaterial(selection, selectionMaterial);
                }
                else
                {
                    selection = null;
                }
            }
        }
    }

    private void ChangeChildrenMaterial(Transform parent, Material material)
    {
        foreach (var child in parent.GetComponentsInChildren<MeshRenderer>())
        {
            child.material = material;
        }
    }
}
