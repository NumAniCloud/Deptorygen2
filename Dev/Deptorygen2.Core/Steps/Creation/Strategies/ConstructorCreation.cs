﻿using System.Collections.Generic;
using System.Linq;
using Deptorygen2.Core.Steps.Semanticses;
using Deptorygen2.Core.Utilities;
using NacHelpers.Extensions;
using Source = Deptorygen2.Core.Steps.Semanticses.ResolutionSemantics;

namespace Deptorygen2.Core.Steps.Creation.Strategies
{
	internal class ConstructorCreation : CreationMethodBase<Source>
	{
		public ConstructorCreation(GenerationSemantics semantics) : base(semantics)
		{
		}

		protected override string GetCreationCode(Source resolution, GivenParameter[] given,
			ICreationAggregator aggregator)
		{
			var request = new MultipleCreationRequest(
				resolution.Dependencies, given, false);
			return $"new {resolution.TypeName.Name}({GetArgList(request, aggregator)})";
		}

		protected override IEnumerable<Source> GetSource(GenerationSemantics semantics)
		{
			var rr = semantics.Factory.Resolvers.Select(x => x.ReturnTypeResolution).FilterNull();
			var rs = semantics.Factory.Resolvers.SelectMany(x => x.Resolutions);
			var crs = semantics.Factory.CollectionResolvers.SelectMany(x => x.Resolutions);
			return rr.Concat(rs).Concat(crs);
		}

		protected override TypeName GetTypeInfo(Source source)
		{
			return source.TypeName;
		}
	}
}