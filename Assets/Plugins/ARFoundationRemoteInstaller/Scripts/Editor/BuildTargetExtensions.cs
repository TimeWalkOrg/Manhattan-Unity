using System;
using UnityEditor;


namespace ARFoundationRemote.Editor {
    public static class BuildTargetExtensions {
        public static BuildTargetGroup ToBuildTargetGroup(this BuildTarget buildTarget) {
            switch (buildTarget) {
                case BuildTarget.iOS:
                    return BuildTargetGroup.iOS;
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneLinux64:
                    return BuildTargetGroup.Standalone;
                case BuildTarget.Android:
                    return BuildTargetGroup.Android;
                case BuildTarget.WebGL:
                    return BuildTargetGroup.WebGL;
                case BuildTarget.WSAPlayer:
                    return BuildTargetGroup.WSA;
                case BuildTarget.PS4:
                    return BuildTargetGroup.PS4;
                case BuildTarget.XboxOne:
                    return BuildTargetGroup.XboxOne;
                case BuildTarget.tvOS:
                    return BuildTargetGroup.tvOS;
                case BuildTarget.Switch:
                    return BuildTargetGroup.Switch;
                case BuildTarget.Lumin:
                    return BuildTargetGroup.Lumin;
                #if UNITY_2019_3_OR_NEWER
                case BuildTarget.Stadia:
                    return BuildTargetGroup.Stadia;
                #endif
                default:
                    throw new Exception("Unknown BuildTarget: " + buildTarget);
            }
        }
    }
}
