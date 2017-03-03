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

        if (selectedTower == null)
        {
            if (GUI.Button(new Rect(left + 4, top + 4, right - left - 8, 40), "Buy Tower - $40")) buyTower();
            top += 44;
        }
        else
        {

        }
    }
    private void buyTower()
    {
        //TODO: check your moneys!

        var t = Instantiate(towerPrefab);
        t.transform.localPosition = selectionTile.transform.localPosition;
        var tower = t.GetComponent<TowerController>();
        tower.x = selectionX;
        tower.y = selectionY;
        selectedTower = tower;
        towers.Add(tower);
    }
}
