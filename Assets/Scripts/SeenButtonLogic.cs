using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeenButtonLogic : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite seen;
    [SerializeField] private Sprite unseen;
    [SerializeField] private Image image;

    public UnityEvent stateChanged = new UnityEvent();
    private bool state = false;

    private void Awake()
    {
        stateChanged.AddListener(ChangeSprite);
        SetState(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchState();
        Debug.Log("STATE CHANGED");
    }
    private void ChangeSprite()
    {
        if (state)
            image.sprite = seen;
        else
            image.sprite = unseen;
    }
    private void SwitchState()
    {
        SetState(!state);
    }

    public void SetState(bool movieSeen)
    {
        state = movieSeen;
        stateChanged.Invoke();
    }


    public bool GetState()
    {
        return state;
    }
}
