// Copyright (c) coherence ApS.
// See the license file in the project root for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using Coherence.ProtocolDef;
	using Coherence.Serializer;
	using UnityEngine;
	using Unity.Collections;
	using Unity.Mathematics;
	using Coherence.Entity;

	public struct GenericCommand : IEntityCommand
	{
		public string name;
		public int paramInt1;
		public int paramInt2;
		public int paramInt3;
		public int paramInt4;
		public float paramFloat1;
		public float paramFloat2;
		public float paramFloat3;
		public float paramFloat4;
		public bool paramBool1;
		public bool paramBool2;
		public bool paramBool3;
		public bool paramBool4;
		public SerializeEntityID paramEntity1;
		public SerializeEntityID paramEntity2;
		public SerializeEntityID paramEntity3;
		public SerializeEntityID paramEntity4;
		public string paramString;
		public byte[] paramBytes;

		public MessageTarget Routing => MessageTarget.All;
		public uint GetComponentType() => Definition.InternalGenericCommand;

		public GenericCommand
		(
			string dataname,
			int dataparamInt1,
			int dataparamInt2,
			int dataparamInt3,
			int dataparamInt4,
			float dataparamFloat1,
			float dataparamFloat2,
			float dataparamFloat3,
			float dataparamFloat4,
			bool dataparamBool1,
			bool dataparamBool2,
			bool dataparamBool3,
			bool dataparamBool4,
			SerializeEntityID dataparamEntity1,
			SerializeEntityID dataparamEntity2,
			SerializeEntityID dataparamEntity3,
			SerializeEntityID dataparamEntity4,
			string dataparamString,
			byte[] dataparamBytes
		)
		{
			name = dataname;
			paramInt1 = dataparamInt1;
			paramInt2 = dataparamInt2;
			paramInt3 = dataparamInt3;
			paramInt4 = dataparamInt4;
			paramFloat1 = dataparamFloat1;
			paramFloat2 = dataparamFloat2;
			paramFloat3 = dataparamFloat3;
			paramFloat4 = dataparamFloat4;
			paramBool1 = dataparamBool1;
			paramBool2 = dataparamBool2;
			paramBool3 = dataparamBool3;
			paramBool4 = dataparamBool4;
			paramEntity1 = dataparamEntity1;
			paramEntity2 = dataparamEntity2;
			paramEntity3 = dataparamEntity3;
			paramEntity4 = dataparamEntity4;
			paramString = dataparamString;
			paramBytes = dataparamBytes;
		}

		public static void Serialize(GenericCommand commandData, IOutProtocolBitStream bitStream)
		{
			bitStream.WriteShortString(commandData.name);
			bitStream.WriteIntegerRange(commandData.paramInt1, 15, -9999);
			bitStream.WriteIntegerRange(commandData.paramInt2, 15, -9999);
			bitStream.WriteIntegerRange(commandData.paramInt3, 15, -9999);
			bitStream.WriteIntegerRange(commandData.paramInt4, 15, -9999);
			var converted_paramFloat1 = CoherenceToUnityConverters.FromUnityfloat(commandData.paramFloat1);
			bitStream.WriteFixedPoint(converted_paramFloat1, 24, 2400);
			var converted_paramFloat2 = CoherenceToUnityConverters.FromUnityfloat(commandData.paramFloat2);
			bitStream.WriteFixedPoint(converted_paramFloat2, 24, 2400);
			var converted_paramFloat3 = CoherenceToUnityConverters.FromUnityfloat(commandData.paramFloat3);
			bitStream.WriteFixedPoint(converted_paramFloat3, 24, 2400);
			var converted_paramFloat4 = CoherenceToUnityConverters.FromUnityfloat(commandData.paramFloat4);
			bitStream.WriteFixedPoint(converted_paramFloat4, 24, 2400);
			bitStream.WriteBool(commandData.paramBool1);
			bitStream.WriteBool(commandData.paramBool2);
			bitStream.WriteBool(commandData.paramBool3);
			bitStream.WriteBool(commandData.paramBool4);
			bitStream.WriteEntity(commandData.paramEntity1);
			bitStream.WriteEntity(commandData.paramEntity2);
			bitStream.WriteEntity(commandData.paramEntity3);
			bitStream.WriteEntity(commandData.paramEntity4);
			bitStream.WriteShortString(commandData.paramString);
			bitStream.WriteBytesList(commandData.paramBytes);
		}

		public static GenericCommand Deserialize(IInProtocolBitStream bitStream)
		{
			var dataname = bitStream.ReadShortString();
			var dataparamInt1 = bitStream.ReadIntegerRange(15, -9999);
			var dataparamInt2 = bitStream.ReadIntegerRange(15, -9999);
			var dataparamInt3 = bitStream.ReadIntegerRange(15, -9999);
			var dataparamInt4 = bitStream.ReadIntegerRange(15, -9999);
			var converted_paramFloat1 = bitStream.ReadFixedPoint(24, 2400);
			var dataparamFloat1 = CoherenceToUnityConverters.ToUnityfloat(converted_paramFloat1);
			var converted_paramFloat2 = bitStream.ReadFixedPoint(24, 2400);
			var dataparamFloat2 = CoherenceToUnityConverters.ToUnityfloat(converted_paramFloat2);
			var converted_paramFloat3 = bitStream.ReadFixedPoint(24, 2400);
			var dataparamFloat3 = CoherenceToUnityConverters.ToUnityfloat(converted_paramFloat3);
			var converted_paramFloat4 = bitStream.ReadFixedPoint(24, 2400);
			var dataparamFloat4 = CoherenceToUnityConverters.ToUnityfloat(converted_paramFloat4);
			var dataparamBool1 = bitStream.ReadBool();
			var dataparamBool2 = bitStream.ReadBool();
			var dataparamBool3 = bitStream.ReadBool();
			var dataparamBool4 = bitStream.ReadBool();
			var dataparamEntity1 = bitStream.ReadEntity();
			var dataparamEntity2 = bitStream.ReadEntity();
			var dataparamEntity3 = bitStream.ReadEntity();
			var dataparamEntity4 = bitStream.ReadEntity();
			var dataparamString = bitStream.ReadShortString();
			var dataparamBytes = bitStream.ReadBytesList();

			return new GenericCommand
			(
				dataname,
				dataparamInt1,
				dataparamInt2,
				dataparamInt3,
				dataparamInt4,
				dataparamFloat1,
				dataparamFloat2,
				dataparamFloat3,
				dataparamFloat4,
				dataparamBool1,
				dataparamBool2,
				dataparamBool3,
				dataparamBool4,
				dataparamEntity1,
				dataparamEntity2,
				dataparamEntity3,
				dataparamEntity4,
				dataparamString,
				dataparamBytes
			){};
		}
	}
}