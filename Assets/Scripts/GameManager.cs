using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public delegate void CurrencyChanged();
public class GameManager : Singleton<GameManager>
{
    public event CurrencyChanged Changed;
    public TowerButton ClickedButton { get; set; }
    private Tower selectedTower;
    private int currency;
    public int Currency
    {
        get { return currency; }
        set
        {
            this.currency = value;
            this.currencyText.text = value.ToString() + "<color=green>$</color>";
            OnCurrencyChanged();
        }
    }
    private int wave=0;
    private int lives;
    private bool gameOver = false;
    public int Lives
    {
        get { return lives; }
        set
        {
            this.lives = value;
            this.livesTxt.text = value.ToString();
            _playerHealthBar.fillAmount = (float)value / 100;
            if(lives<=0)
            {
                this.lives = 0;
                GameOver();
            }
        }
    }
    public Animator Animator
    {
        get { return _animator; }
    }
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Image _playerHealthBar;
    [SerializeField]
    private GameObject statsPanel;
    [SerializeField]
    private TextMeshProUGUI statsTxt;
    [SerializeField]
    private TextMeshProUGUI livesTxt;
    [SerializeField]
    private TextMeshProUGUI currencyText;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private GameObject waveButton;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject UpgradePanel;
    private List<Monster> activeMonsters = new List<Monster>();
    public ObjectPool Pool
    {
        get;set;
    }
    public bool WaveActive
    {
        get
        {
            return activeMonsters.Count > 0;
        }
    }
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
    private void Start()
    {
        _playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHP").GetComponent<Image>();
        Currency = 50;
        Lives = 100;
    }
    public void PickTower(TowerButton towerButton)
    {
        if (Currency>=towerButton.Price && !WaveActive)
        {
         this.ClickedButton = towerButton;
         Hover.Instance.Activate(towerButton.Sprite);
        }
    }
    public void BuyTower()
    {
        if(Currency >= ClickedButton.Price)
        {
            Currency -= ClickedButton.Price;
            ClickedButton = null;
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Hover.Instance.Deactivate();
        }
    }
    public void OnCurrencyChanged()
    {
        if(Changed!=null)
        {
            Changed();
        }
    }
    private void Update()
    {
        HandleEscape();
    }
    public void SelectTower(Tower tower)
    {
        if(selectedTower!=null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        UpgradePanel.SetActive(true);
        selectedTower.Select();
    }
    public void DeSelectTower()
    {
        if(selectedTower!=null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
        UpgradePanel.SetActive(false);
       // ShowSelectedTowerStats();
    }
    private void HandleEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }
    public void StartWave()
    {
        wave++;
        waveText.text = string.Format("WAVE: <color=blue>{0}</color>", wave);
        StartCoroutine(SpawnWave());
        waveButton.SetActive(false);
    }
    private IEnumerator SpawnWave()
    {
        for(int i=0;i<wave;i++)
        {

        int monterIndex = Random.Range(0, 3);
        string type = string.Empty;
        switch(monterIndex)
        {
            case 0:
                type = "RedMonster";
                break;
            case 1:
                type = "BlueMonster";
                break;
            case 2:
                type = "GreenMonster";
                break;
            
        }
        GameObject monster = Pool.GetObject(type);
        activeMonsters.Add(monster.GetComponent<Monster>());
        SoundManager.Instance.Play(Sounds.EnemySpawn);
            yield return new WaitForSeconds(2.5f);
        }
    }
    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
        if(!WaveActive)
        {
            waveButton.SetActive(true);
        }
    }
    public void GameOver()
    {
        if(!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }
    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateTooltip();
    }
    public void SetTooltipText(string txt)
    {
        statsTxt.text = txt;
    }
    public void UpdateTooltip()
    {
        if(selectedTower!=null)
        {
            SetTooltipText(selectedTower.GetStats());
        }
    }
    public void UpgradeTower()
    {
        if(selectedTower!= null)
        {
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
                SoundManager.Instance.Play(Sounds.ButtonClick);
            }
            ;
        }
    }
    public void Selltower()
    {
        if(selectedTower!=null)
        {
            Currency += selectedTower.Price / 2;
            selectedTower.GetComponentInParent<Tile>().IsEmpty = true;
            SoundManager.Instance.Play(Sounds.ButtonClick2);
            Destroy(selectedTower.transform.parent.gameObject);
            DeSelectTower();
            //ShowSelectedTowerStats();
        }
    }

}
