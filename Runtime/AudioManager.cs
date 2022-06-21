//---------------------------------------------------------------------//
//                    GNU GENERAL PUBLIC LICENSE                       //
//                       Version 2, June 1991                          //
//                                                                     //
// Copyright (C) Wells Hsu, wellshsu@outlook.com, All rights reserved. //
// Everyone is permitted to copy and distribute verbatim copies        //
// of this license document, but changing it is not allowed.           //
//                  SEE LICENSE.md FOR MORE DETAILS.                   //
//---------------------------------------------------------------------//
using EP.U3D.LIBRARY.ASSET;
using EP.U3D.LIBRARY.BASE;
using UnityEngine;

namespace EP.U3D.LIBRARY.AUDIO
{
    public class AudioManager
    {
        public static GameObject GO;
        public static AudioListener Listener;
        private static bool mMuteSound = false;
        private static bool mMuteMusic = false;

        public static void Initialize(Transform root, string path)
        {
            GameObject audio = AssetManager.LoadAsset(path, typeof(GameObject)) as GameObject;
            if (audio == null)
            {
                Helper.LogError("load audiocontroller error at path {0}", path);
            }
            else
            {
                audio = Object.Instantiate(audio);
                if (audio == null)
                {
                    Helper.LogError("instantiate audiocontroller error at path {0}", path);
                }
                else
                {
                    audio.name = "Audio";
                    audio.transform.parent = root;
                    GO = audio;
                    Listener = GO.GetComponent<AudioListener>();
                    if (Listener == null) Helper.LogWarning("missing AudioListener on AudioController go.");
                    UnityEngine.SceneManagement.SceneManager.sceneLoaded += (a1, a2) =>
                    {
                        var roots = a1.GetRootGameObjects();
                        var has = false;
                        foreach (var v in roots)
                        {
                            var comp = v.GetComponentInChildren<AudioListener>(true);
                            if (comp)
                            {
                                has = true;
                                break;
                            }
                        }
                        if (Listener) Listener.enabled = !has;
                    };
                }
            }
        }

        public static bool MuteSound
        {
            get
            {
                return mMuteSound;
            }
            set
            {
                mMuteSound = value;
            }
        }

        public static bool MuteMusic
        {
            get
            {
                return mMuteMusic;
            }
            set
            {
                mMuteMusic = value;
                if (value)
                {
                    StopMusic();
                }
            }
        }

        public static void PlayMusic(string[] playlist, bool forcePlay = false)
        {
            if (mMuteMusic) return;
            if (playlist == null || playlist.Length == 0) return;
            string[] currentPlaylist = AudioController.GetMusicPlaylist();
            bool needPlay = false;
            if (currentPlaylist == null || currentPlaylist.Length == 0 || playlist.Length != currentPlaylist.Length)
            {
                needPlay = true;
            }
            else
            {
                for (int i = 0; i < currentPlaylist.Length; i++)
                {
                    string str = currentPlaylist[i];
                    string compareStr = playlist[i];
                    if (str != compareStr)
                    {
                        needPlay = true; break;
                    }
                }
            }
            if (needPlay || forcePlay)
            {
                AudioController.SetMusicPlaylist(playlist);
                AudioController.PlayMusicPlaylist();
            }
        }

        public static void StopMusic(float fadeOut = 0f)
        {
            fadeOut = Mathf.Max(fadeOut, 0);
            AudioController.StopMusic(fadeOut);
        }

        public static void PauseMusic(float fadeOut = 0f)
        {
            fadeOut = Mathf.Max(fadeOut, 0);
            AudioController.PauseMusic(fadeOut);
        }

        public static void UnPauseMusic(float fadeOut = 0f)
        {
            fadeOut = Mathf.Max(fadeOut, 0);
            AudioController.UnpauseMusic(fadeOut);
        }

        public static void PlaySound(string sound, float pitch = 1.0f)
        {
            PlaySound(sound, Vector3.zero, null, pitch);
        }

        public static void PlaySound(string sound, Vector3 pos)
        {
            PlaySound(sound, pos, null);
        }

        public static void PlaySound(string sound, Vector3 pos, Transform parent)
        {
            PlaySound(sound, pos, parent, 1.0f);
        }

        public static void PlaySound(string sound, Vector3 pos, Transform parent, float pitch)
        {
            if (mMuteSound == false)
            {
                var item = AudioController.Play(sound, pos, parent);
                if (item) item.pitch = pitch;
            }
        }

        public static void StopSound(string sound)
        {
            AudioController.Stop(sound);
        }

        public static void PauseAll(float fadeOut = 0f)
        {
            fadeOut = Mathf.Max(fadeOut, 0);
            AudioController.PauseAll(fadeOut);
        }

        public static void UnPauseAll(float fadeOut = 0f)
        {
            fadeOut = Mathf.Max(fadeOut, 0);
            AudioController.UnpauseAll(fadeOut);
        }

        public static void SetCategoryVolume(string category, float volume)
        {
            AudioController.SetCategoryVolume(category, volume);
        }

        public static bool IsPlaylistPlaying()
        {
            return AudioController.IsPlaylistPlaying();
        }
    }
}