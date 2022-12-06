using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy {

  public class LootPiece : MonoBehaviour, ISavedProgress {

    public GameObject Skull;
    public GameObject PickupFxPrefab;
    public TextMeshPro LootText;
    public GameObject PickupPopup;

    private Loot _loot;
    private bool _picked;
    private string _id;

    private int _listPosition;

    private WorldData _worldData;

    public void Construct(WorldData worldData) {
      _id = GetComponent<UniqueId>().Id;
      _worldData = worldData;
      _listPosition = -1;
    }

    public void Construct(string id) {
      _id = id;
    }

    public void Initialize(Loot loot) {
      _loot = loot;
    }

    private void OnTriggerEnter(Collider other) => PickUp();

    private void PickUp() {
      if (_picked)
        return;
      _picked = true;
      UpdateWorldData();
      HideSkull();
      ShowText();
      PlayPickupFx();
      StartCoroutine(StartDestroyTimer());
    }

    private void UpdateWorldData() {
      _worldData.LootData.Collect(_loot);
      if (_listPosition != -1) {
        _worldData.LootData.unPicked.RemoveAt(_listPosition);
      }
    }

    private void HideSkull() =>
      Skull.SetActive(false);

    private IEnumerator StartDestroyTimer() {
      yield return new WaitForSeconds(1.5f);

      Destroy(gameObject);
    }

    private void PlayPickupFx() =>
      Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

    private void ShowText() {
      LootText.text = $"{_loot.Value}";
      PickupPopup.SetActive(true);
    }

    public void LoadProgress(PlayerProgress progress) {
      List<UnpickedLoot> loots = progress.WorldData.LootData.unPicked;
      for (int i = 0; i < loots.Count; i++) {
        if (loots[i].LootId == _id) {
          Debug.Log("Initialized");
          Initialize(loots[i].Loot);
          transform.position = loots[i].Position.AsUnityVector();
          _listPosition = i;
        }
      }

      _worldData = progress.WorldData;
    }

    public void UpdateProgress(PlayerProgress progress) {
      if (!_picked) {
        Vector3 position = transform.position;
        progress.WorldData.LootData.unPicked.Add(new UnpickedLoot(new Vector3Data(position.x, position.y, position.z),
          _loot, _id));
      }
    }
  }

}