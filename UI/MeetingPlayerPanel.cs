using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;
public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField] private Image characterImg;
    [SerializeField] private Text nicknameText;
    [SerializeField] private GameObject deadPlayerBlock;// 죽은 플레이어 표시
    [SerializeField] private GameObject reportSign; // 신고자 표시
    [SerializeField] private GameObject voteButtons;
    [SerializeField] private GameObject voteSign;
    [SerializeField] private GameObject voterPrefab;
    [SerializeField] private Transform voterParentTransform;
    
    [HideInInspector]
    public InGameCharacterMover targetPlayer;

    public void SetPlayer(InGameCharacterMover target)
    {
        Material inst = Instantiate(characterImg.material);
        characterImg.material = inst;

        targetPlayer = target;
        characterImg.material.SetColor("_PlayerColor",PlayerColor.GetColor(targetPlayer.playerColor));
        nicknameText.text = target.nickname;

        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        if (((myCharacter.playerType & EPlayerType.Imposter) == EPlayerType.Imposter) &&
            (targetPlayer.playerType & EPlayerType.Imposter) == EPlayerType.Imposter)
        {
            nicknameText.color = Color.red;
        }
        bool isDead = (targetPlayer.playerType& EPlayerType.Ghost) == EPlayerType.Ghost;
        deadPlayerBlock.SetActive(isDead);
        GetComponent<Button>().interactable = !isDead;
        reportSign.SetActive(targetPlayer.isReporter);
    }
    
    public void OnClickPlayerPanel()
    {
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        if (myCharacter.isVote) return;
            
        if ((myCharacter.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            InGameUIManager.Instance.MeetingUI.SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
    }

    public void OpenResult()
    {
        voterParentTransform.gameObject.SetActive(true);
    }

    public void Select()
    {
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        myCharacter.CmdVoteEjectPlayer(targetPlayer.playerColor);
        UnSelect();
    }

    public void UnSelect()
    {
        voteButtons.SetActive(false);
    }

    public void UpdatePanel(EPlayerColor voterColor)
    {
        var voter = Instantiate(voterPrefab, voterParentTransform).GetComponent<Image>();
        voter.material = Instantiate(voter.material);
        voter.material.SetColor("_PlayerColor",PlayerColor.GetColor(voterColor));
    }

    public void UpdateVoteSign(bool isVoted)
    {
        voteSign.SetActive(isVoted);
    }
}
