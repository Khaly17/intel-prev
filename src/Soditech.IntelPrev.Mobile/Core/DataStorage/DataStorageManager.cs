using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SavvyTech.R2A.Core.Secure;
using Soditech.IntelPrev.Mobile.Core.Runtime;

namespace Soditech.IntelPrev.Mobile.Core.DataStorage
{
    /// <summary>
    /// Uses Preferences Application Properties to save data.
    /// If you need to store secure values such as password, use ISecureStorage.
    /// </summary>
    public class DataStorageManager :  IDataStorageManager
    {
        private static void StorePrimitive(string key, object value)
        {
            Preferences.Set(key, value == null ? null : value.ToString());
        }

        private static void StoreObject(string key, object value)
        {
            Preferences.Set(key, JsonSerializer.Serialize(value));
        }

        private T GetPrimitive<T>(string key, T defaultValue = default(T))
        {
            if (!HasKey(key))
            {
                return defaultValue;
            }
            
            return (T)Convert.ChangeType(Preferences.Get(key, null), typeof(T));
        }

        private T RetrieveObject<T>(string key, T defaultValue = default(T))
        {
            var value = Preferences.Get(key, "");

            return string.IsNullOrEmpty(value) ? defaultValue : JsonSerializer.Deserialize<T>(value)!;
        }

        public bool HasKey(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public T Retrieve<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false)
        {
            var value = TypeHelperExtended.IsPrimitive(typeof(T), false) ?
                GetPrimitive(key, defaultValue) :
                RetrieveObject(key, defaultValue);

            if (!shouldDecrpyt)
            {
                return value;
            }

            var decrypted = SimpleStringCipher.Decrypt(Convert.ToString(value));
            return (T)Convert.ChangeType(decrypted, typeof(T));
        }

        public async Task StoreAsync<T>(string key, T value, bool shouldEncrypt = false)
        {
            if (TypeHelperExtended.IsPrimitive(typeof(T), false))
            {
                if (shouldEncrypt)
                {
                    StorePrimitive(key, SimpleStringCipher.Encrypt(Convert.ToString(value)));
                }
                else
                {
                    StorePrimitive(key, value);
                }
            }
            else
            {
                StoreObject(key, value);
            }

            await Task.CompletedTask;
        }

        public void RemoveIfExists(string key)
        {
            if (HasKey(key))
            {
                Preferences.Remove(key);
            }
        }
    }
}