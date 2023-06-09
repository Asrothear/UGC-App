﻿using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FssDiscoveryScan : SchemaFilter
{
    public FssDiscoveryScan(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/fssdiscoveryscan/1";
        Merge(inp);
        GetCoords(inp);
        var items = Data["message"] as JObject;
        if (items != null)
        {
            items.Remove("Progress");
        }
    }
}