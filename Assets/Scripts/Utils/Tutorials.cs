using System;
using TMPro;
using UnityEngine;

namespace UI.RuleEditor
{
    public class Tutorials : MonoBehaviour
    {
        public int tutorialNumber =1;

        public TextMeshProUGUI taskLabel;

        private void Start()
        {
            UpdateTaskLabel();
        }
        
        public void UpdateTaskLabel()
        {
            switch (tutorialNumber)
            {
                case 1:
                    taskLabel.text = "Change the color of the cube in blue";
                    break;
                case 2:
                    taskLabel.text =
                        "Record the action of hiding the cube";
                    break;
                case 3:
                    taskLabel.text = "Record the action of laser pointing the cube";
                    break;
                
            }
        }

    }
}