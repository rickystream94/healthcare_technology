using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ColorManager
    {
        private Dictionary<string, Color> colorDict;
        private static ColorManager _instance;

        public static ColorManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ColorManager();
                return _instance;
            }
        }

        private ColorManager()
        {
            colorDict = new Dictionary<string, Color>();
            colorDict.Add(EnemyController.LEG_RIGHT, Color.red);
            colorDict.Add(EnemyController.LEG_LEFT, Color.blue);
            colorDict.Add(EnemyController.TILT_RIGHT, Color.green);
            colorDict.Add(EnemyController.TILT_LEFT, Color.magenta);
        }
        public Color GetValueFromKey(string key)
        {
            Color color;
            if (!colorDict.TryGetValue(key, out color))
                color = Color.black;
            return color;
        }
    }
}
