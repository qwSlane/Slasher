using System;

namespace CodeBase.Data {

  [Serializable]
  public class UnpickedLoot {
    public Vector3Data Position;
    public string LootId;
    public Loot Loot;

    public UnpickedLoot(Vector3Data position, Loot loot, string id) {
      Position = position;
      Loot = loot;
      LootId = id;
    }

  }

}