using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Leon.CreateFolders
{
    public class CreateFolders : EditorWindow
    {
        private static string _projectName = "PROJECT_NAME";

        [MenuItem("Assets/Create Leon's Default Folders")]
        private static void SetUpFolders()
        {
            CreateFolders window = GetWindow<CreateFolders>();
            window.titleContent = new GUIContent("Leon's Folder Structure");
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
            window.ShowPopup();
        }

        private static void CreateAllFolders()
        {
            List<string> mainFolders = new List<string>()
            {
                "Animations",
                "Audio",
                "Art",
                "_Level",
                "Code",
                "Docs",
                "_ImportedAssets",
                "ScriptableObjects",
                "Src"
            };

            List<string> levelFolder = new List<string>()
            {
                "_Scenes",
                "Prefabs",
                "UI",
                "Lightning"
            };

            List<string> codeFolder = new List<string>()
            {
                "Scripts",
                "Shaders"
            };

            List<string> scriptableObjectsFolder = new List<string>()
            {
                "Scripts",
                "Data"
            };

            List<string> artFolder = new List<string>()
            {
                "Materials",
                "Models",
                "Textures",
                "Sprites"
            };

            List<string> spritesFolder = new List<string>()
            {
                "UI",
                "Icons"
            };

            List<string> srcFolder = new List<string>()
            {
                "Framework",
                "Shaders"
            };
            
            List<string> audioFolder = new List<string>()
            {
                "Music",
                "Sound"
            };

            foreach (var folder in mainFolders)
            {
                if (!Directory.Exists($"Assets/{_projectName}/{folder}"))
                    Directory.CreateDirectory($"Assets/{_projectName}/{folder}");
            }

            CreateSecondLevelFolder(levelFolder, "_Level");
            CreateSecondLevelFolder(codeFolder, "Code");
            CreateSecondLevelFolder(scriptableObjectsFolder, "ScriptableObjects");
            CreateSecondLevelFolder(artFolder, "Art");
            CreateSecondLevelFolder(srcFolder, "Src");
            CreateSecondLevelFolder(audioFolder, "Audio");
            CreateThirdLevelFolder(spritesFolder, "Art", "Sprites");
            
            AssetDatabase.Refresh();
        }

        private static void CreateSecondLevelFolder(List<string> subfolders, string parentFolder)
        {
            subfolders.ForEach(subfolder =>
            {
                if (!Directory.Exists($"Assets/{_projectName}/{parentFolder}/{subfolder}"))
                    Directory.CreateDirectory($"Assets/{_projectName}/{parentFolder}/{subfolder}");
            });
        }

        private static void CreateThirdLevelFolder(List<string> thirdLevelFolders,
            string parentFolder,
            string secondLevelFolder)
        {
            thirdLevelFolders.ForEach(thirdLevelFolder =>
            {
                if (!Directory.Exists($"Assets/{_projectName}/{parentFolder}/{secondLevelFolder}/{thirdLevelFolder}"))
                    Directory.CreateDirectory(
                        $"Assets/{_projectName}/{parentFolder}/{secondLevelFolder}/{thirdLevelFolder}");
            });
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Type the project name that is going to be used as the root folder");
            _projectName = EditorGUILayout.TextField("Project Name:", _projectName);
            Repaint();
            GUILayout.Space(70);
            if (GUILayout.Button("Generate Leon's Folder Structure"))
            {
                CreateAllFolders();
                Close();
            }
        }
    }
}