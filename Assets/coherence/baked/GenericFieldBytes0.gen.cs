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

	public struct GenericFieldBytes0 : ICoherenceComponentData
	{
		public byte[] bytes;

		public override string ToString()
		{
			return $"GenericFieldBytes0(bytes: {bytes})";
		}

		public uint GetComponentType() => Definition.InternalGenericFieldBytes0;

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
			var other = (GenericFieldBytes0)data;
			if ((mask & 0x01) != 0)
			{
				bytes = other.bytes;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			uint mask = 0;
			var newData = (GenericFieldBytes0)data;

			if (bytes.DiffersFrom(newData.bytes)) {
				mask |= 0b00000000000000000000000000000001;
			}

			return mask;
		}

		public static void Serialize(GenericFieldBytes0 data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				bitStream.WriteBytesList(data.bytes);
			}
			mask >>= 1;
		}

		public static (GenericFieldBytes0, uint, uint?) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new GenericFieldBytes0();
			if (bitStream.ReadMask())
			{
				val.bytes = bitStream.ReadBytesList();
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask, null);
		}
	}
}