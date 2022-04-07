using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer rangeSpriteRenderer;
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
        rangeSpriteRenderer.enabled = true;
    }
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        GameManager.Instance.ClickedButton = null;
        rangeSpriteRenderer.enabled = false;

    }
}
