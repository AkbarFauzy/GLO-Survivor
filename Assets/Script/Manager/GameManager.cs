using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Player;
using Survivor.Mechanic.UI;
using Survivor.Manager.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Survivor.Manager {
    public class GameManager : Observer
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.CapsLock)) {
                NotifyEvents(Events.SpawnExpOrb, new Vector2(Random.Range(0,10f) , Random.Range(0,10f) ));
                Debug.Log("Orb Spawn");
            }
        }

    }
}

