using System.Diagnostics;
using DiscordGameSDKWrapper;

namespace UGC_App.RichPresence;
public class DiscordHandler
{
    private static Discord? _discord;
    internal static ActivityManager? ActivityManager;

    internal static void Start()
    {
        if(_discord != null)return;
        _discord = new Discord(900125197431091240, (ulong)CreateFlags.Default);
        UserManager userManager = _discord.GetUserManager();
        //userManager.OnCurrentUserUpdate += OnCurrentUserUpdate;
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    _discord.RunCallbacks();
                    Thread.Sleep(16);
                }catch{}
            }
        });
        ActivityManager = _discord.GetActivityManager();
    }

    internal static void StopRichPresence()
    {
        _discord?.Dispose();
        _discord = null;
    }
    
    private static void OnCurrentUserUpdate()
    {
        User currentUser = _discord.GetUserManager().GetCurrentUser();
        Debug.WriteLine($"Benutzer-ID: {currentUser.Id}");
        Debug.WriteLine($"Benutzername: {currentUser.Username}#{currentUser.Discriminator}");
    }

    internal class SetActivity
    {
        internal static void Location()
        {
            if(_discord == null)return;
            var _activity = new DiscordGameSDKWrapper.Activity()
            {
                Details = $"Fliegt durch {Config.Instance.LastSystem}"
            };
            ActivityManager?.UpdateActivity(_activity,null);
        }
        internal static void Supercruise()
        {
            if(_discord == null)return;
            var _activity = new DiscordGameSDKWrapper.Activity()
            {
                Details = $"Fliegt durch {Config.Instance.LastSystem} im Supercruise"
            };
            ActivityManager?.UpdateActivity(_activity,null);
        }

        internal static void Docked()
        {
            if(_discord == null)return;
            var _activity = new DiscordGameSDKWrapper.Activity()
            {
                Details = $"Angedockt auf {Config.Instance.LastDocked}",
                State = $"In {Config.Instance.LastSystem}"
                
            };
            ActivityManager?.UpdateActivity(_activity,null);
        }
        
        internal static void UnDocked()
        {
            if(_discord == null)return;
            var _activity = new DiscordGameSDKWrapper.Activity()
            {
                Details = $"Ist von {Config.Instance.LastDocked} gestartet.",
                State = $"In {Config.Instance.LastSystem}"
                
            };
            ActivityManager?.UpdateActivity(_activity,null);
        }

        public static void SupercruiseExit(string? toString)
        {
            if(_discord == null)return;
            var _activity = new DiscordGameSDKWrapper.Activity()
            {
                Details = $"Ist bei {toString} aus dem Supercruis getreten.",
                State = $"In {Config.Instance.LastSystem}"
                
            };
            ActivityManager?.UpdateActivity(_activity,null);
        }
    }
}