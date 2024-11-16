using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image Logo;
    //[SerializeField] Animator LogoAnimator;
    [SerializeField] GameObject Book;

    public enum MainMenuState
    {
        COMPANYLOGO, BOOK_INTRO, BOOK_ZOOM, BOOK_OPEN
    }

    private MainMenuState _state;

    static MainMenuManager _instance;

    public static MainMenuManager Instance {  get { return _instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateState(MainMenuState state)
    {
        switch (state)
        {
            case MainMenuState.COMPANYLOGO:
                Book.SetActive(false);
                Logo.enabled = true;
                //LogoAnimator.SetTrigger("play");
                break;

            case MainMenuState.BOOK_INTRO:
                Book.SetActive(true);
                Logo.enabled = false;
                break;
        }

    }

    public void NextState()
    {
        UpdateState(_state++);
    }
}
