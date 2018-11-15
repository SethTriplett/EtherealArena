using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLoadingAndTransitioning : MonoBehaviour, IEventListener {

    private readonly static string DUMMY_NAME = "Dummy";
    private readonly List<Conversation> conversationList = new List<Conversation>();
    private Message[] welcomeMessages = {
        new Message(DUMMY_NAME, "Welcome to the Ethereal Arena!"),
        new Message(DUMMY_NAME, "As you know, the Ethereal Arena is a tournament home to fighters striving to become champions."),
        new Message(DUMMY_NAME, "Anyway, let's start with some warm ups."),
        new Message(DUMMY_NAME, "Firstly, move around with the left stick or the arrow keys."),
    };
    private Message[] attackTutorialMessages = {
        new Message(DUMMY_NAME, "Ok, looking good."),
        new Message(DUMMY_NAME, "Next let's try some attacks."),
        new Message(DUMMY_NAME, "Use the A button or the Z key to attack me."),
    };
    
    private Message[] attackTutorialAltMessages = {
        new Message(DUMMY_NAME, "Hey now! I didn't say to hit me yet!"),
        new Message(DUMMY_NAME, "..."),
        new Message(DUMMY_NAME, "Ok, now you can hit me."),
    };
    
    private Message[] skillSwapTutorialMessages = {
        new Message(DUMMY_NAME, "Oof, good stuff."),
        new Message(DUMMY_NAME, "Looks like you know a couple different skills."),
        new Message(DUMMY_NAME, "Use the X button or the C key to try toggling skills."),
    };
    
    private Message[] skillSwapTutorialAltMessages = {
        new Message(DUMMY_NAME, "Alright, looks like you know what you're doing already."),
        new Message(DUMMY_NAME, "Blah blah, 3 skills, different strengths, yada yada."),
        new Message(DUMMY_NAME, "You get the point, so let's hurry up."),
    };
    
    private Message[] skillExplainationMessages = {
        new Message(DUMMY_NAME, "Each skill is good for different situations."),
        new Message(DUMMY_NAME, "Spirit Shot is a well-balanced, versitile skill good in most any situation."),
        new Message(DUMMY_NAME, "Flamethrower has a shorter range, but it really racks up damage if you burn enemies continuously."),
        new Message(DUMMY_NAME, "The Light Bow lets you charge up power to release it in a big burst."),
        new Message(DUMMY_NAME, "Remember, you can choose whichever one you want with the X button or the C key."),
    };

    private Message[] dodgeTutorialMessages = {
        new Message(DUMMY_NAME, "Don't go thinking you're the only one out there who wants to punch stuff."),
        new Message(DUMMY_NAME, "You're opponents will have plenty of attacks to keep you occupied."),
        new Message(DUMMY_NAME, "Here, try to dodge this attack."),
    };
    
    private Message[] phaseChangeTutorialMessages = {
        new Message(DUMMY_NAME, "Of course, you won't be facing down the same attacks every time."),
        new Message(DUMMY_NAME, "Opponents will start to show you their stronger attacks the longer you fight them."),
        new Message(DUMMY_NAME, "Hit me a bit more and I'll show you. I'll let you actually damage me now."),
    };
    
    private Message[] phaseChangeTwoMessages = {
        new Message(DUMMY_NAME, "UUOOOOOOOHHHHHH, I'M PUMPED UP NOW!"),
    };
    
    private Message[] healthTutorialMessages = {
        new Message(DUMMY_NAME, "Something like that."),
        new Message(DUMMY_NAME, "Keep an eye on your health. You can't last out there forever."),
    };
    
    private Message[] healthTutorialAltMessages = {
        new Message(DUMMY_NAME, "Something like that."),
        new Message(DUMMY_NAME, "Keep an eye open while fighting. No matter how hard things get, there's always a way through."),
    };

    private Message[] closingMessages = {
        new Message(DUMMY_NAME, "I think you're getting the hang of it now."),
        new Message(DUMMY_NAME, "Let's get you out there. Good luck."),
    };

    private int level;
    private int maxPhase;

    void Awake() {
        Conversation conversation1 = new Conversation(welcomeMessages);
        Conversation conversation2 = new Conversation(attackTutorialMessages);
        Conversation conversation3 = new Conversation(skillSwapTutorialMessages);
        Conversation conversation5 = new Conversation(skillExplainationMessages);
        Conversation conversation6 = new Conversation(dodgeTutorialMessages);
        Conversation conversation7 = new Conversation(phaseChangeTutorialMessages);
        Conversation conversation8 = new Conversation(phaseChangeTwoMessages);
        Conversation conversation9 = new Conversation(healthTutorialMessages);
        Conversation conversation10 = new Conversation(closingMessages);

        Conversation conversation11 = new Conversation(attackTutorialAltMessages);
        Conversation conversation12 = new Conversation(skillSwapTutorialAltMessages);
        Conversation conversation13 = new Conversation(healthTutorialAltMessages);

        conversationList.Add(conversation1);
        conversationList.Add(conversation2);
        conversationList.Add(conversation3);

        conversationList.Add(conversation5);
        conversationList.Add(conversation6);
        conversationList.Add(conversation7);
        conversationList.Add(conversation8);
        conversationList.Add(conversation9);
        conversationList.Add(conversation10);
        conversationList.Add(conversation11);
        conversationList.Add(conversation12);
        conversationList.Add(conversation13);
    }

    void Start() {
        DialogueManager.Instance.SetConversationList(conversationList);
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
    }
    
    public int GetPhaseMaxHP(int nextPhase, int level, int maxPhase) {
        float totalHealth = 505;
        if (nextPhase == 1) {
            if (maxPhase == 1) {
                return (int) totalHealth;
            } else if (maxPhase == 2) {
                return 5;
            } else if (maxPhase == 3) {
                return 10;
            }
        } else if (nextPhase == 2) {
            if (maxPhase == 2) {
                return (int) totalHealth - 5;
            } else if (maxPhase == 3) {
                return 25;
            }
        } else if (nextPhase == 3) {
            return (int) totalHealth - 35;
        }
        Debug.LogError("Wrong phase given.");
        return 0;
    }

    void SetStats(int phase, int level, int maxPhase) {
        EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
        KnifeDummy knifeDummy = GetComponent<KnifeDummy>();
        if (maxPhase > 0) {
            enemyStatus.SetMaxPhase(maxPhase);
        } else {
            if (level > 0 && level < 80) {
                enemyStatus.SetMaxPhase(1);
                maxPhase = 1;
            } else if (level == 0 || level < 90) {
                enemyStatus.SetMaxPhase(2);
                maxPhase = 2;
            } else {
                enemyStatus.SetMaxPhase(3);
                maxPhase = 3;
            }
            this.maxPhase = maxPhase;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        knifeDummy.SetPlayerTransform(player.transform);
        enemyStatus.maxHealth = GetPhaseMaxHP(phase, level, maxPhase);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent startingDataEvent = e as EnemyStartingDataEvent;
            if (startingDataEvent.level < 0) startingDataEvent.level = 0;
            SetStats(1, startingDataEvent.level, startingDataEvent.maxPhase);
            this.level = startingDataEvent.level;
            this.maxPhase = startingDataEvent.maxPhase;
        } else if (e.GetType() == typeof(PhaseTransitionEvent)) {
            PhaseTransitionEvent phaseTransitionEvent = e as PhaseTransitionEvent;
            SetStats(phaseTransitionEvent.nextPhase, level, maxPhase);
        }
    }

 
}
