using UnityEngine;

namespace CodeBase.Services.Randomizer {

  public class UnityRandomService : IRandomService {

    public int Next(int minValue, int maxValue) => 
      Random.Range(minValue, maxValue);
  }

}