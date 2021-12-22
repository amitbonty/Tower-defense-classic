using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private int price;
    [SerializeField]
    private TextMeshProUGUI priceText;
    private Color r;
   
    public int Price
    {
        get
        {
            return price;
        }
    }
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }
  

    public Sprite Sprite { get => sprite;  }
    private void Start()
    {
        priceText.text = Price.ToString() + "<color=green>$</color>";
        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
        r = GetComponent<Image>().color;
    }
    private void PriceCheck()
    {
        if (Price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = r;
            priceText.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;
            priceText.color = Color.gray;
        }  
    }
    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        switch(type)
        {
            case "Fire":
                Tower tower = towerPrefab.GetComponentInChildren<Tower>();
                //Upgrades = new TowerUpgrade[]
                //{
                //    new TowerUpgrade(50,3,4f),
                //    new TowerUpgrade(60,4,4f)
                //};
                tooltip = string.Format("<color=#FFA500FF><size=45><b>Fire</b></size></color>\n\nDamage: {0} \nAS : {1} \nHas a chance to slow enemies.",tower.Damage,tower.ProjectileSpeed);
                break;
            case "Frost":
                Tower tower1 = towerPrefab.GetComponentInChildren<Tower>();
                //Upgrades = new TowerUpgrade[]
                //{
                //    new TowerUpgrade(55,6,4f),
                //    new TowerUpgrade(65,7,4f)
                //};
                tooltip = string.Format("<color=#00FFFFFF><size=45><b>Frost</b></size></color>\n\nDamage: {0} \nAS : {1} \nHas a chance to slow enemies.", tower1.Damage, tower1.ProjectileSpeed);

                break;
            case "Poison":
                Tower tower2 = towerPrefab.GetComponentInChildren<Tower>();
                //Upgrades = new TowerUpgrade[]
                //{
                //    new TowerUpgrade(52,4,4f),
                //    new TowerUpgrade(62,5,4f)
                //};
                tooltip = string.Format("<color=#00FF00FF><size=45><b>Poison</b></size></color>\n\nDamage: {0} \nAS : {1} \nHas a chance to slow enemies.", tower2.Damage, tower2.ProjectileSpeed);
                break;
        }
        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
