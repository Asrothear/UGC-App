namespace UGC_App.Order.Model;

public class SystemHistoryData
{

    public string starSystem { get; set; }
    public ulong systemAddress { get; set; }
    public string starPos { get; set; }
    public ulong population { get; set; }
    public string systemGovernment { get; set; }
    public string systemAllegiance { get; set; }
    public string systemEconomy { get; set; }
    public string systemSecondEconomy { get; set; }
    public string systemSecurity { get; set; }
    public string lastBGSData { get; set; }
    public HashSet<HisoryModel> systemHistory { get; set; } = new();
    public class HisoryModel
    {
        public string timestamp { get; set; } = "";
        public HashSet<FactionsO> factions { get; set; } = new();
        public SystemFactionO systemFaction { get; set; } = new();
        public HashSet<ConflictsO>? conflicts { get; set; } = new();

        public class FactionsO
        {
            public string name { get; set; } = "";
            public string factionState { get; set; } = "";
            public string government { get; set; } = "";
            public double influence { get; set; }
            public string allegiance { get; set; } = "";
            public string happiness { get; set; } = "";
            public string happiness_Localised { get; set; } = "";
            public HashSet<ActiveStatesO>? activeStates { get; set; } = new();
            public HashSet<PendingStatesO>? pendingStates { get; set; } = new();

            public class ActiveStatesO
            {
                public string name { get; set; } = "";
            }

            public class PendingStatesO
            {
                public string name { get; set; } = "";
                public string trend { get; set; } = "";
            }
        }

        public class SystemFactionO
        {
            public string name { get; set; }
        }

        public class ConflictsO
        {
            public string warType { get; set; }
            public string status { get; set; }
            public ConflictFaction faction1 { get; set; }
            public ConflictFaction faction2 { get; set; }

            public class ConflictFaction
            {
                public string name { get; set; }
                public string stake { get; set; }
                public int wonDays { get; set; }
            }
        }

    }
}

public class SystemListing
{
    public string StarSystem { get; set; } = "";
    public ulong SystemAddress { get; set; }
    public DateTime LastBgsData { get; set; }
    public int Count { get; set; }
        
}
