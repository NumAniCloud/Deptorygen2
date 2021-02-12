﻿using System;
using System.Linq;
using System.Text;
using Deptorygen2.Core.Steps.Creation;
using Deptorygen2.Core.Steps.Definitions.Syntaxes;
using NacHelpers.Extensions;

namespace Deptorygen2.Core.Steps.Writing
{
	internal class SourceCodeBuilder2
	{
		private readonly RootNode _root;
		private readonly ICreationAggregator _creation;

		public SourceCodeBuilder2(SourceTreeDefinition definition)
		{
			_root = definition.Root;
			_creation = definition.Creation;
		}

		public SourceFile Write()
		{
			var fileName = _root.Namespace.Class.Name + ".g.cs";
			var contents = Render();
			return new SourceFile(fileName, contents);
		}

		private string Render()
		{
			var builder = new StringBuilder();

			foreach (var usingNode in _root.Usings)
			{
				builder.AppendLine($"using {usingNode.Namespace};");
			}

			builder.AppendLine();

			AppendBlock(builder, $"namespace {_root.Namespace.Name}", inner =>
			{
				RenderClass(_root.Namespace.Class, inner);
			});

			return builder.ToString();
		}

		private void RenderClass(ClassNode @class, StringBuilder builder)
		{
			AppendBlock(builder, $"partial class {@class.Name}", inner =>
			{
				foreach (var field in @class.Fields)
				{
					inner.AppendLine($"private readonly {field.Type.Text} {field.Name};");
				}

				inner.AppendLine();

				RenderConstructor(@class.Constructor, inner);

				inner.AppendLine();

				foreach (var method in @class.Methods)
				{
					RenderMethod(method, inner);
				}
			});
		}

		private void RenderMethod(MethodNode method, StringBuilder builder)
		{
			var paramList = method.Parameters
				.Select(x => $"{x.Type.Text} {x.Name}")
				.Join(", ");

			var access = method.Accessibility.ToString().ToLower();
			var ret = method.ReturnType.Text;

			AppendBlock(builder, $"{access} partial {ret} {method.Name}({paramList})", inner =>
			{
				var given = method.Parameters.Select(x => new GivenParameter(x.Type.TypeName, x.Name))
					.ToArray();

				CreationRequest request = new(method.ReturnType.TypeName, given, true);

				inner.AppendLine(_creation.GetInjection(request));
			});
		}

		private void RenderConstructor(ConstructorNode ctor, StringBuilder builder)
		{
			var paramList = ctor.Parameters
				.Select(x => $"{x.Type.Text} {x.Name}")
				.Join(", ");

			AppendBlock(builder, $"public {ctor.Name}({paramList})", inner =>
			{
				foreach (var assignment in ctor.Assignments)
				{
					inner.AppendLine($"{assignment.Dest} = {assignment.Src};");
				}
			});
		}

		private void AppendBlock(StringBuilder builder, string header, Action<StringBuilder> build)
		{
			builder.AppendLine(header);
			builder.AppendLine("{");

			var innerBuilder = new StringBuilder();
			build(innerBuilder);

			builder.AppendLine(innerBuilder.ToString().Indent(1).TrimEnd());
			builder.AppendLine("}");
		}
	}
}
