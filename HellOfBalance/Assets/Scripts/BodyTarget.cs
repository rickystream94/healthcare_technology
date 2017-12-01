using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class BodyTarget
    {
        internal Color Color { get; set; }
        internal ColorManager colorManager;
        internal string Target { get; set; }
        public BodyTarget(string target, GameObject hazardInstance)
        {
            colorManager = ColorManager.Instance;
            Target = target;
            Color = colorManager.GetValueFromKey(target);
            SetRendererProperties(hazardInstance);
        }
        public void SetRendererProperties(GameObject instance)
        {
            Renderer renderer = instance.GetComponent<Renderer>();
            colorManager.GetValueFromKey(Target);
            renderer.material.SetColor("_OutlineColor", Color);
            renderer.material.SetFloat("_Outline", 0.005f);
        }
    }
}
