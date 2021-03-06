﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceSharp.Circuits;
using SpiceSharp.Diagnostics;
using System.Numerics;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// This 
    /// </summary>
    public class AC : Simulation
    {
        /// <summary>
        /// Enumerations
        /// </summary>
        public enum StepTypes { Decade, Octave, Linear };

        /// <summary>
        /// Configuration for AC simulations
        /// </summary>
        public class Configuration : SimulationConfiguration
        {
            /// <summary>
            /// Gets or sets the maximum number of iterations for calculating the operating point
            /// </summary>
            public int DcMaxIterations { get; set; } = 50;

            /// <summary>
            /// Gets or sets the flag for keeping the operating point information
            /// </summary>
            public bool KeepOpInfo { get; set; } = false;
        }
        protected Configuration MyConfig { get { return (Configuration)Config; } }

        /// <summary>
        /// Gets or sets the number of steps
        /// </summary>
        public int NumberSteps { get; set; } = 10;

        /// <summary>
        /// Gets or sets the starting frequency
        /// </summary>
        public double StartFreq { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the stopping frequency
        /// </summary>
        public double StopFreq { get; set; } = 1.0e3;

        /// <summary>
        /// Gets or sets the type of step used
        /// </summary>
        public StepTypes StepType { get; set; } = StepTypes.Decade;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the simulation</param>
        /// <param name="config">The configuration</param>
        public AC(string name, Configuration config = null) 
            : base(name, config ?? new Configuration())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the simulation</param>
        /// <param name="type">The step type</param>
        /// <param name="num">The number of frequency points</param>
        /// <param name="start">The first frequency point</param>
        /// <param name="stop">The last frequency point</param>
        public AC(string name, StepTypes type, int num, double start, double stop)
            : base(name, new Configuration())
        {
            StartFreq = start;
            StopFreq = stop;
            NumberSteps = num;
            StepType = type;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="ckt">The circuit</param>
        public override void Execute(Circuit ckt)
        {
            var state = ckt.State;
            var cstate = state.Complex;

            double freq = 0.0, freqdelta = 0.0;
            int n = 0;

            // Calculate the step
            switch (StepType)
            {
                case StepTypes.Decade:
                    freqdelta = Math.Exp(Math.Log(10.0) / NumberSteps);
                    n = (int)Math.Floor(Math.Log(StopFreq / StartFreq) / Math.Log(freqdelta));
                    break;

                case StepTypes.Octave:
                    freqdelta = Math.Exp(Math.Log(2.0) / NumberSteps);
                    n = (int)Math.Floor(Math.Log(StopFreq / StartFreq) / Math.Log(freqdelta));
                    break;

                case StepTypes.Linear:
                    if (NumberSteps > 1)
                    {
                        freqdelta = (StopFreq - StartFreq) / (NumberSteps - 1);
                        n = NumberSteps;
                    }
                    else
                    {
                        freqdelta = double.PositiveInfinity;
                        n = 1;
                    }
                    break;

                default:
                    throw new CircuitException("Invalid step type");
            }

            // Calculate the operating point
            state.Complex.Laplace = 0.0;
            state.Domain = CircuitState.DomainTypes.Frequency;
            state.UseIC = false;
            state.UseDC = true;
            state.UseSmallSignal = false;
            this.Op(ckt, MyConfig.DcMaxIterations);

            // Load all in order to calculate the AC info for all devices
            state.UseDC = false;
            state.UseSmallSignal = true;
            foreach (var c in ckt.Components)
                c.Load(ckt);

            // Export operating point if requested
            if (MyConfig.KeepOpInfo)
                Export(ckt);

            // Calculate the AC solution
            state.UseDC = false;
            freq = StartFreq;

            // Sweep the frequency
            for (int i = 0; i < n; i++)
            {
                // Calculate the current frequency
                state.Complex.Laplace = new Complex(0.0, 2.0 * Circuit.CONSTPI * freq);

                // Solve
                this.AcIterate(ckt);

                // Export the timepoint
                Export(ckt);

                // Increment the frequency
                switch (StepType)
                {
                    case StepTypes.Decade:
                    case StepTypes.Octave:
                        freq = freq * freqdelta;
                        break;

                    case StepTypes.Linear:
                        freq = StartFreq + i * freqdelta;
                        break;
                }
            }
        }
    }
}
