using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4
}

public class Square : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    /// <summary>
    /// G是从开始点A到当前方块的移动量
    /// </summary>
    public int G { get; private set; }
    /// <summary>
    /// H是从当前方块到目标点的移动量估算值
    /// </summary>
    public int H { get; private set; }
    /// <summary>
    /// F = G + H
    /// </summary>
    public int F { get; private set; }
    /// <summary>
    /// 是否为障碍物
    /// </summary>
    public bool IsObstacle { get; private set; }

    public void Calculation()
    {
        H = GetH(_demo.EndSquare);
        F = G + H;
        ShowFGH();
    }

    private AStarDemo _demo;

    public void Awake()
    {
        _demo = GetComponentInParent<AStarDemo>();
    }

    internal void SetObstacle(bool isObstacle)
    {
        IsObstacle = isObstacle;
        SetColor(Color.gray);
    }

    /// <summary>
    /// 曼哈顿长
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private int GetH(Square target)
    {
        return Mathf.Abs(target.X - X) + Mathf.Abs(target.Y - Y);
    }


    public Square GetSquare(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return _demo.GetTargetSquare(X, Y + 1);
            case Direction.Down: return _demo.GetTargetSquare(X, Y - 1);
            case Direction.Left: return _demo.GetTargetSquare(X - 1, Y);
            case Direction.Right: return _demo.GetTargetSquare(X + 1, Y);
            default: return null;
        }
    }

    internal void Init(int x, int y)
    {
        X = x;
        Y = y;
        transform.localPosition = -_demo.Offset + (new Vector3(X, Y) * 100);
        var name = string.Format("({0},{1})", X, Y);
        gameObject.name = name;
        transform.Find("Text").GetComponent<Text>().text = name;
        SetColor(Color.white);
    }

    internal void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    /// <summary>
    /// 周围非障碍物的格子
    /// </summary>
    /// <returns></returns>
    public List<Square> GetAroundSquare()
    {
        var result = new List<Square>();
        //遍历上下左右四个方向的可移动的点
        for (int i = 1; i <= 4; i++)
        {
            var direction = (Direction)i;
            var temp = GetSquare(direction);
            if (temp != null && !temp.IsObstacle)
            {
                result.Add(temp);
            }
        }
        return result;
    }


    public void ShowFGH()
    {
        transform.Find("F").GetComponent<Text>().text = F.ToString();
        transform.Find("G").GetComponent<Text>().text = G.ToString();
        transform.Find("H").GetComponent<Text>().text = H.ToString();
    }

    internal void AddG()
    {
        G++;
    }
}





