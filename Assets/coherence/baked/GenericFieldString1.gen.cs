// Copyright (c) coherence ApS.
// See the license file in the project root for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using Coherence.ProtocolDef;
	using Coherence.Serializer;
	using Coherence.SimulationFrame;
	using Coherence.Entity;
	using Coherence.Utils;
	using Coherence.Toolkit;
	using UnityEngine;
	using Unity.Collections;
	using Unity.Mathematics;

	public struct GenericFieldString1 : ICoherenceComponentData
	{
		public string name;

		public override string ToString()
		{
			return $"GenericFieldString1(name: {name})";
		}

		public uint GetComponentType() => Definition.InternalGenericFieldString1;

		public const int order = 0;

		public int GetComponentOrder() => order;

		public AbsoluteSimulationFrame Frame;


		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (GenericFieldString1)data;
			if ((mask & 0x01) != 0)
			{
				name = other.name;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			uint mask = 0;
			var newData = (GenericFieldString1)data;

			if (name.DiffersFrom(newData.name)) {
				mask |= 0b00000000000000000000000000000001;
			}

			return mask;
		}

		public static void Serialize(GenericFieldString1 data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				bitStream.WriteShortString(data.name);
			}
			mask >>= 1;
		}

		public static (GenericFieldString1, uint, uint?) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new GenericFieldString1();
			if (bitStream.ReadMask())
			{
				val.name = bitStream.ReadShortString();
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask, null);
		}
	}
}