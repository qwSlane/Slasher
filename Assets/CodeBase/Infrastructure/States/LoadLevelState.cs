using System;
using System.Linq;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States {

  public class LoadLevelState : IPayloadedState<string> {
    private const string InitialPointTag = "InitialPoint";
    private const string EnemySpawnerTag = "EnemySpawner";
    private const string RestorerTag = "Restorer";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private IStaticDataService _staticData;


    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
      IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData) {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
    }

    public void Enter(string sceneName) {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

    private void OnLoaded() {
      InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders() {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders.ToList())
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld() {
      InitSpawners();
      RestoreLoot();

      GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(InitialPointTag));
      InitHud(hero);

      CameraFollow(hero);
    }

    private void RestoreLoot() {
      foreach (UnpickedLoot loot in _progressService.Progress.WorldData.LootData.unPicked) {
        LootPiece piece = _gameFactory.CreateLoot();
        piece.Construct(loot.LootId);
      }
    }

    private void InitSpawners() {
      string sceneKey = SceneManager.GetActiveScene().name;
      LevelStaticData levelData = _staticData.ForLevel(sceneKey);
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners) {
        _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
      }
    }

    private void InitHud(GameObject hero) {
      GameObject hud = _gameFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);
  }

}