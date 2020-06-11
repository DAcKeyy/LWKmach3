using UnityEngine;

public class LoadLeavesManager : MonoBehaviour
{
    [SerializeField]private GameObject StartLeaves = null;
    [SerializeField] private GameObject GameLeaves = null;

    void Start()
    {
        if (GlobalDataBase.PrevScene == "Game") GameLeaves.SetActive(true);
        else if (GlobalDataBase.PrevScene == "Start") StartLeaves.SetActive(true);
    }
}
