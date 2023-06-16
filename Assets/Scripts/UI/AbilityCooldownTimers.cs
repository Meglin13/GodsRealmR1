using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class AbilityCooldownTimers : UIScript, ISingle
    {
        public static AbilityCooldownTimers Instance;
        public void Initialize()
        {
            Instance = this;
        }

        private Label SpecialTimer;
        private Label DistractTimer;
        private Label UltimateTimer;

        private Dictionary<SkillType, Label> Timers;

        internal override void OnBind()
        {
            Initialize();
            base.OnBind();

            SpecialTimer = root.Q<Label>("SpecialTimer");
            DistractTimer = root.Q<Label>("DistractTimer");
            UltimateTimer = root.Q<Label>("UltimateTimer");

            Timers = new Dictionary<SkillType, Label>()
            { 
                { SkillType.Special, SpecialTimer },
                { SkillType.Ultimate, UltimateTimer },
                { SkillType.Distract, DistractTimer }
            };
        }

        private void Update()
        {
            foreach (var item in Timers)
            {
                var skill = PartyManager.Instance.GetPlayer().EntityStats.SkillSet[item.Key];

                if (skill.Cooldown)
                {
                    item.Value.style.visibility = Visibility.Visible;
                    var cooldown = Math.Round((decimal)(skill.TimeStamp - Time.time), 1);
                    item.Value.text = cooldown.ToString(); 
                }
                else
                    item.Value.style.visibility = Visibility.Hidden;
            }
        }
    }
}