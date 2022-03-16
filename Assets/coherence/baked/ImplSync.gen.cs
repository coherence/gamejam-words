// Copyright (c) coherence ApS.
// See the license file in the project root for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Toolkit
{
	using Unity.Collections;
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using global::Coherence.Generated;
	using Coherence.Entity;
	using Coherence.ProtocolDef;
	using Log;
	using Logger = Log.Logger;

	public class CoherenceSyncImpl
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnRuntimeMethodLoad()
		{
			CoherenceSync.ComponentNameFromTypeId = ComponentNameFromTypeId;
			CoherenceSync.CreateInitialComponents = CreateInitialComponents;
			CoherenceSync.ReceiveGenericCommand = ReceiveGenericCommand;
			CoherenceSync.SendGenericCommand = SendGenericCommand;
			CoherenceSync.CreateConnectedEntityUpdateInternal = CreateConnectedEntityUpdateInternal;
			CoherenceSync.GetConnectedEntityComponentIdInternal = GetConnectedEntityComponentIdInternal;
			CoherenceSync.UpdateTag = UpdateTag;
			CoherenceSync.RemoveTag = RemoveTag;
		}

		private static string ComponentNameFromTypeId(uint componentTypeId)
		{
			var componentName = Definition.ComponentNameForTypeId(componentTypeId);

			if (string.IsNullOrEmpty(componentName))
			{
				throw new Exception($"Unhandled component type id: {componentTypeId}");
			}

			return componentName;
		}

		private static ICoherenceComponentData[] CreateInitialComponents(CoherenceSync self)
		{
			var comps = new List<ICoherenceComponentData>();
			comps.Add(new WorldPosition() { value = self.coherencePosition });

			if (self.SelectedSynchronizedPrefabOption == CoherenceSync.SynchronizedPrefabOptions.UsePrefabMapper)
			{
				PrefabId prefabId = PrefabMapper.instance.GetPrefabId(self.PrefabGuid);
				comps.Add(new GenericPrefabId() { value = (int)prefabId.Value });
			}
#if COHERENCE_DISABLE_NAME_BASED_PREFAB_LOADING
			else
			{
				Debug.LogWarning($"{self} is using SelectedSynchronizedPrefabOption '{self.SelectedSynchronizedPrefabOption}', but name-based prefab loading has been disabled in this project. No remote prefab will be instantiated.");
			}
#else
			else if (!string.IsNullOrEmpty(self.remoteVersionPrefabName))
			{
				comps.Add(new GenericPrefabReference() { prefab = self.remoteVersionPrefabName });
			}
			else
			{
				Debug.LogWarning($"{self} is missing remoteVersionPrefabName, SelectedSynchronizedPrefabOption={self.SelectedSynchronizedPrefabOption}");
			}
#endif

			if (!string.IsNullOrEmpty(self.coherenceUUID))
			{
				comps.Add(new UniqueID() { uuid = self.coherenceUUID });
			}

			if (self.lifetimeType != CoherenceSync.LifetimeType.SessionBased)
			{
				comps.Add(new Persistence());
			}

			return comps.ToArray();
		}

        private static void SendGenericCommand(CoherenceSync self, string commandName, MessageTarget messageTarget, byte[] data, SerializeEntityID[] entityIDs, Logger logger)
        {
            SerializeEntityID targetEntity = self.EntityID;

            if (messageTarget == MessageTarget.AuthorityOnly && self.Client.EntityIsOwned(targetEntity))
            {
                logger.Warning($"Can't send {MessageTarget.AuthorityOnly} command to entity that is owned. Command={commandName} EntityId={targetEntity}");
                return;
            }

            if (!self.Client.EntityExists(targetEntity))
            {
                logger.Warning($"Can't send command to entity that doesn't exist. Command={commandName} EntityId={targetEntity}");
                return;
            }

            if (entityIDs.Length > GenericNetworkCommandArgs.MAX_ENTITY_REFS)
            {
                logger.Warning($"Can't send command that has more than {GenericNetworkCommandArgs.MAX_ENTITY_REFS} entityID parameters. Command={commandName} EntityId={targetEntity}");
                return;
            }

            var sIDs = new SerializeEntityID[GenericNetworkCommandArgs.MAX_ENTITY_REFS];
            if (entityIDs != null)
            {
                for (int i = 0; i < entityIDs.Length; i++)
                {
                    sIDs[i] = entityIDs[i];
                }
            }

            var command = new GenericCommand
            {
                name = String.IsNullOrEmpty(commandName) ? "" : commandName,
                commandData = data,
                entityParam1 = sIDs[0],
                entityParam2 = sIDs[1],
                entityParam3 = sIDs[2],
                entityParam4 = sIDs[3],
            };

            self.Client.SendCommand(command, messageTarget, targetEntity);
        }

        private static void ReceiveGenericCommand(CoherenceSync self, IEntityCommand command, MessageTarget target, Logger logger)
        {
            if (!(command is GenericCommand))
            {
                logger.Warning($"[coherenceSync] Received unknown type of command in reflected mode: {command.GetType()}. {self}");
				return;
            }

            var genericCommand = (GenericCommand)command;
            var commandName = genericCommand.name;
            var commandData = genericCommand.commandData;
            var entityIDs = new SerializeEntityID[GenericNetworkCommandArgs.MAX_ENTITY_REFS];
            entityIDs[0] = genericCommand.entityParam1;
            entityIDs[1] = genericCommand.entityParam2;
            entityIDs[2] = genericCommand.entityParam3;
            entityIDs[3] = genericCommand.entityParam4;

            self.ProcessGenericNetworkCommand(commandName, target, commandData, entityIDs);
        }

		private static (ICoherenceComponentData[], uint[]) CreateConnectedEntityUpdateInternal(CoherenceSync sync, SerializeEntityID parentID)
		{
			var updates = new ICoherenceComponentData[]
			{
				new ConnectedEntity()
				{
					value = parentID,
				},
			};

			var masks = new uint[]
			{
				0b1,
			};

			return (updates, masks);
		}

		private static uint GetConnectedEntityComponentIdInternal()
		{
			return Definition.InternalConnectedEntity;
		}

		private static void UpdateTag(IClient client, SerializeEntityID liveQuery, string tag)
		{
			var components = new ICoherenceComponentData[]
			{
				new Tag { tag = tag }
			};

			var masks = new uint[]
			{
				0xff,
			};

			client.UpdateComponents(liveQuery, components, masks);
		}

		private static void RemoveTag(IClient client, SerializeEntityID liveQuery)
		{
			client.RemoveComponents(liveQuery, new []{Definition.InternalTag});
		}
	}
}
