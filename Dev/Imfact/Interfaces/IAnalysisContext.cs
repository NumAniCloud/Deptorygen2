﻿using System.Collections.Generic;
using Imfact.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Imfact.Interfaces
{
	internal interface IAnalysisContext
	{
		Dictionary<TypeId, ConstructorRecord> Constructors { get; }
		ITypeSymbol? GetTypeSymbol(TypeSyntax syntax);
		IMethodSymbol? GetMethodSymbol(MethodDeclarationSyntax syntax);
		INamedTypeSymbol? GetNamedTypeSymbol(TypeDeclarationSyntax syntax);
		IPropertySymbol? GetPropertySymbol(PropertyDeclarationSyntax syntax);
	}

	internal record ConstructorRecord(TypeNode ClassType, Accessibility Accessibility, ParameterRecord[] Parameters);

	internal record ParameterRecord(TypeNode Type, string Name);
}
