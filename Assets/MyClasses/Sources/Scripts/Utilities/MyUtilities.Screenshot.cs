/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Screenshot (version 1.0)
 */

using UnityEngine;
using System.IO;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Capture a screenshot.
        /// </summary>
        public static Texture2D CaptureScreenshot(string fileName)
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            File.WriteAllBytes(GetScreenshotFilePath(fileName), texture.EncodeToPNG());

            return texture;
        }

        /// <summary>
        /// Return a screenshot by name.
        /// </summary>
        public static Texture2D GetScreenshot(string fileName)
        {
            string filePath = GetScreenshotFilePath(fileName);
            if (File.Exists(filePath))
            {
                Texture2D texture = new Texture2D(4, 4);
                texture.LoadImage(File.ReadAllBytes(filePath));

                return texture;
            }

            return null;
        }

        /// <summary>
        /// Return a file path of a screenshot.
        /// </summary>
        public static string GetScreenshotFilePath(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName;
        }
    }
}