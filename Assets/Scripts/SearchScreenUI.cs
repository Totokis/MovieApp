using Lean.Gui;
using UnityEngine;

public class SearchScreenUI : MonoBehaviour
{
    [SerializeField] private SearchScreen searchScreen;
    [SerializeField] private LeanButton addNewMovieButton;

    private void Awake()
    {
        addNewMovieButton.OnClick.AddListener(searchScreen.AddNewMovie);
    }
}
