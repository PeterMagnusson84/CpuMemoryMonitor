using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace CPUandMemoryMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Välkomstmeddelande
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.Speak("welcome to CPU and memory monitor");

            #region My performance counters
            //Denna visar nuvarande CPU användning i procent.
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();
            //Visar nuvarande tillgänligt minne.
            PerformanceCounter perfMemoryCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemoryCount.NextValue();
            //Visar hur länge systemet varit igång.
            PerformanceCounter perfuptimeCount = new PerformanceCounter("System", "System Up Time");
            perfuptimeCount.NextValue();
            #endregion

            TimeSpan upTimeSpan = TimeSpan.FromSeconds(perfuptimeCount.NextValue());
            string systemUptimeMessage = string.Format("Your system has been running for {0} days {1} hours {2} minutes {3} seconds",
                (int)upTimeSpan.TotalDays,
                (int)upTimeSpan.Hours,
                (int)upTimeSpan.Minutes,
                (int)upTimeSpan.Seconds
                );

            //Meddelar hur länge systemet har varit igång.
            synth.Speak(systemUptimeMessage);

            while (true)
            {
                int currentCpuValue = (int)perfCpuCount.NextValue();
                int currentMemoryValue = (int)perfMemoryCount.NextValue();

                //Varje sekund visas CPU användingen i procent
                Console.WriteLine("CPU användning: {0}%", (int)currentCpuValue);
                Console.WriteLine("Tillgänligt minne: {0}MB", (int)currentMemoryValue);
                

                if (currentCpuValue > 50)
                {
                    if (currentCpuValue > 90)
                    {
                        string vocaleCpuMessage = String.Format("Warning the current cpu load is {0} percent", currentCpuValue);
                        synth.Speak(vocaleCpuMessage);
                    }
                    else
                    {
                        string vocaleCpuMessage = String.Format("The current cpu load is {0} percent", currentCpuValue);
                        synth.Speak(vocaleCpuMessage);
                    }
                    
                }

                if (currentMemoryValue < 3024)
                {
                    string vocaleMemoryMessage = String.Format("You currently have {0} megabytes left available", currentMemoryValue);
                    synth.Speak(vocaleMemoryMessage);
                }

                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
