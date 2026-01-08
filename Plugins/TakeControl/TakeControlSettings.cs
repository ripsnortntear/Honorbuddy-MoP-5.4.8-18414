using System;
using System.IO;
using Styx.Helpers;

namespace TakeControl
{
    class TakeControlSettings : Settings
    {
        /// <summary>
        /// Singleton to the TakeControl configuration.
        /// </summary>
        public static readonly TakeControlSettings Instance = new TakeControlSettings();

        /// <summary>
        /// Performs an action on the settings object.
        /// </summary>
        public static void Do(Action<TakeControlSettings> action)
        {
            Instance.Load();
            action(Instance);
            Instance.Save();
        }

        /// <summary>
        /// Returns a variable in the settings object.
        /// </summary>
        public static T Get<T>(Func<TakeControlSettings, T> func)
        {
            Instance.Load();
            return func(Instance);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TakeControlSettings"/> object.
        /// </summary>
        public TakeControlSettings() : base(Path.Combine(CharacterSettingsDirectory, "TakeControl.xml"))
        {}

        /// <summary>
        /// The key for blacklisting all objects in a radius.
        /// </summary>
        [Setting, DefaultValue("")]
        public string KeyBlObjects { get; set; }

        /// <summary>
        /// The radius in which all objects are blacklisted.
        /// </summary>
        [Setting, DefaultValue(5)]
        public int BlAllObjectsRadius { get; set; }

        /// <summary>
        /// The duration which objects are blacklisted.
        /// </summary>
        [Setting, DefaultValue(20)]
        public int BlAllObjectsTime { get; set; }

        /// <summary>
        /// The key for blacklisting the current target.
        /// </summary>
        [Setting, DefaultValue("")]
        public string KeyBlTarget { get; set; }

        /// <summary>
        /// The duration which the target is blacklisted.
        /// </summary>
        [Setting, DefaultValue(20)]
        public int BlTargetTime { get; set; }

        /// <summary>
        /// The key for pausing HB.
        /// </summary>
        [Setting, DefaultValue("")]
        public string KeyPause { get; set; }

        /// <summary>
        /// The key for starting or stopping HB.
        /// </summary>
        [Setting, DefaultValue("")]
        public string KeyStartOrStop { get; set; }

        /// <summary>
        /// The key for restarting HB.
        /// </summary>
        [Setting, DefaultValue("")]
        public string KeyRestart { get; set; }
    }
}