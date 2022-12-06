using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI {

  public class LootCounter : MonoBehaviour, ISavedProgressReader {
    
    public TextMeshProUGUI Counter;
    private WorldData _worldData;

    public void Construct(WorldData worldData) {
      _worldData = worldData;
      _worldData.LootData.Changed += UpdateCounter;
    }

    private void Start() {
      UpdateCounter();
    }

    private void UpdateCounter() {
      Counter.text = $"{_worldData.LootData.Collected}";
    }

    public void LoadProgress(PlayerProgress progress) {
      Counter.text = $"{progress.WorldData.LootData.Collected}";
    }
  }

}