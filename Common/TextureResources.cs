using System.IO;
using System.Reflection;
using System;
using UnityEngine;
using System.Linq;

namespace SleepyCommon
{
    public static class TextureResources
    {
        public static Texture2D? LoadDllResource(Assembly myAssembly, string resourceName, int iWidth, int iHeight, bool mip = false, bool failIfNotFound = true)
        {
            try
            {
                string sFullResourceName = myAssembly.GetManifestResourceNames().Single(str => str.EndsWith(resourceName));
                Stream myStream = myAssembly.GetManifestResourceStream(sFullResourceName);
                if (myStream is null)
                {
                    if (failIfNotFound)
                    {
                        throw new Exception($"Resource stream {resourceName} not found!");
                    }

                    CDebug.LogError("Resource " + resourceName + " not found (not an error)");
                    return null;
                }

                var texture = new Texture2D(
                    width: iWidth,
                    height: iHeight,
                    format: TextureFormat.ARGB32,
                    mipmap: mip);

                texture.LoadImage(ReadToEnd(myStream));

                return texture;
            }
            catch (Exception e)
            {
                CDebug.Log("Failed to load resources", e);
                return null;
            }
        }

        static byte[]? ReadToEnd(Stream stream)
        {
            var originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                var readBuffer = new byte[4096];

                var totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(
                            readBuffer,
                            totalBytesRead,
                            readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead != readBuffer.Length)
                        continue;

                    var nextByte = stream.ReadByte();
                    if (nextByte == -1)
                        continue;

                    var temp = new byte[readBuffer.Length * 2];
                    Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                    Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                    readBuffer = temp;
                    totalBytesRead++;
                }

                var buffer = readBuffer;
                if (readBuffer.Length == totalBytesRead)
                    return buffer;

                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                return buffer;
            }
            catch (Exception e)
            {
                CDebug.Log(e);
                return null;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }
    }
}