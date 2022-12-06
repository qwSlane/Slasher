using System;
using System.Collections.Generic;
using CodeBase.Logic;

namespace CodeBase.Data {

  [Serializable]
  public class LootData {

    public int Collected;

    public List<UnpickedLoot> unPicked = new List<UnpickedLoot>();
    
    public Action Changed;

    public void Collect(Loot loot) {
      Collected += loot.Value;
      Changed?.Invoke();
    }
  }

}