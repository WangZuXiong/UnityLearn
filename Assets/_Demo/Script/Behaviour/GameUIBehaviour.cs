using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameUIBehaviour : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public void Show()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(string.Format("Player Winner加分：{0}\n", GameData.Config.WinScore.ToString()));
        builder.Append(string.Format("Player Loser扣分：{0}\n", GameData.Config.LoseScore.ToString()));
        builder.Append(string.Format("Team Winner扣除的体能：{0}\n", GameData.Config.WinEnergy.ToString()));
        builder.Append(string.Format("Team Loser扣除的体能：{0}\n", GameData.Config.LoseEnergy.ToString()));
        builder.Append(string.Format("Main City 总血量：{0}\n", GameData.Config.CityTotalBlood.ToString()));
        builder.Append(string.Format("Main City 遭受攻击时扣除血量：{0}\n", GameData.Config.CityBlood.ToString()));
        builder.Append(string.Format("进攻对方Main City获得的积分：{0}\n", GameData.Config.MainCityScore.ToString()));
        builder.Append(string.Format("进攻对方Main City扣除的体能：{0}\n", GameData.Config.MainCityEnergy.ToString()));
        builder.Append(string.Format("占领中立场加积分速度：{0}/{1}s", GameData.Config.InNeutralScore.ToString(),GameData.Config.InNeutralTime.ToString()));

        _text.text = builder.ToString();
    }
}
