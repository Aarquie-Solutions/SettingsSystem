using System;
using UnityEngine;

namespace AarquieSolutions.SettingsSystem
{
    public class Setting<TSettingType> where TSettingType : IComparable, IConvertible
    {
        private readonly string key;

        private TSettingType value;

        public TSettingType Value
        {
            get => value;
            set
            {
                this.value = value;
                if (valueUpdatedDelegate != null)
                {
                    valueUpdatedDelegate(this.value);
                }

                SaveSetting();
            }
        }

        private readonly TSettingType defaultValue;
        private event Action<TSettingType> valueUpdatedDelegate;


        public Setting(string settingKey, TSettingType defaultValue, Action<TSettingType> valueUpdated = null)
        {
            this.key = settingKey;
            this.defaultValue = defaultValue;
            this.valueUpdatedDelegate = valueUpdated;
            Value = LoadValue();
        }


        public TSettingType LoadValue()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            if (typeof(TSettingType) == typeof(bool))
            {
                return (TSettingType) (object) (PlayerPrefs.GetInt(key) == 1);
            }
            else if (typeof(TSettingType) == typeof(int))
            {
                return (TSettingType) (object) PlayerPrefs.GetInt(key);
            }
            else if (typeof(TSettingType) == typeof(float))
            {
                return (TSettingType) (object) PlayerPrefs.GetFloat(key);
            }
            else if (typeof(TSettingType) == typeof(string))
            {
                return (TSettingType) (object) PlayerPrefs.GetString(key);
            }
            else
            {
                Debug.LogError("Setting is not a type that is supported.");
                return defaultValue;
            }
        }

        public void SaveSetting()
        {
            if (typeof(TSettingType) == typeof(bool))
            {
                PlayerPrefs.SetInt(key, (int) Convert.ChangeType(Value.CompareTo(true) == 0 ? 1 : 0, typeof(int)));
            }
            else if (typeof(TSettingType) == typeof(int))
            {
                PlayerPrefs.SetInt(key, (int) Convert.ChangeType(Value, typeof(int)));
            }
            else if (typeof(TSettingType) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, (float) Convert.ChangeType(Value, typeof(float)));
            }
            else if (typeof(TSettingType) == typeof(string))
            {
                PlayerPrefs.SetString(key, (string) Convert.ChangeType(Value, typeof(string)));
            }

            PlayerPrefs.Save();
        }
    }
}