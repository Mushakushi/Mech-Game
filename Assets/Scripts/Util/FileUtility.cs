using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileUtility : MonoBehaviour
{
    /// <summary>
    /// Returns file at Resources/<paramref name="filePath"/> of type T
    /// </summary>
    /// <typeparam name="T">The type of file to load</typeparam>
    /// <param name="filePath">Path of file</param>
    /// <param name="filter">Type of file filter</param>
    /// <returns>The file as type T</returns>
    public static T LoadFile<T>(string filePath, Type filter = null) where T : class
    {
        T file; 
        
        // get file as type T
        if (filter != null) file = Resources.Load(filePath, typeof(T)) as T;
        else file = Resources.Load(filePath) as T;

        // null check 
        if (file == null) throw new Exception($"Missing file Assets/Resources/{filePath} or file is malformed! File load failed.");

        // return file 
        return file; 
    }
}
