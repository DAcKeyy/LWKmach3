using UnityEngine;
using UnityEngine.UI;

public class GridAutoSize : MonoBehaviour
{
    [SerializeField] GameObject Conatainer = null;

    void Update()
    {
        float wight = Conatainer.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2((wight / 3)-20, (wight / 4)-20);
        Conatainer.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}