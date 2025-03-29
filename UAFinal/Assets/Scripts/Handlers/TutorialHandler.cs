using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] GameObject[] TutorialParts;
    private int currentPart = 0;

    private void Start()
    {
        if (LevelHandler.Instance.GetLevel().LevelNumber == 1)
            TutorialParts[currentPart].SetActive(true);
        else
            gameObject.SetActive(false);
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
