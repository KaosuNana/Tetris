using DrawObject;
using GameScene;

namespace BlockObject;

enum Change_Type
{
    Left,
    Right,
}

internal class BlockWorker : IDraw
{
    List<DrawObject.DrawObject> blocks;

    //每个形态的方块信息
    Dictionary<DrawType, BlockInfo> blockInfoDic;

    BlockInfo nowBlockInfo;

    int nowInfoIndex;

    public BlockWorker()
    {
        //初始化 装块信息 
        blockInfoDic = new Dictionary<DrawType, BlockInfo>()
        {
            { DrawType.Cube, new BlockInfo(DrawType.Cube) },
            { DrawType.Line, new BlockInfo(DrawType.Line) },
            { DrawType.Tank, new BlockInfo(DrawType.Tank) },
            { DrawType.Left_Ladder, new BlockInfo(DrawType.Left_Ladder) },
            { DrawType.Right_Ladder, new BlockInfo(DrawType.Right_Ladder) },
            { DrawType.Left_Long_Ladder, new BlockInfo(DrawType.Left_Long_Ladder) },
            { DrawType.Right_Long_Ladder, new BlockInfo(DrawType.Right_Long_Ladder) },
        };

        RandomCreateBlock();
    }

    public void Draw()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].Draw();
        }
    }

    public void RandomCreateBlock()
    {
        //随机方块类型
        Random r = new Random();
        DrawType type = (DrawType)r.Next(1, 8);
        blocks = new List<DrawObject.DrawObject>()
        {
            new(type),
            new(type),
            new(type),
            new(type),
        };

        //需要初始化方块位置
        //原点位置 我们随机 自己定义 方块List中第0个就是我们的原点方块
        blocks[0].pos = new Position(24, -5);
        //其它三个方块的位置
        //取出方块的形态信息 来进行具体的随机
        //把取出来的 方块具体的形态信息 存起来 用于之后变形
        nowBlockInfo = blockInfoDic[type];
        //随机几种形态中的一种来设置方块的信息
        nowInfoIndex = r.Next(0, nowBlockInfo.Count);
        //取出其中一种形态的坐标信息
        Position[] pos = nowBlockInfo[nowInfoIndex];
        //另外的三个小方块进行设置 计算
        for (int i = 0; i < pos.Length; i++)
        {
            //取出来的pos是相对原点方块的坐标 所以需要进行计算
            blocks[i + 1].pos = blocks[0].pos + pos[i];
        }
    }

    public void ClearDraw()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].ClearDraw();
        }
    }

    public void Change(Change_Type type)
    {
        //变之前把之前的位置擦除
        ClearDraw();

        switch (type)
        {
            case Change_Type.Left:
                --nowInfoIndex;
                if (nowInfoIndex < 0)
                    nowInfoIndex = nowBlockInfo.Count - 1;
                break;
            case Change_Type.Right:
                ++nowInfoIndex;
                if (nowInfoIndex >= nowBlockInfo.Count)
                    nowInfoIndex = 0;
                break;
        }

        //得到索引目的 是得到对应形态的 位置偏移信息
        //用于设置另外的三个小方块
        Position[] pos = nowBlockInfo[nowInfoIndex];
        //将另外的三个小方块进行设置 计算
        for (int i = 0; i < pos.Length; i++)
        {
            //取出来的pos是相对原点方块的坐标 所以需要进行计算
            blocks[i + 1].pos = blocks[0].pos + pos[i];
        }

        //变之后再来绘制
        Draw();
    }

    public bool CanChange(Change_Type type, Map map)
    {
        //用一个临时变量记录 当前索引 不变化当前索引
        //变化这个临时变量
        int nowIndex = nowInfoIndex;

        switch (type)
        {
            case Change_Type.Left:
                --nowIndex;
                if (nowIndex < 0)
                    nowIndex = nowBlockInfo.Count - 1;
                break;
            case Change_Type.Right:
                ++nowIndex;
                if (nowIndex >= nowBlockInfo.Count)
                    nowIndex = 0;
                break;
        }

        //通过临时索引 取出形态信息 用于重合判断
        Position[] nowPos = nowBlockInfo[nowIndex];
        //判断是否超出地图边界
        Position tempPos;
        for (int i = 0; i < nowPos.Length; i++)
        {
            tempPos = blocks[0].pos + nowPos[i];
            //判断左右边界 和 下边界
            if (tempPos.x is < 2 or >= Game.w - 2 ||
                tempPos.y >= map.h)
                return false;
        }

        //判断是否和地图上的动态方块重合
        foreach (var t in nowPos)
        {
            tempPos = blocks[0].pos + t;
            if (map.dynamicWalls.Any(t1 => tempPos == t1.pos))
                return false;
        }

        return true;
    }

    public void MoveRL(Change_Type type)
    {
        //在动之前 要得到原来的坐标 进行擦除
        ClearDraw();

        //根据传入的类型 决定左动还是右动
        //左动 x-2,y0  右动x+2 y 0
        //得到我们的便宜位置
        var movePos = new Position(type == Change_Type.Left ? -2 : 2, 0);
        //遍历我的所有小方块
        foreach (var t in blocks)
        {
            t.pos += movePos;
        }

        //动之后 再画上去
        Draw();
    }

    public bool CanMoveRL(Change_Type type, Map map)
    {
        //根据传入的类型 决定左动还是右动
        //左动 x-2,y0  右动x 2 y 0
        //得到我们的便宜位置
        var movePos = new Position(type == Change_Type.Left ? -2 : 2, 0);

        //要不和左右边界重合
        //动过后的结果 不能直接改小方块的位置 改了就覆水难收
        //这只是想预判断 所以 得一个临时变量用于判断即可
        Position pos;
        for (int i = 0; i < blocks.Count; i++)
        {
            pos = blocks[i].pos + movePos;
            if (pos.x < 2 || pos.x >= Game.w - 2)
                return false;
        }

        //要不和动态方块重合了
        foreach (var t1 in blocks)
        {
            pos = t1.pos + movePos;
            if (map.dynamicWalls.Any(t => pos == t.pos))
                return false;
        }
        
        return true;
    }
    
    public void AutoMove()
    {
        //变位置之前擦除
        ClearDraw();

        //首要要得到移动的多少
        //Position downMove = new Position(0, 1);
        //得到所有的方块 让其向下移动
        for (int i = 0; i < blocks.Count; i++)
        {
            //blocks[i].pos += downMove;
            blocks[i].pos.y += 1;
        }

        //变了位置再画
        Draw();
    }

    public bool CanMove(Map map)
    {
        //用临时变量存储 下一次移动的位置 然后用于进行重合判断
        var movePos = new Position(0, 1);
        Position pos;
        //边界
        foreach (var t in blocks)
        {
            pos = t.pos + movePos;
            if (pos.y < map.h) continue;
            //停下来 给予地图动态方块
            map.AddWalls(blocks);
            //随机创建新的方块
            RandomCreateBlock();
            return false;
        }

        //动态方块
        foreach (var t1 in blocks)
        {
            pos = t1.pos + movePos;
            if (map.dynamicWalls.Any(t => pos == t.pos))
            {
                map.AddWalls(blocks);
                //随机创建新的方块
                RandomCreateBlock();
                return false;
            }
        }

        return true;
    }
}