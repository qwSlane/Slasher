using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Enemy {

  public class LootRestorer : MonoBehaviour, ISavedProgressReader {
    private IGameFactory _factory;

    public void Awake() {
      _factory = AllServices.Container.Single<IGameFactory>();
    }

    public void LoadProgress(PlayerProgress progress) {
      foreach (UnpickedLoot unpickedLoot in progress.WorldData.LootData.unPicked) {
        SpawnLoot(unpickedLoot);
      }
      progress.WorldData.LootData.unPicked.Clear();
    }

    private void SpawnLoot(UnpickedLoot unpickedLoot) {
      LootPiece lootPiece = _factory.CreateLoot();
      lootPiece.transform.position = unpickedLoot.Position.AsUnityVector();
      lootPiece.Initialize(unpickedLoot.Loot);
    }
  }

}