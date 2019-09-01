using UnityEngine;

public class OffGameObject : MonoBehaviour
{
    public void Destroy()
    {
        this.gameObject.SetActive(false);
    }
    public void Enable()
    {
        this.gameObject.SetActive(true);
    }
}
