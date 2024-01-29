using UnityEngine;

namespace DltFramework
{
    public partial class EntityItem
    {
        /// <summary>
        ///   <para>Logs a message to the Unity Console.</para>
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Log(object message)
        {
            DebugFrameComponent.Log(message);
        }

        /// <summary>
        ///   <para>Logs a message to the Unity Console.</para>
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void Log(object message, Object context)
        {
            DebugFrameComponent.Log(message, context);
        }

        /// <summary>
        ///   <para>Logs a formatted message to the Unity Console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public void LogFormat(string format, params object[] args)
        {
            DebugFrameComponent.LogFormat(format, args);
        }

        /// <summary>
        ///   <para>Logs a formatted message to the Unity Console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogFormat(Object context, string format, params object[] args)
        {
            DebugFrameComponent.LogFormat(context, format, args);
        }

        /// <summary>
        ///   <para>Logs a formatted message to the Unity Console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="logType">Type of message e.g. warn or error etc.</param>
        /// <param name="logOptions">Option flags to treat the log message special.</param>
        public void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            DebugFrameComponent.LogFormat(logType, logOptions, context, format, args);
        }

        /// <summary>
        ///   <para>A variant of Debug.Log that logs an error message to the console.</para>
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void LogError(object message)
        {
            DebugFrameComponent.LogError(message);
        }

        /// <summary>
        ///   <para>A variant of Debug.Log that logs an error message to the console.</para>
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogError(object message, Object context)
        {
            DebugFrameComponent.LogError(message, context);
        }

        /// <summary>
        ///   <para>Logs a formatted error message to the Unity console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public void LogErrorFormat(string format, params object[] args)
        {
            DebugFrameComponent.LogErrorFormat(format, args);
        }

        /// <summary>
        ///   <para>Logs a formatted error message to the Unity console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogErrorFormat(Object context, string format, params object[] args)
        {
            DebugFrameComponent.LogErrorFormat(context, format, args);
        }

        public void LogWarning(object message)
        {
            DebugFrameComponent.LogWarning(message);
        }

        /// <summary>
        ///   <para>A variant of Debug.Log that logs a warning message to the console.</para>
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogWarning(object message, Object context)
        {
            DebugFrameComponent.LogWarning(message, context);
        }

        /// <summary>
        ///   <para>Logs a formatted warning message to the Unity Console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public void LogWarningFormat(string format, params object[] args)
        {
            DebugFrameComponent.LogWarningFormat(format, args);
        }

        /// <summary>
        ///   <para>Logs a formatted warning message to the Unity Console.</para>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void LogWarningFormat(Object context, string format, params object[] args)
        {
            DebugFrameComponent.LogWarningFormat(context, format, args);
        }

        /// <summary>
        ///   <para>Assert a condition and logs an error message to the Unity console on failure.</para>
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        public void Assert(bool condition)
        {
            DebugFrameComponent.Assert(condition);
        }

        /// <summary>
        ///   <para>Assert a condition and logs an error message to the Unity console on failure.</para>
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="context">Object to which the message applies.</param>
        public void Assert(bool condition, Object context)
        {
            DebugFrameComponent.Assert(condition, context);
        }

        /// <summary>
        ///   <para>Assert a condition and logs an error message to the Unity console on failure.</para>
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Assert(bool condition, object message)
        {
            DebugFrameComponent.Assert(condition, message);
        }

        public void Assert(bool condition, string message)
        {
            DebugFrameComponent.Assert(condition, message);
        }

        /// <summary>
        ///   <para>Assert a condition and logs an error message to the Unity console on failure.</para>
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public void Assert(bool condition, object message, Object context)
        {
            DebugFrameComponent.Assert(condition, message, context);
        }

        public void Assert(bool condition, string message, Object context)
        {
            DebugFrameComponent.Assert(condition, message, context);
        }

        /// <summary>
        ///   <para>Assert a condition and logs a formatted error message to the Unity console on failure.</para>
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public void AssertFormat(bool condition, string format, params object[] args)
        {
            DebugFrameComponent.AssertFormat(condition, format, args);
        }
    }
}