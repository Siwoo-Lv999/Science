using System.IO;
using _LumenLib.PoolingSystem.Runtime;
using _LumenLib.SoundSystem.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chehy.Editor
{
    public static class LumenAudioSetup
    {
        private const string ConfigFolder = "Assets/Members/Chehy/Sound/Config";
        private const string PrefabFolder = "Assets/Members/Chehy/Sound/Prefabs";
        private const string SoundDataFolder = "Assets/Members/Chehy/Sound/Sound Data";
        private const string ScenePath = "Assets/Members/Chehy/ChehyScene.unity";

        private const string PoolItemPath = ConfigFolder + "/SoundPlayerPoolItem.asset";
        private const string PoolListPath = ConfigFolder + "/AudioPoolingList.asset";
        private const string SoundPlayerPrefabPath = PrefabFolder + "/SoundPlayer.prefab";
        private const string AudioSystemPrefabPath = PrefabFolder + "/LumenAudioSystem.prefab";

        private static readonly string[] ClipPaths =
        {
            "Assets/Members/Chehy/Sound/Sound Clips/PlayerJump.mp3",
            "Assets/Members/Chehy/Sound/Sound Clips/PlayerWalk.mp3",
            "Assets/Members/Chehy/Sound/Sound Clips/PowerOnOff.mp3",
            "Assets/Members/Chehy/Sound/Sound Clips/UIButton.mp3"
        };

        [InitializeOnLoadMethod]
        private static void ScheduleInitialSetup()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(AudioSystemPrefabPath) != null)
                return;

            EditorApplication.delayCall += SetupAudioSystem;
        }

        [MenuItem("Tools/LumenLib/Setup Audio System")]
        public static void SetupAudioSystem()
        {
            EnsureFolder(ConfigFolder);
            EnsureFolder(PrefabFolder);
            EnsureFolder(SoundDataFolder);

            PoolItemSO poolItem = LoadOrCreateAsset<PoolItemSO>(PoolItemPath);
            GameObject soundPlayerPrefab = CreateSoundPlayerPrefab(poolItem);
            ConfigurePoolItem(poolItem, soundPlayerPrefab);

            PoolingListSO poolList = LoadOrCreateAsset<PoolingListSO>(PoolListPath);
            ConfigurePoolList(poolList, poolItem);

            GameObject audioSystemPrefab = CreateAudioSystemPrefab(poolList, poolItem);
            CreateSoundDataAssets();
            AddAudioSystemToScene(audioSystemPrefab);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[LumenAudioSetup] Audio system setup completed successfully.");
        }

        private static GameObject CreateSoundPlayerPrefab(PoolItemSO poolItem)
        {
            GameObject root = new GameObject("SoundPlayer");
            AudioSource source = root.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f;

            SoundPlayer player = root.AddComponent<SoundPlayer>();
            SerializedObject serializedPlayer = new SerializedObject(player);
            serializedPlayer.FindProperty("poolItem").objectReferenceValue = poolItem;
            serializedPlayer.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, SoundPlayerPrefabPath);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static void ConfigurePoolItem(PoolItemSO poolItem, GameObject prefab)
        {
            SerializedObject serializedItem = new SerializedObject(poolItem);
            serializedItem.FindProperty("<ItemName>k__BackingField").stringValue = "LumenSoundPlayer";
            serializedItem.FindProperty("<Prefab>k__BackingField").objectReferenceValue = prefab;
            serializedItem.FindProperty("<PoolSize>k__BackingField").intValue = 8;
            serializedItem.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(poolItem);
        }

        private static void ConfigurePoolList(PoolingListSO poolList, PoolItemSO poolItem)
        {
            SerializedObject serializedList = new SerializedObject(poolList);
            SerializedProperty items = serializedList.FindProperty("<PoolList>k__BackingField");
            items.arraySize = 1;
            items.GetArrayElementAtIndex(0).objectReferenceValue = poolItem;
            serializedList.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(poolList);
        }

        private static GameObject CreateAudioSystemPrefab(PoolingListSO poolList, PoolItemSO poolItem)
        {
            GameObject root = new GameObject("LumenAudioSystem");

            ObjectPool objectPool = root.AddComponent<ObjectPool>();
            SerializedObject serializedPool = new SerializedObject(objectPool);
            serializedPool.FindProperty("poolList").objectReferenceValue = poolList;
            serializedPool.ApplyModifiedPropertiesWithoutUndo();

            AudioService audioService = root.AddComponent<AudioService>();
            SerializedObject serializedService = new SerializedObject(audioService);
            serializedService.FindProperty("soundPlayerPoolItem").objectReferenceValue = poolItem;
            serializedService.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, AudioSystemPrefabPath);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static void CreateSoundDataAssets()
        {
            foreach (string clipPath in ClipPaths)
            {
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);
                if (clip == null)
                {
                    Debug.LogWarning($"[LumenAudioSetup] Audio clip was not found: {clipPath}");
                    continue;
                }

                string assetPath = $"{SoundDataFolder}/{Path.GetFileNameWithoutExtension(clipPath)}.asset";
                SoundClipSO soundData = LoadOrCreateAsset<SoundClipSO>(assetPath);
                SerializedObject serializedData = new SerializedObject(soundData);
                serializedData.FindProperty("audioType").enumValueIndex =
                    (int)_LumenLib.SoundSystem.Runtime.AudioType.Sfx;
                serializedData.FindProperty("clip").objectReferenceValue = clip;
                serializedData.FindProperty("isLoop").boolValue = false;
                serializedData.FindProperty("randomizePitch").boolValue = false;
                serializedData.FindProperty("randomPitchModifier").floatValue = 0.05f;
                serializedData.FindProperty("volume").floatValue = 1f;
                serializedData.FindProperty("pitch").floatValue = 1f;
                serializedData.FindProperty("startTime").floatValue = 0f;
                serializedData.FindProperty("endTime").floatValue = clip.length;
                serializedData.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(soundData);
            }
        }

        private static void AddAudioSystemToScene(GameObject audioSystemPrefab)
        {
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            GameObject existingAudioSystem = FindRoot(scene, "LumenAudioSystem");

            if (existingAudioSystem == null)
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(audioSystemPrefab, scene);
                instance.name = "LumenAudioSystem";
            }

            GameObject legacySoundManager = FindRoot(scene, "SoundManager");
            if (legacySoundManager != null)
            {
                legacySoundManager.SetActive(false);
                legacySoundManager.name = "SoundManager_Legacy_Disabled";
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        private static GameObject FindRoot(Scene scene, string name)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (root.name == name)
                    return root;
            }

            return null;
        }

        private static T LoadOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
                return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void EnsureFolder(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);

                current = next;
            }
        }
    }
}
