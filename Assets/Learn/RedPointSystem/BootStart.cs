using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BootStart : MonoBehaviour
{
    public Image ImgMail;
    public Image ImgMailSystem;
    public Image ImgMailTeam;
    public Text TexMail;
    public Text TexMailSystem;
    public Text TexMailTeam;

    private void Start()
    {
        RedPointSystem redPointSystem = new RedPointSystem();
        redPointSystem.InitRedPointTreeNode();

        //设置红点的节点处理函数
        redPointSystem.SetRedPointNodeCallBack(RedPointConst.MAIL, OnMailRedPointChange);
        redPointSystem.SetRedPointNodeCallBack(RedPointConst.MAIL_SYSTEM, OnMailSystemRedPointChange);
        redPointSystem.SetRedPointNodeCallBack(RedPointConst.MAIL_TEAM, OnMailTeamRedPointChange);


        redPointSystem.SetInvoke(RedPointConst.MAIL_SYSTEM, 10);
        redPointSystem.SetInvoke(RedPointConst.MAIL_TEAM, 8);

    }

    private void OnMailTeamRedPointChange(RedPointNode node)
    {
        ImgMailTeam.gameObject.SetActive(node.PointNum > 0);
        TexMailTeam.text = node.PointNum.ToString();
    }

    private void OnMailSystemRedPointChange(RedPointNode node)
    {
        ImgMailSystem.gameObject.SetActive(node.PointNum > 0);
        TexMailSystem.text = node.PointNum.ToString();
    }

    private void OnMailRedPointChange(RedPointNode node)
    {
        ImgMail.gameObject.SetActive(node.PointNum > 0);
        TexMail.text = node.PointNum.ToString();
    }
}
