﻿using System;
using System.Threading;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Hera
{
    public partial class Fpsware : CombatRoutine
    {

        // ************************************************************************************
        public string Environment;                                          // Used for dynamic environment loading
        private bool _isCCLoaded;                                           // A variable I use to check if the CC is loaded
        private static LocalPlayer Me { get { return ObjectManager.Me; } }  // A global property of ME
        private static WoWUnit CT { get { return Me.CurrentTarget; } }      // A global property of my current target
        public override string Name { get { return String.Format("{0}({1}) by {2}", CCName, _versionNumber, AuthorName); } }
        public override WoWClass Class { get { return CCClass; } }
        // ************************************************************************************

        #region CC Constructor
        public Fpsware()
        {
            // If you are not this class don't continue past this point. 
            if (Me.Class != CCClass) return;

            // Register the events of the Stop / Start buttons in HB
            BotEvents.OnBotStarted += BotEvents_OnBotStarted;
            BotEvents.OnBotStopped += BotEvents_OnBotStopped;

            LoadSettings(true);
        }
        #endregion

        #region Configuration Form Stuff
        // All very straight forward here. The name of the UI form is called UIForm
        private static Thread Thread { get; set; }
        private UIForm2 _gui;

        // This overrides HB OnButtonPress event. This where we assign the form to a thread and start the thread
        public override void OnButtonPress()
        {
            Thread = new Thread(OpenGui);
            Thread.IsBackground = true;
            Thread.Name = "GuiThread";
            Thread.Start();
        }
        public void OpenGui()
        {
            _gui = new UIForm2(); _gui.ShowDialog();
        }

        public override bool WantButton
        {
            get { return Settings.ShowUI.Contains("always"); }
        }
        #endregion

        #region Load Settings
        private void LoadSettings(bool silent)
        {
            if (!silent) Utils.Log("Loading settings ...");

            Settings.Load();

            if (!silent) Utils.Log(!_isCCLoaded ? "Settings loaded" : "Settings have been updated with your changes", Utils.Colour("DarkRed"));
            Settings.DirtyData = false;

        }
        #endregion
    }
}
