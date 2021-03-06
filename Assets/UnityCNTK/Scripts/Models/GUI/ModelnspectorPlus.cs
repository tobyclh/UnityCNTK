﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System;
using CNTK;
namespace UnityCNTK
{
    [CustomEditor(typeof(_Model), true)]
    public class ModelnspectorPlus : Editor
    {
        private string path;
        private List<Variable> inputList = new List<Variable>();
        private List<Variable> outputList = new List<Variable>();
        private bool isValidPath = false;
        private TextAsset _model;
        public override void OnInspectorGUI()
        {
            _Model model = (_Model)target;
            DrawDefaultInspector();


            if (model.rawModel != null)
            {
                isValidPath = false;
                if (_model != model.rawModel)
                {
                    try
                    {
                        var function = Function.Load(model.rawModel.bytes, DeviceDescriptor.CPUDevice);
                        isValidPath = true;
                        inputList = function.Arguments.ToList();
                        outputList = function.Outputs.ToList();
                        _model = model.rawModel;
                    }
                    catch
                    {
                    }

                }
                else
                {
                    isValidPath = true;
                }
            }

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

        }
    }



}

