using UnityEngine;
using UnityEngine.UI;

public class GridAutoSize : MonoBehaviour
{
    [SerializeField] GameObject Conatainer = null;


    void OnEnable()
    {
        float wight = Conatainer.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2((wight / 3)-20, (wight / 4)-20);
        Conatainer.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }

    private void Start()
    {
        SetContentLenght();
    }

    void SetContentLenght()
    {
        this.GetComponent<RectTransform>().sizeDelta =
            new Vector2(0, (1 + this.transform.childCount / Conatainer.GetComponent<GridLayoutGroup>().constraintCount)
            * (Conatainer.GetComponent<GridLayoutGroup>().cellSize.y + Conatainer.GetComponent<GridLayoutGroup>().spacing.y));
    }
}