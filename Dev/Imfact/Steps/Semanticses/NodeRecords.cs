﻿using System.Linq;
using System.Text;
using Imfact.Entities;
using Imfact.Steps.Semanticses.Interfaces;
using Imfact.Utilities;
using Microsoft.CodeAnalysis;

namespace Imfact.Steps.Semanticses
{
	internal record Factory(FactoryCommon Common,
		Delegation[] Delegations, Inheritance[] Inheritances, EntryResolver[] EntryResolvers)
		: FactoryCommon(Common);

	internal record Delegation(FactoryCommon Common, string PropertyName)
		: FactoryCommon(Common), IProbablyDisposable
	{
		public string MemberName => PropertyName;
	}

	internal record Inheritance(FactoryCommon Common, Parameter[] Parameters)
		: FactoryCommon(Common);

	internal record FactoryCommon(TypeNode Type,
		Resolver[] Resolvers,
		MultiResolver[] MultiResolvers) : IFactorySemantics;

	internal record Resolver(ResolverCommon Common, Resolution? ReturnTypeResolution)
		: ResolverCommon(Common)
	{
		public Resolution ActualResolution = Common.Resolutions
			.Append(ReturnTypeResolution)
			.FilterNull()
			.First();

		protected override bool PrintMembers(StringBuilder builder)
		{
			builder.Append($"{Accessibility} {ReturnType.Name} {MethodName}({Parameters.Select(x => x.ParameterName).Join(", ")})");
			return true;
		}
	}

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
		Hook[] Hooks) : IResolverSemantics
	{
		public override int GetHashCode()
		{
			var value = 1;
			value += (int) Accessibility * 3;
			value += ReturnType.Id.GetHashCode() * 7;
			value += MethodName.GetHashCode() * 19;
			value += Parameters.Length * 11;
			value += Resolutions.Length * 23;
			value += Hooks.Length * 2;
			return value;
		}
	}

	internal record Dependency(TypeNode TypeName, string FieldName) : IProbablyDisposable
	{
		public TypeNode Type => TypeName;
		public string MemberName => FieldName;
		public string ParameterName => FieldName.TrimStart("_".ToCharArray());
	}

	internal record Hook(TypeNode HookType, string FieldName) : IProbablyDisposable
	{
		public TypeNode Type => HookType;
		public string MemberName => FieldName;
	}

	internal record Parameter(TypeNode Type, string ParameterName);

	internal record Resolution(TypeNode TypeName,
		TypeNode[] Dependencies,
		DisposableType DisposableType);

	internal record EntryResolver(string MethodName,
		TypeNode ReturnType,
		Parameter[] Parameters,
		Accessibility Accessibility);

	internal interface IProbablyDisposable
	{
		TypeNode Type { get; }
		string MemberName { get; }
	}
}
