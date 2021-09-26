using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class ScriptableCard : ScriptableObject
{
    public GameManager.Cards id;
    public GameManager.CardType type;

    public string cardName;
    public string description;
    public float manaCost;
    public float damage;
}
