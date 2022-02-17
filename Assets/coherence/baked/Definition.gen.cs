// Copyright (c) coherence ApS.
// See the license file in the project root for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using Coherence.ProtocolDef;
	using Coherence.Brook;
	using Coherence.Entity;
	using Coherence.Serializer;
	using System.Collections.Generic;

	public class Definition : IDefinition
	{
		public const string schemaId = "5c425b3841b12048850fedd9a0fcf2babb999a59";
		public const uint InternalWorldPosition = 0;
		public const uint InternalWorldOrientation = 1;
		public const uint InternalLocalUser = 2;
		public const uint InternalWorldPositionQuery = 3;
		public const uint InternalArchetypeComponent = 4;
		public const uint InternalPersistence = 5;
		public const uint InternalConnectedEntity = 6;
		public const uint InternalUniqueID = 7;
		public const uint InternalConnection = 8;
		public const uint InternalGlobal = 9;
		public const uint InternalGlobalQuery = 10;
		public const uint InternalTag = 11;
		public const uint InternalTagQuery = 12;
		public const uint InternalInputOwnerComponent = 13;
		public const uint InternalGenericPrefabReference = 14;
		public const uint InternalGenericScale = 15;
		public const uint InternalGenericFieldInt0 = 16;
		public const uint InternalGenericFieldInt1 = 17;
		public const uint InternalGenericFieldInt2 = 18;
		public const uint InternalGenericFieldInt3 = 19;
		public const uint InternalGenericFieldInt4 = 20;
		public const uint InternalGenericFieldInt5 = 21;
		public const uint InternalGenericFieldInt6 = 22;
		public const uint InternalGenericFieldInt7 = 23;
		public const uint InternalGenericFieldInt8 = 24;
		public const uint InternalGenericFieldInt9 = 25;
		public const uint InternalGenericFieldBool0 = 26;
		public const uint InternalGenericFieldBool1 = 27;
		public const uint InternalGenericFieldBool2 = 28;
		public const uint InternalGenericFieldBool3 = 29;
		public const uint InternalGenericFieldBool4 = 30;
		public const uint InternalGenericFieldBool5 = 31;
		public const uint InternalGenericFieldBool6 = 32;
		public const uint InternalGenericFieldBool7 = 33;
		public const uint InternalGenericFieldBool8 = 34;
		public const uint InternalGenericFieldBool9 = 35;
		public const uint InternalGenericFieldFloat0 = 36;
		public const uint InternalGenericFieldFloat1 = 37;
		public const uint InternalGenericFieldFloat2 = 38;
		public const uint InternalGenericFieldFloat3 = 39;
		public const uint InternalGenericFieldFloat4 = 40;
		public const uint InternalGenericFieldFloat5 = 41;
		public const uint InternalGenericFieldFloat6 = 42;
		public const uint InternalGenericFieldFloat7 = 43;
		public const uint InternalGenericFieldFloat8 = 44;
		public const uint InternalGenericFieldFloat9 = 45;
		public const uint InternalGenericFieldVector0 = 46;
		public const uint InternalGenericFieldVector1 = 47;
		public const uint InternalGenericFieldVector2 = 48;
		public const uint InternalGenericFieldVector3 = 49;
		public const uint InternalGenericField2dVector0 = 50;
		public const uint InternalGenericField2dVector1 = 51;
		public const uint InternalGenericField2dVector2 = 52;
		public const uint InternalGenericField2dVector3 = 53;
		public const uint InternalGenericFieldString0 = 54;
		public const uint InternalGenericFieldString1 = 55;
		public const uint InternalGenericFieldString2 = 56;
		public const uint InternalGenericFieldString3 = 57;
		public const uint InternalGenericFieldString4 = 58;
		public const uint InternalGenericFieldQuaternion0 = 59;
		public const uint InternalGenericFieldEntity0 = 60;
		public const uint InternalGenericFieldEntity1 = 61;
		public const uint InternalGenericFieldEntity2 = 62;
		public const uint InternalGenericFieldEntity3 = 63;
		public const uint InternalGenericFieldEntity4 = 64;
		public const uint InternalGenericFieldEntity5 = 65;
		public const uint InternalGenericFieldEntity6 = 66;
		public const uint InternalGenericFieldEntity7 = 67;
		public const uint InternalGenericFieldEntity8 = 68;
		public const uint InternalGenericFieldEntity9 = 69;
		public const uint InternalGenericFieldBytes0 = 70;
		public const uint InternalAuthorityTransfer = 0;
		public const uint InternalGenericCommand = 1;
		public const uint InternalTransferAction = 0;
		public const uint InternalTestCube = 0;

		private static readonly Dictionary<uint, string> componentNamesForTypeIds = new Dictionary<uint, string>() {
			{ 0, "WorldPosition" },
			{ 1, "WorldOrientation" },
			{ 2, "LocalUser" },
			{ 3, "WorldPositionQuery" },
			{ 4, "ArchetypeComponent" },
			{ 5, "Persistence" },
			{ 6, "ConnectedEntity" },
			{ 7, "UniqueID" },
			{ 8, "Connection" },
			{ 9, "Global" },
			{ 10, "GlobalQuery" },
			{ 11, "Tag" },
			{ 12, "TagQuery" },
			{ 13, "InputOwnerComponent" },
			{ 14, "GenericPrefabReference" },
			{ 15, "GenericScale" },
			{ 16, "GenericFieldInt0" },
			{ 17, "GenericFieldInt1" },
			{ 18, "GenericFieldInt2" },
			{ 19, "GenericFieldInt3" },
			{ 20, "GenericFieldInt4" },
			{ 21, "GenericFieldInt5" },
			{ 22, "GenericFieldInt6" },
			{ 23, "GenericFieldInt7" },
			{ 24, "GenericFieldInt8" },
			{ 25, "GenericFieldInt9" },
			{ 26, "GenericFieldBool0" },
			{ 27, "GenericFieldBool1" },
			{ 28, "GenericFieldBool2" },
			{ 29, "GenericFieldBool3" },
			{ 30, "GenericFieldBool4" },
			{ 31, "GenericFieldBool5" },
			{ 32, "GenericFieldBool6" },
			{ 33, "GenericFieldBool7" },
			{ 34, "GenericFieldBool8" },
			{ 35, "GenericFieldBool9" },
			{ 36, "GenericFieldFloat0" },
			{ 37, "GenericFieldFloat1" },
			{ 38, "GenericFieldFloat2" },
			{ 39, "GenericFieldFloat3" },
			{ 40, "GenericFieldFloat4" },
			{ 41, "GenericFieldFloat5" },
			{ 42, "GenericFieldFloat6" },
			{ 43, "GenericFieldFloat7" },
			{ 44, "GenericFieldFloat8" },
			{ 45, "GenericFieldFloat9" },
			{ 46, "GenericFieldVector0" },
			{ 47, "GenericFieldVector1" },
			{ 48, "GenericFieldVector2" },
			{ 49, "GenericFieldVector3" },
			{ 50, "GenericField2dVector0" },
			{ 51, "GenericField2dVector1" },
			{ 52, "GenericField2dVector2" },
			{ 53, "GenericField2dVector3" },
			{ 54, "GenericFieldString0" },
			{ 55, "GenericFieldString1" },
			{ 56, "GenericFieldString2" },
			{ 57, "GenericFieldString3" },
			{ 58, "GenericFieldString4" },
			{ 59, "GenericFieldQuaternion0" },
			{ 60, "GenericFieldEntity0" },
			{ 61, "GenericFieldEntity1" },
			{ 62, "GenericFieldEntity2" },
			{ 63, "GenericFieldEntity3" },
			{ 64, "GenericFieldEntity4" },
			{ 65, "GenericFieldEntity5" },
			{ 66, "GenericFieldEntity6" },
			{ 67, "GenericFieldEntity7" },
			{ 68, "GenericFieldEntity8" },
			{ 69, "GenericFieldEntity9" },
			{ 70, "GenericFieldBytes0" },
		};

		public static string ComponentNameForTypeId(uint typeId)
		{
			if(componentNamesForTypeIds.TryGetValue(typeId, out string componentName))
			{
				return componentName;
			}
			else
			{
				return "";
			}
		}

		public (ICoherenceComponentData, uint, uint?) ReadComponentUpdate(uint componentType, IInBitStream bitStream)
		{
			_ = Deserialize.ReadComponentOwnership(bitStream); // Read and discard unused bit from stream.
			var inProtocolStream = new InProtocolBitStream(bitStream);

			switch (componentType)
			{
				case InternalWorldPosition:
					return WorldPosition.Deserialize(inProtocolStream);
				case InternalWorldOrientation:
					return WorldOrientation.Deserialize(inProtocolStream);
				case InternalLocalUser:
					return LocalUser.Deserialize(inProtocolStream);
				case InternalWorldPositionQuery:
					return WorldPositionQuery.Deserialize(inProtocolStream);
				case InternalArchetypeComponent:
					return ArchetypeComponent.Deserialize(inProtocolStream);
				case InternalPersistence:
					return Persistence.Deserialize(inProtocolStream);
				case InternalConnectedEntity:
					return ConnectedEntity.Deserialize(inProtocolStream);
				case InternalUniqueID:
					return UniqueID.Deserialize(inProtocolStream);
				case InternalConnection:
					return Connection.Deserialize(inProtocolStream);
				case InternalGlobal:
					return Global.Deserialize(inProtocolStream);
				case InternalGlobalQuery:
					return GlobalQuery.Deserialize(inProtocolStream);
				case InternalTag:
					return Tag.Deserialize(inProtocolStream);
				case InternalTagQuery:
					return TagQuery.Deserialize(inProtocolStream);
				case InternalInputOwnerComponent:
					return InputOwnerComponent.Deserialize(inProtocolStream);
				case InternalGenericPrefabReference:
					return GenericPrefabReference.Deserialize(inProtocolStream);
				case InternalGenericScale:
					return GenericScale.Deserialize(inProtocolStream);
				case InternalGenericFieldInt0:
					return GenericFieldInt0.Deserialize(inProtocolStream);
				case InternalGenericFieldInt1:
					return GenericFieldInt1.Deserialize(inProtocolStream);
				case InternalGenericFieldInt2:
					return GenericFieldInt2.Deserialize(inProtocolStream);
				case InternalGenericFieldInt3:
					return GenericFieldInt3.Deserialize(inProtocolStream);
				case InternalGenericFieldInt4:
					return GenericFieldInt4.Deserialize(inProtocolStream);
				case InternalGenericFieldInt5:
					return GenericFieldInt5.Deserialize(inProtocolStream);
				case InternalGenericFieldInt6:
					return GenericFieldInt6.Deserialize(inProtocolStream);
				case InternalGenericFieldInt7:
					return GenericFieldInt7.Deserialize(inProtocolStream);
				case InternalGenericFieldInt8:
					return GenericFieldInt8.Deserialize(inProtocolStream);
				case InternalGenericFieldInt9:
					return GenericFieldInt9.Deserialize(inProtocolStream);
				case InternalGenericFieldBool0:
					return GenericFieldBool0.Deserialize(inProtocolStream);
				case InternalGenericFieldBool1:
					return GenericFieldBool1.Deserialize(inProtocolStream);
				case InternalGenericFieldBool2:
					return GenericFieldBool2.Deserialize(inProtocolStream);
				case InternalGenericFieldBool3:
					return GenericFieldBool3.Deserialize(inProtocolStream);
				case InternalGenericFieldBool4:
					return GenericFieldBool4.Deserialize(inProtocolStream);
				case InternalGenericFieldBool5:
					return GenericFieldBool5.Deserialize(inProtocolStream);
				case InternalGenericFieldBool6:
					return GenericFieldBool6.Deserialize(inProtocolStream);
				case InternalGenericFieldBool7:
					return GenericFieldBool7.Deserialize(inProtocolStream);
				case InternalGenericFieldBool8:
					return GenericFieldBool8.Deserialize(inProtocolStream);
				case InternalGenericFieldBool9:
					return GenericFieldBool9.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat0:
					return GenericFieldFloat0.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat1:
					return GenericFieldFloat1.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat2:
					return GenericFieldFloat2.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat3:
					return GenericFieldFloat3.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat4:
					return GenericFieldFloat4.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat5:
					return GenericFieldFloat5.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat6:
					return GenericFieldFloat6.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat7:
					return GenericFieldFloat7.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat8:
					return GenericFieldFloat8.Deserialize(inProtocolStream);
				case InternalGenericFieldFloat9:
					return GenericFieldFloat9.Deserialize(inProtocolStream);
				case InternalGenericFieldVector0:
					return GenericFieldVector0.Deserialize(inProtocolStream);
				case InternalGenericFieldVector1:
					return GenericFieldVector1.Deserialize(inProtocolStream);
				case InternalGenericFieldVector2:
					return GenericFieldVector2.Deserialize(inProtocolStream);
				case InternalGenericFieldVector3:
					return GenericFieldVector3.Deserialize(inProtocolStream);
				case InternalGenericField2dVector0:
					return GenericField2dVector0.Deserialize(inProtocolStream);
				case InternalGenericField2dVector1:
					return GenericField2dVector1.Deserialize(inProtocolStream);
				case InternalGenericField2dVector2:
					return GenericField2dVector2.Deserialize(inProtocolStream);
				case InternalGenericField2dVector3:
					return GenericField2dVector3.Deserialize(inProtocolStream);
				case InternalGenericFieldString0:
					return GenericFieldString0.Deserialize(inProtocolStream);
				case InternalGenericFieldString1:
					return GenericFieldString1.Deserialize(inProtocolStream);
				case InternalGenericFieldString2:
					return GenericFieldString2.Deserialize(inProtocolStream);
				case InternalGenericFieldString3:
					return GenericFieldString3.Deserialize(inProtocolStream);
				case InternalGenericFieldString4:
					return GenericFieldString4.Deserialize(inProtocolStream);
				case InternalGenericFieldQuaternion0:
					return GenericFieldQuaternion0.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity0:
					return GenericFieldEntity0.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity1:
					return GenericFieldEntity1.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity2:
					return GenericFieldEntity2.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity3:
					return GenericFieldEntity3.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity4:
					return GenericFieldEntity4.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity5:
					return GenericFieldEntity5.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity6:
					return GenericFieldEntity6.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity7:
					return GenericFieldEntity7.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity8:
					return GenericFieldEntity8.Deserialize(inProtocolStream);
				case InternalGenericFieldEntity9:
					return GenericFieldEntity9.Deserialize(inProtocolStream);
				case InternalGenericFieldBytes0:
					return GenericFieldBytes0.Deserialize(inProtocolStream);
				default:
					return (null, 0, 0);
			}
		}

		public void WriteComponentUpdate(ICoherenceComponentData data, uint mask, IOutProtocolBitStream protocolStream)
		{
			switch (data.GetComponentType())
			{
				case InternalWorldPosition:
					WorldPosition.Serialize((WorldPosition)data, mask, protocolStream);
					break;
				case InternalWorldOrientation:
					WorldOrientation.Serialize((WorldOrientation)data, mask, protocolStream);
					break;
				case InternalLocalUser:
					LocalUser.Serialize((LocalUser)data, mask, protocolStream);
					break;
				case InternalWorldPositionQuery:
					WorldPositionQuery.Serialize((WorldPositionQuery)data, mask, protocolStream);
					break;
				case InternalArchetypeComponent:
					ArchetypeComponent.Serialize((ArchetypeComponent)data, mask, protocolStream);
					break;
				case InternalPersistence:
					Persistence.Serialize((Persistence)data, mask, protocolStream);
					break;
				case InternalConnectedEntity:
					ConnectedEntity.Serialize((ConnectedEntity)data, mask, protocolStream);
					break;
				case InternalUniqueID:
					UniqueID.Serialize((UniqueID)data, mask, protocolStream);
					break;
				case InternalConnection:
					Connection.Serialize((Connection)data, mask, protocolStream);
					break;
				case InternalGlobal:
					Global.Serialize((Global)data, mask, protocolStream);
					break;
				case InternalGlobalQuery:
					GlobalQuery.Serialize((GlobalQuery)data, mask, protocolStream);
					break;
				case InternalTag:
					Tag.Serialize((Tag)data, mask, protocolStream);
					break;
				case InternalTagQuery:
					TagQuery.Serialize((TagQuery)data, mask, protocolStream);
					break;
				case InternalInputOwnerComponent:
					InputOwnerComponent.Serialize((InputOwnerComponent)data, mask, protocolStream);
					break;
				case InternalGenericPrefabReference:
					GenericPrefabReference.Serialize((GenericPrefabReference)data, mask, protocolStream);
					break;
				case InternalGenericScale:
					GenericScale.Serialize((GenericScale)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt0:
					GenericFieldInt0.Serialize((GenericFieldInt0)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt1:
					GenericFieldInt1.Serialize((GenericFieldInt1)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt2:
					GenericFieldInt2.Serialize((GenericFieldInt2)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt3:
					GenericFieldInt3.Serialize((GenericFieldInt3)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt4:
					GenericFieldInt4.Serialize((GenericFieldInt4)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt5:
					GenericFieldInt5.Serialize((GenericFieldInt5)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt6:
					GenericFieldInt6.Serialize((GenericFieldInt6)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt7:
					GenericFieldInt7.Serialize((GenericFieldInt7)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt8:
					GenericFieldInt8.Serialize((GenericFieldInt8)data, mask, protocolStream);
					break;
				case InternalGenericFieldInt9:
					GenericFieldInt9.Serialize((GenericFieldInt9)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool0:
					GenericFieldBool0.Serialize((GenericFieldBool0)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool1:
					GenericFieldBool1.Serialize((GenericFieldBool1)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool2:
					GenericFieldBool2.Serialize((GenericFieldBool2)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool3:
					GenericFieldBool3.Serialize((GenericFieldBool3)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool4:
					GenericFieldBool4.Serialize((GenericFieldBool4)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool5:
					GenericFieldBool5.Serialize((GenericFieldBool5)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool6:
					GenericFieldBool6.Serialize((GenericFieldBool6)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool7:
					GenericFieldBool7.Serialize((GenericFieldBool7)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool8:
					GenericFieldBool8.Serialize((GenericFieldBool8)data, mask, protocolStream);
					break;
				case InternalGenericFieldBool9:
					GenericFieldBool9.Serialize((GenericFieldBool9)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat0:
					GenericFieldFloat0.Serialize((GenericFieldFloat0)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat1:
					GenericFieldFloat1.Serialize((GenericFieldFloat1)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat2:
					GenericFieldFloat2.Serialize((GenericFieldFloat2)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat3:
					GenericFieldFloat3.Serialize((GenericFieldFloat3)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat4:
					GenericFieldFloat4.Serialize((GenericFieldFloat4)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat5:
					GenericFieldFloat5.Serialize((GenericFieldFloat5)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat6:
					GenericFieldFloat6.Serialize((GenericFieldFloat6)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat7:
					GenericFieldFloat7.Serialize((GenericFieldFloat7)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat8:
					GenericFieldFloat8.Serialize((GenericFieldFloat8)data, mask, protocolStream);
					break;
				case InternalGenericFieldFloat9:
					GenericFieldFloat9.Serialize((GenericFieldFloat9)data, mask, protocolStream);
					break;
				case InternalGenericFieldVector0:
					GenericFieldVector0.Serialize((GenericFieldVector0)data, mask, protocolStream);
					break;
				case InternalGenericFieldVector1:
					GenericFieldVector1.Serialize((GenericFieldVector1)data, mask, protocolStream);
					break;
				case InternalGenericFieldVector2:
					GenericFieldVector2.Serialize((GenericFieldVector2)data, mask, protocolStream);
					break;
				case InternalGenericFieldVector3:
					GenericFieldVector3.Serialize((GenericFieldVector3)data, mask, protocolStream);
					break;
				case InternalGenericField2dVector0:
					GenericField2dVector0.Serialize((GenericField2dVector0)data, mask, protocolStream);
					break;
				case InternalGenericField2dVector1:
					GenericField2dVector1.Serialize((GenericField2dVector1)data, mask, protocolStream);
					break;
				case InternalGenericField2dVector2:
					GenericField2dVector2.Serialize((GenericField2dVector2)data, mask, protocolStream);
					break;
				case InternalGenericField2dVector3:
					GenericField2dVector3.Serialize((GenericField2dVector3)data, mask, protocolStream);
					break;
				case InternalGenericFieldString0:
					GenericFieldString0.Serialize((GenericFieldString0)data, mask, protocolStream);
					break;
				case InternalGenericFieldString1:
					GenericFieldString1.Serialize((GenericFieldString1)data, mask, protocolStream);
					break;
				case InternalGenericFieldString2:
					GenericFieldString2.Serialize((GenericFieldString2)data, mask, protocolStream);
					break;
				case InternalGenericFieldString3:
					GenericFieldString3.Serialize((GenericFieldString3)data, mask, protocolStream);
					break;
				case InternalGenericFieldString4:
					GenericFieldString4.Serialize((GenericFieldString4)data, mask, protocolStream);
					break;
				case InternalGenericFieldQuaternion0:
					GenericFieldQuaternion0.Serialize((GenericFieldQuaternion0)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity0:
					GenericFieldEntity0.Serialize((GenericFieldEntity0)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity1:
					GenericFieldEntity1.Serialize((GenericFieldEntity1)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity2:
					GenericFieldEntity2.Serialize((GenericFieldEntity2)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity3:
					GenericFieldEntity3.Serialize((GenericFieldEntity3)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity4:
					GenericFieldEntity4.Serialize((GenericFieldEntity4)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity5:
					GenericFieldEntity5.Serialize((GenericFieldEntity5)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity6:
					GenericFieldEntity6.Serialize((GenericFieldEntity6)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity7:
					GenericFieldEntity7.Serialize((GenericFieldEntity7)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity8:
					GenericFieldEntity8.Serialize((GenericFieldEntity8)data, mask, protocolStream);
					break;
				case InternalGenericFieldEntity9:
					GenericFieldEntity9.Serialize((GenericFieldEntity9)data, mask, protocolStream);
					break;
				case InternalGenericFieldBytes0:
					GenericFieldBytes0.Serialize((GenericFieldBytes0)data, mask, protocolStream);
					break;
			}
		}

		private IEntityCommand ReadCommand(uint commandType, IInProtocolBitStream bitStream)
		{
			switch (commandType)
			{
				case Definition.InternalAuthorityTransfer:
					return AuthorityTransfer.Deserialize(bitStream);
				case Definition.InternalGenericCommand:
					return GenericCommand.Deserialize(bitStream);
				default:
					break;
			}

			return default;
		}

		private IEntityEvent ReadEvent(uint eventType, IInProtocolBitStream bitStream)
		{
			switch (eventType)
			{
				case Definition.InternalTransferAction:
					return TransferAction.Deserialize(bitStream);
				default:
					break;
			}

			return default;
		}

		private IEntityInput ReadInput(uint inputType, IInProtocolBitStream bitStream)
		{
			switch (inputType)
			{
				case Definition.InternalTestCube:
					return TestCube.Deserialize(bitStream);
				default:
					break;
			}

			return default;
		}

		public (IEntityCommand command, SerializeEntityID entityId, MessageTarget messageTarget)[]
			ReadCommands(IInBitStream bitStream)
		{
			var numMessages = bitStream.ReadUint8();

			var commandData = new (IEntityCommand command, SerializeEntityID entityId, MessageTarget messageTarget)[numMessages];

			for (var i = 0; i < numMessages; i++)
			{
				commandData[i].entityId = DeserializerTools.DeserializeEntityID(bitStream);
				commandData[i].messageTarget = DeserializerTools.DeserializeMessageTarget(bitStream);
				var componentType = DeserializerTools.DeserializeComponentTypeID(bitStream);
				var inBitStream = new Coherence.Serializer.InProtocolBitStream(bitStream);
				commandData[i].command = ReadCommand(componentType, inBitStream);
			}

			return commandData;
		}

		public (IEntityEvent[], SerializeEntityID[]) ReadEvents(IInBitStream bitStream)
		{
			var numMessages = bitStream.ReadUint8();

			var ids = new SerializeEntityID[numMessages];
			var messages = new IEntityEvent[numMessages];

			for (var i = 0; i < numMessages; i++)
			{
				ids[i] = DeserializerTools.DeserializeEntityID(bitStream);
				_ = DeserializerTools.DeserializeMessageTarget(bitStream);
				var componentType = DeserializerTools.DeserializeComponentTypeID(bitStream);
				var inBitStream = new Coherence.Serializer.InProtocolBitStream(bitStream);
				messages[i] = ReadEvent(componentType, inBitStream);
			}

			return (messages, ids);
		}

		public (IEntityInput input, long frame, SerializeEntityID entityId)[] ReadInputs(IInBitStream bitStream)
		{
			var numMessages = bitStream.ReadUint8();

			var inputData = new (IEntityInput input, long frame, SerializeEntityID entityId)[numMessages];

			for (var i = 0; i < numMessages; i++)
			{
				inputData[i].entityId = DeserializerTools.DeserializeEntityID(bitStream);
				_ = DeserializerTools.DeserializeMessageTarget(bitStream);
				var componentType = DeserializerTools.DeserializeComponentTypeID(bitStream);
				var inBitStream = new Coherence.Serializer.InProtocolBitStream(bitStream);
				inputData[i].frame = (long)bitStream.ReadUint64();
				inputData[i].input = ReadInput(componentType, inBitStream);
			}

			return inputData;
		}

		public void WriteCommand(IEntityCommand data, uint commandType, IOutProtocolBitStream bitStream)
		{
			switch (commandType)
			{
				case Definition.InternalAuthorityTransfer:
					AuthorityTransfer.Serialize((AuthorityTransfer)data, bitStream);
					break;
				case Definition.InternalGenericCommand:
					GenericCommand.Serialize((GenericCommand)data, bitStream);
					break;
				default:
					break;
			}
		}

		public void WriteEvent(IEntityEvent data, uint eventType, IOutProtocolBitStream bitStream)
		{
			switch (eventType)
			{
				case Definition.InternalTransferAction:
					TransferAction.Serialize((TransferAction)data, bitStream);
					break;
				default:
					break;
			}
		}

		public void WriteInput(IEntityInput data, uint inputType, IOutProtocolBitStream bitStream)
		{
			var inputData = (InputData)data;
			bitStream.WriteInt64(inputData.Frame);

			switch (inputType)
			{
				case Definition.InternalTestCube:
					TestCube.Serialize((TestCube)inputData.Input, bitStream);
					break;
				default:
					break;
			}
		}

		public bool IsAuthorityRequest(IEntityCommand entityCommand)
		{
			return entityCommand.GetComponentType() == Definition.InternalAuthorityTransfer;
		}

		public ushort GetAuthorityRequestParticipant(IEntityCommand entityCommand)
		{
			var command = (AuthorityTransfer)entityCommand;
			return (ushort)command.participant;
		}

		public bool IsTransferAction(IEntityEvent entityEvent)
		{
			return entityEvent.GetComponentType() == Definition.InternalTransferAction;
		}

		public bool IsTransferActionAuthorized(IEntityEvent entityEvent)
		{
			var action = (TransferAction)entityEvent;
			return action.accepted;
		}

		public IEntityCommand CreateAuthorityTransferRequest(ushort participant, AuthorityMode mode)
		{
			return new AuthorityTransfer(participant, (int)mode);
		}

		public IEntityEvent CreateTransferAction(ushort participant, bool accepted)
		{
			return new TransferAction(participant, accepted);
		}

		public ICoherenceComponentData GeneratePersistenceData()
		{
			var persistence = new Persistence();

			return persistence;
		}

		public ICoherenceComponentData GenerateCoherenceUUIDData(string uuid)
		{
			var uniqueID = new UniqueID();
			uniqueID.uuid = uuid;

			return uniqueID;
		}

		public ICoherenceComponentData GenerateGlobalQueryComponent()
		{
			return new GlobalQuery();
		}

		public ICoherenceComponentData GenerateInputOwnerComponent(uint clientId)
		{
			return new InputOwnerComponent
			{
				clientId = (int)clientId
			};
		}

		public string ExtractCoherenceUUID(ICoherenceComponentData data)
		{
			var uniqueID = (UniqueID)data;
			return uniqueID.uuid;
		}

		public bool IsConnectedEntity(ICoherenceComponentData data)
		{
			return data.GetComponentType() == Definition.InternalConnectedEntity;
		}

		public SerializeEntityID ExtractConnectedEntityID(ICoherenceComponentData data)
		{
			var connectedEntity = (ConnectedEntity)data;

			return connectedEntity.value;
		}

		public string ExtractCoherenceTag(ICoherenceComponentData data)
		{
			var tag = (Tag)data;
			return tag.tag;
		}
	}
}