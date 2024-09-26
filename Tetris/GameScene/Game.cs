namespace GameScene;

public enum Scene_Type
{
    Begin,
    Game,
    End,
}

public class Game
{
    public const int w = 50;
    public const int h = 35;

    public static ISceneUpdate nowScene;

    public Game()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(w, h);
        Console.SetBufferSize(w, h);

        ChangeScene(Scene_Type.Begin);
    }

    public void Start()
    {
        while (true)
        {
            if (nowScene != null)
                nowScene.Update();
        }
    }

    public static void ChangeScene(Scene_Type type)
    {
        Console.Clear();

        switch (type)
        {
            case Scene_Type.Begin:
                nowScene = new BeginScene();
                break;
            case Scene_Type.Game:
                nowScene = new GameScene();
                break;
            case Scene_Type.End:
                nowScene = new EndScene();
                break;
        }
    }
}