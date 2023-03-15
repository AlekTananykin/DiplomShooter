using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Controllers
{
    internal class PlayerPanelController : IInitialization, IPlayerPanel, IExecute
    {
        private GameObject _view;
        private GameObject _rightPanel;

        internal PlayerPanelController(GameObject view)
        {
            _view = view;
            _rightPanel = GameObject.Find("RightPanelText");
        }

        public void Execute(float deltaTime)
        {
            Debug.Log("PlayerPanelController");
        }

        public void Initialize()
        {
            Debug.Log("init");
        }

        public void MessageReceiver(string message)
        {
            var text = _rightPanel.GetComponent<Text>();
            text.text = message + "is killed.";
        }
    }
}
