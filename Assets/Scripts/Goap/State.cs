
using System;

namespace GOAP {

    /// <summary>
    /// Flag Enum containing possible states for a IGoapCharacter
    /// </summary>
    [Flags]
    public enum State {

        IsHungry = 1,
        HasFoodTarget = 2,
        HasFood = 4,
        HasEaten = 8,
        IsThirsty = 16,
        HasWaterTarget = 32,
        IsAtWater = 64,
        HasDrunk = 128,
        WantsToMate = 256,
        HasMateTarget = 512,
        IsAtMate = 1024,
        HasMated = 2048,
        IsTired = 4096,
        IsSafe = 8192,
        IsAlert = 16384,
        IsInDanger = 32768,
        HasRested = 65536,
        Explored = 131072,

        EndOfFlags = 262144 

    }
}