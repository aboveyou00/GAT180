using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ForestTileController : MonoBehaviour
{
    public MapController map;
    public SpriteRenderer spriteRenderer;
    public int x, y;

    public Sprite forestNESWSprite;
    public Sprite forestESWSprite;
    public Sprite forestNSWSprite;
    public Sprite forestNEWSprite;
    public Sprite forestNESSprite;
    public Sprite forestSWSprite;
    public Sprite forestEWSprite;
    public Sprite forestESSprite;
    public Sprite forestNWSprite;
    public Sprite forestNSSprite;
    public Sprite forestNESprite;
    public Sprite forestNSprite;
    public Sprite forestESprite;
    public Sprite forestSSprite;
    public Sprite forestWSprite;
    public Sprite forestSprite;

    private void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitTile(object[] args)
    {
        map = (MapController)args[0];
        x = (int)args[1];
        y = (int)args[2];
        chooseTileSprite();
    }

    private void chooseTileSprite()
    {
        var forest = map.map[x, y];
        bool n = map.map[x, y + 1] == forest;
        bool e = map.map[x + 1, y] == forest;
        bool s = map.map[x, y - 1] == forest;
        bool w = map.map[x - 1, y] == forest;
        this.spriteRenderer.sprite = getSprite(n, e, s, w);
    }
    private Sprite getSprite(bool n, bool e, bool s, bool w)
    {
        if (n)
        {
            if (e)
            {
                if (s) return w ? forestNESWSprite : forestNESSprite;
                else return w ? forestNEWSprite : forestNESprite;
            }
            else
            {
                if (s) return w ? forestNSWSprite : forestNSSprite;
                else return w ? forestNWSprite : forestNSprite;
            }
        }
        else
        {
            if (e)
            {
                if (s) return w ? forestESWSprite : forestESSprite;
                else return w ? forestEWSprite : forestESprite;
            }
            else
            {
                if (s) return w ? forestSWSprite : forestSSprite;
                else return w ? forestWSprite : forestSprite;
            }
        }
    }
}
