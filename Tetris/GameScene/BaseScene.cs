namespace GameScene;

internal abstract class BaseScene : ISceneUpdate
{
    protected int nowSelIndex;
    protected string strTitle;
    protected string strOne;

    public abstract void EnterJDoSomthing();

    public void Update()
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.SetCursorPosition(Game.w / 2 - strTitle.Length, 5);
        Console.Write(strTitle);

        Console.SetCursorPosition(Game.w / 2 - strOne.Length, 8);
        Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
        Console.Write(strOne);
        Console.SetCursorPosition(Game.w / 2 - 4, 10);
        Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
        Console.Write("结束游戏");

        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.W:
                --nowSelIndex;
                if (nowSelIndex < 0)
                    nowSelIndex = 0;

                break;
            case ConsoleKey.S:
                ++nowSelIndex;
                if (nowSelIndex > 1)
                    nowSelIndex = 1;

                break;
            case ConsoleKey.J:
                EnterJDoSomthing();
                break;
        }
    }
}