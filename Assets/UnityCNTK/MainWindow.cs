using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
namespace UnityCNTK
{
    public class MainWindow : EditorWindow
    {
        public Model _profile;


        [MenuItem("Window/SubstanceBaker/Main Panel")]
        public static void Init()
        {
            
            EditorWindow.GetWindow(typeof(MainWindow)).Show();
        }

        void OnGUI()
        {

        }

        [MenuItem("CONTEXT/ProceduralMaterial/Bake And Replace")]
        private static void BakeAndReplace(MenuCommand menuCommand)
        {
        }

        [MenuItem("CONTEXT/ProceduralMaterial/Bake Without Replace")]
        private static void BakeNoReplace(MenuCommand menuCommand)
        {
        }





    }
}