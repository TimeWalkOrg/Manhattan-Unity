#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;


namespace ARFoundationRemote.Editor {
    [CustomEditor(typeof(ARFoundationRemoteInstaller), true), CanEditMultipleObjects]
    public class ARFoundationRemoteInstallerInspector : UnityEditor.Editor {
        bool isARFoundationEmbedded;
        static bool? isPluginAddedToIgnore;

        
        void OnEnable() {
            isARFoundationEmbedded = ARFoundationRemoteInstaller.getArf()?.source == PackageSource.Embedded;
            #if AR_FOUNDATION_REMOTE_INSTALLED
            if (isARFoundationEmbedded) {
                FixesForEditorSupport.Apply();
            }
            
            if (InstallerSettings.AddGitIgnore) {
                GitRepoCheck.AddGitIgnore();
                InstallerSettings.AddGitIgnore = false;
            }
            
            isPluginAddedToIgnore = GitRepoCheck.IsPluginAddedToIgnore();
            #endif
        }

        public override void OnInspectorGUI() {
            if (EditorApplication.isCompiling) {
                return;
            }
            
            DrawDefaultInspector();
            showMethodsInInspector(targets);
        }

        void showMethodsInInspector(params Object[] inspectorTargets) {
            GUILayout.Space(8);
            var instance = inspectorTargets.First() as ARFoundationRemoteInstaller;
            Assert.IsNotNull(instance);

            if (!ARFoundationRemoteInstaller.isInstalled) {
                if (GUILayout.Button("Install Plugin")) {
                    ARFoundationRemoteInstaller.installPlugin_internal();
                }
            } else {
                if (isARFoundationEmbedded) {
                    GUILayout.Space(16);
                    GUILayout.Label("AR Companion app", EditorStyles.boldLabel);
                    var boldButtonStyle = new GUIStyle(GUI.skin.button) {fontStyle = FontStyle.Bold};
                    if (GUILayout.Button("Install AR Companion App", boldButtonStyle)) {
                        execute(CompanionAppInstaller.BuildAndRun);
                    }

                    if (GUILayout.Button("Build AR Companion and show in folder", new GUIStyle(GUI.skin.button))) {
                        execute(CompanionAppInstaller.Build);
                    }

                    if (GUILayout.Button("Delete AR Companion app build folder")) {
                        execute(CompanionAppInstaller.DeleteCompanionAppBuildFolder);
                    }

                    GUILayout.Space(8);
                    if (GUILayout.Button("Open Settings", boldButtonStyle)) {
                        #if AR_FOUNDATION_REMOTE_INSTALLED
                        SettingsService.OpenProjectSettings($"Project/XR Plug-in Management/{Runtime.Constants.packageName}");
                        #endif
                    }

                    GUILayout.Space(16);
                    /*if (GUILayout.Button("Apply AR Foundation fixes")) {
                        #if AR_FOUNDATION_REMOTE_INSTALLED
                            FixesForEditorSupport.Apply();
                        #endif
                    }*/

                    /*if (GUILayout.Button("Undo AR Foundation fixes")) {
                        #if AR_FOUNDATION_REMOTE_INSTALLED
                        FixesForEditorSupport.Undo();
                        #endif
                    }*/
                } else {
                    if (GUILayout.Button("Embed AR Foundation Package")) {
                        ARFoundationRemoteInstaller.EmbedARFoundation();
                    }        
                }
            
                GUILayout.Space(16);
                GUILayout.Label("Installation", EditorStyles.boldLabel);
                if (!isARFoundationEmbedded && GUILayout.Button("Un-install Plugin", new GUIStyle(GUI.skin.button) {normal = {textColor = Color.red}})) {
                    instance.UnInstallPlugin();
                }
                
                #if AR_FOUNDATION_REMOTE_INSTALLED
                if (isPluginAddedToIgnore == false && GUILayout.Button("Ignore plugin in git repository")) {
                    isPluginAddedToIgnore = true;
                    GitRepoCheck.AddGitIgnore();
                }
                #endif
            }
            
            if (isARFoundationEmbedded && GUILayout.Button("Un-embed AR Foundation Package")) {
                ARFoundationRemoteInstaller.UnEmbedARFoundation();
            }

            #if AR_FOUNDATION_REMOTE_INSTALLED
            if (isPluginAddedToIgnore == true) {
                GUILayout.Space(48);
                GUILayout.Label("How to add the plugin to git repository?", EditorStyles.boldLabel);
                GUILayout.Label("Before adding the plugin to your git repository, please check that:\n" +
                                "- You've purchased a plugin license for every team member who will use the plugin.\n" +
                                "- Your git repository is private.", 
                    new GUIStyle(GUI.skin.label) {wordWrap = true, richText = true, margin = new RectOffset(4, 4, 4, 4)});
                if (GUILayout.Button("Add plugin to git")) {
                    if (GitRepoCheck.RemovePluginFromGitIgnore()) {
                        isPluginAddedToIgnore = false;
                    }
                }
            }

            ARFoundationRemoteLoaderSettingsInspector.TryDrawVersionUpgrade();
            #endif
            
            /*GUILayout.Space(100);
            if (GUILayout.Button("DEBUG")) {
            }*/
        }

        static void execute(Action action) {
            action();
        }
    }
}
#endif // UNITY_EDITOR
