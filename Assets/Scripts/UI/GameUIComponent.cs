using Adic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.UI;
using RaceGame.Scripts.Interfaces.Services;

namespace RaceGame.Scripts.UI
{
    public class GameUIComponent : MonoBehaviour
    {
        [Header("Default UI")]
        [SerializeField] private CanvasGroup defaultUI;
        
        [Header("Game UI")]
        [SerializeField] private CanvasGroup gameUI;
        [SerializeField] private Button closeButton;

        [Inject] 
        private IEntityGeneratorService entityGeneratorService;
        
        private void Start()
        {
            this.Inject();
        }
        
        [Inject]
        private void PostConstruct()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            entityGeneratorService.UserVehicleRX
                .Where(vehicle => vehicle != null)
                .Subscribe(vehicle => ToggleViews(false))
                .AddTo(this);

            closeButton.OnClickAsObservable()
                .Subscribe(x => ToggleViews(true))
                .AddTo(this);
            
            closeButton.OnClickAsObservable()
                .Delay(TimeSpan.FromMilliseconds(200))
                .Subscribe(x => entityGeneratorService.DestroyUserVehicle())
                .AddTo(this);
        }

        private void ToggleViews(bool showDefault)
        {
            defaultUI.alpha = showDefault ? 1f : 0f;
            defaultUI.interactable = showDefault;

            gameUI.alpha = showDefault ? 0f : 1f;
            gameUI.interactable = !showDefault;
        }
    }
}
