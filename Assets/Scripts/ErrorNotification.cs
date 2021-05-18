using UnityEngine;
using UnityEngine.Events;

public class ErrorNotification
{
    string message;
    public UnityEvent MessageSet = new UnityEvent();
    public string Message
    {
        get => message;
        set
        {
            message = value;
            MessageSet.Invoke();
        }
    }
}
