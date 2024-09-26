namespace GameScene;

internal class BeginScene : BaseScene
{
    public BeginScene()
    {
        strTitle = "俄罗斯方块";
        strOne = "开始游戏";
    }

    public override void EnterJDoSomthing()
    {
        //按J键做什么的逻辑
        if (nowSelIndex == 0)
            Game.ChangeScene(Scene_Type.Game);
        else
            Environment.Exit(0);
    }
}