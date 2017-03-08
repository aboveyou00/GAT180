using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameController game;

    public GameObject towerPrefab;
    public List<TowerController> towers;

    public GameObject hoverTile;
    public SelectionController selectionTile;

    private int selectionX = -1, selectionY = -1;
    private TowerController selectedTower;

    public int money = 120;
    public int towerPrice = 40;
    public float towerPriceIncrease = 1.2f;

    public float initialUpgradePrice = 15;
    public float upgradePriceIncrease = 1.2f;
    public float rangePriceWeight = 1.5f, damagePriceWeight = 1, speedPriceWeight = 1, chainHitPriceWeight = 1, criticalHitPriceWeight = 1, poisonPriceWeight = 1;

    private float nextUpgradePrice()
    {
        return initialUpgradePrice * Mathf.Pow(upgradePriceIncrease, selectedTower.UpgradesPurchased);
    }

    private void Start()
    {
        towers = new List<TowerController>();
        hoverTile.SetActive(false);
        selectionTile.gameObject.SetActive(false);
    }

    private void Update()
    {
        var mp = Input.mousePosition;
        var x = (int)Math.Floor(mp[0] / 60);
        var y = (int)Math.Floor(mp[1] / 60);

        updateHoverTile(x, y);
        updateSelection();

        if (Input.GetKeyDown("space")) Time.timeScale = (Time.timeScale == 0 ? game.playSpeed : 0);
    }
    private void updateHoverTile(int x, int y)
    {
        if (Input.GetMouseButtonDown(0) && (x >= 0 && y >= 0 && x < game.map.map.Width && y < game.map.map.Height))
        {
            selectionX = x;
            selectionY = y;
        }
        var tile = game.map.map[x, y];
        if (tile == 0 && (selectionX != x || selectionY != y) && (x >= 0 && y >= 0 && x < game.map.map.Width && y < game.map.map.Height))
        {
            hoverTile.transform.localPosition = new Vector3(x * 60, y * 60, hoverTile.transform.localPosition.z);
            hoverTile.SetActive(true);
        }
        else
        {
            hoverTile.SetActive(false);
        }
    }
    private void updateSelection()
    {
        var x = selectionX;
        var y = selectionY;
        var tile = game.map.map[x, y];
        selectedTower = null;
        if (tile == 0)
        {
            selectionTile.transform.localPosition = new Vector3(x * 60, y * 60, selectionTile.transform.localPosition.z);
            selectionTile.gameObject.SetActive(true);
            selectedTower = towers.Find(tc => tc.x == x && tc.y == y);
            selectionTile.Range = (selectedTower != null ? selectedTower.Range : 0);
        }
        else
        {
            selectionTile.gameObject.SetActive(false);
        }
    }

    private void OnGUI()
    {
        int left = Screen.width - 172;
        int right = Screen.width;
        int top = Screen.height - 4;

        GUI.color = new Color(.2f, .2f, 1);

        if (GUI.Button(new Rect(left + 4, top - 40, right - left - 8, 40), Time.timeScale == 0 ? "Play" : "Pause"))
        {
            //'\u23f8'.ToString() //Pause
            Time.timeScale = (Time.timeScale == 0 ? game.playSpeed : 0);
        }
        top -= 44;

        if (GUI.Button(new Rect(left + 4, top - 40, (right - left - 8) / 2, 40), "Normal")) //'\u25b6'.ToString()
        {
            game.playSpeed = 1;
            Time.timeScale = game.playSpeed;
        }
        if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top - 40, (right - left - 8) / 2, 40), "Fast")) //'\u23e9'.ToString()
        {
            game.playSpeed = 3;
            Time.timeScale = game.playSpeed;
        }
        top -= 44;

        top = 60;
        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "Money: $" + money);
        top += 44;

        if (!selectionTile.gameObject.activeSelf) return;
        if (selectedTower == null)
        {
            GUI.enabled = money >= towerPrice;
            if (GUI.Button(new Rect(left + 4, top + 4, right - left - 8, 40), "Buy Tower - $" + towerPrice)) buyTower();
            GUI.enabled = true;
            top += 44;
        }
        else
        {
            float upgPrice = nextUpgradePrice();

            var rangedPrice = (int)(upgPrice * rangePriceWeight);
            GUI.enabled = money >= rangedPrice;
            GUI.Label(new Rect(left + 4, top + 4, (right - left - 8) / 2, 40), "Range: " + (selectedTower.rangeUpgrades + 1));
            if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top + 4, (right - left - 8) / 2, 40), "+ ($" + rangedPrice + ")")) buyUpgrade(ref selectedTower.rangeUpgrades, rangedPrice);
            top += 44;

            var damagePrice = (int)(upgPrice * damagePriceWeight);
            GUI.enabled = money >= damagePrice;
            GUI.Label(new Rect(left + 4, top + 4, (right - left - 8) / 2, 40), "Damage: " + (selectedTower.damageUpgrades + 1));
            if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top + 4, (right - left - 8) / 2, 40), "+ ($" + damagePrice + ")")) buyUpgrade(ref selectedTower.damageUpgrades, damagePrice);
            top += 44;

            var speedPrice = (int)(upgPrice * speedPriceWeight);
            GUI.enabled = money >= speedPrice;
            GUI.Label(new Rect(left + 4, top + 4, (right - left - 8) / 2, 40), "Speed: " + (selectedTower.speedUpgrades + 1));
            if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top + 4, (right - left - 8) / 2, 40), "+ ($" + speedPrice + ")")) buyUpgrade(ref selectedTower.speedUpgrades, speedPrice);
            top += 44;

            var chainHitPrice = (int)(upgPrice * chainHitPriceWeight);
            GUI.enabled = money >= chainHitPrice;
            GUI.Label(new Rect(left + 4, top + 4, (right - left - 8) / 2, 40), "Chain Hit: " + (selectedTower.chainHitUpgrades + 1));
            if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top + 4, (right - left - 8) / 2, 40), "+ ($" + chainHitPrice + ")")) buyUpgrade(ref selectedTower.chainHitUpgrades, chainHitPrice);
            top += 44;

            var criticalHitPrice = (int)(upgPrice * criticalHitPriceWeight);
            GUI.enabled = money >= criticalHitPrice;
            GUI.Label(new Rect(left + 4, top + 4, (right - left - 8) / 2, 40), "Critical Hit: " + (selectedTower.criticalHitUpgrades + 1));
            if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top + 4, (right - left - 8) / 2, 40), "+ ($" + criticalHitPrice + ")")) buyUpgrade(ref selectedTower.criticalHitUpgrades, criticalHitPrice);
            top += 44;

            var poisonPrice = (int)(upgPrice * poisonPriceWeight);
            GUI.enabled = money >= poisonPrice;
            GUI.Label(new Rect(left + 4, top + 4, (right - left - 8) / 2, 40), "Poison: " + (selectedTower.poisonUpgrades + 1));
            if (GUI.Button(new Rect(left + ((right - left) / 2) + 4, top + 4, (right - left - 8) / 2, 40), "+ ($" + poisonPrice + ")")) buyUpgrade(ref selectedTower.poisonUpgrades, poisonPrice);
            top += 44;

            GUI.enabled = true;
            if (GUI.Button(new Rect(left + 4, top + 4, right - left - 8, 40), "Sell Tower - +$" + getTowerPrice(selectedTower))) sellTower();
            top += 44;
        }
    }
    private void buyTower()
    {
        if (money < towerPrice) return;
        money -= towerPrice;
        towerPrice = (int)(towerPrice * towerPriceIncrease);

        var t = Instantiate(towerPrefab);
        t.transform.localPosition = selectionTile.transform.localPosition;
        var tower = t.GetComponent<TowerController>();
        tower.game = game;
        tower.x = selectionX;
        tower.y = selectionY;
        selectedTower = tower;
        towers.Add(tower);
    }
    private void buyUpgrade(ref int upgradeVar, int price)
    {
        if (money < price) return;
        upgradeVar++;
        money -= price;
    }
    private void sellTower()
    {
        money += getTowerPrice(selectedTower);
        towers.Remove(selectedTower);
        Destroy(selectedTower.gameObject);
        selectedTower = null;
    }
    private int getTowerPrice(TowerController tower)
    {
        return 20;
    }

    public void banish(EnemyController enemy)
    {
        money -= (int)enemy.banishCost;
        enemy.banishCost *= 2;
    }
}
