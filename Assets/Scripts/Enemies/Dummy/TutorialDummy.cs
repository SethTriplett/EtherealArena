using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This behavior has become a beautiful heaping mess trying to connect a not so complicated state machine
public class TutorialDummy : MonoBehaviour, IEventListener {

    private enum TutorialSegment {
        FightOpening,
        WelcomeDialog,
        MovementControls,
        AttackDialog,
        AttackDialogAlt,
        AttackControls,
        SkillSwap,
        SkillSwapControls,
        SkillSwapAlt,
        SkillExplaination,
        AfterSkillExplaination,
        DodgeDialog,
        DodgeDemo,
        PhaseTutorial,
        PhaseTutorialDemo,
        PhaseTutorialTwo,
        StrongAttackDemo,
        HealthTutorial,
        AfterHealthTutorial,
        Closing
    }

    private TutorialSegment currentSegment;
    private float doNothingTimer;
    private KnifeDummy knifeDummyScript;
    private EnemyStatus enemyStatusScript;
    private bool playerMoved;
    private int playerSwappedSkills;
    
    // Secret speed run strat if player does actions early
    // Determines and tracks alternate conversations
    private bool attackedEarly;
    private bool swappedEarly;

    void Awake() {
        knifeDummyScript = GetComponent<KnifeDummy>();
        enemyStatusScript = GetComponent<EnemyStatus>();
    }

    void Start() {
        doNothingTimer = 2.75f;
        currentSegment = TutorialSegment.FightOpening;
        attackedEarly = false;
        swappedEarly = false;
        playerMoved = false;
        playerSwappedSkills = 0;
        enemyStatusScript.SetInvulnerability(true);
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(ConversationEndEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyCurrentHealthEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(TutorialSkillSwapEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(TutorialPlayerMovedEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(ConversationEndEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyCurrentHealthEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(TutorialSkillSwapEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(TutorialPlayerMovedEvent), this);
    }

    void Update() {
        if (doNothingTimer > 0f) {
            doNothingTimer -= Time.deltaTime;
        } else {
            if (currentSegment == TutorialSegment.FightOpening) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.MovementControls && playerMoved) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.SkillSwapControls && playerSwappedSkills > 2) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.DodgeDemo) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.StrongAttackDemo) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.AfterSkillExplaination) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.AfterHealthTutorial) {
                StartNextSegment();
            }
        }
    }

    private void StartNextSegment() {
        switch (this.currentSegment) {
            case TutorialSegment.FightOpening:
                // Go to Welcome dialog
                this.currentSegment = TutorialSegment.WelcomeDialog;
                DialogueManager.Instance.NextConversation();
                break;
            case TutorialSegment.WelcomeDialog:
                // Go to movement demo
                this.currentSegment = TutorialSegment.MovementControls;
                this.doNothingTimer = 4f;
                break;
            case TutorialSegment.MovementControls:
                // Go to normal attack dialog
                if (!attackedEarly) {
                    this.currentSegment = TutorialSegment.AttackDialog;
                    DialogueManager.Instance.NextConversation();
                } else {
                // Go to alt attack dialog
                    this.currentSegment = TutorialSegment.AttackDialogAlt;
                    DialogueManager.Instance.SelectConversation(9);
                }
                break;
            case TutorialSegment.AttackDialog:
                // Wait for an attack
                this.currentSegment = TutorialSegment.AttackControls;
                break;
            case TutorialSegment.AttackDialogAlt:
                // Wait for an attack
                this.currentSegment = TutorialSegment.AttackControls;
                break;
            case TutorialSegment.AttackControls:
                // Go to first skill dialog
                if (!(attackedEarly && swappedEarly)) {
                    this.currentSegment = TutorialSegment.SkillSwap;
                    DialogueManager.Instance.SelectConversation(2);
                    DialogueManager.Instance.SetConversationCount(3);
                } else {
                // Go to alt route
                    this.currentSegment = TutorialSegment.SkillSwapAlt;
                    DialogueManager.Instance.SelectConversation(10);
                }
                break;
            case TutorialSegment.SkillSwap:
                // Wait for them to mess around
                this.currentSegment = TutorialSegment.SkillSwapControls;
                doNothingTimer = 3f;
                break;
            case TutorialSegment.SkillSwapControls:
                // Go to skill explainations
                this.currentSegment = TutorialSegment.SkillExplaination;
                DialogueManager.Instance.NextConversation();
                break;
            case TutorialSegment.SkillSwapAlt:
                // Just go to the 2nd phase now.
                this.currentSegment = TutorialSegment.PhaseTutorialDemo;
                knifeDummyScript.KnifeTossDemo();
                break;
            case TutorialSegment.SkillExplaination:
                // Slight break between dialog
                this.currentSegment = TutorialSegment.AfterSkillExplaination;
                doNothingTimer = 0.1f;
                break;
            case TutorialSegment.AfterSkillExplaination:
                // Go to dodge dialog
                this.currentSegment = TutorialSegment.DodgeDialog;
                DialogueManager.Instance.NextConversation();
                break;
            case TutorialSegment.DodgeDialog:
                // Let them dodge the attack
                this.currentSegment = TutorialSegment.DodgeDemo;
                knifeDummyScript.KnifeTossDemo();
                doNothingTimer = 6f;
                break;
            case TutorialSegment.DodgeDemo:
                // Go to phase tutorial
                this.currentSegment = TutorialSegment.PhaseTutorial;
                DialogueManager.Instance.NextConversation();
                break;
            case TutorialSegment.PhaseTutorial:
                // Wait to be hit
                this.currentSegment = TutorialSegment.PhaseTutorialDemo;
                enemyStatusScript.SetInvulnerability(false);
                break;
            case TutorialSegment.PhaseTutorialDemo:
                // After being hit to 2nd phase, scream
                this.currentSegment = TutorialSegment.PhaseTutorialTwo;
                DialogueManager.Instance.SelectConversation(6);
                DialogueManager.Instance.SetConversationCount(7);
                break;
            case TutorialSegment.PhaseTutorialTwo:
                // Use the Stronger attack
                this.currentSegment = TutorialSegment.StrongAttackDemo;
                knifeDummyScript.JackTheRipperDemo();
                doNothingTimer = 10f;
                break;
            case TutorialSegment.StrongAttackDemo:
                this.currentSegment = TutorialSegment.HealthTutorial;
                DialogueManager.Instance.NextConversation();
                break;
            case TutorialSegment.HealthTutorial:
                // Separation between two dialogs
                this.currentSegment = TutorialSegment.AfterHealthTutorial;
                doNothingTimer = 0.01f;
                break;
            case TutorialSegment.AfterHealthTutorial:
                this.currentSegment = TutorialSegment.Closing;
                DialogueManager.Instance.NextConversation();
                break;
            case TutorialSegment.Closing:
                SelfDestruct();
                break;
        }
    }

    private void SelfDestruct() {
        EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
        enemyStatus.TakeDamage(1000);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(ConversationEndEvent)) {
            if (currentSegment == TutorialSegment.WelcomeDialog) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.AttackDialog
                       || currentSegment == TutorialSegment.AttackDialogAlt) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.SkillSwap) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.SkillExplaination) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.DodgeDialog) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.PhaseTutorial) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.PhaseTutorialTwo) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.HealthTutorial) {
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.Closing) {
                StartNextSegment();
            }
        } else if (e.GetType() == typeof(EnemyCurrentHealthEvent)) {
            if (currentSegment == TutorialSegment.MovementControls) {
                attackedEarly = true;
                StartNextSegment();
            } else if (currentSegment == TutorialSegment.AttackControls) {
                StartNextSegment();
            }
        } else if (e.GetType() == typeof(PhaseTransitionEvent)) {
            currentSegment = TutorialSegment.PhaseTutorialDemo;
            StartNextSegment();
        } else if (e.GetType() == typeof(TutorialPlayerMovedEvent)) {
            playerMoved = true;
        } else if (e.GetType() == typeof(TutorialSkillSwapEvent)) {
            if (currentSegment == TutorialSegment.AttackControls
                || currentSegment == TutorialSegment.AttackDialogAlt
                || currentSegment == TutorialSegment.WelcomeDialog
                || currentSegment == TutorialSegment.MovementControls) {
                swappedEarly = true;
            }
            playerSwappedSkills++;
        }
    }

}