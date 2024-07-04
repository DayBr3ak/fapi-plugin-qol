using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BreakInfinity;
using FapiQolPlugin;
using UnityEngine;
using UnityEngine.EventSystems;

public class SweetPotatoesStatTracker : MonoBehaviour
{

    class Average {
        private List<BigDouble> gainValues = new List<BigDouble>();
        private List<float> timeValues = new List<float>();
        private float elapsed = 0.0f;
        public float samplingRate = 3.0f;
        public int maxListSize = 5;

        public BigDouble cachedAverage = BigDouble.Zero;

        public int SampleCount() {
            return gainValues.Count;
        }

        public void AddGain(BigDouble gain)
        {
            gainValues.Add(gain);
            timeValues.Add(Time.time);
            if (gainValues.Count > maxListSize) {
                gainValues.RemoveAt(0);
                timeValues.RemoveAt(0);
            }
        }

        public bool Update(BigDouble gain) {
            elapsed += Time.deltaTime;
            if (elapsed >= samplingRate)
            {
                AddGain(gain);
                CalculateAverageGainPerSecond();
                elapsed = 0f;
                return true;
            }
            return false;
        }

        public BigDouble CalculateAverageGainPerSecond()
        {
            BigDouble totalGain = BigDouble.Zero;
            float totalTime = 0f;

            for (int i = 1; i < gainValues.Count; i++)
            {
                BigDouble gain = gainValues[i] - gainValues[i - 1];
                float deltaTime = timeValues[i] - timeValues[i - 1];
                totalGain += gain;
                totalTime += deltaTime;
            }

            // BigDouble totalGain = gainValues[gainValues.Count - 1] - gainValues[0];
            // float totalTime = timeValues[gainValues.Count - 1] - timeValues[0];

            var avg =  totalTime > 0 ? totalGain / totalTime : BigDouble.Zero;
            cachedAverage = avg;
            return avg;
        }
    }

    private Average smallWindow;
    private Average largeWindow;

    void Start()
    {
        Plugin.StaticLogger.LogInfo("SweetPotatoesStatTracker attached and running.");
        smallWindow = new Average();
        largeWindow = new Average {
            maxListSize = 120,
            samplingRate = 30
        };

        // UpdateLabel();
    }

    private BigDouble getSwp() {
        try {
            return GameManager.i.PD.SweetPotatoes;
        } catch {
            return BigDouble.Zero;
        }
    }

    public void Update()
    {
        BigDouble gain = getSwp();
        if (smallWindow.Update(gain)) {
        }
        if (largeWindow.Update(gain)) {
        }
        UpdateLabel();
    }

    private void UpdateLabel() {
        // Calculate and print the average gain per second
        string label1 = Form.Numb(smallWindow.cachedAverage, 6, 3, 2, 3, 0);
        string label2 = Form.Numb(largeWindow.cachedAverage, 6, 3, 2, 3, 0);

        if (largeWindow.SampleCount() < 2) {
            label2 = label1;
        }

        Plugin.StaticLogger.LogInfo("Average Gain Per Second: " + label1 + "   " + label2);
        GameManager.i.SPM.Description.text = GameManager.i.COFS.CowFacShopDesc;
        GameManager.i.SPM.Description.text += $"\n (15 sec) SWP/s {label1}";
        GameManager.i.SPM.Description.text += $"\n (1 hour) SWP/s {label2}";
    }

}
