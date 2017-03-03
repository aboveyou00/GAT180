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
    public GameObject selectionTile;

    private int selectionX, selectionY;
    private TowerController selectedTower;

    public int money = 120;
    public int towerPrice = 40;
    public float towerPriceIncrease = 1.2f;

    private void Start()
    {
        towers = new List<TowerController>();
        hoverTile.SetActive(false);
        selectionTile.SetActive(false);
    }

    private void Update()
    {
        var mp = Input.mousePosition;
        var x = (int)Math.Floor(mp[0] / 60);
        var y = (int)Math.Floor(mp[1] / 60);

        updateHoverTile(x, y);
    }
    private void updateHoverTile(int x, int y)
    {
        if (Input.GetMouseButtonDown(0) && (x >= 0 && y >= 0 && x < game.map.map.Width && y < game.map.map.Height)) updateSelection(x, y);
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
    private void updateSelection(int x, int y)
    {
        selectionX = x;
        selectionY = y;
        var tile = game.map.map[x, y];
        selectedTower = null;
        if (tile == 0)
        {
            selectionTile.transform.localPosition = new Vector3(x * 60, y * 60, selectionTile.transform.localPosition.z);
            selectionTile.SetActive(true);
            selectedTower = towers.Find(tc => tc.x == x && tc.y == y);
        }
        else
        {
            selectionTile.SetActive(false);
        }
    }

    private void OnGUI()
    {
        if (!selectionTile.activeSelf) return;

        int left = Screen.width - 172;
        int right = Screen.width;
        int top = 60;

        GUI.color = new Color(.2f, .2f, 1);

        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "Money: $" + money);
        top += 44;

        if (selectedTower == null)
        {
            GUI.enabled = money >= towerPrice;
            if (GUI.Button(new Rect(left + 4, top + 4, right - left - 8, 40), "Buy Tower - $" + towerPrice)) buyTower();
            top += 44;
        }
        else
        {
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
        tower.x = selectionX;
        tower.y = selectionY;
        selectedTower = tower;
        towers.Add(tower);
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
}
