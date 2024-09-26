using DrawObject;
using BlockObject;
using InputController;

namespace GameScene;

internal class GameScene : ISceneUpdate
{
    private Map map;
    BlockWorker blockWorker;

    public GameScene()
    {
        map = new Map(this);
        blockWorker = new BlockWorker();

        InputThread.Instance.inputEvent += CheckInputThread;
    }

    public void Update()
    {
        //锁里面不要包含 休眠 不然会影响别人
        lock (blockWorker)
        {
            //地图绘制
            map.Draw();
            //搬运工绘制
            blockWorker.Draw();
            //自动向下移动
            if (blockWorker.CanMove(map))
                blockWorker.AutoMove();
        }
        //用线程休眠的形式 
        Thread.Sleep(200);
    }

    void CheckInputThread()
    {
        if (Console.KeyAvailable)
        {
            //避免影响主线程
            lock (blockWorker)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                        //判断能不能变形
                        if (blockWorker.CanChange(Change_Type.Left, map))
                            blockWorker.Change(Change_Type.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        if (blockWorker.CanChange(Change_Type.Right, map))
                            blockWorker.Change(Change_Type.Right);
                        break;
                    case ConsoleKey.A:
                        if (blockWorker.CanMoveRL(Change_Type.Left, map))
                            blockWorker.MoveRL(Change_Type.Left);
                        break;
                    case ConsoleKey.D:
                        if (blockWorker.CanMoveRL(Change_Type.Right, map))
                            blockWorker.MoveRL(Change_Type.Right);
                        break;
                    case ConsoleKey.S:
                        //向下动
                        if (blockWorker.CanMove(map))
                            blockWorker.AutoMove();
                        break;
                }
            }
        }
    }

    public void StopThread()
    {
        //移除输入事件监听
        InputThread.Instance.inputEvent -= CheckInputThread;
    }
}