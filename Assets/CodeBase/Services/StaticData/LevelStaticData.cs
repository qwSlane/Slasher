using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Services.StaticData {

  [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
  public class LevelStaticData: ScriptableObject {

    public string LevelKey;
    public List<EnemySpawnerData> EnemySpawners;
  }

}