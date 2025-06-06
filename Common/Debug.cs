﻿using System;
using System.Diagnostics;
using System.Reflection;

namespace SleepyCommon
{
    public static class CDebug
    {
        public static void Log(string sText, bool bAddCallingFunction = true)
        {
            if (bAddCallingFunction)
            {
                UnityEngine.Debug.Log($"[{UserModBase.BaseInstance.Name}] {GetCallingFunction()}: {sText}");
            }
            else
            {
                UnityEngine.Debug.Log($"[{UserModBase.BaseInstance.Name}]: {sText}");
            }
        }

        public static void LogError(string sText, bool bAddCallingFunction = true)
        {
            if (bAddCallingFunction)
            {
                UnityEngine.Debug.LogError($"[{UserModBase.BaseInstance.Name}] {GetCallingFunction()}: {sText}");
            }
            else
            {
                UnityEngine.Debug.LogError($"[{UserModBase.BaseInstance.Name}]: {sText}");
            }
            
        }

        public static void Log(Exception ex)
        {
            LogError("");
            UnityEngine.Debug.LogException(ex);
        }

        public static void Log(string sText, Exception ex)
        {
            LogError(sText);
            UnityEngine.Debug.LogException(ex);
            if (ex.InnerException is not null)
            {
                UnityEngine.Debug.LogException(ex.InnerException);
            }
        }

        public static string GetCallingFunction()
        {
            StackTrace stackTrace = new StackTrace();
            if (stackTrace.FrameCount >= 3)
            {
                StackFrame frame = stackTrace.GetFrame(2);
                MethodBase method = frame.GetMethod();
                var Class = method.ReflectedType;
                var Namespace = Class.Namespace;
                return Namespace + "." + Class.Name + "." + method.Name;
            }
            return "Unknown";
        }
    }
}
