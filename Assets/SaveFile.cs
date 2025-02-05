using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public class BetterDict
    {
        public System.Action onSave;
        public Dictionary<string, string> _data = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                if (!_data.ContainsKey(key))
                {
                    _data[key] = string.Empty;
                }
                return _data[key];
            }
            set
            {
                _data[key] = value;
            }

        }

        public float GetFloat(string key)
        {
            if (this[key] == "")
            {
                this[key] = "0";
                return 0;
            }
            if (float.TryParse(this[key], out float swag))
            {
                return swag;
            }
            else
            {
                this[key] = "0";
                return 0;
            }
        }
        public int GetInt(string key)
        {
            if (this[key] == "")
            {
                this[key] = "0";
                return 0;
            }
            if (int.TryParse(this[key], out int swag))
            {
                return swag;
            }
            else
            {
                this[key] = "0";
                return 0;
            }
        }

        public void AddInt(string key, int value)
        {
            if (this[key] == "")
            {
                this[key] = value.ToString();
            }
            if (int.TryParse(this[key], out int swag))
            {
                this[key] = (swag + value).ToString();
            }
            else
            {
                this[key] = value.ToString();
            }
        }
        public void AddFloat(string key, float value)
        {
            if (this[key] == "")
            {
                this[key] = value.ToString();
            }
            if (float.TryParse(this[key], out float swag))
            {
                this[key] = (swag + value).ToString();
            }
            else
            {
                this[key] = value.ToString();
            }
        }

    }
    public static BetterDict variables = new();

    float SaveCooldown = 2;
    private void Start()
    {
        Load();
        InvokeRepeating("Save", 1, 0.1f);
    }
    public void Save()
    {
        if (variables.onSave != null) variables.onSave.Invoke();
        string save = "";
        foreach (var pair in variables._data)
        {
            save += pair.Key + "|" + pair.Value + '\n';
        }
        File.WriteAllText(Application.persistentDataPath + "/save", save);
    }
    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save"))
        {
            string[] save = File.ReadAllText(Application.persistentDataPath + "/save").Split("\n");
            foreach (var variable in save)
            {
                string[] vars = variable.Split("|");
                if (vars.Length > 1)
                    variables[vars[0]] = vars[1];
            }
        }
        else
        {
            File.Create(Application.persistentDataPath + "/save").Close();
            variables["coins"] = "0";
        }

    }
}
public class AsFloat
{
    static AsFloat _instance;
    public static AsFloat Instance
    {
        get
        {
            if (_instance == null) _instance = new AsFloat();
            return _instance;
        }
    }
    public float this[string key]
    {
        get
        {
            float value;
            if (float.TryParse(SaveSystem.variables[key], out value))
            {
                return value;
            }
            return 0;
        }
        set
        {
            SaveSystem.variables[key] = value.ToString();
        }
    }
}