using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    GameObject CreateHero(GameObject at);
    GameObject CreateHud();
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    void Cleanup();
    void Register(ISavedProgressReader savedProgress);
    LootPiece CreateLoot();
    void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
  }
}