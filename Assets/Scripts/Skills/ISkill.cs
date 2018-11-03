using UnityEngine;

public interface ISkill {

    void UseSkill(Transform transform);

    // Used for the Light Bow
    void ReleaseSkill();

    // Also for the Light Bow
    void AimSkill(Transform transform);

    // Again for the Light Bow. Inccured when the skill becomes the player's active skill
    void SetActiveSkill();
    
    // Lastly, for the Light Bow. Inccured when the skill is no longer the player's active skill
    void SetInactiveSkill();

}