using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleController : MonoBehaviour
{
    [Serializable]
    private struct Config
    {
        public Wheel Wheel;
        public float HorizontalLayoutGroupSpacing;
        public float FullInfoImgBgOffsetY;
        public float FullInfoImgBgOffsetX;
        public Vector2 ScrollViewLocalPosition;
        public Vector2 ScrollViewSizeDalta;
        public float ContentHeight;
        public Vector2 RootLocalPosition;
        public Vector2 RootLocalSizeDalta;
        public float RootScale;
        public Vector3 ChampionInfoOffset;
    }

    [Serializable]
    private enum Wheel : int
    {
        Wheel3 = 3,
        Wheel4 = 4
    }

    [SerializeField]
    private List<Config> configs;
    private readonly float TempTan = Mathf.Tan(82 * Mathf.Deg2Rad);

    public Dictionary<string, Transform> AllianceInfoDict { get; private set; } = new Dictionary<string, Transform>();
    public Dictionary<string, ConnectingLine> LineDict { get; private set; } = new Dictionary<string, ConnectingLine>();

    public void ClearInfo()
    {
        foreach (var item in AllianceInfoDict)
        {
            var t = item.Value.Find("ImgBg/LogoRoot");
            if (t.childCount > 0)
            {
                Destroy(t.GetChild(0).gameObject);
            }

            var isFull = item.Value.name.StartsWith("1");
            if (isFull)
            {
                item.Value.Find("ImgBg/TexName").GetComponent<Text>().text = string.Empty;
                item.Value.Find("ImgBg/TexService").GetComponent<Text>().text = string.Empty;
            }
            else
            {
                item.Value.Find("ImgBg/Empty").gameObject.Show();
            }
            item.Value.Find("ImgBg").GetComponent<CanvasGroup>().alpha = .8f;
            item.Value.Find("ImgBg/WinFlag").gameObject.SetActive(false);
        }

        foreach (var item in LineDict)
        {
            item.Value.ResetAnimation();
        }
    }

    public IEnumerator Create(int totalWheel, Action callback)
    {
        var config = configs.Find(t => t.Wheel == (Wheel)totalWheel);
        if (config.Wheel == 0)
        {
            yield break;
        }

        var fullOriginal = Resources.Load<GameObject>("UI/Alliance/Prefab/ChallengeCompetition/Item/FullInfo");
        var original = Resources.Load<GameObject>("UI/Alliance/Prefab/ChallengeCompetition/Item/Info");
        var linesOriginal = Resources.Load<ConnectingLine>("UI/Alliance/Prefab/ChallengeCompetition/Item/Lines");

        transform.Find("Scroll View").localPosition = config.ScrollViewLocalPosition;
        transform.Find("Scroll View").GetComponent<RectTransform>().sizeDelta = config.ScrollViewSizeDalta;
        transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>().SetHeight(config.ContentHeight);

        var root = transform.Find("Scroll View/Viewport/Content/Root");
        root.GetComponent<RectTransform>().sizeDelta = config.RootLocalSizeDalta;
        root.localScale = Vector3.one * config.RootScale;
        root.GetComponent<RectTransform>().anchoredPosition = config.RootLocalPosition;

        for (int k = 0; k < 2; k++)
        {
            var isRight = k == 1;
            var subRoot = root.Find(isRight ? "Right" : "Left");

            for (int i = 0; i < totalWheel; i++)
            {
                subRoot.GetComponent<HorizontalLayoutGroup>().spacing = config.HorizontalLayoutGroupSpacing;

                var currentWheelIndex = i;
                if (!isRight)
                {
                    currentWheelIndex = totalWheel - currentWheelIndex - 1;
                }

                var tempName = (totalWheel - currentWheelIndex).ToString();
                var infoParent = new GameObject(tempName, typeof(RectTransform)).transform;
                var layoutGroup = infoParent.gameObject.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childAlignment = TextAnchor.MiddleCenter;
                layoutGroup.childControlWidth = layoutGroup.childControlHeight = false;
                infoParent.SetParent(subRoot);
                infoParent.rotation = Quaternion.Euler(0, 0, -9.5f);
                infoParent.localScale = Vector3.one;
                infoParent.localPosition = Vector3.zero;

                var isShowFullInfo = currentWheelIndex == totalWheel - 1;
                if (isShowFullInfo)
                {
                    infoParent.GetComponent<RectTransform>().SetWidth(300);
                }
                var count = (int)Mathf.Pow(2f, currentWheelIndex);
                for (int j = 0; j < count; j++)
                {
                    GameObject info;
                    if (isShowFullInfo)
                    {
                        info = Instantiate(fullOriginal, infoParent);
                        var imgBg = info.transform.Find("ImgBg");
                        imgBg.localPosition += new Vector3(config.FullInfoImgBgOffsetY / TempTan, config.FullInfoImgBgOffsetY) * ((j + 1) % 2 == 0 ? 1 : -1);
                        imgBg.localPosition += new Vector3(config.FullInfoImgBgOffsetX, 0) * (!isRight ? 1 : -1);
                    }
                    else
                    {
                        info = Instantiate(original, infoParent);
                    }
                    info.name = string.Format("{0}-{1}", tempName, (j + 1 + (isRight ? count : 0)).ToString());
                    AllianceInfoDict.Add(info.name, info.transform);
                    yield return null;
                }
            }
        }

        //冠军
        var championInfoOriginal = Resources.Load<GameObject>("UI/Alliance/Prefab/ChallengeCompetition/Item/ChampionInfo");
        var championInfo = Instantiate(championInfoOriginal, root);
        championInfo.transform.name = "Champion";
        var wInfo = AllianceInfoDict[totalWheel.ToString() + "-1"];
        championInfo.transform.position = wInfo.TransformPoint(config.ChampionInfoOffset);
        AllianceInfoDict.Add(championInfo.transform.name, championInfo.transform);
        yield return null;

        //var scale = GetComponentInParent<Canvas>().transform.localScale.x;
        //create Line
        var lineContent = transform.Find("Scroll View/Viewport/Content/LineContent");
        foreach (var item in AllianceInfoDict)
        {
            var t = item.Key.Split('-');
            if (t.Length < 2)
            {
                continue;
            }
            if (!int.TryParse(t[0], out int itemWheel))
            {
                continue;
            }
            if (!int.TryParse(t[1], out int itemIndex))
            {
                continue;
            }
            if (itemWheel > 1)
            {
                var targetWheel = itemWheel - 1;
                var targetIndex1 = itemIndex * 2 - 1;
                var targetIndex2 = itemIndex * 2;

                var targetName1 = string.Format("{0}-{1}", targetWheel.ToString(), targetIndex1.ToString());
                var targetName2 = string.Format("{0}-{1}", targetWheel.ToString(), targetIndex2.ToString());

                var lines = Instantiate(linesOriginal, lineContent);
                lines.transform.position = item.Value.position;
                lines.name = item.Key;
                var target1 = AllianceInfoDict[targetName1].Find("ImgBg");
                var target2 = AllianceInfoDict[targetName2].Find("ImgBg");

                //if (targetWheel == 1)
                //{
                //    var isLeft = targetIndex1 <= Mathf.Pow(2, totalWheel) * 0.5f;
                //    var x = (isLeft ? 77 : -77) * scale;
                //    target1 += new Vector3(x, 0, 0);
                //    target2 += new Vector3(x, 0, 0);
                //}


                lines.SetLine(target1, target2);
                LineDict.Add(lines.name, lines);
            }
        }

        callback?.Invoke();
    }

    public bool EnableScrollView = false;

    private void Awake()
    {
        transform.Find("Scroll View").GetComponent<ScrollRect>().enabled = EnableScrollView;
        transform.Find("Scroll View/Viewport").GetComponent<RectMask2D>().enabled = EnableScrollView;
    }

    public void SetEnableScrollView(bool enable)
    {
        EnableScrollView = enable;
        Awake();
    }
}

/*
 
        private bool TryGetKeyByIndex(int index, out int key)
    {
        var temp = 0;
        foreach (var item in _infoPBDict)
        {
            if (index == temp)
            {
                key = item.Key;
                return true;
            }
            temp++;
        }
        key = -1;
        return false;
    }

     private void InitToggle(int groupCount)
    {
        var toggleGroup = transform.Find("ToggleGroup").GetComponent<ToggleGroup>();
        var toggleOriginal = toggleGroup.transform.Find("Toggle").GetComponent<Toggle>();

        for (int i = 0; i < groupCount - 1; i++)
        {
            Instantiate(toggleOriginal, toggleGroup.transform);
        }

        var toggles = toggleGroup.transform.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            var toggle = toggles[i];
            toggle.group = toggleGroup;
            var texLabel = toggle.transform.Find("Label").GetComponent<Text>();
            char g = (char)(i + 65);
            var index = i;
            texLabel.text = LanguageService.Instance.GetStringFormatByKey("AllianceChallengeCompetition/GroupName", g.ToString());
            toggle.onValueChanged.AddListener((t) =>
            {
                texLabel.fontSize = t ? 38 : 36;
                texLabel.color = t ? Color.white : UnityConst.ColorTranslucent;

                if (t)
                {
                    if (TryGetKeyByIndex(index, out int groupId))
                    {
                        ClearInfo();
                        InitAllianceInfos(groupId);
                    }
                }
            });
        }
        if (toggles.Length > 0)
        {
            toggles[0].isOn = true;
        }
    }
 */
