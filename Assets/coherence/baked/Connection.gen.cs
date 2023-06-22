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

	public struct Connection : ICoherenceComponentData
	{
		public uint id;
		public int type;

		public override string ToString()
		{
			return $"Connection(id: {id}, type: {type})";
		}

		public uint GetComponentType() => Definition.InternalConnection;

		public const int order = 0;

		public uint FieldsMask => 0b00000000000000000000000000000011;

		public int GetComponentOrder() => order;
		public bool IsSendOrdered() { return false; }

		public AbsoluteSimulationFrame Frame;
	
		private static readonly uint _id_Min = 0;
		private static readonly uint _id_Max = 2147483647;
		private static readonly int _type_Min = 0;
		private static readonly int _type_Max = 8;

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (Connection)data;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				id = other.id;
			}
			mask >>= 1;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				type = other.type;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");

		}

		public static uint Serialize(Connection data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				Coherence.Utils.Bounds.Check(data.id, _id_Min, _id_Max, "Connection.id");
				data.id = Coherence.Utils.Bounds.Clamp(data.id, _id_Min, _id_Max);
				var fieldValue = data.id;

				bitStream.WriteUIntegerRange(fieldValue, 31, 0);
			}
			mask >>= 1;
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				Coherence.Utils.Bounds.Check(data.type, _type_Min, _type_Max, "Connection.type");
				data.type = Coherence.Utils.Bounds.Clamp(data.type, _type_Min, _type_Max);
				var fieldValue = data.type;

				bitStream.WriteIntegerRange(fieldValue, 3, 0);
			}
			mask >>= 1;

			return mask;
		}

		public static (Connection, uint) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new Connection();
	
			if (bitStream.ReadMask())
			{
				val.id = bitStream.ReadUIntegerRange(31, 0);
				mask |= 0b00000000000000000000000000000001;
			}
			if (bitStream.ReadMask())
			{
				val.type = bitStream.ReadIntegerRange(3, 0);
				mask |= 0b00000000000000000000000000000010;
			}
			return (val, mask);
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
			var last = lastSent as Connection?;
	
		}
	}
}