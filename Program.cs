using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace CPU_Monitor
{
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        // 
        // THIS IS WHERE SHIT HAPPENS!
        //
        static void Main(string[] args)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(Environment.TickCount);
            Console.WriteLine("Welcome to this CPU and Memory Monitor");

            // Text before application starts running
            Console.WriteLine("This application was created by Diabloxx!");
            Console.WriteLine("Do not republish this application without any credits to creator");
            Console.WriteLine("System uptime {0}Days {1}Hours {2}Minutes {3}Seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            

            // List of messages that will be selected when CPU is running 100%
            List<string> cpuMaxedOutMessages = new List<string>();
            cpuMaxedOutMessages.Add("WARNING: Your CPU is gonna catch fire soon! You better slow down!");
            cpuMaxedOutMessages.Add("WARNING: Oh my god. You should consider turning off your computer!");
            cpuMaxedOutMessages.Add("WARNING: Stop downloading all that porn! Its maxing my performance!");
            cpuMaxedOutMessages.Add("WARNING: Your CPU is officially chasing Justin Bieber!");
            cpuMaxedOutMessages.Add("RED ALERT! RED ALERT! RED ALERT! RED ALERT! RED ALERT! I FARTED.");
            cpuMaxedOutMessages.Add("Your proccesor is probably AMD");

            // The dice! LIKE DND!
            Random rand = new Random(); 
            
            // This will talk to the user when the program is launched!
            synth.Speak("Welcome to CPU Monitor version 1. Created by Diabloxx");

            // This will pull the current CPU load in percentage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            // This will show the current available memory on the screen
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            // Gives the program availability to show Uptime of computer in Days, Hours, Minutes and Seconds.
            PerformanceCounter perfUpTimeCount = new PerformanceCounter("System", "System Up Time");
            perfUpTimeCount.NextValue();

            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUpTimeCount.NextValue());

            string systemUptimeMessage = string.Format("The current system uptime is {0} days {1} hours {2} minutes {3} seconds",
                (int)uptimeSpan.TotalDays,
                (int)uptimeSpan.Hours,
                (int)uptimeSpan.Minutes,
                (int)uptimeSpan.Seconds
                );

            // Tell the user what the current system uptime is
            Speak(systemUptimeMessage, VoiceGender.Female, 2);

            int speechSpeed = 1;

            // Infinate While Loop
            while (true)
            {   
                // Get the current performance counter values. 
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();

                // Every 1 second show CPU Usage
                Console.WriteLine("CPU Load        : {0}%", currentCpuPercentage);
                // Prints Available Memory Every 1 seconds
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);

                // Only tells me when CPU has reached 80 Percent load

                if (currentCpuPercentage > 80)
                {
                    if (currentCpuPercentage == 100)
                    {
                        if (speechSpeed < 5)
                        {
                            speechSpeed++;
                        }
                        string cpuLoadVocalMessage = cpuMaxedOutMessages[rand.Next(6)];
                        Speak(cpuLoadVocalMessage, VoiceGender.Female, speechSpeed);
                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The Current CPU load is {0} percent", currentCpuPercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Male, 5);
                    }
                }

                // Only tell me if memory is below 1 Gigabyte
                if (currentAvailableMemory < 1024)
                {
                    // Text to Speech of current CPU load and available memory
                    string memAvailableVocalMessage = String.Format("You currently have {0} megabytes of memory available", currentAvailableMemory);
                    Speak(memAvailableVocalMessage, VoiceGender.Male, 10);
                }

                Thread.Sleep(1000);
            } // end of loop 
        }
        /// <summary>
        /// Speaks with a selected voice 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>


        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }


        /// <summary>
        /// Speaks with a selected voice at selected speed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        /// <param name="rate"></param>
            public static void Speak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            Speak(message, voiceGender);
        }
    }
}
