﻿using Deptorygen2.Core.Interfaces;
using Deptorygen2.Core.Steps.Semanticses.Interfaces;
using Microsoft.CodeAnalysis;

namespace Deptorygen2.Core.Steps.Semanticses.Nodes
{
	internal record Generation(
		string[] RequiredNamespaces, Factory Factory,
		Dependency[] Dependencies, DisposableInfo DisposableInfo);

	internal record Factory(FactoryCommon Common,
		Delegation[] Delegations, Inheritance[] Inheritances, EntryResolver[] EntryResolvers)
		: FactoryCommon(Common);

	internal record Delegation(FactoryCommon Common, string PropertyName) : FactoryCommon(Common);

	internal record Inheritance(FactoryCommon Common) : FactoryCommon(Common);

	internal record FactoryCommon(TypeNode Type,
		Resolver[] Resolvers,
		MultiResolver[] MultiResolvers) : IFactorySemantics;

	internal record Resolver(ResolverCommon Common, Resolution? ReturnTypeResolution)
		: ResolverCommon(Common);

	internal record MultiResolver(ResolverCommon Common)
		: ResolverCommon(Common)
	{
		public TypeNode ElementType => ReturnType.TypeArguments[0];
	}

	internal record ResolverCommon(
		Accessibility Accessibility,
		TypeNode ReturnType,
		string MethodName,
		Parameter[] Parameters,
		Resolution[] Resolutions,
		Hook[] Hooks) : IResolverSemantics;

	internal record Dependency(TypeNode TypeName, string FieldName);

	internal record Hook(TypeNode HookType, string FieldName);
}