﻿using TMPro;
using UnityEngine;

namespace UI.Hud
{
    public class PlayerWeaponHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text shotsCounter;
        
        public void SetShots(int left, int total)
        {
            if (left > 0)
                shotsCounter.text = $"{left}/{total}";
            else
                shotsCounter.text = "";
        }
    }
}