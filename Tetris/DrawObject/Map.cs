using GameScene;

namespace DrawObject;

internal class Map : IDraw
{
    public int w;

    public int h;

    //固定墙壁
    private List<DrawObject> walls = new List<DrawObject>();

    //动态墙壁
    public List<DrawObject> dynamicWalls = new List<DrawObject>();

    GameScene.GameScene nowGameScene;

    //记录动态行有多少方块，用于消除行
    private int[] recordInfo;

    public Map(GameScene.GameScene scene)
    {
        this.nowGameScene = scene;
        h = Game.h - 6;
        recordInfo = new int[h];
        w = 0;

        for (int i = 0; i < Game.w; i += 2)
        {
            walls.Add(new DrawObject(DrawType.Wall, i, h));
            ++w;
        }

        w -= 2;

        for (int i = 0; i < h; i++)
        {
            walls.Add(new DrawObject(DrawType.Wall, 0, i));
            walls.Add(new DrawObject(DrawType.Wall, Game.w - 2, i));
        }
    }

    public void Draw()
    {
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].Draw();
        }

        //绘制动态墙壁 有才绘制
        for (int i = 0; i < dynamicWalls.Count; i++)
        {
            dynamicWalls[i].Draw();
        }
    }

    /// <summary>
    /// 提供给外部添加动态方块的函数
    /// </summary>
    /// <param name="walls"></param>
    public void AddWalls(List<DrawObject> walls)
    {
        for (int i = 0; i < walls.Count; i++)
        {
            //传递方块进来时 把其类型改成 墙壁类型
            walls[i].ChangeType(DrawType.Wall);
            dynamicWalls.Add(walls[i]);
            //在动态墙壁添加处 发现 位置顶满了 就结束
            if (walls[i].pos.y <= 0)
            {
                //关闭输入线程
                this.nowGameScene.StopThread();
                //场景切换 切换到结束界面
                Game.ChangeScene(Scene_Type.End);
                return;
            }

            //进行添加动态墙壁的计数
            //根据索引来得到行
            //h 是 Game.h - 6
            //y 最大为 Game.h - 7
            recordInfo[h - 1 - walls[i].pos.y] += 1;
        }
        //
        // //先把之前的动态小方块擦掉
        ClearDraw();
        // //检测移除
        CheckClear();
        // //再绘制动态小方块
        Draw();
    }
    
    /// <summary>
    /// 方块变成墙壁后，更新现有动态墙壁
    /// </summary>
    public void ClearDraw()
    {
        //绘制动态墙壁 有才绘制
        for (int i = 0; i < dynamicWalls.Count; i++)
        {
            dynamicWalls[i].ClearDraw();
        }
    }

    public void CheckClear()
    {
        List<DrawObject> delList = new List<DrawObject>();
        //检测对应行是否满足消除 w - 2
        for (int i = 0; i < recordInfo.Length; i++)
        {
            //必须满足条件 才证明满了
            //小方块计数 == w（这个w已经是去掉了左右两边的固定墙壁）
            if (recordInfo[i] == w)
            {
                //1.这一行的所有小方块移除
                for (int j = 0; j < dynamicWalls.Count; j++)
                {
                    //当前通过动态方块的y计算它在哪一行 如果行号
                    //和当前记录索引一致 就证明 应该移除
                    if (i == h - 1 - dynamicWalls[j].pos.y)
                    {
                        //移除这个方块 为了安全移除 添加一个记录列表
                        delList.Add(dynamicWalls[j]);
                    }
                    //2.要这一行之上的所有小方块下移一个单位
                    //如果当前的这个位置 是该行以上 那就该小方块 下移一格
                    else if (h - 1 - dynamicWalls[j].pos.y > i)
                    {
                        ++dynamicWalls[j].pos.y;
                    }
                }
                //移除待删除的小方块
                for (int j = 0; j < delList.Count; j++)
                {
                    dynamicWalls.Remove(delList[j]);
                }

                //3.记录小方块数量的数组从上到下迁移
                for (int j = i; j < recordInfo.Length - 1; j++)
                {
                    recordInfo[j] = recordInfo[j + 1];
                }
                //置空最顶的计数
                recordInfo[recordInfo.Length - 1] = 0;

                //跨掉一行后 再次去从头检测是否跨层
                CheckClear();
                break;
            }
        }
    }
}