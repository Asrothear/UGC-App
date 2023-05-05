﻿using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FSSBodySignals : SchemaFilter
{
    public FSSBodySignals(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/fssbodysignals/1";
        Merge(inp);
        GetSystemMeta(inp);
    }
}