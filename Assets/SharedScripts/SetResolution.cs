using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArkhamShared
{
    public static class SetResolution
    {
        public static void SettingResolution()
        {
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 60;
            if (Array.Exists(Screen.resolutions, r => r.width >= 1920)) Screen.SetResolution(1920, 1080, true, 60);
            else Screen.SetResolution(1280, 720, true, 60);
        }
    }
}
