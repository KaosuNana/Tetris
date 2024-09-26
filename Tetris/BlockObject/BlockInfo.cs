using DrawObject;

namespace BlockObject;

internal class BlockInfo
{
    //对应方块的四个坐标信息
    private List<Position[]> list;

    public BlockInfo(DrawType type)
    {
        list = new List<Position[]>();

        switch (type)
        {
            case DrawType.Cube:
                //添加了一个形状的位置信息
                list.Add(new Position[3]
                {
                    new Position(2, 0),
                    new Position(0, 1),
                    new Position(2, 1)
                });
                break;
            case DrawType.Line:
                //初始化 长条形状的4种形态的坐标信息
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(0, 1),
                    new Position(0, 2)
                });
                list.Add(new Position[3]
                {
                    new Position(-4, 0),
                    new Position(-2, 0),
                    new Position(2, 0)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -2),
                    new Position(0, -1),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(-2, 0),
                    new Position(2, 0),
                    new Position(4, 0)
                });
                break;
            case DrawType.Tank:
                list.Add(new Position[3]
                {
                    new Position(-2, 0),
                    new Position(2, 0),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(-2, 0),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(-2, 0),
                    new Position(2, 0)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(2, 0),
                    new Position(0, 1)
                });
                break;
            case DrawType.Left_Ladder:
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(2, 0),
                    new Position(2, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(2, 0),
                    new Position(0, 1),
                    new Position(-2, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(-2, -1),
                    new Position(-2, 0),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(2, -1),
                    new Position(-2, 0)
                });
                break;
            case DrawType.Right_Ladder:
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(-2, 0),
                    new Position(-2, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(-2, -1),
                    new Position(0, -1),
                    new Position(2, 0)
                });
                list.Add(new Position[3]
                {
                    new Position(2, -1),
                    new Position(2, 0),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(0, 1),
                    new Position(2, 1),
                    new Position(-2, 0)
                });
                break;
            case DrawType.Left_Long_Ladder:
                list.Add(new Position[3]
                {
                    new Position(-2, -1),
                    new Position(0, -1),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(2, -1),
                    new Position(-2, 0),
                    new Position(2, 0)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(2, 1),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(2, 0),
                    new Position(-2, 0),
                    new Position(-2, 1)
                });
                break;
            case DrawType.Right_Long_Ladder:
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(0, 1),
                    new Position(2, -1)
                });
                list.Add(new Position[3]
                {
                    new Position(2, 0),
                    new Position(-2, 0),
                    new Position(2, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(0, -1),
                    new Position(-2, 1),
                    new Position(0, 1)
                });
                list.Add(new Position[3]
                {
                    new Position(-2, -1),
                    new Position(-2, 0),
                    new Position(2, 0)
                });
                break;
        }
    }

    //索引器
    /// <summary>
    /// 提供给外部根据索引快速获取 位置偏移信息的
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Position[] this[int index]
    {
        get
        {
            if (index < 0)
                return list[0];
            return index >= list.Count ? list[^1] : list[index];
        }
    }

    /// <summary>
    /// 提供给外部 获取 形态有几种
    /// </summary>
    public int Count => list.Count;
}