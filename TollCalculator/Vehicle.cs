﻿namespace TollCalculator
{
    using System;

    public interface IVehicle
    {
        string GetVehicleType();
    }

    public enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}