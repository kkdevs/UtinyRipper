﻿using UtinyRipper.AssetExporters;
using UtinyRipper.Exporter.YAML;

namespace UtinyRipper.Classes.ParticleSystems
{
	public class RotationBySpeedModule : ParticleSystemModule
	{
		/// <summary>
		/// 5.3.0 and greater
		/// </summary>
		public static bool IsReadAxes(Version version)
		{
			return version.IsGreaterEqual(5, 3);
		}

		private MinMaxCurve GetExportX(Version version)
		{
			return IsReadAxes(version) ? X : new MinMaxCurve(0.0f);
		}
		private MinMaxCurve GetExportY(Version version)
		{
			return IsReadAxes(version) ? Y : new MinMaxCurve(0.0f);
		}

		public override void Read(AssetStream stream)
		{
			base.Read(stream);

			if (IsReadAxes(stream.Version))
			{
				X.Read(stream);
				Y.Read(stream);
			}
			Curve.Read(stream);
			if (IsReadAxes(stream.Version))
			{
				SeparateAxes = stream.ReadBoolean();
				stream.AlignStream(AlignType.Align4);
			}
			
			Range.Read(stream);
		}

		public override YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = (YAMLMappingNode)base.ExportYAML(container);
			node.Add("x", GetExportX(container.Version).ExportYAML(container));
			node.Add("y", GetExportY(container.Version).ExportYAML(container));
			node.Add("curve", Curve.ExportYAML(container));
			node.Add("separateAxes", SeparateAxes);
			node.Add("range", Range.ExportYAML(container));
			return node;
		}

		public bool SeparateAxes { get; private set; }

		public MinMaxCurve X;
		public MinMaxCurve Y;
		public MinMaxCurve Curve;
		public Vector2f Range;
	}
}
