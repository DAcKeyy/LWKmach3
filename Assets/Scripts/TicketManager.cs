using UnityEngine;
using TMPro;

public class TicketManager : MonoBehaviour
{
    [SerializeField] GameObject DailyTimePanel = null;
    [SerializeField] GameObject TicketPanel = null;
    private GameObject ticetsNumber;

    void Start()
    {
        ticetsNumber = TicketPanel.gameObject.transform.Find("Text (TMP) Ticket count").gameObject;
        CheckTickets();
    }

    void CheckTickets()
    {
        if (GlobalDataBase.Tickets == 0)
        {
            TicketPanel.SetActive(false);
            DailyTimePanel.SetActive(true);

        }

        else
        {
            ticetsNumber.GetComponent<TMP_Text>().text = GlobalDataBase.Tickets.ToString();
        }
    }

    string GetTimeFromServer()
    {
        return null;
    }
}
