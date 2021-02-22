﻿using Deptorygen2.Core.Entities;
using Deptorygen2.Core.Steps.Semanticses.Nodes;
using Microsoft.CodeAnalysis;

namespace Deptorygen2.Core.Interfaces
{
	internal interface IResolverSemantics
	{
		TypeNode ReturnType { get; }
		string MethodName { get; }
		Parameter[] Parameters { get; }
		Accessibility Accessibility { get; }
		Resolution[] Resolutions { get; }
		Hook[] Hooks { get; }
	}
}