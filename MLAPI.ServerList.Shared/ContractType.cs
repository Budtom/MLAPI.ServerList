﻿namespace MLAPI.ServerList.Shared
{
    public enum ContractType
    {
        Int8,
        Int16,
        Int32,
        Int64,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        String,
        Buffer,
        Guid,
        Location
    }

    public class Geolocation
    {
        public string type { get; set; }

        public float[] coordinates { get; set; }
    }
}
