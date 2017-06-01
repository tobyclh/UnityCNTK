using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEditor;
using System;
using CNTK;
using UnityEngine;

namespace UnityCNTK
{
    [CustomEditor(typeof(_Model), true)]
    public class ModelnspectorPlus : Editor
    {
        private string path;
        private List<Variable> inputList = new List<Variable>();
        private List<Variable> outputList = new List<Variable>();
        private bool isValidPath = false;

        public override void DrawPreview(Rect previewArea)
        {
            base.DrawPreview(previewArea);
            path = null;
        }
        public override void OnInspectorGUI()
        {
            _Model model = (_Model)target;
            DrawDefaultInspector();

            if (isValidPath)
            {
                EditorGUILayout.LabelField("Inputs");
                foreach (var input in inputList)
                {
                    EditorGUILayout.LabelField(input.AsString());
                }
                EditorGUILayout.LabelField("Outputs");
                foreach (var output in outputList)
                {
                    EditorGUILayout.LabelField(output.AsString());
                }
            }
            else
            {
                EditorGUILayout.LabelField("Invalid Model");
            }
            if (model.relativeModelPath != null)
            {
                var relativeModelPath = model.relativeModelPath;
                if (path != relativeModelPath)
                {
                    isValidPath = false;
                    path = relativeModelPath;
                    var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
                    if (System.IO.File.Exists(absolutePath))
                    {
                        try
                        {
                            var function = Function.Load(absolutePath, DeviceDescriptor.CPUDevice);
                            isValidPath = true;
                            inputList = function.Arguments.ToList();
                            outputList = function.Outputs.ToList();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }



    }
}

