using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PartySetupScript : UIScript
    {
        private List<EntityStats> Characters;

        private Button StartRunBT;

        internal override void OnBind()
        {
            base.OnBind();

            Characters = new List<EntityStats>();
            //Characters = Resources.LoadAll("ScriptableObjects/Character", typeof(DealerStats)).Cast<DealerStats>().ToList();

            StartRunBT = root.Q<Button>("StartBT");
            StartRunBT.clicked += StartRun;
        }

        private void StartRun()
        {
            if (Characters.Count == 0)
            {
                UIManager.Instance.ChangeScene("RunScene", gameObject);
            }
            else
            {
                List<CharacterScript> subListObjects = Resources.LoadAll("Prefabs/Characters", typeof(CharacterScript)).Cast<CharacterScript>().ToList();
                
                
                foreach (var item in subListObjects)
                {
                    if (true)
                    {

                    }
                }
            }
        }
    } 
}