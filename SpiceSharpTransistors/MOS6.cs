﻿using System;
using SpiceSharp.Circuits;
using SpiceSharp.Diagnostics;
using SpiceSharp.Parameters;
using SpiceSharp.Components.Transistors;
using System.Numerics;

namespace SpiceSharp.Components
{
    public class MOS6 : CircuitComponent
    {
        /// <summary>
        /// Gets or sets the device model
        /// </summary>
        public MOS6Model Model { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        [SpiceName("temp"), SpiceInfo("Instance temperature (in Kelvin)")]
        public Parameter<double> MOS6temp { get; } = new Parameter<double>();
        [SpiceName("w"), SpiceInfo("Width")]
        public Parameter<double> MOS6w { get; } = new Parameter<double>();
        [SpiceName("l"), SpiceInfo("Length")]
        public Parameter<double> MOS6l { get; } = new Parameter<double>();
        [SpiceName("as"), SpiceInfo("Source area")]
        public Parameter<double> MOS6sourceArea { get; } = new Parameter<double>();
        [SpiceName("ad"), SpiceInfo("Drain area")]
        public Parameter<double> MOS6drainArea { get; } = new Parameter<double>();
        [SpiceName("ps"), SpiceInfo("Source perimeter")]
        public Parameter<double> MOS6sourcePerimiter { get; } = new Parameter<double>();
        [SpiceName("pd"), SpiceInfo("Drain perimeter")]
        public Parameter<double> MOS6drainPerimiter { get; } = new Parameter<double>();
        [SpiceName("nrs"), SpiceInfo("Source squares")]
        public Parameter<double> MOS6sourceSquares { get; } = new Parameter<double>();
        [SpiceName("nrd"), SpiceInfo("Drain squares")]
        public Parameter<double> MOS6drainSquares { get; } = new Parameter<double>();
        [SpiceName("off"), SpiceInfo("Device initially off")]
        public bool MOS6off { get; set; }
        [SpiceName("icvbs"), SpiceInfo("Initial B-S voltage")]
        public Parameter<double> MOS6icVBS { get; } = new Parameter<double>();
        [SpiceName("icvds"), SpiceInfo("Initial D-S voltage")]
        public Parameter<double> MOS6icVDS { get; } = new Parameter<double>();
        [SpiceName("icvgs"), SpiceInfo("Initial G-S voltage")]
        public Parameter<double> MOS6icVGS { get; } = new Parameter<double>();
        [SpiceName("dnode"), SpiceInfo("Number of the drain node ")]
        public int MOS6dNode { get; private set; }
        [SpiceName("gnode"), SpiceInfo("Number of the gate node ")]
        public int MOS6gNode { get; private set; }
        [SpiceName("snode"), SpiceInfo("Number of the source node ")]
        public int MOS6sNode { get; private set; }
        [SpiceName("bnode"), SpiceInfo("Number of the node ")]
        public int MOS6bNode { get; private set; }
        [SpiceName("dnodeprime"), SpiceInfo("Number of int. drain node")]
        public int MOS6dNodePrime { get; private set; }
        [SpiceName("snodeprime"), SpiceInfo("Number of int. source node ")]
        public int MOS6sNodePrime { get; private set; }
        [SpiceName("sourceconductance"), SpiceInfo("Source conductance")]
        public double MOS6sourceConductance { get; private set; }
        [SpiceName("drainconductance"), SpiceInfo("Drain conductance")]
        public double MOS6drainConductance { get; private set; }
        [SpiceName("von"), SpiceInfo("Turn-on voltage")]
        public double MOS6von { get; private set; }
        [SpiceName("vdsat"), SpiceInfo("Saturation drain voltage")]
        public double MOS6vdsat { get; private set; }
        [SpiceName("sourcevcrit"), SpiceInfo("Critical source voltage")]
        public double MOS6sourceVcrit { get; private set; }
        [SpiceName("drainvcrit"), SpiceInfo("Critical drain voltage")]
        public double MOS6drainVcrit { get; private set; }
        [SpiceName("id"), SpiceName("cd"), SpiceInfo("Drain current")]
        public double MOS6cd { get; private set; }
        [SpiceName("ibs"), SpiceInfo("B-S junction capacitance")]
        public double MOS6cbs { get; private set; }
        [SpiceName("ibd"), SpiceInfo("B-D junction capacitance")]
        public double MOS6cbd { get; private set; }
        [SpiceName("gmbs"), SpiceInfo("Bulk-Source transconductance")]
        public double MOS6gmbs { get; private set; }
        [SpiceName("gm"), SpiceInfo("Transconductance")]
        public double MOS6gm { get; private set; }
        [SpiceName("gds"), SpiceInfo("Drain-Source conductance")]
        public double MOS6gds { get; private set; }
        [SpiceName("gbd"), SpiceInfo("Bulk-Drain conductance")]
        public double MOS6gbd { get; private set; }
        [SpiceName("gbs"), SpiceInfo("Bulk-Source conductance")]
        public double MOS6gbs { get; private set; }
        [SpiceName("cbd"), SpiceInfo("Bulk-Drain capacitance")]
        public double MOS6capbd { get; private set; }
        [SpiceName("cbs"), SpiceInfo("Bulk-Source capacitance")]
        public double MOS6capbs { get; private set; }
        [SpiceName("cbd0"), SpiceInfo("Zero-Bias B-D junction capacitance")]
        public double MOS6Cbd { get; private set; }
        [SpiceName("cbdsw0"), SpiceInfo(" ")]
        public double MOS6Cbdsw { get; private set; }
        [SpiceName("cbs0"), SpiceInfo("Zero-Bias B-S junction capacitance")]
        public double MOS6Cbs { get; private set; }
        [SpiceName("cbssw0"), SpiceInfo(" ")]
        public double MOS6Cbssw { get; private set; }

        /// <summary>
        /// Extra variables
        /// </summary>
        public double MOS6tKv { get; private set; }
        public double MOS6tKc { get; private set; }
        public double MOS6tSurfMob { get; private set; }
        public double MOS6tPhi { get; private set; }
        public double MOS6tVbi { get; private set; }
        public double MOS6tVto { get; private set; }
        public double MOS6tSatCur { get; private set; }
        public double MOS6tSatCurDens { get; private set; }
        public double MOS6tCbd { get; private set; }
        public double MOS6tCbs { get; private set; }
        public double MOS6tCj { get; private set; }
        public double MOS6tCjsw { get; private set; }
        public double MOS6tBulkPot { get; private set; }
        public double MOS6tDepCap { get; private set; }
        public double MOS6f2d { get; private set; }
        public double MOS6f3d { get; private set; }
        public double MOS6f4d { get; private set; }
        public double MOS6f2s { get; private set; }
        public double MOS6f3s { get; private set; }
        public double MOS6f4s { get; private set; }
        public double MOS6senPertFlag { get; private set; }
        public double MOS6mode { get; private set; }
        public double MOS6cgs { get; private set; }
        public double MOS6cgd { get; private set; }
        public double MOS6cgb { get; private set; }
        public int MOS6states { get; private set; }

        /// <summary>
        /// Constants
        /// </summary>
        private const int MOS6vbd = 0;
        private const int MOS6vbs = 1;
        private const int MOS6vgs = 2;
        private const int MOS6vds = 3;
        private const int MOS6capgs = 4;
        private const int MOS6qgs = 5;
        private const int MOS6cqgs = 6;
        private const int MOS6capgd = 7;
        private const int MOS6qgd = 8;
        private const int MOS6cqgd = 9;
        private const int MOS6capgb = 10;
        private const int MOS6qgb = 11;
        private const int MOS6cqgb = 12;
        private const int MOS6qbd = 13;
        private const int MOS6cqbd = 14;
        private const int MOS6qbs = 15;
        private const int MOS6cqbs = 16;
        private const double MAX_EXP_ARG = 709.0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the device</param>
        public MOS6(string name) : base(name, 4)
        {
        }

        /// <summary>
        /// Get the model
        /// </summary>
        /// <returns></returns>
        public override CircuitModel GetModel() => Model;

        /// <summary>
        /// Setup the device
        /// </summary>
        /// <param name="ckt">The circuit</param>
        public override void Setup(Circuit ckt)
        {
            // Allocate nodes
            var nodes = BindNodes(ckt);
            MOS6dNode = nodes[0].Index;
            MOS6gNode = nodes[1].Index;
            MOS6sNode = nodes[2].Index;
            MOS6bNode = nodes[3].Index;

            // Allocate states
            MOS6states = ckt.State.GetState(17);
            MOS6vdsat = 0;
            MOS6von = 0;

            /* allocate a chunk of the state vector */

            if ((Model.MOS6drainResistance != 0 ||
                (Model.MOS6sheetResistance != 0 &&
                MOS6drainSquares != 0)) &&
                MOS6dNodePrime == 0)
            {
                MOS6dNodePrime = CreateNode(ckt).Index;
            }
            else
            {
                MOS6dNodePrime = MOS6dNode;
            }

            if ((Model.MOS6sourceResistance != 0 ||
                (Model.MOS6sheetResistance != 0 &&
                MOS6sourceSquares != 0)) &&
                MOS6sNodePrime == 0)
            {
                MOS6sNodePrime = CreateNode(ckt).Index;
            }
            else
            {
                MOS6sNodePrime = MOS6sNode;
            }
            /* macro to make elements with built in test for out of memory */
        }

        /// <summary>
        /// Do temperature-dependent calculations
        /// </summary>
        /// <param name="ckt">The circuit</param>
        public override void Temperature(Circuit ckt)
        {
            double vt;
            double ratio;
            double fact2;
            double kt;
            double egfet;
            double arg;
            double pbfact;
            double ratio4;
            double phio;
            double pbo;
            double gmaold;
            double capfact;
            double gmanew;
            double czbd;
            double czbdsw;
            double sarg;
            double sargsw;
            double czbs;
            double czbssw;

            /* perform the parameter defaulting */
            if (!MOS6temp.Given)
                MOS6temp.Value = ckt.State.Temperature;
            vt = MOS6temp * Circuit.CONSTKoverQ;
            ratio = MOS6temp / Model.MOS6tnom;
            fact2 = MOS6temp / Circuit.CONSTRefTemp;
            kt = MOS6temp * Circuit.CONSTBoltz;
            egfet = 1.16 - (7.02e-4 * MOS6temp * MOS6temp) /
            (MOS6temp + 1108);
            arg = -egfet / (kt + kt) + 1.1150877 / (Circuit.CONSTBoltz * (Circuit.CONSTRefTemp + Circuit.CONSTRefTemp));
            pbfact = -2 * vt * (1.5 * Math.Log(fact2) + Circuit.CHARGE * arg);
            
            if (MOS6l - 2 * Model.MOS6latDiff <= 0)
                CircuitWarning.Warning(this, $"{Name}: effective channel length less than zero");
            ratio4 = ratio * Math.Sqrt(ratio);
            MOS6tKv = Model.MOS6kv;
            MOS6tKc = Model.MOS6kc / ratio4;
            MOS6tSurfMob = Model.MOS6surfaceMobility / ratio4;
            phio = (Model.MOS6phi - Model.pbfact1) / Model.fact1;
            MOS6tPhi = fact2 * phio + pbfact;
            MOS6tVbi = Model.MOS6vt0 - Model.MOS6type * (Model.MOS6gamma * Math.Sqrt(Model.MOS6phi))
                + .5 * (Model.egfet1 - egfet) + Model.MOS6type * .5 * (MOS6tPhi - Model.MOS6phi);
            MOS6tVto = MOS6tVbi + Model.MOS6type * Model.MOS6gamma * Math.Sqrt(MOS6tPhi);
            MOS6tSatCur = Model.MOS6jctSatCur * Math.Exp(-egfet / vt + Model.egfet1 / Model.vtnom);
            MOS6tSatCurDens = Model.MOS6jctSatCurDensity * Math.Exp(-egfet / vt + Model.egfet1 / Model.vtnom);
            pbo = (Model.MOS6bulkJctPotential - Model.pbfact1) / Model.fact1;
            gmaold = (Model.MOS6bulkJctPotential - pbo) / pbo;
            capfact = 1 / (1 + Model.MOS6bulkJctBotGradingCoeff * (4e-4 * (Model.MOS6tnom - Circuit.CONSTRefTemp) - gmaold));
            MOS6tCbd = Model.MOS6capBD * capfact;
            MOS6tCbs = Model.MOS6capBS * capfact;
            MOS6tCj = Model.MOS6bulkCapFactor * capfact;
            capfact = 1 / (1 + Model.MOS6bulkJctSideGradingCoeff * (4e-4 * (Model.MOS6tnom - Circuit.CONSTRefTemp) - gmaold));
            MOS6tCjsw = Model.MOS6sideWallCapFactor * capfact;
            MOS6tBulkPot = fact2 * pbo + pbfact;
            gmanew = (MOS6tBulkPot - pbo) / pbo;
            capfact = (1 + Model.MOS6bulkJctBotGradingCoeff * (4e-4 * (MOS6temp - Circuit.CONSTRefTemp) - gmanew));
            MOS6tCbd *= capfact;
            MOS6tCbs *= capfact;
            MOS6tCj *= capfact;
            capfact = (1 + Model.MOS6bulkJctSideGradingCoeff * (4e-4 * (MOS6temp - Circuit.CONSTRefTemp) - gmanew));
            MOS6tCjsw *= capfact;
            MOS6tDepCap = Model.MOS6fwdCapDepCoeff * MOS6tBulkPot;
            if ((MOS6tSatCurDens == 0) ||
                (MOS6drainArea == 0) ||
                (MOS6sourceArea == 0))
            {
                MOS6sourceVcrit = MOS6drainVcrit = vt * Math.Log(vt / (Circuit.CONSTroot2 * MOS6tSatCur));
            }
            else
            {
                MOS6drainVcrit = vt * Math.Log(vt / (Circuit.CONSTroot2 * MOS6tSatCurDens * MOS6drainArea));
                MOS6sourceVcrit = vt * Math.Log(vt / (Circuit.CONSTroot2 * MOS6tSatCurDens * MOS6sourceArea));
            }

            if (Model.MOS6capBD.Given)
            {
                czbd = MOS6tCbd;
            }
            else
            {
                if (Model.MOS6bulkCapFactor.Given)
                {
                    czbd = MOS6tCj * MOS6drainArea;
                }
                else
                {
                    czbd = 0;
                }
            }
            if (Model.MOS6sideWallCapFactor.Given)
            {
                czbdsw = MOS6tCjsw * MOS6drainPerimiter;
            }
            else
            {
                czbdsw = 0;
            }
            arg = 1 - Model.MOS6fwdCapDepCoeff;
            sarg = Math.Exp((-Model.MOS6bulkJctBotGradingCoeff) * Math.Log(arg));
            sargsw = Math.Exp((-Model.MOS6bulkJctSideGradingCoeff) * Math.Log(arg));
            MOS6Cbd = czbd;
            MOS6Cbdsw = czbdsw;
            MOS6f2d = czbd * (1 - Model.MOS6fwdCapDepCoeff *
                (1 + Model.MOS6bulkJctBotGradingCoeff)) * sarg / arg + czbdsw * (1 - Model.MOS6fwdCapDepCoeff *
                (1 + Model.MOS6bulkJctSideGradingCoeff)) * sargsw / arg;
            MOS6f3d = czbd * Model.MOS6bulkJctBotGradingCoeff * sarg / arg / MOS6tBulkPot
                + czbdsw * Model.MOS6bulkJctSideGradingCoeff * sargsw / arg / MOS6tBulkPot;
            MOS6f4d = czbd * MOS6tBulkPot * (1 - arg * sarg) / (1 - Model.MOS6bulkJctBotGradingCoeff)
                + czbdsw * MOS6tBulkPot * (1 - arg * sargsw) / (1 - Model.MOS6bulkJctSideGradingCoeff)
                - MOS6f3d / 2 * (MOS6tDepCap * MOS6tDepCap) - MOS6tDepCap * MOS6f2d;
            if (Model.MOS6capBS.Given)
            {
                czbs = MOS6tCbs;
            }
            else
            {
                if (Model.MOS6bulkCapFactor.Given)
                {
                    czbs = MOS6tCj * MOS6sourceArea;
                }
                else
                {
                    czbs = 0;
                }
            }
            if (Model.MOS6sideWallCapFactor.Given)
            {
                czbssw = MOS6tCjsw * MOS6sourcePerimiter;
            }
            else
            {
                czbssw = 0;
            }
            arg = 1 - Model.MOS6fwdCapDepCoeff;
            sarg = Math.Exp((-Model.MOS6bulkJctBotGradingCoeff) * Math.Log(arg));
            sargsw = Math.Exp((-Model.MOS6bulkJctSideGradingCoeff) * Math.Log(arg));
            MOS6Cbs = czbs;
            MOS6Cbssw = czbssw;
            MOS6f2s = czbs * (1 - Model.MOS6fwdCapDepCoeff *
                (1 + Model.MOS6bulkJctBotGradingCoeff)) * sarg / arg + czbssw * (1 - Model.MOS6fwdCapDepCoeff *
                (1 + Model.MOS6bulkJctSideGradingCoeff)) * sargsw / arg;
            MOS6f3s = czbs * Model.MOS6bulkJctBotGradingCoeff * sarg / arg / MOS6tBulkPot
                + czbssw * Model.MOS6bulkJctSideGradingCoeff * sargsw / arg / MOS6tBulkPot;
            MOS6f4s = czbs * MOS6tBulkPot * (1 - arg * sarg) / (1 - Model.MOS6bulkJctBotGradingCoeff)
                + czbssw * MOS6tBulkPot * (1 - arg * sargsw) / (1 - Model.MOS6bulkJctSideGradingCoeff)
                - MOS6f3s / 2 * (MOS6tDepCap * MOS6tDepCap) - MOS6tDepCap * MOS6f2s;

            if (Model.MOS6drainResistance.Given)
            {
                if (Model.MOS6drainResistance != 0)
                {
                    MOS6drainConductance = 1 / Model.MOS6drainResistance;
                }
                else
                {
                    MOS6drainConductance = 0;
                }
            }
            else if (Model.MOS6sheetResistance.Given)
            {
                if ((!MOS6drainSquares.Given) ||
                    (MOS6drainSquares == 0))
                {
                    MOS6drainSquares.Value = 1;
                }
                if (Model.MOS6sheetResistance != 0)
                {
                    MOS6drainConductance =
                    1 / (Model.MOS6sheetResistance * MOS6drainSquares);
                }
                else
                {
                    MOS6drainConductance = 0;
                }
            }
            else
            {
                MOS6drainConductance = 0;
            }
            if (Model.MOS6sourceResistance.Given)
            {
                if (Model.MOS6sourceResistance != 0)
                {
                    MOS6sourceConductance = 1 / Model.MOS6sourceResistance;
                }
                else
                {
                    MOS6sourceConductance = 0;
                }
            }
            else if (Model.MOS6sheetResistance.Given)
            {
                if ((!MOS6sourceSquares.Given) ||
                (MOS6sourceSquares == 0))
                {
                    MOS6sourceSquares.Value = 1;
                }
                if (Model.MOS6sheetResistance != 0)
                {
                    MOS6sourceConductance =
                    1 / (Model.MOS6sheetResistance * MOS6sourceSquares);
                }
                else
                {
                    MOS6sourceConductance = 0;
                }
            }
            else
            {
                MOS6sourceConductance = 0;
            }
        }

        /// <summary>
        /// Load the device
        /// </summary>
        /// <param name="ckt">The circuit</param>
        public override void Load(Circuit ckt)
        {
            var state = ckt.State;
            var rstate = state.Real;
            var method = ckt.Method;
            double vt;
            int Check;
            double EffectiveLength;
            double DrainSatCur;
            double SourceSatCur;
            double GateSourceOverlapCap;
            double GateDrainOverlapCap;
            double GateBulkOverlapCap;
            double betac;
            double OxideCap;
            double vgs;
            double vds;
            double vbs;
            double vbd;
            double vgb;
            double vgd;
            double vgdo;
            double delvbs;
            double delvbd;
            double delvgs;
            double delvds;
            double delvgd;
            double cdhat;
            double cbhat;
            double von;
            double evbs;
            double evbd;
            double vbsvbd;
            double sarg;
            double vdshere;
            double vgon;
            double vdsat;
            double cdrain;
            double arg;
            double vonbm = 0.0;
            double idsat;
            double lambda;
            double vdst;
            double vdst2;
            double vdstg;
            double ivdst1;
            double sargsw;
            double vgs1;
            double vgd1;
            double vgb1;
            double capgs = 0.0;
            double capgd = 0.0;
            double capgb = 0.0;
            double gcgs;
            double ceqgs;
            double gcgd;
            double ceqgd;
            double gcgb;
            double ceqgb;
            double ceqbs;
            double ceqbd;
            int xnrm;
            int xrev;
            double cdreq;

            vt = Circuit.CONSTKoverQ * MOS6temp;
            Check = 1;

            /*DETAILPROF*/

            /* first, we compute a few useful values - these could be
			* pre-computed, but for historical reasons are still done
			* here.  They may be moved at the expense of instance size
			*/

            EffectiveLength = MOS6l - 2 * Model.MOS6latDiff;
            if ((MOS6tSatCurDens == 0) ||
            (MOS6drainArea == 0) ||
            (MOS6sourceArea == 0))
            {
                DrainSatCur = MOS6tSatCur;
                SourceSatCur = MOS6tSatCur;
            }
            else
            {
                DrainSatCur = MOS6tSatCurDens *
                MOS6drainArea;
                SourceSatCur = MOS6tSatCurDens *
                MOS6sourceArea;
            }
            GateSourceOverlapCap = Model.MOS6gateSourceOverlapCapFactor *
            MOS6w;
            GateDrainOverlapCap = Model.MOS6gateDrainOverlapCapFactor *
            MOS6w;
            GateBulkOverlapCap = Model.MOS6gateBulkOverlapCapFactor *
            EffectiveLength;
            betac = MOS6tKc * MOS6w / EffectiveLength;
            OxideCap = Model.MOS6oxideCapFactor * EffectiveLength *
            MOS6w;
            /* 
			* ok - now to do the start-up operations
			*
			* we must get values for vbs, vds, and vgs from somewhere
			* so we either predict them or recover them from last iteration
			* These are the two most common cases - either a prediction
			* step or the general iteration step and they
			* share some code, so we put them first - others later on
			*/
            
            if (state.Init == CircuitState.InitFlags.InitFloat || state.UseSmallSignal || (state.Init == CircuitState.InitFlags.InitFix && !MOS6off))
            {
                /* PREDICTOR */

                /* general iteration */

                vbs = Model.MOS6type * (
                rstate.OldSolution[MOS6bNode] -
                rstate.OldSolution[MOS6sNodePrime]);
                vgs = Model.MOS6type * (
                rstate.OldSolution[MOS6gNode] -
                rstate.OldSolution[MOS6sNodePrime]);
                vds = Model.MOS6type * (
                rstate.OldSolution[MOS6dNodePrime] -
                rstate.OldSolution[MOS6sNodePrime]);
                /* PREDICTOR */

                /* now some common crunching for some more useful quantities */

                vbd = vbs - vds;
                vgd = vgs - vds;
                vgdo = state.States[0][MOS6states + MOS6vgs] -
                state.States[0][MOS6states + MOS6vds];
                delvbs = vbs - state.States[0][MOS6states + MOS6vbs];
                delvbd = vbd - state.States[0][MOS6states + MOS6vbd];
                delvgs = vgs - state.States[0][MOS6states + MOS6vgs];
                delvds = vds - state.States[0][MOS6states + MOS6vds];
                delvgd = vgd - vgdo;

                /* these are needed for convergence testing */

                if (MOS6mode >= 0)
                {
                    cdhat =
                    MOS6cd -
                    MOS6gbd * delvbd +
                    MOS6gmbs * delvbs +
                    MOS6gm * delvgs +
                    MOS6gds * delvds;
                }
                else
                {
                    cdhat =
                    MOS6cd -
                    (MOS6gbd -
                    MOS6gmbs) * delvbd -
                    MOS6gm * delvgd +
                    MOS6gds * delvds;
                }
                cbhat =
                MOS6cbs +
                MOS6cbd +
                MOS6gbd * delvbd +
                MOS6gbs * delvbs;
                /*
				
				*/

                /*DETAILPROF*/
                /*NOBYPASS*/
                /*
				
				*/

                /*DETAILPROF*/
                /* ok - bypass is out, do it the hard way */

                von = Model.MOS6type * MOS6von;

                /* 
				* limiting
				*  we want to keep device voltages from changing
				* so fast that the exponentials churn out overflows
				* and similar rudeness
				*/

                if (state.States[0][MOS6states + MOS6vds] >= 0)
                {
                    vgs = Transistor.DEVfetlim(vgs, state.States[0][MOS6states + MOS6vgs]
                    , von);
                    vds = vgs - vgd;
                    vds = Transistor.DEVlimvds(vds, state.States[0][MOS6states + MOS6vds]);
                    vgd = vgs - vds;
                }
                else
                {
                    vgd = Transistor.DEVfetlim(vgd, vgdo, von);
                    vds = vgs - vgd;
                    vds = -Transistor.DEVlimvds(-vds, -(state.States[0][MOS6states + MOS6vds]));
                    vgs = vgd + vds;
                }
                if (vds >= 0)
                {
                    vbs = Transistor.DEVpnjlim(vbs, state.States[0][MOS6states + MOS6vbs],
                    vt, MOS6sourceVcrit, ref Check);
                    vbd = vbs - vds;
                }
                else
                {
                    vbd = Transistor.DEVpnjlim(vbd, state.States[0][MOS6states + MOS6vbd],
                    vt, MOS6drainVcrit, ref Check);
                    vbs = vbd + vds;
                }
                /*NODELIMITING*/
                /*
				
				*/

                /*DETAILPROF*/
            }
            else
            {

                /* ok - not one of the simple cases, so we have to
				* look at all of the possibilities for why we were
				* called.  We still just initialize the three voltages
				*/
                if (state.Init == CircuitState.InitFlags.InitJct && !MOS6off)
                {
                    vds = Model.MOS6type * MOS6icVDS;
                    vgs = Model.MOS6type * MOS6icVGS;
                    vbs = Model.MOS6type * MOS6icVBS;
                    if ((vds == 0) && (vgs == 0) && (vbs == 0) &&
                        (method != null || state.UseDC || state.Domain == CircuitState.DomainTypes.None || !state.UseIC))
                    {
                        vbs = -1;
                        vgs = Model.MOS6type * MOS6tVto;
                        vds = 0;
                    }
                }
                else
                {
                    vbs = vgs = vds = 0;
                }
            }
            /*
			
			*/

            /*DETAILPROF*/

            /*
			* now all the preliminaries are over - we can start doing the
			* real work
			*/
            vbd = vbs - vds;
            vgd = vgs - vds;
            vgb = vgs - vbs;

            /*
			* bulk-source and bulk-drain diodes
			*   here we just evaluate the ideal diode current and the
			*   corresponding derivative (conductance).
			*/
            if (vbs <= 0)
            {
                MOS6gbs = SourceSatCur / vt;
                MOS6cbs = MOS6gbs * vbs;
                MOS6gbs += state.Gmin;
            }
            else
            {
                evbs = Math.Exp(Math.Min(MAX_EXP_ARG, vbs / vt));
                MOS6gbs = SourceSatCur * evbs / vt + state.Gmin;
                MOS6cbs = SourceSatCur * (evbs - 1);
            }
            if (vbd <= 0)
            {
                MOS6gbd = DrainSatCur / vt;
                MOS6cbd = MOS6gbd * vbd;
                MOS6gbd += state.Gmin;
            }
            else
            {
                evbd = Math.Exp(Math.Min(MAX_EXP_ARG, vbd / vt));
                MOS6gbd = DrainSatCur * evbd / vt + state.Gmin;
                MOS6cbd = DrainSatCur * (evbd - 1);
            }

            /* now to determine whether the user was able to correctly
			* identify the source and drain of his device
			*/
            if (vds >= 0)
            {
                /* normal mode */
                MOS6mode = 1;
            }
            else
            {
                /* inverse mode */
                MOS6mode = -1;
            }
            /*
			
			*/

            /*DETAILPROF*/
            {
                /*
				*     this block of code evaluates the drain current and its 
				*     derivatives using the n-th power MOS model and the 
				*     charges associated with the gate, channel and bulk for 
				*     mosfets
				*
				*/

                vbsvbd = (MOS6mode == 1 ? vbs : vbd);
                if (vbsvbd <= 0)
                {
                    sarg = Math.Sqrt(MOS6tPhi - vbsvbd);
                }
                else
                {
                    sarg = Math.Sqrt(MOS6tPhi);
                    sarg = sarg - vbsvbd / (sarg + sarg);
                    sarg = Math.Max(0, sarg);
                }
                vdshere = vds * MOS6mode;
                von = (MOS6tVbi * Model.MOS6type) + Model.MOS6gamma * sarg
                - Model.MOS6gamma1 * vbsvbd;
                // -Model.MOS6sigma * vdshere;
                vgon = (MOS6mode == 1 ? vgs : vgd) - von;

                if (vgon <= 0)
                {
                    /*
					*     cutoff region
					*/
                    vdsat = 0;
                    cdrain = 0;
                    MOS6gm = 0;
                    MOS6gds = 0;
                    MOS6gmbs = 0;

                }
                else
                {
                    if (sarg <= 0)
                    {
                        arg = 0;
                    }
                    else
                    {
                        if ((MOS6mode == 1 ? vbs : vbd) <= 0)
                        {
                            vonbm = Model.MOS6gamma1
                            + Model.MOS6gamma / (sarg + sarg);
                        }
                        else
                        {
                            vonbm = Model.MOS6gamma1
                            + Model.MOS6gamma / 2 / Math.Sqrt(MOS6tPhi);
                        }
                    }
                    sarg = Math.Log(vgon);
                    vdsat = Model.MOS6kv * Math.Exp(sarg * Model.MOS6nv);
                    idsat = betac * Math.Exp(sarg * Model.MOS6nc);
                    lambda = Model.MOS6lamda0 - Model.MOS6lamda1 * vbsvbd;
                    /*
					*     saturation region
					*/
                    cdrain = idsat * (1 + lambda * vdshere);
                    MOS6gm = cdrain * Model.MOS6nc / vgon;
                    MOS6gds = MOS6gm * Model.MOS6sigma
                    + idsat * lambda;
                    MOS6gmbs = MOS6gm * vonbm
                    - idsat * Model.MOS6lamda1 * vdshere;
                    if (vdsat > vdshere)
                    {
                        /*
						*     linear region
						*/
                        vdst = vdshere / vdsat;
                        vdst2 = (2 - vdst) * vdst;
                        vdstg = -vdst * Model.MOS6nv / vgon;
                        ivdst1 = cdrain * (2 - vdst - vdst);
                        cdrain = cdrain * vdst2;
                        MOS6gm = MOS6gm * vdst2 + ivdst1 * vdstg;
                        MOS6gds = MOS6gds * vdst2 + ivdst1
                        * (1 / vdsat + vdstg * Model.MOS6sigma);
                        MOS6gmbs = MOS6gmbs * vdst2
                        + ivdst1 * vdstg * vonbm;
                    }
                }
                /*
				*     finished
				*/
            }
            /*
			
			*/

            /*DETAILPROF*/

            /* now deal with n vs p polarity */

            MOS6von = Model.MOS6type * von;
            MOS6vdsat = Model.MOS6type * vdsat;
            /* line 490 */
            /*
			*  COMPUTE EQUIVALENT DRAIN CURRENT SOURCE
			*/
            MOS6cd = MOS6mode * cdrain - MOS6cbd;

            if (state.Domain == CircuitState.DomainTypes.Time || state.UseSmallSignal)
            {
                /* 
				* now we do the hard part of the bulk-drain and bulk-source
				* diode - we evaluate the non-linear capacitance and
				* charge
				*
				* the basic equations are not hard, but the implementation
				* is somewhat long in an attempt to avoid log/exponential
				* evaluations
				*/
                /*
				*  charge storage elements
				*
				*.. bulk-drain and bulk-source depletion capacitances
				*/
                /*CAPBYPASS*/
                {
                    /* can't bypass the diode capacitance calculations */
                    /*CAPZEROBYPASS*/
                    if (vbs < MOS6tDepCap)
                    {
                        arg = 1 - vbs / MOS6tBulkPot;
                        /*
						* the following block looks somewhat long and messy,
						* but since most users use the default grading
						* coefficients of .5, and sqrt is MUCH faster than an
						* Math.Exp(Math.Log()) we use this special case code to buy time.
						* (as much as 10% of total job time!)
						*/
                        if (Model.MOS6bulkJctBotGradingCoeff ==
                        Model.MOS6bulkJctSideGradingCoeff)
                        {
                            if (Model.MOS6bulkJctBotGradingCoeff == .5)
                            {
                                sarg = sargsw = 1 / Math.Sqrt(arg);
                            }
                            else
                            {
                                sarg = sargsw =
                                Math.Exp(-Model.MOS6bulkJctBotGradingCoeff *
                                Math.Log(arg));
                            }
                        }
                        else
                        {
                            if (Model.MOS6bulkJctBotGradingCoeff == .5)
                            {
                                sarg = 1 / Math.Sqrt(arg);
                            }
                            else
                            {
                                /*NOSQRT*/
                                sarg = Math.Exp(-Model.MOS6bulkJctBotGradingCoeff *
                                Math.Log(arg));
                            }
                            if (Model.MOS6bulkJctSideGradingCoeff == .5)
                            {
                                sargsw = 1 / Math.Sqrt(arg);
                            }
                            else
                            {
                                /*NOSQRT*/
                                sargsw = Math.Exp(-Model.MOS6bulkJctSideGradingCoeff *
                                Math.Log(arg));
                            }
                        }
                        /*NOSQRT*/
                        state.States[0][MOS6states + MOS6qbs] =
                        MOS6tBulkPot * (MOS6Cbs *
                        (1 - arg * sarg) / (1 - Model.MOS6bulkJctBotGradingCoeff)
                        + MOS6Cbssw *
                        (1 - arg * sargsw) /
                        (1 - Model.MOS6bulkJctSideGradingCoeff));
                        MOS6capbs = MOS6Cbs * sarg +
                        MOS6Cbssw * sargsw;
                    }
                    else
                    {
                        state.States[0][MOS6states + MOS6qbs] = MOS6f4s +
                        vbs * (MOS6f2s + vbs * (MOS6f3s / 2));
                        MOS6capbs = MOS6f2s + MOS6f3s * vbs;
                    }
                    /*CAPZEROBYPASS*/
                }
                /*CAPBYPASS*/
                /* can't bypass the diode capacitance calculations */
                {
                    /*CAPZEROBYPASS*/
                    if (vbd < MOS6tDepCap)
                    {
                        arg = 1 - vbd / MOS6tBulkPot;
                        /*
						* the following block looks somewhat long and messy,
						* but since most users use the default grading
						* coefficients of .5, and sqrt is MUCH faster than an
						* Math.Exp(Math.Log()) we use this special case code to buy time.
						* (as much as 10% of total job time!)
						*/
                        if (Model.MOS6bulkJctBotGradingCoeff == .5 &&
                        Model.MOS6bulkJctSideGradingCoeff == .5)
                        {
                            sarg = sargsw = 1 / Math.Sqrt(arg);
                        }
                        else
                        {
                            if (Model.MOS6bulkJctBotGradingCoeff == .5)
                            {
                                sarg = 1 / Math.Sqrt(arg);
                            }
                            else
                            {
                                /*NOSQRT*/
                                sarg = Math.Exp(-Model.MOS6bulkJctBotGradingCoeff *
                                Math.Log(arg));
                            }
                            if (Model.MOS6bulkJctSideGradingCoeff == .5)
                            {
                                sargsw = 1 / Math.Sqrt(arg);
                            }
                            else
                            {
                                /*NOSQRT*/
                                sargsw = Math.Exp(-Model.MOS6bulkJctSideGradingCoeff *
                                Math.Log(arg));
                            }
                        }
                        /*NOSQRT*/
                        state.States[0][MOS6states + MOS6qbd] =
                        MOS6tBulkPot * (MOS6Cbd *
                        (1 - arg * sarg)
                        / (1 - Model.MOS6bulkJctBotGradingCoeff)
                        + MOS6Cbdsw *
                        (1 - arg * sargsw)
                        / (1 - Model.MOS6bulkJctSideGradingCoeff));
                        MOS6capbd = MOS6Cbd * sarg +
                        MOS6Cbdsw * sargsw;
                    }
                    else
                    {
                        state.States[0][MOS6states + MOS6qbd] = MOS6f4d +
                        vbd * (MOS6f2d + vbd * MOS6f3d / 2);
                        MOS6capbd = MOS6f2d + vbd * MOS6f3d;
                    }
                    /*CAPZEROBYPASS*/
                }
                /*
				
				*/

                /*DETAILPROF*/

                if (method != null)
                {
                    /* (above only excludes tranop, since we're only at this
					* point if tran or tranop )
					*/

                    /*
					*    calculate equivalent conductances and currents for
					*    depletion capacitors
					*/

                    /* integrate the capacitors and save results */
                    var result = method.Integrate(state, MOS6states + MOS6qbd, MOS6capbd);
                    MOS6gbd += result.Geq;
                    MOS6cbd += state.States[0][MOS6states + MOS6cqbd];
                    MOS6cd -= state.States[0][MOS6states + MOS6cqbd];
                    result = method.Integrate(state, MOS6states + MOS6qbs, MOS6capbs);
                    MOS6gbs += result.Geq;
                    MOS6cbs += state.States[0][MOS6states + MOS6cqbs];
                }
            }

            /*DETAILPROF*/

            /*
			*  check convergence
			*/
            if (!MOS6off || (state.Init != CircuitState.InitFlags.InitFix && !state.UseSmallSignal))
            {
                if (Check == 1)
                    state.IsCon = false;
            }
            /*
			
			*/

            /*DETAILPROF*/

            /* save things away for next time */

            state.States[0][MOS6states + MOS6vbs] = vbs;
            state.States[0][MOS6states + MOS6vbd] = vbd;
            state.States[0][MOS6states + MOS6vgs] = vgs;
            state.States[0][MOS6states + MOS6vds] = vds;

            /*
			
			*/

            /*DETAILPROF*/
            /*
			 *     meyer's capacitor model
			 */
            if (state.Domain == CircuitState.DomainTypes.Time || state.UseSmallSignal)
            {
                /*
				*     calculate meyer's capacitors
				*/
                /* 
				* new cmeyer - this just evaluates at the current time,
				* expects you to remember values from previous time
				* returns 1/2 of non-constant portion of capacitance
				* you must add in the other half from previous time
				* and the constant part
				*/
                if (MOS6mode > 0)
                {
                    double icapgs, icapgd, icapgb;
                    Transistor.DEVqmeyer(vgs, vgd, vgb, von, vdsat, out icapgs, out icapgd, out icapgb, MOS6tPhi, OxideCap);
                    state.States[0][MOS6states + MOS6capgs] = icapgs;
                    state.States[0][MOS6states + MOS6capgd] = icapgd;
                    state.States[0][MOS6states + MOS6capgb] = icapgb;
                }
                else
                {
                    double icapgs, icapgd, icapgb;
                    Transistor.DEVqmeyer(vgd, vgs, vgb, von, vdsat, out icapgd, out icapgs, out icapgb, MOS6tPhi, OxideCap);
                    state.States[0][MOS6states + MOS6capgs] = icapgs;
                    state.States[0][MOS6states + MOS6capgd] = icapgd;
                    state.States[0][MOS6states + MOS6capgb] = icapgb;
                }
                vgs1 = state.States[1][MOS6states + MOS6vgs];
                vgd1 = vgs1 - state.States[1][MOS6states + MOS6vds];
                vgb1 = vgs1 - state.States[1][MOS6states + MOS6vbs];
                if ((state.Domain == CircuitState.DomainTypes.Time || state.UseDC) || state.UseSmallSignal)
                {
                    capgs = 2 * state.States[0][MOS6states + MOS6capgs] + GateSourceOverlapCap;
                    capgd = 2 * state.States[0][MOS6states + MOS6capgd] + GateDrainOverlapCap;
                    capgb = 2 * state.States[0][MOS6states + MOS6capgb] + GateBulkOverlapCap;
                }
                else
                {
                    capgs = (state.States[0][MOS6states + MOS6capgs] + state.States[1][MOS6states + MOS6capgs] + GateSourceOverlapCap);
                    capgd = (state.States[0][MOS6states + MOS6capgd] + state.States[1][MOS6states + MOS6capgd] + GateDrainOverlapCap);
                    capgb = (state.States[0][MOS6states + MOS6capgb] + state.States[1][MOS6states + MOS6capgb] + GateBulkOverlapCap);
                }

                /*DETAILPROF*/
                /*
				*     store small-signal parameters (for meyer's model)
				*  all parameters already stored, so done...
				*/

                /*PREDICTOR*/
                if (method != null)
                {
                    state.States[0][MOS6states + MOS6qgs] = (vgs - vgs1) * capgs + state.States[1][MOS6states + MOS6qgs];
                    state.States[0][MOS6states + MOS6qgd] = (vgd - vgd1) * capgd + state.States[1][MOS6states + MOS6qgd];
                    state.States[0][MOS6states + MOS6qgb] = (vgb - vgb1) * capgb + state.States[1][MOS6states + MOS6qgb];
                }
                else
                {
                    /* TRANOP only */
                    state.States[0][MOS6states + MOS6qgs] = vgs * capgs;
                    state.States[0][MOS6states + MOS6qgd] = vgd * capgd;
                    state.States[0][MOS6states + MOS6qgb] = vgb * capgb;
                }
                /*PREDICTOR*/
            }

            if (method == null || method.SavedTime == 0.0)
            {
                /*
				*  initialize to zero charge conductances 
				*  and current
				*/
                gcgs = 0;
                ceqgs = 0;
                gcgd = 0;
                ceqgd = 0;
                gcgb = 0;
                ceqgb = 0;
            }
            else
            {
                if (capgs == 0)
                    state.States[0][MOS6states + MOS6cqgs] = 0;
                if (capgd == 0)
                    state.States[0][MOS6states + MOS6cqgd] = 0;
                if (capgb == 0)
                    state.States[0][MOS6states + MOS6cqgb] = 0;

				// calculate equivalent conductances and currents for
				// meyer"s capacitors
                method.Integrate(state, out gcgs, out ceqgs, MOS6states + MOS6qgs, capgs);
                method.Integrate(state, out gcgd, out ceqgd, MOS6states + MOS6qgd, capgd);
                method.Integrate(state, out gcgb, out ceqgb, MOS6states + MOS6qgb, capgb);
                ceqgs = ceqgs - gcgs * vgs + method.Slope * state.States[0][MOS6states + MOS6qgs];
                ceqgd = ceqgd - gcgd * vgd + method.Slope * state.States[0][MOS6states + MOS6qgd];
                ceqgb = ceqgb - gcgb * vgb + method.Slope * state.States[0][MOS6states + MOS6qgb];
            }
            /*
			*     store charge storage info for meyer's cap in lx table
			*/

            /*
			*  load current vector
			*/
            ceqbs = Model.MOS6type * (MOS6cbs - (MOS6gbs - state.Gmin) * vbs);
            ceqbd = Model.MOS6type * (MOS6cbd - (MOS6gbd - state.Gmin) * vbd);
            if (MOS6mode >= 0)
            {
                xnrm = 1;
                xrev = 0;
                cdreq = Model.MOS6type * (cdrain - MOS6gds * vds - MOS6gm * vgs - MOS6gmbs * vbs);
            }
            else
            {
                xnrm = 0;
                xrev = 1;
                cdreq = -(Model.MOS6type) * (cdrain - MOS6gds * (-vds) - MOS6gm * vgd - MOS6gmbs * vbd);
            }

            // Rhs vector
            rstate.Solution[MOS6gNode] -= (Model.MOS6type * (ceqgs + ceqgb + ceqgd));
            rstate.Solution[MOS6bNode] -= (ceqbs + ceqbd - Model.MOS6type * ceqgb);
            rstate.Solution[MOS6dNodePrime] += (ceqbd - cdreq + Model.MOS6type * ceqgd);
            rstate.Solution[MOS6sNodePrime] += cdreq + ceqbs + Model.MOS6type * ceqgs;

            // load y matrix
            rstate.Matrix[MOS6dNode, MOS6dNode] += (MOS6drainConductance);
            rstate.Matrix[MOS6gNode, MOS6gNode] += ((gcgd + gcgs + gcgb));
            rstate.Matrix[MOS6sNode, MOS6sNode] += (MOS6sourceConductance);
            rstate.Matrix[MOS6bNode, MOS6bNode] += (MOS6gbd + MOS6gbs + gcgb);
            rstate.Matrix[MOS6dNodePrime, MOS6dNodePrime] += (MOS6drainConductance + MOS6gds + MOS6gbd + xrev * (MOS6gm + MOS6gmbs) + gcgd);
            rstate.Matrix[MOS6sNodePrime, MOS6sNodePrime] += (MOS6sourceConductance + MOS6gds + MOS6gbs + xnrm * (MOS6gm + MOS6gmbs) + gcgs);
            rstate.Matrix[MOS6dNode, MOS6dNodePrime] += (-MOS6drainConductance);
            rstate.Matrix[MOS6gNode, MOS6bNode] -= gcgb;
            rstate.Matrix[MOS6gNode, MOS6dNodePrime] -= gcgd;
            rstate.Matrix[MOS6gNode, MOS6sNodePrime] -= gcgs;
            rstate.Matrix[MOS6sNode, MOS6sNodePrime] += (-MOS6sourceConductance);
            rstate.Matrix[MOS6bNode, MOS6gNode] -= gcgb;
            rstate.Matrix[MOS6bNode, MOS6dNodePrime] -= MOS6gbd;
            rstate.Matrix[MOS6bNode, MOS6sNodePrime] -= MOS6gbs;
            rstate.Matrix[MOS6dNodePrime, MOS6dNode] += (-MOS6drainConductance);
            rstate.Matrix[MOS6dNodePrime, MOS6gNode] += ((xnrm - xrev) * MOS6gm - gcgd);
            rstate.Matrix[MOS6dNodePrime, MOS6bNode] += (-MOS6gbd + (xnrm - xrev) * MOS6gmbs);
            rstate.Matrix[MOS6dNodePrime, MOS6sNodePrime] += (-MOS6gds - xnrm * (MOS6gm + MOS6gmbs));
            rstate.Matrix[MOS6sNodePrime, MOS6gNode] += (-(xnrm - xrev) * MOS6gm - gcgs);
            rstate.Matrix[MOS6sNodePrime, MOS6sNode] += (-MOS6sourceConductance);
            rstate.Matrix[MOS6sNodePrime, MOS6bNode] += (-MOS6gbs - (xnrm - xrev) * MOS6gmbs);
            rstate.Matrix[MOS6sNodePrime, MOS6dNodePrime] += (-MOS6gds - xrev * (MOS6gm + MOS6gmbs));
        }
    }
}
