using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image Logo;
    //[SerializeField] Animator LogoAnimator;
    [SerializeField] GameObject Book;
    private Animator BookAnimator;
    private Animator CameraAnimator;
    private BoxCollider BookCollider;

    public enum MainMenuState
    {
        COMPANYLOGO, BOOK_INTRO, BOOK_OPEN
    }

    private MainMenuState _state;

    static MainMenuManager _instance;

    public static MainMenuManager Instance {  get { return _instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        _state = MainMenuState.COMPANYLOGO;
        UpdateState(_state);
        BookAnimator = Book.GetComponent<Animator>();
        CameraAnimator = Camera.main.GetComponent<Animator>();
        BookCollider = Book.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == MainMenuState.BOOK_INTRO)
        {
            JournalClickTest();
        }
    }

    void UpdateState(MainMenuState state)
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

            case MainMenuState.BOOK_OPEN:
                BookCollider.enabled = false;
                BookAnimator.SetTrigger("open");

                break;
        }

    }

    public void NextState()
    {
        UpdateState(++_state);
    }

    private void JournalClickTest()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null && hit.collider.CompareTag("MainMenuJournal"))
                {
                    CameraAnimator.SetTrigger("zoom");
                }
            }
        }
    }
}
