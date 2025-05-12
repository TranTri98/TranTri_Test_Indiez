using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightJoystick : MonoBehaviour
{
    [SerializeField] private FixedJoystick rightJoystick; // aiming joystick
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private Transform playerModel;

    void Update()
    {
        if (GameManager.I.IsGameOver) return;
        Vector2 aimInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);

        if (aimInput.magnitude > 0.2f)
        {
            // Calculate aiming direction
            Vector3 direction = new Vector3(aimInput.x, 0, aimInput.y);
            playerModel.forward = direction;

            // Auto fire
            weaponSlot.OnFireButtonPressed(); // call fire directly from code
        }
    }
}
