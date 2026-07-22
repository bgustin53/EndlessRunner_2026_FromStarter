using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private UIDocument uiDocument;
    
    // UI Element Fields
    private Label _scoreLabel;
    private Label _timeLabel;
    private RadioButtonGroup _timerRadio;
    private Button _startButton;

    // UI Fields 
    private int score;
    private int timeRemaining = 60;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        VisualElement root = uiDocument.rootVisualElement;

        // Query the 4 visual elements from the UXML root
        _scoreLabel = root.Q<Label>("scoreLabel");
        _timeLabel = root.Q<Label>("timeLabel");
        _timerRadio = root.Q<RadioButtonGroup>("timerRadio");
        _startButton = root.Q<Button>("startButton");
        _timeLabel.style.visibility = Visibility.Hidden;

        // Register button click listener
        if (_startButton != null)
        {
            _startButton.clicked += OnStartButtonClicked;
        }
    }

    private void OnDisable()
    {
        // Unregister to avoid memory leaks/null callbacks
        if (_startButton != null)
        {
            _startButton.clicked -= OnStartButtonClicked;
        }
    }

    private void OnStartButtonClicked()
    {
        // Get zero-based selected index of the RadioButtonGroup (0 or 1)
        int selectedOptionIndex = _timerRadio != null ? _timerRadio.value : 0;
        
        StartGame(selectedOptionIndex);
    }

    // Method stub called when Start button is pressed
    private void StartGame(int selectedRadioIndex)
    {
        if(selectedRadioIndex == 1)
        {
            _timeLabel.style.visibility = Visibility.Visible;
            InvokeRepeating("Timer", 0, 1);
        }
        _timerRadio.style.visibility = Visibility.Hidden;
        _startButton.style.visibility = Visibility.Hidden;
        GameManager.Instance.gameOver = false;
    }

    // Example helper methods to update display text during gameplay
    public void UpdateScore(int update)
    {
        score += update;
        if (_scoreLabel != null)
            _scoreLabel.text = $"Score: {score}";
    }

    private void UpdateTime()
    {
        if (_timeLabel != null)
            _timeLabel.text = $"Time Remaining: {timeRemaining}";
    }

    private void Timer()
    {
        UpdateTime();
        timeRemaining--;
        if(timeRemaining < 0 || GameManager.Instance.gameOver)
        {
            CancelInvoke();
            GameOver();
        }
    }

    public void GameOver()
    {
        _timeLabel.style.visibility = Visibility.Visible;
        _timeLabel.text = "Game Over";
        GameManager.Instance.gameOver = true;
    }
}