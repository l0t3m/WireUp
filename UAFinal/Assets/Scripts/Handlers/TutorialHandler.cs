using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] GameObject[] TutorialParts;
    [SerializeField] VideoClip[] VideoClips;
    [SerializeField] RenderTexture[] VideoRenderTexture;
    [SerializeField] GameObject VideoManager;
    [SerializeField] RawImage RawImage;
    private int currentPart = 0;

    private void Start()
    {
        if (LevelHandler.Instance.GetLevel().LevelNumber == 1)
            TutorialParts[currentPart].SetActive(true);
        else
            gameObject.SetActive(false);

        if (VideoClips[currentPart] != null)
        {
            RawImage.gameObject.SetActive(true);
            VideoManager.GetComponent<VideoPlayer>().clip = VideoClips[currentPart];
            VideoManager.GetComponent<VideoPlayer>().targetTexture = VideoRenderTexture[currentPart];
            RawImage.texture = VideoRenderTexture[currentPart];
        }
    }

    public void ShowNext()
    {
        if (currentPart < TutorialParts.Length-1)
        {

            TutorialParts[currentPart].SetActive(false);
            currentPart++;
            TutorialParts[currentPart].SetActive(true);

            if (VideoClips[currentPart] != null)
            {
                RawImage.gameObject.SetActive(true);
                VideoManager.GetComponent<VideoPlayer>().clip = VideoClips[currentPart];
                VideoManager.GetComponent<VideoPlayer>().targetTexture = VideoRenderTexture[currentPart];
                RawImage.texture = VideoRenderTexture[currentPart];
            }
            else
            {
                VideoManager.GetComponent<VideoPlayer>().enabled = false;
                RawImage.gameObject.SetActive(false);
            }

            

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
