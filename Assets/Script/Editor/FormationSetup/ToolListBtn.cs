// create by liudi

using UnityEditor;
using UnityEngine;

namespace FormationSetup
{
    public class ToolListBtn
    {
        [MenuItem("Im30/方阵配置")]
        private static void NewMenuOption()
        {
            //create setup window;
            FormationSetupWindow.Init();
        }
    }
}