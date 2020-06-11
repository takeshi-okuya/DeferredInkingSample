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
                    originalShaders[mat.name] = mat.shader;
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
                    var mat = new Material(meshes[i].material);

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

                    var mesh = meshes[i];
                    mesh.material = mat;
                    meshes[i] = mesh;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}