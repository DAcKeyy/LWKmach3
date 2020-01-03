using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class ServerLoadingProcess : MonoBehaviour
{
    [SerializeField] Image loadIndicator;
    [SerializeField] List<Sprite> Sprites;

    [SerializeField] private float fillValue = 0.02f;

    public IEnumerator LoadAsynchronously(UnityWebRequest webRequest)
    {
        while (!webRequest.isDone)
        {
            loadIndicator.fillAmount += fillValue;

            if (loadIndicator.fillAmount == 1f)
            {
                loadIndicator.fillClockwise = !loadIndicator.fillClockwise;
                fillValue *= -1;
            }

            if (loadIndicator.fillAmount == 0f)
            {
                loadIndicator.sprite = Sprites[Random.Range(0, Sprites.Count)];

                loadIndicator.fillClockwise = !loadIndicator.fillClockwise;
                fillValue *= -1;
            }

            yield return null;
        }

        loadIndicator.fillAmount = 0f;
    }
}
