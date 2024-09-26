namespace InputController;

internal class InputThread
{
    Thread inputThread;
    
    public event Action inputEvent;
    
    static InputThread instance = new InputThread();

    public static InputThread Instance => instance;

    InputThread()
    {
        inputThread = new Thread(InputCheck);
        inputThread.IsBackground = true;
        inputThread.Start();
    }
    
    void InputCheck()
    {
        while (true)
        {
            inputEvent?.Invoke();
        }
    }
}