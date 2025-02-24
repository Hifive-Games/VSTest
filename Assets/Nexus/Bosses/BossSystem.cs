using System.Collections.Generic;
using UnityEngine;

namespace BossSystem
{
    public enum BossStateType
    {
        Spawning,
        Fighting,
        Dying
    }

    public enum BossPhaseType
    {
        BeforeSpell,
        Spell,
        Combine
    }

    public interface IBossState
    {
        BossStateType StateType { get; }
        void EnterState(BossController controller);
        void UpdateState(BossController controller);
        void ExitState(BossController controller);
    }

    public interface IBossPhase
    {
        BossPhaseType PhaseType { get; }
        void EnterPhase(BossController controller);
        void UpdatePhase(BossController controller);
        void ExitPhase(BossController controller);
    }

    public class BossController
    {
        private IBossState _currentState;
        private IBossPhase _currentPhase;
        public IBossPhase CurrentPhase => _currentPhase;
        public IBossState CurrentState => _currentState;

        public float MaxHP { get; set; }
        public float CurrentHP { get; set; }

        public BossController(IBossState initialState, IBossPhase initialPhase)
        {
            ChangeState(initialState);
            ChangePhase(initialPhase);
        }

        public void Update()
        {
            _currentState?.UpdateState(this);
            _currentPhase?.UpdatePhase(this);
        }

        public void ChangeState(IBossState newState)
        {
            Debug.Log($"Changing state from {_currentState?.StateType} to {newState.StateType}");
            _currentState?.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }

        public void ChangePhase(IBossPhase newPhase)
        {
            Debug.Log($"Changing phase from {_currentPhase?.PhaseType} to {newPhase.PhaseType}");
            _currentPhase?.ExitPhase(this);
            _currentPhase = newPhase;
            _currentPhase.EnterPhase(this);
        }
    }

    public class SpawningState : IBossState
    {
        private float spawnTimer = 3f;

        public BossStateType StateType => BossStateType.Spawning;

        public void EnterState(BossController controller)
        {
            // Initialize HP and set initial phase
            controller.CurrentHP = controller.MaxHP;
            //controller.ChangePhase(new BeforeSpellPhase());
        }

        public void UpdateState(BossController controller)
        {
            // Transition to Fighting after spawn logic
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                controller.ChangeState(new FightingState());
            }
        }

        public void ExitState(BossController controller)
        {
            // Cleanup spawning if needed
        }
    }
    public class FightingState : IBossState
    {
        public BossStateType StateType => BossStateType.Fighting;

        private float timer = 3f; // Example timer for phase transitions

        public void EnterState(BossController controller)
        {
            // Initialize fighting state
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                controller.ChangePhase(new BeforeSpellPhase());
            }
        }

        public void UpdateState(BossController controller)
        {
            // Check HP to transition to dying
            if (controller.CurrentHP <= 0)
            {
                controller.ChangeState(new DyingState());
                timer = 3f; // Reset timer for next use
                return;
            }

            // Example for changing phases based on HP %
            float hpPercent = (controller.CurrentHP / controller.MaxHP) * 100f;

            if (hpPercent <= 33 && controller.CurrentPhase.PhaseType != BossPhaseType.Combine)
            {
                controller.ChangePhase(new CombinePhase());
            }
            else if (hpPercent <= 66 && hpPercent > 33 && controller.CurrentPhase.PhaseType != BossPhaseType.Spell)
            {
                controller.ChangePhase(new GenericSpellPhase());
            }
            else if (hpPercent <= 100 && hpPercent > 66 && controller.CurrentPhase.PhaseType != BossPhaseType.BeforeSpell)
            {
                controller.ChangePhase(new BeforeSpellPhase());
            }
        }

        public void ExitState(BossController controller)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Transition to next state
                controller.ChangeState(new DyingState());
            }
        }
    }
    public class DyingState : IBossState
    {
        public BossStateType StateType => BossStateType.Dying;

        private float timer = 5f; // Example timer for death animation

        public void EnterState(BossController controller)
        {
            // Trigger death animation/effects
            timer = 5f; // Reset timer for next use
            Debug.Log("Boss is dying!");
        }

        public void UpdateState(BossController controller)
        {
            // Once dying animation completes, remove the boss
            // (Unity example) Object.Destroy(controller.gameObject);
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Debug.Log("Boss has died!");
            }
        }

        public void ExitState(BossController controller)
        {
            // Post-death cleanup and transition to spell drop or similar
            Debug.Log("Boss has been removed! Spells can now be dropped.");
        }
    }

    // Example phases
    public class BeforeSpellPhase : IBossPhase
    {
        public BossPhaseType PhaseType => BossPhaseType.BeforeSpell;
        public void EnterPhase(BossController controller) { /* ... */ }
        public void UpdatePhase(BossController controller) { /* ... */ }
        public void ExitPhase(BossController controller) { /* ... */ }
    }

    public class SpellPhase : IBossPhase
    {
        public BossPhaseType PhaseType => BossPhaseType.Spell;
        public List<Spell> spells = new List<Spell>();

        public void EnterPhase(BossController controller) { /* ... */ }
        public void UpdatePhase(BossController controller) { /* ... */ }
        public void ExitPhase(BossController controller) { /* ... */ }
    }

    public class CombinePhase : IBossPhase
    {
        public BossPhaseType PhaseType => BossPhaseType.Combine;
        public void EnterPhase(BossController controller) { /* ... */ }
        public void UpdatePhase(BossController controller) { /* ... */ }
        public void ExitPhase(BossController controller) { /* ... */ }
    }
}

public enum ActionType
{
    MoveCloser,
    MoveAway,
    Attack
}