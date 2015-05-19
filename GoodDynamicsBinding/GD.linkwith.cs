using System;
using ObjCRuntime;

[assembly: LinkWith ("GD.a", LinkTarget.Arm64 | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator | LinkTarget.Simulator64)]
