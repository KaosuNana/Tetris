namespace DrawObject;

internal enum DrawType
{
    /// <summary>
    /// 墙壁
    /// </summary>
    Wall,

    /// <summary>
    /// 正方形方块
    /// </summary>
    Cube,

    /// <summary>
    /// 直线
    /// </summary>
    Line,

    /// <summary>
    /// 坦克
    /// </summary>
    Tank,

    /// <summary>
    /// 左梯子
    /// </summary>
    Left_Ladder,

    /// <summary>
    /// 右梯子
    /// </summary>
    Right_Ladder,

    /// <summary>
    /// 左长梯子
    /// </summary>
    Left_Long_Ladder,

    /// <summary>
    /// 右长梯子
    /// </summary>
    Right_Long_Ladder,
}

internal class DrawObject : IDraw
{
    public Position pos;
    public DrawType type;

    public DrawObject(DrawType type)
    {
        this.type = type;
    }

    public DrawObject(DrawType type, int x, int y) : this(type)
    {
        this.pos = new Position(x, y);
    }

    public void Draw()
    {
        //屏幕外不用再绘制
        if (pos.y < 0)
            return;

        Console.SetCursorPosition(pos.x, pos.y);

        switch (type)
        {
            case DrawType.Wall:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case DrawType.Cube:
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case DrawType.Line:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case DrawType.Tank:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            case DrawType.Left_Ladder:
            case DrawType.Right_Ladder:
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            case DrawType.Left_Long_Ladder:
            case DrawType.Right_Long_Ladder:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                break;
        }

        Console.Write("■");
    }

    /// <summary>
    /// 切换方块类型 主要用于搬砖下落到地图时 把搬砖类型编程墙壁类型
    /// </summary>
    /// <param name="type"></param>
    public void ChangeType(DrawType type)
    {
        this.type = type;
    }
    
    public void ClearDraw()
    {
        if (pos.y < 0)
            return;
        Console.SetCursorPosition(pos.x, pos.y);
        Console.Write("  ");
    }
}