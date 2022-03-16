// Copyright (c) coherence ApS.
// See the license file in the project root for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Unity.Collections;
	using Unity.Mathematics;
	using Coherence.Toolkit;
	using Coherence.Entity;
	using Coherence.ProtocolDef;
	using UnityEngine.Scripting;

	[Preserve]
	[AddComponentMenu("coherence/Baked/PlayerCursor")]
	[RequireComponent(typeof(CoherenceSync))]
	public class CoherenceSyncPlayerCursor : CoherenceSyncBaked
	{
		private CoherenceSync coherenceSync;

		// Cached references to MonoBehaviours on this GameObject
		private Coherence.Toolkit.CoherenceSync _transformViaCoherenceSync;
		private Player _player;

		// Last sent data for each component (used for diffing)
		private WorldPosition _transformViaCoherenceSync_WorldPosition_lastSentData = default;
		private WorldOrientation _transformViaCoherenceSync_WorldOrientation_lastSentData = default;
		private PlayerCursor_Player _player_PlayerCursor_Player_lastSentData = default;

		private string lastSerializedCoherenceUUID;

		private IBinding InternalWorldPosition_Translation_value_Binding;
		private InterpolationState<float3> InternalWorldPosition_Translation_value_InterpolationState;
		private NativeList<InterpolationSample<float3>> InternalWorldPosition_Translation_value_InterpolationSamples;

		private IBinding InternalWorldOrientation_Rotation_value_Binding;
		private InterpolationState<quaternion> InternalWorldOrientation_Rotation_value_InterpolationState;
		private NativeList<InterpolationSample<quaternion>> InternalWorldOrientation_Rotation_value_InterpolationSamples;

		private IBinding InternalPlayerCursor_Player_PlayerCursor_Player_playerName_Binding;

		private IBinding InternalPlayerCursor_Player_PlayerCursor_Player_startOnFrame_Binding;

		private InputBuffer<TestCube> inputBuffer;
		private TestCube currentInput;
		private long lastAddedFrame = -1;

		private IClient client => coherenceSync.Client;
		private CoherenceMonoBridge monoBridge => coherenceSync.MonoBridge;
		private long currentSimulationFrame => coherenceInput.CurrentSimulationFrame;
		private CoherenceInput coherenceInput;

		protected void Awake()
		{
			coherenceSync = GetComponent<CoherenceSync>();
			coherenceSync.usingReflection = false;
			coherenceInput = coherenceSync.Input;
			_transformViaCoherenceSync = GetComponent<Coherence.Toolkit.CoherenceSync>();
			_player = GetComponent<Player>();

			bool inputsRequireSubsequentFrames = coherenceSync.simulationType == CoherenceSync.SimulationType.ClientSide;
			inputBuffer = new InputBuffer<TestCube>(coherenceInput.InitialBufferSize, coherenceInput.InitialBufferDelay, inputsRequireSubsequentFrames);

			if (!coherenceSync.TryGetBinding(Type.GetType("UnityEngine.Transform, UnityEngine"), "position", out InternalWorldPosition_Translation_value_Binding))
			{
				Debug.LogError("[CoherenceSync] Couldn't find binding (UnityEngine.Transform, UnityEngine).position");
			}

			if (!coherenceSync.TryGetBinding(Type.GetType("UnityEngine.Transform, UnityEngine"), "rotation", out InternalWorldOrientation_Rotation_value_Binding))
			{
				Debug.LogError("[CoherenceSync] Couldn't find binding (UnityEngine.Transform, UnityEngine).rotation");
			}

			if (!coherenceSync.TryGetBinding(Type.GetType("Player, Assembly-CSharp"), "playerName", out InternalPlayerCursor_Player_PlayerCursor_Player_playerName_Binding))
			{
				Debug.LogError("[CoherenceSync] Couldn't find binding (Player, Assembly-CSharp).playerName");
			}

			if (!coherenceSync.TryGetBinding(Type.GetType("Player, Assembly-CSharp"), "startOnFrame", out InternalPlayerCursor_Player_PlayerCursor_Player_startOnFrame_Binding))
			{
				Debug.LogError("[CoherenceSync] Couldn't find binding (Player, Assembly-CSharp).startOnFrame");
			}
			if (coherenceInput.UseFixedSimulationFrames)
			{
				monoBridge.OnLateFixedNetworkUpdate += SendInputState;
			}

			coherenceInput.internalOnInputReceived += OnInput;
		}

		public override List<ICoherenceComponentData> CreateEntity()
		{
			if (coherenceSync.UsesLODsAtRuntime)
			{
				var components = new List<ICoherenceComponentData>()
				{
					new ArchetypeComponent
					{
						index = Archetypes.IndexForName[coherenceSync.Archetype.ArchetypeName]
					}
				};

				return components;
			}

			return null;
		}

		private void OnDestroy()
		{
			coherenceSync.interpolationCollection.float3SampleSet.Release(InternalWorldPosition_Translation_value_InterpolationSamples, InternalWorldPosition_Translation_value_Binding);
			coherenceSync.interpolationCollection.quaternionSampleSet.Release(InternalWorldOrientation_Rotation_value_InterpolationSamples, InternalWorldOrientation_Rotation_value_Binding);
			if (monoBridge != null)
			{
				monoBridge.OnLateFixedNetworkUpdate -= SendInputState;
			}
		}

		public override void Initialize(CoherenceSync sync)
		{
            if (coherenceSync == null)
            { 
                coherenceSync = sync;
            }
            
			lastSerializedCoherenceUUID = coherenceSync.coherenceUUID;

			sync.Input.internalSetButtonState = SetButtonState;
			sync.Input.internalSetButtonRangeState = SetButtonRangeState;
			sync.Input.internalSetAxisState = SetAxisState;
			sync.Input.internalSetStringState = SetStringState;

			sync.Input.internalGetButtonState = GetButtonState;
			sync.Input.internalGetButtonRangeState = GetButtonRangeState;
			sync.Input.internalGetAxisState = GetAxisState;
			sync.Input.internalGetStringState = GetStringState;

			sync.Input.internalRequestBuffer = () => inputBuffer;

			if (!coherenceSync.isSimulated)
			{
				InitializeInterpolation();
			}
		}

		public override void InitializeInterpolation()
		{
			if (!InternalWorldPosition_Translation_value_InterpolationSamples.IsCreated)
			{
				var binding = InternalWorldPosition_Translation_value_Binding;
				InternalWorldPosition_Translation_value_InterpolationState = coherenceSync.interpolationCollection.float3StateSet.Get(binding);
				InternalWorldPosition_Translation_value_InterpolationState.binding = binding;
				InternalWorldPosition_Translation_value_InterpolationSamples = coherenceSync.interpolationCollection.float3SampleSet.Get(binding);
			}
			else
			{
				InternalWorldPosition_Translation_value_InterpolationState.velocity = default;
				InternalWorldPosition_Translation_value_InterpolationSamples.Clear();
			}
			if (!InternalWorldOrientation_Rotation_value_InterpolationSamples.IsCreated)
			{
				var binding = InternalWorldOrientation_Rotation_value_Binding;
				InternalWorldOrientation_Rotation_value_InterpolationState = coherenceSync.interpolationCollection.quaternionStateSet.Get(binding);
				InternalWorldOrientation_Rotation_value_InterpolationState.binding = binding;
				InternalWorldOrientation_Rotation_value_InterpolationSamples = coherenceSync.interpolationCollection.quaternionSampleSet.Get(binding);
			}
			else
			{
				InternalWorldOrientation_Rotation_value_InterpolationState.velocity = default;
				InternalWorldOrientation_Rotation_value_InterpolationSamples.Clear();
			}
		}

		public override void OnAuthorityChanged(bool hasAuthorityNow)
		{
			if (!hasAuthorityNow)
			{
				InitializeInterpolation();
			}
		}

		public override void ReceiveCommand(IEntityCommand command)
		{
			switch(command)
			{
				default:
					Debug.LogWarning($"[CoherenceSyncPlayerCursor] Unhandled command: {command.GetType()}.");
					break;
			}
		}

		public override void SendUpdates()
		{
			SendComponentUpdates();
			if (!coherenceInput.UseFixedSimulationFrames)
			{
				SendInputState();
			}
		}

		private void Update()
		{
			if ((coherenceSync.InterpolationLocation & CoherenceSync.InterpolationLoop.Update) > 0)
			{
				PerformInterpolation();
			}
		}

		private void LateUpdate()
		{
			if ((coherenceSync.InterpolationLocation & CoherenceSync.InterpolationLoop.LateUpdate) > 0)
			{
				PerformInterpolation();
			}
		}

		private void FixedUpdate()
		{
			if (coherenceSync.HasPhysics || (coherenceSync.InterpolationLocation & CoherenceSync.InterpolationLoop.FixedUpdate) > 0)
			{
				PerformInterpolation();
			}
		}

		public override (List<ICoherenceComponentData>, List<uint>) GetLatestChanges()
		{
			var updates = new List<ICoherenceComponentData>();
			var masks = new List<uint>();

			if (coherenceSync.isSimulated)
			{

				// Send Translation / WorldPosition
				{
					var update = new WorldPosition();

					update.value = (_transformViaCoherenceSync.coherencePosition);

					uint mask = _transformViaCoherenceSync_WorldPosition_lastSentData.DiffWith(update);

					if(mask != 0 && coherenceSync.IsReadyToSample(typeof(WorldPosition), Time.time))
					{
						updates.Add(update);
						masks.Add(mask);
						_transformViaCoherenceSync_WorldPosition_lastSentData = update;
					}
				}

				// Send Rotation / WorldOrientation
				{
					var update = new WorldOrientation();

					update.value = (_transformViaCoherenceSync.coherenceRotation);

					uint mask = _transformViaCoherenceSync_WorldOrientation_lastSentData.DiffWith(update);

					if(mask != 0 && coherenceSync.IsReadyToSample(typeof(WorldOrientation), Time.time))
					{
						updates.Add(update);
						masks.Add(mask);
						_transformViaCoherenceSync_WorldOrientation_lastSentData = update;
					}
				}

				// Send PlayerCursor_Player / PlayerCursor_Player
				{
					var update = new PlayerCursor_Player();

					update.playerName = (_player.playerName ?? "");
					update.startOnFrame = (_player.startOnFrame);

					uint mask = _player_PlayerCursor_Player_lastSentData.DiffWith(update);

					if(mask != 0 && coherenceSync.IsReadyToSample(typeof(PlayerCursor_Player), Time.time))
					{
						updates.Add(update);
						masks.Add(mask);
						_player_PlayerCursor_Player_lastSentData = update;
					}
				}

				if (coherenceSync.coherenceUUID != lastSerializedCoherenceUUID)
				{
					var update = CoherenceMonoBridge.GetRootDefinition().GenerateCoherenceUUIDData(coherenceSync.coherenceUUID);
					var mask = 0b11111111111111111111111111111111;

					updates.Add(update);
					masks.Add(mask);

					lastSerializedCoherenceUUID = coherenceSync.coherenceUUID;
				}
			}

			return (updates, masks);
		}

		static float3 Vector3ToFloat(object o)
		{
			Vector3 v = (Vector3)o;
			return new float3(v.x, v.y, v.z);
		}

		private void PerformInterpolation()
		{
			if (coherenceSync.isSimulated)
			{
				return;
			}

			var localFrame = (ulong)client.NetworkTime.ClientSimulationFrame.Frame;
			if (InternalWorldPosition_Translation_value_Binding.CanInterpolate)
			{
				var InternalWorldPosition_value_target = InterpolationSystem.PerformInterpolation(
					InternalWorldPosition_Translation_value_InterpolationSamples,
					ref InternalWorldPosition_Translation_value_InterpolationState,
					localFrame,
					_transformViaCoherenceSync.coherencePosition,
					monoBridge.NetworkTime.TimeAsDouble);

				_transformViaCoherenceSync.coherencePosition = (InternalWorldPosition_value_target);
			}
			if (InternalWorldOrientation_Rotation_value_Binding.CanInterpolate)
			{
				var InternalWorldOrientation_value_target = InterpolationSystem.PerformInterpolation(
					InternalWorldOrientation_Rotation_value_InterpolationSamples,
					ref InternalWorldOrientation_Rotation_value_InterpolationState,
					localFrame,
					_transformViaCoherenceSync.coherenceRotation,
					monoBridge.NetworkTime.TimeAsDouble);

				_transformViaCoherenceSync.coherenceRotation = (InternalWorldOrientation_value_target);
			}
		}

		private void SendComponentUpdates()
		{
			var entity = coherenceSync.EntityID;

			if (coherenceSync.isSimulated)
			{
				var (updates, masks) = GetLatestChanges();

				if (updates.Count > 0)
				{
					client.UpdateComponents(coherenceSync.EntityID, updates.ToArray(), masks.ToArray());
				}
			}
		}

		public override void ApplyComponentUpdates(EntityData entityData)
		{
			// Sort component changes by (order, index) using Linq to ensure consistent replication order, regardless of ordering in packet.
			// Multiplying by a constant value is faster than using Linq .ThenBy extension method.
			var orderedChanges = entityData.Store.Values.OrderBy(c => c.Data.GetComponentOrder() * 10000 + c.Data.GetComponentType());

			foreach (var change in orderedChanges)
			{
				var componentType = change.Data.GetComponentType();

				switch(componentType)
				{

					case 0:
					{
						// Translation
						var data = (WorldPosition)change.Data;
						var mask = change.Mask;
						if((mask & 0b00000000000000000000000000000001) != 0)
						{
							if (InternalWorldPosition_Translation_value_Binding.CanInterpolate)
							{
								var settings = InternalWorldPosition_Translation_value_InterpolationState.binding.InterpolationSettings;
								var buffer = InternalWorldPosition_Translation_value_InterpolationSamples;
								var value = data.value;
								var cleared = InterpolationSystem.UpdateInterpolationSamples(ref buffer, ref InternalWorldPosition_Translation_value_InterpolationState, value);
								InterpolationSystem.AppendInterpolationSample(ref buffer, value, (ulong)data.GetSimulationFrame().Frame);
								var newLatency = InterpolationSystem.CalculateLatency<float3>(buffer, settings);
								if (newLatency >= 0f)
								{
									InternalWorldPosition_Translation_value_InterpolationState.latency = newLatency;
								}

								if (cleared)
								{
									_transformViaCoherenceSync.coherencePosition = (data.value);
								}
							}
							else
							{
								_transformViaCoherenceSync.coherencePosition = (data.value);
							}
						}
						break;
					}

					case 1:
					{
						// Rotation
						var data = (WorldOrientation)change.Data;
						var mask = change.Mask;
						if((mask & 0b00000000000000000000000000000001) != 0)
						{
							if (InternalWorldOrientation_Rotation_value_Binding.CanInterpolate)
							{
								var settings = InternalWorldOrientation_Rotation_value_InterpolationState.binding.InterpolationSettings;
								var buffer = InternalWorldOrientation_Rotation_value_InterpolationSamples;
								var value = data.value;
								var cleared = InterpolationSystem.UpdateInterpolationSamples(ref buffer, ref InternalWorldOrientation_Rotation_value_InterpolationState, value);
								InterpolationSystem.AppendInterpolationSample(ref buffer, value, (ulong)data.GetSimulationFrame().Frame);
								var newLatency = InterpolationSystem.CalculateLatency<quaternion>(buffer, settings);
								if (newLatency >= 0f)
								{
									InternalWorldOrientation_Rotation_value_InterpolationState.latency = newLatency;
								}

								if (cleared)
								{
									_transformViaCoherenceSync.coherenceRotation = (data.value);
								}
							}
							else
							{
								_transformViaCoherenceSync.coherenceRotation = (data.value);
							}
						}
						break;
					}

					case 76:
					{
						// PlayerCursor_Player
						var data = (PlayerCursor_Player)change.Data;
						var mask = change.Mask;
						if((mask & 0b00000000000000000000000000000001) != 0)
						{
							_player.playerName = (String)(data.playerName);
						}
						if((mask & 0b00000000000000000000000000000010) != 0)
						{
							_player.startOnFrame = (Int64)(data.startOnFrame);
						}
						break;
					}

					case Definition.InternalArchetypeComponent:
						// Handled internally by coherence Client Core
						break;
					case Definition.InternalGenericPrefabReference:
						// Handled internally by the coherence Mono Bridge
						break;
					case Definition.InternalUniqueID:
						coherenceSync.coherenceUUID = lastSerializedCoherenceUUID = CoherenceMonoBridge.GetRootDefinition().ExtractCoherenceUUID(change.Data);
						break;
					case Definition.InternalPersistence:
						break;
					case Definition.InternalConnectedEntity:
						var newConnectedEntity = CoherenceMonoBridge.GetRootDefinition().ExtractConnectedEntityID(change.Data);
						coherenceSync.ConnectedEntityChanged(newConnectedEntity);
						break;
					case Definition.InternalInputOwnerComponent:
						var newInputOwner = (InputOwnerComponent)change.Data;
						coherenceSync.Input.SetInputOwner((uint)newInputOwner.clientId);
						break;
					case Definition.InternalConnection:
					case Definition.InternalGlobal:
					case Definition.InternalGlobalQuery:
					case Definition.InternalTag:
					case Definition.InternalTagQuery:
						break;
					default:
						Debug.LogWarning($"Unhandled component type ID: {componentType}", this);
						break;
					}
			}
		}

		private void SetButtonState(string name, bool value)
		{
			switch(name)
			{
				default:
					Debug.LogError($"No input button of name: {name}");
					break;
			}
		}

		private void SetButtonRangeState(string name, float value)
		{
			switch(name)
			{
			case "key":
				currentInput.key = value;
				break;
			default:
				Debug.LogError($"No input button range of name: {name}");
				break;
			}
		}

		private void SetAxisState(string name, Vector2 value)
		{
			switch(name)
			{
			case "Mov":
				currentInput.Mov = value;
				break;
			default:
				Debug.LogError($"No input axis of name: {name}");
				break;
			}
		}

		private void SetStringState(string name, string value)
		{
			switch(name)
			{
				default:
					Debug.LogError($"No input button of name: {name}");
					break;
			}
		}

		private void SendInputState()
		{
			if (!coherenceInput.IsProducer || !coherenceInput.IsReadyToProcessInputs || !coherenceInput.IsInputOwner)
			{
				return;
			}

			if (lastAddedFrame != currentSimulationFrame)
			{
				inputBuffer.AddInput(currentInput, currentSimulationFrame);
				lastAddedFrame = currentSimulationFrame;
			}

			while (inputBuffer.DequeueForSending(currentSimulationFrame, out long frameToSend, out TestCube input, out bool differs))
			{
				if (coherenceInput.IsServerSimulated && !differs)
				{
					continue;
				}

				coherenceInput.DebugOnInputSent(frameToSend, input);
				client.SendInput(input, frameToSend, coherenceSync.EntityID);
			}
		}

		private bool ShouldPollCurrentInput(long frame)
		{
			return coherenceInput.IsProducer && coherenceInput.Delay == 0 && frame == currentSimulationFrame;
		}

		private bool GetButtonState(string name, long? simulationFrame)
		{
			long frame = simulationFrame.GetValueOrDefault(currentSimulationFrame);

			switch(name)
			{
				default:
					Debug.LogError($"No input button of name: {name}");
					break;
			}

			return false;
		}

		private float GetButtonRangeState(string name, long? simulationFrame)
		{
			long frame = simulationFrame.GetValueOrDefault(currentSimulationFrame);

			switch(name)
			{
			case "key":
				{
					if (ShouldPollCurrentInput(frame))
					{
						return coherenceInput.IsProducer ? currentInput.Compressedkey : currentInput.key;
					}

					inputBuffer.TryGetInput(frame, out TestCube input);
					return coherenceInput.IsProducer ? input.Compressedkey : input.key;
				}
			default:
				Debug.LogError($"No input button range of name: {name}");
				break;
			}

			return 0f;
		}

		private Vector2 GetAxisState(string name, long? simulationFrame)
		{
			long frame = simulationFrame.GetValueOrDefault(currentSimulationFrame);

			switch(name)
			{
			case "Mov":
				{
					if (ShouldPollCurrentInput(frame))
					{
						return coherenceInput.IsProducer ? currentInput.CompressedMov : currentInput.Mov;
					}

					inputBuffer.TryGetInput(frame, out TestCube input);
					return coherenceInput.IsProducer ? input.CompressedMov : input.Mov;
				}
			default:
				Debug.LogError($"No input axis of name: {name}");
				break;
			}

			return Vector2.zero;
		}

		private string GetStringState(string name, long? simulationFrame)
		{
			long frame = simulationFrame.GetValueOrDefault(currentSimulationFrame);

			switch(name)
			{
				default:
					Debug.LogError($"No input button of name: {name}");
					break;
			}

			return null;
		}

		private void OnInput(IEntityInput entityInput, long frame)
		{
			var input = (TestCube)entityInput;
			coherenceInput.DebugOnInputReceived(frame, entityInput);
			inputBuffer.ReceiveInput(input, frame);
		}
	}
}
