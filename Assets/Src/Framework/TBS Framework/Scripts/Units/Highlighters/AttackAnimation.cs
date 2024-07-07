using System.Collections;
using TbsFramework.Grid;
using UnityEngine;

namespace TbsFramework.Units.Highlighters
{
    public class AttackAnimation : UnitHighlighter
    {
        public float Magnitude = 1f;

        private Vector3 _originalPosition;
        private Coroutine _coroutine;

        private void OnEnable() => CellGrid.Instance.TurnEnded += OnTurnEnded;
        private void OnDisable() => CellGrid.Instance.TurnEnded += OnTurnEnded;

        public override void Apply(Unit unit, Unit otherUnit)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                unit.transform.localPosition = _originalPosition;
            }

            _coroutine = StartCoroutine(AttackAnimationCoroutine(unit, otherUnit));
        }

        private IEnumerator AttackAnimationCoroutine(Unit unit, Unit otherUnit)
        {
            var StartingPosition = unit.transform.position;
            _originalPosition = unit.transform.localPosition;

            var heading = otherUnit.transform.localPosition - unit.transform.localPosition;
            var direction = heading / heading.magnitude * Magnitude;
            float startTime = Time.time;

            while (startTime + 0.25f > Time.time)
            {
                unit.transform.localPosition = Vector3.Lerp(unit.transform.localPosition,
                    unit.transform.localPosition + (direction / 2.5f),
                    ((startTime + 0.25f) - Time.time));
                yield return 0;
            }

            startTime = Time.time;
            while (startTime + 0.25f > Time.time)
            {
                unit.transform.localPosition = Vector3.Lerp(unit.transform.localPosition,
                    unit.transform.localPosition - (direction / 2.5f),
                    ((startTime + 0.25f) - Time.time));
                yield return 0;
            }

            unit.transform.position = StartingPosition;
            _originalPosition = unit.transform.localPosition;
        }

        private void OnTurnEnded(object sender, bool something)
        {
            _coroutine = null;
            _originalPosition = Vector3.zero;
        }
    }
}