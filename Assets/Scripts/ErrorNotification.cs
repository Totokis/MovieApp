using UnityEngine.Events;

public class ErrorNotification
{
    private string message;
    public string Message
    {
        get => message;
        set
        {
            message = value;
            MessageSet.Invoke();
        }
    }
    public UnityEvent MessageSet = new UnityEvent();

}
