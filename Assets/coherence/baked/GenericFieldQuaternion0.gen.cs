// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

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
	using Coherence.Brook;
	using Coherence.Toolkit;
	using UnityEngine;

	public struct GenericFieldQuaternion0 : ICoherenceComponentData
	{
		public Quaternion Value;

		public override string ToString()
		{
			return $"GenericFieldQuaternion0(Value: {Value})";
		}

		public uint GetComponentType() => Definition.InternalGenericFieldQuaternion0;

		public const int order = 0;

		public int GetComponentOrder() => order;

		public AbsoluteSimulationFrame Frame;

		private static readonly float _Value_Epsilon = 2.3283064365386963e-10f;

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (GenericFieldQuaternion0)data;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				Value = other.Value;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			uint mask = 0;
			var newData = (GenericFieldQuaternion0)data;

			if (Value.DiffersFrom(newData.Value, _Value_Epsilon)) {
				mask |= 0b00000000000000000000000000000001;
			}

			return mask;
		}

		public static void Serialize(GenericFieldQuaternion0 data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				bitStream.WriteQuaternion((data.Value.ToCoreQuaternion()), 32);
			}
			mask >>= 1;
		}

		public static (GenericFieldQuaternion0, uint, uint?) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new GenericFieldQuaternion0();
			if (bitStream.ReadMask())
			{
				val.Value = (bitStream.ReadQuaternion(32)).ToUnityQuaternion();
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask, null);
		}

		/// <summary>
		/// Resets byte array references to the local array instance that is kept in the lastSentData.
		/// If the array content has changed but remains of same length, the new content is copied into the local array instance.
		/// If the array length has changed, the array is cloned and overwrites the local instance.
		/// If the array has not changed, the reference is reset to the local array instance.
		/// Otherwise, changes to other fields on the component might cause the local array instance reference to become permanently lost.
		/// </summary>
		public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
		{
			var last = lastSent as GenericFieldQuaternion0?;
		}
	}
}