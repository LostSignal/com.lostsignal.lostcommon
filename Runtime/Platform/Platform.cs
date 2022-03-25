//-----------------------------------------------------------------------
// <copyright file="Platform.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Networking;

    //// NOTE [bgish]:  Windows Universal May Support System.IO.File class now
    //// TODO add events for pen and mouse detected, that way if someone uses a pen
    //// TODO controller too?  maybe only if InControl is detected?

    public static class Platform
    {
        // TODO [bgish] - make sure <uses-permission android:name="android.permission.VIBRATE"/> is in the AndroidManifest.xml file
        #if UNITY_ANDROID && !UNITY_EDITOR
        private static readonly AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static readonly AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        private static readonly AndroidJavaObject Vibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        #endif

        public delegate void OnResetDelegate();

        public static event OnResetDelegate OnReset;

        public enum UnityEditorPlatform
        {
            Windows,
            Mac,
            Linux,
            Unknown,
        }

        public static bool IsApplicationQuitting { get; private set; }

        public static bool IsPlayingOrEnteringPlaymode
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                #if UNITY_EDITOR
                return UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
                #else
                return true;
                #endif
            }
        }

        // TODO [bgish]: Try to make this function work by purely using Application.platform
        public static DevicePlatform CurrentDevicePlatform
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return DevicePlatform.iOS;

                    case RuntimePlatform.Android:
                        return DevicePlatform.Android;

                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsEditor:
                        return DevicePlatform.Windows;

                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        return DevicePlatform.Mac;

                    case RuntimePlatform.LinuxPlayer:
                    case RuntimePlatform.LinuxEditor:
                        return DevicePlatform.Linux;

                    case RuntimePlatform.WSAPlayerX86:
                    case RuntimePlatform.WSAPlayerX64:
                    case RuntimePlatform.WSAPlayerARM:
                        return DevicePlatform.WindowsUniversal;

                    case RuntimePlatform.GameCoreXboxOne:
                    case RuntimePlatform.XboxOne:
                        return DevicePlatform.XboxOne;

                    case RuntimePlatform.GameCoreXboxSeries:
                        return DevicePlatform.XboxSeries;

                    case RuntimePlatform.WebGLPlayer:
                        return DevicePlatform.WebGL;

                    case RuntimePlatform.Lumin:
                        return DevicePlatform.MagicLeap;

                    case RuntimePlatform.PS4:
                        return DevicePlatform.PS4;

                    case RuntimePlatform.PS5:
                        return DevicePlatform.PS5;

                    case RuntimePlatform.tvOS:
                    case RuntimePlatform.Switch:
                    case RuntimePlatform.Stadia:
                    case RuntimePlatform.CloudRendering:
                    case RuntimePlatform.EmbeddedLinuxArm64:
                    case RuntimePlatform.EmbeddedLinuxArm32:
                    case RuntimePlatform.EmbeddedLinuxX64:
                    case RuntimePlatform.EmbeddedLinuxX86:
                    case RuntimePlatform.LinuxServer:
                    case RuntimePlatform.WindowsServer:
                    case RuntimePlatform.OSXServer:
                    default:
                        throw new NotImplementedException($"Platform {Application.platform} unsupported.");
                }
            }
        }

        public static UnityEditorPlatform EditorPlatform
        {
            get => Application.platform switch
            {
                RuntimePlatform.WindowsEditor => UnityEditorPlatform.Windows,
                RuntimePlatform.OSXEditor => UnityEditorPlatform.Mac,
                RuntimePlatform.LinuxEditor => UnityEditorPlatform.Linux,
                _ => UnityEditorPlatform.Unknown,
            };
        }

        public static bool IsUnityCloudBuild
        {
            get
            {
                // TODO [bgish]: Should be able to figure this out by finding build config json file
                #if UNITY_CLOUD_BUILD
                return true;
                #else
                return false;
                #endif
            }
        }

        public static bool IsIosOrAndroid
        {
            get => CurrentDevicePlatform switch
            {
                DevicePlatform.iOS => true,
                DevicePlatform.Android => true,
                _ => false,
            };
        }

        public static bool IsTouchSupported
        {
            get { return Input.touchSupported; }
        }

        public static bool IsMousePresent
        {
            get { return Input.mousePresent; }
        }

        public static bool IsPenPresent
        {
            get { return false; }
        }

        public static void QuitApplication()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        public static void Vibrate(long milliseconds)
        {
            switch (CurrentDevicePlatform)
            {
                case DevicePlatform.Android:
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    Vibrator.Call("vibrate", milliseconds);
                    #endif
                    break;

                case DevicePlatform.iOS:
                    #if UNITY_IOS
                    Handheld.Vibrate();
                    #endif
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetStreamingAssetsURL(string path)
        {
            return CurrentDevicePlatform switch
            {
                DevicePlatform.Android => Path.Combine(Application.streamingAssetsPath, path).Replace(@"\", "/"),
                _ => "file://" + Path.Combine(Application.streamingAssetsPath, path).Replace(@"\", "/"),
            };
        }

        public static void GoToStore()
        {
            switch (CurrentDevicePlatform)
            {
                case DevicePlatform.Android:
                    Application.OpenURL(string.Format("market://details?id={0}", Application.identifier));
                    break;

                case DevicePlatform.iOS:
                    Application.OpenURL(string.Format("itms-apps://itunes.apple.com/app/{0}", Application.identifier));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static void RateApp()
        {
            GoToStore();
        }

        public static void SendEmail(string email, string subject = null, string body = null)
        {
            string mailToUrl = "mailto:" + email;

            if (string.IsNullOrEmpty(subject) == false)
            {
                mailToUrl += "?subject=" + UnityWebRequest.EscapeURL(subject).Replace("+", "%20");
            }

            if (string.IsNullOrEmpty(body) == false)
            {
                mailToUrl += "?body=" + UnityWebRequest.EscapeURL(body).Replace("+", "%20");
            }

            Application.OpenURL(mailToUrl);
        }

        [EditorEvents.OnExitingPlayMode]
        public static void Reset()
        {
            try
            {
                OnReset?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static bool DoesLocalFileExist(string localFileName)
        {
            switch (CurrentDevicePlatform)
            {
                case DevicePlatform.iOS:
                case DevicePlatform.Android:
                case DevicePlatform.Windows:
                case DevicePlatform.Mac:
                case DevicePlatform.Linux:
                    {
                        try
                        {
                            return File.Exists(Path.Combine(Application.persistentDataPath, localFileName));
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error in Platform.DoesLocalFileExist({localFileName})");
                            Debug.LogException(ex);
                            return false;
                        }
                    }

                case DevicePlatform.WindowsUniversal:
                case DevicePlatform.WebGL:
                    {
                        return PlayerPrefs.HasKey(localFileName);
                    }

                case DevicePlatform.XboxOne:
                case DevicePlatform.XboxSeries:
                case DevicePlatform.PS4:
                case DevicePlatform.PS5:
                case DevicePlatform.MagicLeap:
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static int GetLocalFile(string localFileName, byte[] buffer)
        {
            switch (CurrentDevicePlatform)
            {
                case DevicePlatform.iOS:
                case DevicePlatform.Android:
                case DevicePlatform.Windows:
                case DevicePlatform.Mac:
                case DevicePlatform.Linux:
                    {
                        try
                        {
                            var path = Path.Combine(Application.persistentDataPath, localFileName);

                            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                int fileLength = (int)fileStream.Length;
                                int index = 0;
                                int count = fileLength;

                                Debug.AssertFormat(buffer.Length > fileLength, "Platform.GetLocalFile byte buffer is too small. Has {0} and needs {1}.", buffer.Length, fileLength);

                                while (count > 0)
                                {
                                    int n = fileStream.Read(buffer, index, count);

                                    if (n == 0)
                                    {
                                        throw new Exception("Unknown Read Error");
                                    }

                                    index += n;
                                    count -= n;
                                }

                                return count;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error in Platform.GetLocalFile({localFileName})");
                            Debug.LogException(ex);

                            return -1;
                        }
                    }

                case DevicePlatform.WindowsUniversal:
                case DevicePlatform.WebGL:
                    {
                        var bytes = Convert.FromBase64String(PlayerPrefs.GetString(localFileName));

                        Debug.AssertFormat(buffer.Length > bytes.Length, "Platform.GetLocalFile byte buffer is too small. Has {0} and needs {1}.", buffer.Length, bytes.Length);

                        Array.Copy(bytes, buffer, bytes.Length);

                        return bytes.Length;
                    }

                case DevicePlatform.XboxOne:
                case DevicePlatform.XboxSeries:
                case DevicePlatform.PS4:
                case DevicePlatform.PS5:
                case DevicePlatform.MagicLeap:
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static void SaveLocalFile(string localFileName, byte[] bytes)
        {
            SaveLocalFile(localFileName, bytes, 0, bytes.Length);
        }

        public static void SaveLocalFile(string localFileName, byte[] bytes, int offset, int count)
        {
            switch (CurrentDevicePlatform)
            {
                case DevicePlatform.iOS:
                case DevicePlatform.Android:
                case DevicePlatform.Windows:
                case DevicePlatform.Mac:
                case DevicePlatform.Linux:
                    {
                        try
                        {
                            var path = Path.Combine(Application.persistentDataPath, localFileName);

                            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                fileStream.Write(bytes, offset, count);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error in Platform.SaveLocalFile({localFileName})");
                            Debug.LogException(ex);
                        }

                        break;
                    }

                case DevicePlatform.WindowsUniversal:
                case DevicePlatform.WebGL:
                    {
                        PlayerPrefs.SetString(localFileName, Convert.ToBase64String(bytes));
                        PlayerPrefs.Save();

                        break;
                    }

                case DevicePlatform.XboxOne:
                case DevicePlatform.XboxSeries:
                case DevicePlatform.PS4:
                case DevicePlatform.PS5:
                case DevicePlatform.MagicLeap:
                default:
                    throw new NotImplementedException();
            }
        }

        [EditorEvents.OnExitingPlayMode]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called By Unity")]
        private static void OnExitingPlayMode()
        {
            IsApplicationQuitting = true;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void RunOnStartup()
        {
            IsApplicationQuitting = false;

            if (Application.isEditor == false)
            {
                Application.quitting += () => IsApplicationQuitting = true;
            }
        }
    }
}

#endif
