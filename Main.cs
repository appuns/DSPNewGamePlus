using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.Security;
using System.Security.Permissions;


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace DSPNewGamePlus
{

    [BepInPlugin("Appun.DSP.plugin.NewGamePlus", "DSPNewGamePlus", "1.1.2")]
    [BepInProcess("DSPGAME.exe")]

    public class Main : BaseUnityPlugin
    {
        public static ConfigEntry<KeyCode> openKey;
        //public static ConfigEntry<KeyCode> KeyConfig2;
        public static string PluginPath = System.IO.Path.GetDirectoryName(
                   System.Reflection.Assembly.GetExecutingAssembly().Location);

        public void Start()
        {
            LogManager.Logger = Logger;

            UI.Create();

        }


        //
        public static void Save()　　//SaveCurrentGame(string saveName)
        {
            LogManager.Logger.LogInfo("save");
            string path = Path.Combine(PluginPath, "DSPNewGamePlus.save");
            FileStream fileStream = new FileStream(path, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            //インベントリ
            PlayerData.Export(binaryWriter);   
            //メカ
            MechaData.Export(binaryWriter);
            //ヒストリ
            HistoryData.Export(binaryWriter);

            binaryWriter.Close(); 
            fileStream.Close();
            UI.openedMessageBox = false;

        }

        public static void Load()
        {
            //読み込み
            var Data = GameMain.data;
            LogManager.Logger.LogInfo("load");
            string path = Path.Combine(PluginPath, "DSPNewGamePlus.save");
            if (!File.Exists(path))
            {
                LogManager.Logger.LogInfo("DSPNewGamePlus.save not found " + path);
                UI.openedMessageBox = false;

                return;
            }
            FileStream fileStream = new FileStream(path, FileMode.Open);

            BinaryReader BinaryReader = new BinaryReader(fileStream);
            Data.history.techStates.Clear();

            //インベントリ
            PlayerData.Import(BinaryReader);
            //メカ
            MechaData.Import(BinaryReader);
            //ヒストリ
            HistoryData.Import(BinaryReader);


            BinaryReader.Close();
            fileStream.Close();

            UI.openedMessageBox = false;
        }

        public static void Update()
        {
            if (UI.openedMessageBox)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UI.openedMessageBox = false;
                    UI.messageBox.FadeOut();
                }
            }
        }


        public static void Cancel()
        {
            UI.openedMessageBox = false;
        }


    }


    public class LogManager
    {
        public static ManualLogSource Logger;
    }
}

