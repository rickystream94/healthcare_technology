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
                {
                    return new ColorManager();
                }
                return _instance;
            }
        }

        private ColorManager()
        {
            colorDict = new Dictionary<string, Color>();
            colorDict.Add(EnemyController.RIGHT_LEG_NAME, Color.red);
            colorDict.Add(EnemyController.LEFT_LEG_NAME, Color.blue);
            colorDict.Add(EnemyController.RIGHT_TILT_NAME, Color.green);
            colorDict.Add(EnemyController.LEFT_TILT_NAME, Color.magenta);
        }
        public void GetValueFromKey(string key, out Color value)
        {
            if (!colorDict.TryGetValue(key, out value))
                value = Color.black;
        }
    }
}
