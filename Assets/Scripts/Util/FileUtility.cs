using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileUtility
{
    public static string voicelinesPath => "Audio/Voicelines";
    public static string levelDataPath => "Scriptable Objects/Level Data";
    public static string animatorPath => "Animation/Animators";
    public static string dialoguePath => "Dialogue";
    public static string bgmPath => "Audio/Music"; 


    /// <summary>
    /// Returns file at Resources/<paramref name="filePath"/> of type T
    /// </summary>
    /// <typeparam name="T">The type of file to load</typeparam>
    /// <param name="filePath">Path of file</param>
    /// <param name="filter">Type of file filter</param>
    /// <returns>The file as type <typeparamref name="T"/></returns>
    public static T LoadFile<T>(string filePath, Type filter = null) where T : class
    {
        T file; 
        
        // get file as type T
        if (filter != null) file = Resources.Load(filePath, typeof(T)) as T;
        else file = Resources.Load(filePath) as T;

        // null check 
        if (file == null)
        {
            throw new Exception($"The file at Assets/Resources/{filePath} is either missing or malformed! File load failed.");
        }

        // return file 
        return file; 
    }
}
