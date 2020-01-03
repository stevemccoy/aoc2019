using System;
using System.Collections.Generic;

namespace aoc2019
{
    public class AmplifierBank
    {
        private static int NumStages { get; } = 5;

        private readonly Computer[] amplifiers = new Computer[NumStages];

        public AmplifierBank(string program, List<int> phaseSettings)
        {
            for (var i = 0; i < NumStages; i++)
            {
                amplifiers[i] = new Computer();
                amplifiers[i].LoadProgram(program);
                amplifiers[i].InputQueue.Enqueue(phaseSettings[i]);
            }
        }

        public int Run(bool withFeedback)
        {
            var signal = 0;
            for (var i = 0; i < NumStages; i++)
            {
                amplifiers[i].InputQueue.Enqueue(signal);
                amplifiers[i].ExecuteProgram(0);
                signal = amplifiers[i].OutputQueue.Dequeue();
            }

            if (withFeedback)
            {
                bool done = false;
                while (!done)
                {
                    for (var i = 0; i < NumStages; i++)
                    {
                        amplifiers[i].InputQueue.Enqueue(signal);
                        var retcode = amplifiers[i].ResumeProgram();
                        if (retcode != 2)
                        {
                            done = true;
                        }

                        signal = amplifiers[i].OutputQueue.Dequeue();
                    }
                }
            }

            return signal;
        }
        public static string PhaseSettingToString(List<int> phaseSettings)
        {
            return String.Join(',', phaseSettings);
        }
    }
}
