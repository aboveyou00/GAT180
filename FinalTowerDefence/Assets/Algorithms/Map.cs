using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Map
{
    private Map(int[][] data)
    {
        if (data == null || data.Length == 0) throw new ArgumentNullException("data");
        var len = data[0] != null ? data[0].Length : 0;
        if (data.Any(d => d == null || d.Length != len)) throw new ArgumentException("data");
        _data = data;
    }

    public static Map Generate(int width, int height)
    {
        if (width <= 1 || width % 2 != 1) throw new ArgumentException("width");
        if (height <= 1 || height % 2 != 1) throw new ArgumentException("height");

        var data = new int[width][];
        for (int q = 0; q < width; q++)
        {
            data[q] = new int[height];
        }
        var map = new Map(data);
        map.generate();
        return map;
    }

    private int[][] _data;

    public int Width
    {
        get
        {
            return _data.Length;
        }
    }
    public int Height
    {
        get
        {
            return _data[0].Length;
        }
    }

    public int this[int x, int y]
    {
        get
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height) return -1;
            return _data[x][y];
        }
        set
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height) throw new InvalidOperationException();
            _data[x][y] = value;
        }
    }
    public int this[Point p]
    {
        get
        {
            return this[p.x, p.y];
        }
        set
        {
            this[p.x, p.y] = value;
        }
    }

    private Point[] path;
    public Point[] GetPath()
    {
        return path;
    }
    public bool IsPointOnPath(int x, int y)
    {
        foreach (var pt in path)
        {
            if (pt.x == x && pt.y == y) return true;
        }
        return false;
    }
    public bool IsPointOnPath(Point p)
    {
        return IsPointOnPath(p.x, p.y);
    }

    private void generate()
    {
        Random rnd = new Random();

        Point current = generateEntry(rnd);
        depthFirstFill(rnd, current);
        calculateRoute();
        decorateTerrain(rnd);
    }
    private Point generateEntry(Random rnd)
    {
        Point start;
        bool horiz = rnd.NextDouble() < .5;
        bool last = rnd.NextDouble() < .5;

        Point forward;
        if (horiz && !last)
        {
            start = new Point(1 + rnd.Next((Width - 1) / 2) * 2, 0);
            forward = new Point(0, 1);
        }
        else if (horiz)
        {
            start = new Point(1 + rnd.Next((Width - 1) / 2) * 2, Height - 1);
            forward = new Point(0, -1);
        }
        else if (!last)
        {
            start = new Point(0, 1 + rnd.Next((Height - 1) / 2) * 2);
            forward = new Point(1, 0);
        }
        else
        {
            start = new Point(Width - 1, 1 + rnd.Next((Height - 1) / 2) * 2);
            forward = new Point(-1, 0);
        }

        this[start] = 1;
        start += forward;
        this[start] = 2;
        return start;
    }
    private void depthFirstFill(Random rnd, Point current)
    {
        begin:
        var dir_idx = rnd.Next(4);
        for (int q = 0; q < 4; q++)
        {
            var dir = cardinal[(q + dir_idx) % 4];
            if (this[current + dir * 2] == 0)
            {
                var lastVal = this[current];
                this[current += dir] = ++lastVal;
                this[current += dir] = ++lastVal;
                depthFirstFill(rnd, current);
                goto begin;
            }
        }
    }
    private void calculateRoute()
    {
        Point maxPos = new Point(0, 0);
        int max = 0;
        for (int q = 0; q < Width; q++)
        {
            for (int w = 0; w < Height; w++)
            {
                var val = this[q, w];
                if (val > max)
                {
                    maxPos = new Point(q, w);
                    max = val;
                }
            }
        }

        var positions = new List<Point>();
        positions.Add(maxPos);
        lookAround(positions, maxPos, max - 1, up);
        path = positions.Reverse<Point>().ToArray();
    }
    private void decorateTerrain(Random rnd)
    {
        for (int q = 0; q < Width; q++)
        {
            for (int w = 0; w < Height; w++)
            {
                var val = this[q, w];
                this[q, w] = (rnd.NextDouble() < .3) ? 2 : 0;
            }
        }

        var swap = new int[Width, Height];
        for (int q = 0; q < Width; q++)
        {
            for (int w = 0; w < Height; w++)
            {
                var forestCount = countAround(q, w, 2);
                swap[q, w] = (forestCount >= 4) ? 2 :
                             (forestCount <= 2) ? 0 :
                                                  this[q, w];
            }
        }
        for (int q = 0; q < Width; q++)
            for (int w = 0; w < Height; w++)
                this[q, w] = swap[q, w];

        foreach (var point in path.Skip(1))
            this[point] = 1;
        this[path.Last()] = 3;
    }
    private int countAround(int x, int y, int val)
    {
        return (this[x - 1, y - 1] == val ? 1 : 0) +
               (this[x + 0, y - 1] == val ? 1 : 0) +
               (this[x + 1, y - 1] == val ? 1 : 0) +
               (this[x - 1, y + 0] == val ? 1 : 0) +
               (this[x + 0, y + 0] == val ? 1 : 0) +
               (this[x + 1, y + 0] == val ? 1 : 0) +
               (this[x - 1, y + 1] == val ? 1 : 0) +
               (this[x + 0, y + 1] == val ? 1 : 0) +
               (this[x + 1, y + 1] == val ? 1 : 0);
    }
    private void lookAround(List<Point> positions, Point currentP, int nextVal, Point forward)
    {
        begin:
        if (nextVal == -1) return;
        if (nextVal == 0 || this[currentP + forward] == nextVal)
        {
            currentP += forward;
            positions.Add(currentP);
            nextVal--;
            goto begin;
        }
        for (int q = 0; q < cardinal.Length; q++)
        {
            forward = cardinal[q];
            if (this[currentP + forward] == nextVal)
            {
                currentP += forward;
                positions.Add(currentP);
                nextVal--;
                goto begin;
            }
        }
    }

    private static Point left = new Point(-1, 0);
    private static Point right = new Point(1, 0);
    private static Point up = new Point(0, -1);
    private static Point down = new Point(0, 1);
    private static Point[] cardinal = new Point[] { left, right, up, down };

    public struct Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x, y;

        public static Point operator *(Point p, int magnitude)
        {
            return new Point(p.x * magnitude, p.y * magnitude);
        }
        public static Point operator +(Point lhs, Point rhs)
        {
            return new Point(lhs.x + rhs.x, lhs.y + rhs.y);
        }
        public static Point operator -(Point lhs, Point rhs)
        {
            return new Point(lhs.x - rhs.x, lhs.y - rhs.y);
        }
        public static Point operator -(Point p)
        {
            return new Point(-p.x, -p.y);
        }
    }
}
