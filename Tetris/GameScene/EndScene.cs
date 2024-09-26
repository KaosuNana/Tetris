namespace GameScene;

internal class EndScene : BaseScene
{
    public EndScene()
    {
        strTitle = "结束游戏";
        strOne = "回到开始界面";
    }

    public override void EnterJDoSomthing()
    {
        //按J键做什么的逻辑
        if (nowSelIndex == 0)
            Game.ChangeScene(Scene_Type.Begin);
        else
            Environment.Exit(0);
    }
}