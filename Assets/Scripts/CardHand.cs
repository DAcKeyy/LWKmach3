using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
 
#pragma warning disable 0649

//нужен для отоброжения тикетов в игру (как карты в руке:)
public class CardHand : MonoBehaviour 
{
    public GameObject CardPrefab;
    [SerializeField] private Ease moveEase;
    [SerializeField] private List<GameObject> Cards;
    [SerializeField] private Transform StartPos;
    [SerializeField] private Transform HandDeck; //Parent
    [SerializeField, Slider(5f, 60f)] private float MaxAngle;
    [SerializeField, Slider(10f, 300f)] private float Gap; //Between cards

    private void Start()//для примера(потом удалить)
    {
        AddCard(CardPrefab);//1
        AddCard(CardPrefab);//2
        AddCard(CardPrefab);//3

        RemoveCardByIndex(0);//2
    }

    public void AddCard(GameObject gameObject)
    {
        Cards.Add(gameObject);
        FitCards();
    }

    public void RemoveCardByIndex(int index)
    {
        if (Cards.Count == 0) return;

        Destroy(Cards[index]);
        Cards.RemoveAt(index);

        FitCards();
    }

    private void FitCards()
    {
        foreach (Transform child in HandDeck) //тк метод многоразовый, удаляем предыдущие объекты (да это хуйня)
        {
            GameObject.Destroy(child.gameObject);
        }

        var twistPerCard = MaxAngle / Cards.Count;
        //расчёт поворота и растояния между каратми
        //в зависимости от колличества карт
        //var angle = 
        for (int I = 0; I < Cards.Count; I++)
        {
            GameObject Card = Instantiate(Cards[I], 
                new Vector3(
                StartPos.position.x - ((Cards.Count - 1) * Gap) / 2 + (I * Gap),
                StartPos.position.y, 
                StartPos.position.z), 
                Quaternion.Euler(0, 0,0f - ((Cards.Count - 1) * -twistPerCard / 2 + (I * twistPerCard))) ) as GameObject;
            Cards[I] = Card;
            Cards[I].GetComponent<UnityEngine.UI.Image>().enabled = true;
            Card.name = "Ticket " + (I + 1);

            Card.transform.SetParent(HandDeck, false);//ставим родителя, вырубаем глобалную орентацию (чтобы скейл был идиентичен предку)          
        }
    }
}
