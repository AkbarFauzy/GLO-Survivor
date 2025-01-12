using Survivor.Character.Player;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Survivor.Manager.UI {
    public class HUDManager : Observer
    {
        public static HUDManager Instance { get; private set; }

        [SerializeField] private Slider _expBar;
        [SerializeField] private TextMeshProUGUI _lvlTxt;
        [SerializeField] private TextMeshProUGUI _timeTxt;
        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private Button RetryButton;

        private float _timer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Subscribe<Player>(Events.PlayerGainingExperience, OnPlayerGainingExperience);
            Subscribe<Player>(Events.OnPlayerLevelUp, OnPlayerLevelUp);
            Subscribe(Events.GameOver, OnGameOver);
            RetryButton.onClick.AddListener(() => LevelLoader.Instance.LoadLevel(0));
        }

        private void FixedUpdate()
        {
            int minutes = Mathf.FloorToInt(_timer / 60f);
            int seconds = Mathf.FloorToInt(_timer % 60f);

            _timer += Time.fixedDeltaTime;
            _timeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }


        private void OnDestroy()
        {
            Unsubscribe<Player>(Events.PlayerGainingExperience, OnPlayerGainingExperience);
            Unsubscribe<Player>(Events.OnPlayerLevelUp, OnPlayerLevelUp);
        }

        void OnPlayerGainingExperience(Player player) {
            Debug.Log("Player Gaining Experience");
            _expBar.value = player.Experience;
        }

        void OnPlayerLevelUp(Player player)
        {
            Debug.Log("Player On LevelUp");
            _expBar.value = 0;
            _expBar.maxValue = player.GetCurrentLevelMaxEXP();
            _lvlTxt.text = "Lvl. " + player.Level;
        }

        void OnGameOver()
        {
            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }
}

