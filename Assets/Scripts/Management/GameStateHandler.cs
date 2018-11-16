using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EtherealArena.WorldMap;

public class GameStateHandler : MonoBehaviour, IEventListener {

    [SerializeField] int startingNode = 0;

    private static GameState state;

    void Awake() {
        if (state == null) {
            state = new GameState();
            state.SetCurrentNode(startingNode);
        }
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(WorldMapMovementEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(BossDefeatedEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(WorldMapMovementEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(BossDefeatedEvent), this);
    }

    public GameState GetState() {
        return state;
    }
    
    public int GetCurrentNode() {
        return state.GetCurrentNode();
    }
    
    public bool GetBossDefeated(EnemyType type, int phase) {
        return state.GetBossDefeated(type, phase);
    }
    
    public void SetBossDefeated(EnemyType type, int phase) {
        state.SetBossDefeated(type, phase);
    }

    public void SetCurrentNode(int newNode) {
        state.SetCurrentNode(newNode);
    }
            
    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(BossDefeatedEvent)) {
            BossDefeatedEvent bossDefeatedEvent = e as BossDefeatedEvent;
            state.SetBossDefeated(bossDefeatedEvent.type, bossDefeatedEvent.phase);
        }
    }
    
}
