using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string MonstersDataPath = "Static Data/Monsters";
    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;

    public void LoadMonsters()
    {
      _monsters = Resources
        .LoadAll<MonsterStaticData>(MonstersDataPath)
        .ToDictionary(x => x.MonsterTypeId, x => x);
      
      
      _levels = Resources
        .LoadAll<LevelStaticData>("Static Data/Levels")
        .ToDictionary(x => x.LevelKey, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
      _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
        ? staticData 
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData 
        : null;
  }
}