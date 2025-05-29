using ICities;
using System.Reflection;

namespace SleepyCommon
{
    public abstract class UserModBase : LoadingExtensionBase, IUserMod
    {
        public static UserModBase BaseInstance { get; set; }

        public bool IsEnabled = false;
        public bool IsLoaded = false;
        // ----------------------------------------------------------------------------------------
        public UserModBase()
        {
            BaseInstance = this;
        }

        public abstract string ModName { get; }

        public abstract string Version { get; }

        public abstract string Description { get; }

        public abstract void OnLevelLoaded();

        // ----------------------------------------------------------------------------------------
        public string Name
        {
            get 
            {
                return $"{ModName} {Version}";
            }
        }

        public string SleepyCommonVersion 
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }


        public void OnEnabled()
        {
            IsEnabled = true;
        }

        public void OnDisabled()
        {
            IsEnabled = false;
        }

        // called when level is loaded
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (ActiveInMode(mode))
            {
                IsLoaded = true;

                OnLevelLoaded();
            }
        }

        // called when unloading begins
        public override void OnLevelUnloading()
        {
            CDebug.Log("OnLevelUnloading");
            IsLoaded = false;
            base.OnLevelUnloading();
        }

        private static bool ActiveInMode(LoadMode mode)
        {
            switch (mode)
            {
                case LoadMode.NewGame:
                case LoadMode.NewGameFromScenario:
                case LoadMode.LoadGame:
                    return true;

                default:
                    return false;
            }
        }
    }
}