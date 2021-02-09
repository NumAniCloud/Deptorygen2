﻿using Deptorygen2.Core.Utilities;

namespace Deptorygen2.Core
{
	internal record SourceCodeDefinition(string[] Usings, string Namespace,
		FactoryDefinition Factory);

	public record DependencyDefinition(TypeName FieldType, string FieldName);

	public record DelegationDefinition(TypeName PropertyType,
		string PropertyName);

	public record TypeInfo(string Namespace, string Name);

	public record ResolverParameterDefinition(TypeName Type, string Name);

	public record HookDefinition(TypeName HookClass);

	public record ResolutionDefinition(TypeName ResolutionType, string[] ConstructorArguments);
}
