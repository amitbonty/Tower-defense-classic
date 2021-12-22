using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{
    [SerializeField]
    private string projectileType;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private GameObject LevelUp;


    private SpriteRenderer spriteRenderer;
    private Monster target;
    private Queue<Monster> monsters = new Queue<Monster>();
    private bool canAttack=true;
    private float attackTimer;

    public int Price
    {
        get;set;
    }
    public TowerUpgrade[] Upgrades { get; set; }
    public int Level { get; private set; }
    public int Damage
    {
        get { return damage; }
    }
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }
    public Monster Target
    {
        get { return target; }
    }
    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level - 1)
            {
                return Upgrades[Level - 1];
            }
            return null;
        }
    }

    public void Select()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
        GameManager.Instance.UpdateTooltip();
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Level = 1;
    }
    private void Update()
    {
        Attack();
    }
    private void Attack()
    {
        if(!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (target == null && monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }
        if(target!=null && target.isActiveAndEnabled)
        {
            if(canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
    }
    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Intialize(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Monster")
        {
            monsters.Enqueue(collision.GetComponent<Monster>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Monster")
        {
            target = null;
        }
    }

    public string GetStats()
    {
        Upgrades = new TowerUpgrade[]
            {
              new TowerUpgrade(5,1,0.25f),
              new TowerUpgrade(5,1,0.25f)
            };
            ;
        if (NextUpgrade!=null)
        {
            
            switch(projectileType)
            {
                case "Fire":
                    return string.Format("\n<color=#FFA500FF><size=45><b>Fire</b></size></color>\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nPrice: {2} <color=#00ff00ff> +{5}</color>\nAS: {3}s<color=#00ff00ff> +{6}s</color>", Level, damage, Price, attackCooldown, 5, 1, 0.25);

                case "Frost":
                    return string.Format("\n<color=#00FFFFFF><size=45><b>Frost</b></size></color>\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nPrice: {2} <color=#00ff00ff> +{5}</color>\nAS: {3}s<color=#00ff00ff> +{6}s</color>", Level, damage, Price, attackCooldown, 5, 1, 0.25);
                    
                case "Poison":
                    return string.Format("\n<color=#00FF00FF><size=45><b>Poison</b></size></color>\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nPrice: {2} <color=#00ff00ff> +{5}</color>\nAS: {3}s<color=#00ff00ff> +{6}s</color>", Level, damage, Price, attackCooldown, 5, 1, 0.25);
                    
            }
          
        }
        return string.Format("\n<color=#FFA500FF><size=45><b>{4}</b></size></color>\nLevel: {0} \nDamage: {1} \nPrice: {2} \nAS: {3}s", Level, damage, Price, attackCooldown,projectileType);
    }
    public void Upgrade()
    {
        LevelUpEffect();
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.attackCooldown -= NextUpgrade.AttackSpeed;
        Level++;
        GameManager.Instance.UpdateTooltip();
        
    }
    private void LevelUpEffect()
    {
        GameObject effect = Instantiate(LevelUp, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
    }
    
}
