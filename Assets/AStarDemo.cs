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
        FindPath(StartSquare);
        yield return new WaitForSeconds(1);
        DebugPath();
    }

    private void DebugPath()
    {
        var length = ResultPath.Count;
        for (int i = 0; i < length; i++)
        {
            ResultPath[i].SetColor(Color.red);
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

    public async Task FindPath(Square current)
    {
        ResultPath.Add(current);
        ClosedSquares.Add(current);
        var aroundSquares = current.GetAroundSquare();
        var aroundSquaresLength = aroundSquares.Count;
        for (int i = 0; i < aroundSquaresLength; i++)
        {
            var temp = aroundSquares[i];
            if (ClosedSquares.Contains(temp))
            {
                continue;
            }
            if (OpenSquares.Contains(temp))
            {
                continue;
            }
            temp.AddG();
            OpenSquares.Add(temp);
        }

        var length = OpenSquares.Count;
        //计算OpenSquares里面的F
        for (int i = 0; i < length; i++)
        {
            OpenSquares[i].Calculation();
        }

        //查找OpenSquares里面F最小的值
        Square min = null;
        for (int i = 0; i < length; i++)
        {
            if (min == null || OpenSquares[i].F < min.F)
            {
                min = OpenSquares[i];
            }
        }

        foreach (var item in OpenSquares)
        {
            if (min != item)
            {
                ClosedSquares.Add(item);
            }
        }


        //记录min并且递归原来的操作
        ResultPath.Add(min);
        ClosedSquares.Add(min);


        //if (Temp++ > 1000)
        //{
        //    return;
        //}

        if (min == EndSquare)
        {
            return;
        }

        await Task.Delay(100);

        await FindPath(min);
    }


    int Temp = 1;
}
