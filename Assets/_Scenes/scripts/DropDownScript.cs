using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WCGL
{
    public class DropDownScript : MonoBehaviour
    {
        Dropdown dropdown;

        Shader deferredInkingShader;
        Shader backPolygonShader;
        DeferredInkingModel[] models;
        Dictionary<string, Shader> originalShaders = new Dictionary<string, Shader>();

        void Start()
        {
            dropdown = GetComponent<Dropdown>();
            deferredInkingShader = Shader.Find("DeferredInking/Line");
            backPolygonShader = Shader.Find("DeferredInking/InvertedOutline");
            models = FindObjectsOfType<DeferredInkingModel>();

            foreach (var model in models)
            {
                foreach (var mesh in model.meshes)
                {
                    var mat = mesh.material;
                    if (mat != null) { originalShaders[mat.name] = mat.shader; }
                }
            }
        }

        public void OnValueChanged()
        {
            int selected = dropdown.value;

            foreach(var model in models)
            {
                var meshes = model.meshes;
                for(int i=0; i < meshes.Count; i++)
                {
                    var mesh = meshes[i];
                    var mat = mesh.material;
                    if (mat == null) continue;

                    if (selected == 0)
                    {
                        mat.shader = originalShaders[mat.name];
                    }
                    else if (selected == 1)
                    {
                        mat.shader = deferredInkingShader;
                    }
                    else
                    {
                        mat.shader = backPolygonShader;
                    }

                    meshes[i] = mesh;
                }
            }
        }

        private void OnDisable()
        {
            foreach (var model in models)
            {
                var meshes = model.meshes;
                for (int i = 0; i < meshes.Count; i++)
                {
                    var mesh = meshes[i];
                    var mat = mesh.material;
                    if (mat == null) continue;

                    mat.shader = originalShaders[mat.name];
                    meshes[i] = mesh;
                }
            }
        }
    }
}