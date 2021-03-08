using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AStarDemo : MonoBehaviour
{
    public int Width = 3;
    public int Height = 3;
    public Square[][] Squares;
    public List<Square> OpenSquares = new List<Square>();
    public List<Square> ClosedSquares = new List<Square>();
    public Square StartSquare;
    public Square EndSquare;
    public Vector3 Offset;


    public List<Square> ResultPath = new List<Square>();


    private void Awake()
    {
        Squares = new Square[Width][];

        var original = Resources.Load<Square>("Square");
        var content = transform;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var temp = Instantiate(original, content);
                temp.Init(i, j);
                AddToSqures(temp);
            }
        }

        StartSquare = Squares[0][0];
        EndSquare = Squares[4][3];
        Squares[2][3].SetObstacle(true);
    }

    private void AddToSqures(Square square)
    {
        var x = square.X;
        var y = square.Y;
        if (Squares[x] == null)
        {
            Squares[x] = new Square[Height];
        }
        Squares[x][y] = square;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        ClosedSquares.Add(StartSquare);
        FindPath();
        yield return new WaitForSeconds(1);
        DebugPath();
    }

    private void DebugPath()
    {
        var length = ClosedSquares.Count;
        for (int i = 0; i < length; i++)
        {
            ClosedSquares[i].SetColor(Color.red);
        }
    }


    public Square GetTargetSquare(int x, int y)
    {
        if (x < Width && x >= 0 && y < Height && y >= 0)
        {
            return Squares[x][y];
        }
        return null;
    }

    /// <summary>
    /// 曼哈顿长
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    public int GetH(Square current)
    {
        return Mathf.Abs(EndSquare.X - current.X) + Mathf.Abs(EndSquare.Y - current.Y);
    }

    public async Task FindPath()
    {
        var s = GetLastItemInClosedSquares();

        var list = s.GetAroundSquare();
        OpenSquares.AddRange(list);
        var min = FindMin(OpenSquares);
        if (min != null)
        {
            OpenSquares.Remove(min);
            ClosedSquares.Add(min);
        }
        await Task.Delay(100);



        if (OpenSquares.Count == 0)
        {
            return;
        }
        await FindPath();
    }

    private Square GetLastItemInClosedSquares()
    {
        return ClosedSquares[ClosedSquares.Count - 1];
    }

    private Square FindMin(List<Square> squares)
    {
        Square min = null;
        var length = squares.Count;
        for (int i = 0; i < length; i++)
        {
            if (min == null || squares[i].F < min.F)
            {
                min = squares[i];
            }
        }
        return min;
    }
}
