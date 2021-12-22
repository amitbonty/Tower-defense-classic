using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Point GridPosition { get; private set; }
    public bool IsEmpty { get; set; }
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);
    private SpriteRenderer spriteRenderer;
    private Tower myTower;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }
    public void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton != null)
        {
            if(IsEmpty)
            {
                ColorTile(emptyColor);
            }
            if(!IsEmpty|| CompareTag("Path"))
            {
               ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton==null&&Input.GetMouseButtonDown(0))
        {
            if(myTower!=null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeSelectTower();
            }
        }
    }

    public void Setup(Point gridPos, Vector3 worldPos)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    
    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.X;
        tower.transform.SetParent(transform);
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
        IsEmpty = false;
        ColorTile(Color.white);
        myTower.Price = GameManager.Instance.ClickedButton.Price;
        GameManager.Instance.BuyTower();
    }
    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }
}
