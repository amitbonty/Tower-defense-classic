using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject bloodSplash;
    public float speed;
    private WayPoints WPoints;
    private int wayPointIndex;
    private int health;
    private Tile tile;
    public bool IsAlive
    {
        get { 
            return health > 0; }
    }
    private int startHealth = 100;
   
    void Start()
    {
        WPoints = GameObject.FindGameObjectWithTag("WayPoints").GetComponent<WayPoints>();
        health = 100;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        MoveInWayPoint();
        SetHealth();
        if(!IsAlive)
        {
            GameManager.Instance.Currency++;
            DeathEffect();
            Release();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="EndPortal")
        {
            Release();
           GameManager.Instance.Animator.Play("CameraShake");
            GameManager.Instance.Lives-=20;
        }
      
    }
    private void Release()
    {
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        wayPointIndex = 0;
        transform.position = GameObject.FindGameObjectWithTag("SpawnPortal").transform.position;
        health = 100;
        GameManager.Instance.RemoveMonster(this);
    }
    private void MoveInWayPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, WPoints.waypoints[wayPointIndex].position, speed * Time.deltaTime);
        Vector3 dir = WPoints.waypoints[wayPointIndex].position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (Vector2.Distance(transform.position, WPoints.waypoints[wayPointIndex].position) < 0.1f)
        {
            if (wayPointIndex < WPoints.waypoints.Length - 1)
            {
                wayPointIndex++;
            }
        }
    }
    public void TakeDamage(int dmg)
    {
        if (isActiveAndEnabled)
        {
           health -= dmg;
        }
          
    }
    private void SetHealth()
    {
        healthBar.fillAmount = (float)health / startHealth;
      
    }
    private void DeathEffect()
    {
       GameObject effect = Instantiate(bloodSplash, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
        SoundManager.Instance.Play(Sounds.EnemyDeath);
    }
    
}
