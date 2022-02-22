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

	public struct GenericFieldVector1 : ICoherenceComponentData
	{
		public float3 Value;

		public override string ToString()
		{
			return $"GenericFieldVector1(Value: {Value})";
		}

		public uint GetComponentType() => Definition.InternalGenericFieldVector1;

		public const int order = 0;

		public int GetComponentOrder() => order;

		public AbsoluteSimulationFrame Frame;

		private static readonly float _Value_Min = -2400f;
		private static readonly float _Value_Max = 2400f;
		private static readonly float _Value_Epsilon = 0.000286102294921875f;

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (GenericFieldVector1)data;
			if ((mask & 0x01) != 0)
			{
				Value = other.Value;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			uint mask = 0;
			var newData = (GenericFieldVector1)data;

			if (Value.DiffersFrom(newData.Value, _Value_Epsilon)) {
				mask |= 0b00000000000000000000000000000001;
			}

			return mask;
		}

		public static void Serialize(GenericFieldVector1 data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				Coherence.Utils.Bounds.Check(data.Value.x, _Value_Min, _Value_Max, "GenericFieldVector1.Value.x");

				Coherence.Utils.Bounds.Check(data.Value.y, _Value_Min, _Value_Max, "GenericFieldVector1.Value.y");

				Coherence.Utils.Bounds.Check(data.Value.z, _Value_Min, _Value_Max, "GenericFieldVector1.Value.z");

				bitStream.WriteVector3f(CoherenceToUnityConverters.FromUnityfloat3(data.Value), 24, 2400);
			}
			mask >>= 1;
		}

		public static (GenericFieldVector1, uint, uint) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new GenericFieldVector1();
			if (bitStream.ReadMask())
			{
				val.Value = CoherenceToUnityConverters.ToUnityfloat3(bitStream.ReadVector3f(24, 2400));
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask, 0);
		}
	}
}