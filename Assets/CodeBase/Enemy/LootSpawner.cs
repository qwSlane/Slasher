using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Enemy {

  public class LootSpawner : MonoBehaviour {

    public EnemyDeath EnemyDeath;
    private IGameFactory _factory;
    private int _lootMin;
    private int _lootMax;
    private IRandomService _randomService;
    

    public void Construct(IGameFactory factory, IRandomService randomservice) {
      _factory = factory;
      _randomService = randomservice;
    }

    private void Start() {
      EnemyDeath.Happened += SpawnLoot;
    }

    private void SpawnLoot() {
      LootPiece _loot = _factory.CreateLoot();
      _loot.transform.position = transform.position;

      Loot lootItem = GenerateLoot();
      _loot.Initialize(lootItem);
    }

    private Loot GenerateLoot() {
      return new Loot() {
        Value = _randomService.Next(_lootMin, _lootMax)
      };
    }

    public void SetLoot(int min, int max) {
      _lootMin = min;
      _lootMax = max;
    }
    
  }

}