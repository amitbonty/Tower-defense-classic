using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade 
{
    public int Damage { get; private set; }
    public int Price { get; private set; }
    public float AttackSpeed { get; private set; }
    public TowerUpgrade(int damage,int price , float aSpeed)
    {
        this.Damage = damage;
        this.Price = price;
        this.AttackSpeed = aSpeed;
    }
}
