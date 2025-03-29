using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] GameObject[] TutorialParts;
    private int currentPart = 0;

    private void Start()
    {
        TutorialParts[currentPart].SetActive(true);
    }

    public void ShowNext()
    {
        if (currentPart < TutorialParts.Length-1)
        {
            TutorialParts[currentPart].SetActive(false);
            currentPart++;
            TutorialParts[currentPart].SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
