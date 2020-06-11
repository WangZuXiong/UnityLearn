/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using System;
using XLua;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class XLuaConfig
{
    
    //热更配置
    [Hotfix]
    public static List<Type> by_property
    {
        get
        {
            List<Type> list = new List<Type>();
            DirectoryInfo di = new DirectoryInfo(Application.dataPath);
            string url = di.Parent.FullName + "/Library/ScriptAssemblies/Assembly-CSharp.dll";
            Type[] arr = Assembly.LoadFile(url).GetTypes();
            foreach (var type in arr)
            {
                if (type.Namespace == null)
                {
                    list.Add(type);
                }
            }
            return list;
        }
    }

    //[LuaCallCSharp]
    //public static List<Type> ProtoBufList
    //{
    //    get
    //    {
    //        List<Type> list = new List<Type>();
    //        DirectoryInfo di = new DirectoryInfo(Application.dataPath);
    //        string url = di.Parent.FullName + "/Library/ScriptAssemblies/Assembly-CSharp.dll";
    //        Type[] arr = Assembly.LoadFile(url).GetTypes();
    //        foreach (var type in arr)
    //        {
    //            if (type.Namespace.IndexOf("com.proto", StringComparison.Ordinal) != -1)
    //            {
    //                list.Add(type);
    //            }
    //        }
    //        return list;
    //    }
    //}

    ////GoKit相关配置
    //[LuaCallCSharp]
    //[ReflectionUse]
    //public static List<Type> goKit = new List<Type>()
    //{
    //    typeof(AbstractGoTween),
    //    typeof(AbstractGoTweenCollection),

    //    typeof(GoEaseAnimationCurve),
    //    typeof(GoEaseBack),
    //    typeof(GoEaseBounce),
    //    typeof(GoEaseCircular),
    //    typeof(GoEaseCubic),
    //    typeof(GoEaseElastic),
    //    typeof(GoEaseExponential),
    //    typeof(GoEaseLinear),
    //    typeof(GoEaseQuadratic),
    //    typeof(GoEaseQuartic),
    //    typeof(GoEaseQuintic),
    //    typeof(GoEaseSinusoidal),

    //    typeof(GoDuplicatePropertyRuleType),
    //    typeof(GoEaseType),
    //    typeof(GoLogLevel),
    //    typeof(GoLookAtType),
    //    typeof(GoLoopType),
    //    typeof(GoShakeType),
    //    typeof(GoSplineType),
    //    typeof(GoTweenState),
    //    typeof(GoUpdateType),

    //    //typeof(GoKitTweenExtensions),

    //    typeof(AbstractMaterialColorTweenProperty),
    //    typeof(AbstractMaterialFloatTweenProperty),
    //    typeof(AbstractMaterialVectorTweenProperty),
    //    typeof(AbstractQuaternionTweenProperty),
    //    typeof(AbstractTweenProperty),
    //    typeof(AbstractVector3TweenProperty),

    //    typeof(ColorTweenProperty),
    //    typeof(FloatTweenProperty),
    //    typeof(IGenericProperty),
    //    typeof(IntTweenProperty),
    //    typeof(Vector2TweenProperty),
    //    typeof(Vector3PathTweenProperty),
    //    typeof(Vector3TweenProperty),
    //    typeof(Vector3XTweenProperty),
    //    typeof(Vector3YTweenProperty),
    //    typeof(Vector3ZTweenProperty),
    //    typeof(Vector4TweenProperty),

    //    typeof(GoSmoothedQuaternion),
    //    typeof(GoSmoothedVector3),
    //    typeof(GoSmoothingType),

    //    typeof(AnchoredPosition3DTweenProperty),
    //    typeof(AnchoredPositionTweenProperty),
    //    typeof(AnchorMaxTweenProperty),
    //    typeof(AnchorMinTweenProperty),
    //    typeof(EulerAnglesTweenProperty),
    //    typeof(MaterialColorTweenProperty),
    //    typeof(MaterialFloatTweenProperty),
    //    typeof(MaterialVectorTweenProperty),
    //    typeof(OffsetTweenProperty),
    //    typeof(PivotTweenProperty),
    //    typeof(PositionPathTweenProperty),
    //    typeof(PositionTweenProperty),
    //    typeof(RotationQuaternionTweenProperty),
    //    typeof(RotationTweenProperty),
    //    typeof(ScalePathTweenProperty),
    //    typeof(ScaleTweenProperty),
    //    typeof(ShakeTweenProperty),
    //    typeof(SizeDeltaTweenProperty),

    //    typeof(AbstractGoSplineSolver),
    //    typeof(GoSpline),
    //    typeof(GoSplineCatmullRomSolver),
    //    typeof(GoSplineCubicBezierSolver),
    //    typeof(GoSplineQuadraticBezierSolver),
    //    typeof(GoSplineStraightLineSolver),

    //    typeof(GoTweenUtils),

    //    typeof(Go),
    //    typeof(GoDummyPath),
    //    //typeof(GoProxyProp),
    //    typeof(GoTween),
    //    typeof(GoTweenChain),
    //    typeof(GoTweenCollectionConfig),
    //    typeof(GoTweenConfig),
    //    typeof(GoTweenFlow),


    //};

    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(Light),
        typeof(Mathf),
        typeof(UnityWebRequest),
        typeof(System.Collections.Generic.List<int>),
        typeof(Action<string>),
        typeof(Action),
        typeof(UnityEngine.Debug)
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
                typeof(Action),
                typeof(Func<double, double, double>),
                typeof(Action<string>),
                typeof(Action<double>),
                typeof(UnityEngine.Events.UnityAction),
                typeof(System.Collections.IEnumerator)
            };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
                new List<string>(){"UnityEngine.Light", "SetLightDirty"},
                new List<string>(){"UnityEngine.Light", "shadowRadius"},
                new List<string>(){"UnityEngine.Light", "shadowAngle"},

                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"}
            };
}
